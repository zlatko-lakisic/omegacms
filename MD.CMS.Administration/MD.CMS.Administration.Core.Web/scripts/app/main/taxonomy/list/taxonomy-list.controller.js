(function () {
  'use strict';

  angular
    .module('app.taxonomy.list')
    .controller('TaxonomyListController', ['$state', '$rootScope', '$mdSidenav', '$mdDialog', '$scope', 'mdFeedbackService', 'mdCustomDialogs', 'taxonomy', TaxonomyListController]);


  /** @ngInject */
    function TaxonomyListController($state, $rootScope, $mdSidenav, $mdDialog, $scope, $mdFeedbackService, dialog, taxonomy) {
        var vm = this;

        // Controllers
        var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
        var taxonomyContentController = new mdBusinessLogic.dataAccess.controllers.taxonomyContentController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();

        // Variables
        vm.currentTaxonomyPath = $state.params.taxonomyPath || 'Root';
        vm.taxonomy = taxonomy;
        vm.currentView = {
            Name: 'grid',
            Label: 'Labels.GridView',
            Icon: 'icon-view-headline'
        };
        var numberOfTaxonomiesToDisplay;
        var numberOfTaxonomyContentsToDisplay;
        var pagerInit = true;
        vm.displayTaxonomies = 1;
        vm.displayTaxonomyContents = 0;
        vm.currentPageIndex = $state.params.currentPageIndex || 0;
        vm.lcid = mdBusinessLogic.settings.lcid || 2057;
        vm.currentPage = 0;
        vm.totalItems = taxonomy.TotalChildren;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";
        vm.showOrder = true;
        vm.hasChildren = checkChildren();
        vm.hasContents = checkContents();
        vm.currentTab = vm.hasChildren || !vm.hasContents ? 0 : 1;
        vm.selected = taxonomy;
        vm.selectedTaxonomy = taxonomy;

        // Methods
        vm.selectTaxonomy = selectTaxonomy;
        vm.selectContent = selectContent;
        vm.toggleContentDetails = toggleContentDetails;
        vm.toggleTaxonomyDetails = toggleTaxonomyDetails;
        vm.toggleSidenav = toggleSidenav;
        vm.toggleView = toggleView;
        vm.getSelectedType = getSelectedType;
        vm.deleteItem = deleteItem;
        vm.goToForm = goToForm;
        vm.changeTab = changeTab;
        vm.redirectoContentLocation = redirectoContentLocation;
        vm.openMenu = openMenu;
        vm.checkContents = checkContents;
        vm.checkChildren = checkChildren;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openTaxonomy = openTaxonomy;
        vm.openContent = openContent;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            if (vm.currentTab == 0) {
                getTaxonomies();
            } else if (vm.currentTab == 1) {
                getTaxonomyContents();
            }
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            if (vm.currentTab === 0) {
                getTaxonomies();
            } else {
                getTaxonomyContents();
            }
        }

        function toggleSearch() {
            vm.isSearchVisible = !vm.isSearchVisible;
            if (!vm.isSearchVisible) {
                if (!vm.searchTerm.length == 0) {
                    vm.cancelSearch();
                }
            }
        }

        function cancelSearch() {
            vm.searchGotResponse = true;
            vm.searchTerm = "";
            vm.search();
        }

        function checkChildren() {
            return vm.taxonomy.ChildrenTotalCount || ((vm.searchTerm || !vm.searchGotResponse) && vm.currentTab == 0);
        }

        function checkContents() {
            return vm.taxonomy.ItemsTotalCount || ((vm.searchTerm || !vm.searchGotResponse) && vm.currentTab == 1);
        }

        function sort(sortingString) {
            vm.sortingString = sortingString;
            if (sortingString != "Order ASC") {
                vm.showOrder = false;
            } else {
                vm.showOrder = true;
            }
            getTaxonomyContents();
        }

        function openMenu($mdMenu, ev) {
            ev.stopImmediatePropagation();
            $mdMenu.open(ev);
        };

        function redirectoContentLocation() {
            if (vm.getSelectedType() == 2) {
                $state.go('app.content_list', {
                    folderPath: vm.selectedContent.Path
                });
            }
        }

        function goToForm(taxonomyId) {
            $state.go('app.taxonomy_forms', {
                taxonomyId: taxonomyId,
                taxonomyPath: vm.taxonomy.TaxonomyPath,
                currentView: vm.currentView
            });
        }

        function selectTaxonomy(item) {
            vm.selectedContent = null;
            vm.selectedTaxonomy = item;
            select(item);
        }

        function selectContent(item) {
            vm.selectedContent = item;
            select(item);
        }

        function select(item) {
            vm.selected = item;
        }

        function openTaxonomy(taxonomy) {
            $state.go("app.taxonomy_list", { taxonomyPath: taxonomy.TaxonomyPath, currentView: vm.currentView.Name });
        }

        function openContent(content) {
            $state.go("app.taxonomy_forms", { path: vm.selectedTaxonomy.TaxonomyPath, id: vm.selectedTaxonomy.Id, action: 'edit' });
        }

        function toggleTaxonomyDetails(item, event) {
            event.stopPropagation();
            selectTaxonomy(item);
            toggleSidenav('details-sidenav');
        }

        function toggleContentDetails(item, event) {
            event.stopPropagation();
            selectContent(item);
            toggleSidenav('details-sidenav');
        }

        function getSelectedType() {
            return (vm.selectedContent != null ? 2 : 1);
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function toggleView(view) {
            if ((vm.currentView.Name == 'grid' && view != 'grid') || view == 'list') {
                vm.currentView = {
                    Name: 'list',
                    Label: 'Labels.ListView',
                    Icon: 'icon-view-module'
                };
            } else if (view == 'grid' || view == undefined) {
                vm.currentView = {
                    Name: 'grid',
                    Label: 'Labels.GridView',
                    Icon: 'icon-view-headline'
                };
            }
        }

        function deleteItem() {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));
            if (getSelectedType() === 2) {
                taxonomyController.delContent(vm.selectedContent.Id, vm.currentTaxonomyPath,
                    function (data) {
                        $mdFeedbackService.reportInfo("delete");
                        changeTab();
                    }, function (error) {
                        $mdFeedbackService.reportError("delete", error);
                    });
            } else if (getSelectedType() === 1) {
                if (vm.selectedTaxonomy.Id <= 1) {
                    $mdFeedbackService.reportError("403");
                } else {
                    dialog.showConfirmDialog(
                        $rootScope.globals.resources.Titles.RemoveQuestion,
                        $rootScope.globals.resources.Labels.RemoveAnswer,
                        $rootScope.globals.resources.Labels.Yes,
                        $rootScope.globals.resources.Labels.No,
                        function () {
                            taxonomyController.del(vm.selectedTaxonomy.Id,
                                function (data) {
                                    $scope.$emit('LoadNav', {
                                        action: 'remove',
                                        type: mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy,
                                        value: angular.copy(vm.selectedTaxonomy)
                                    });
                                    for (var i = 0; i < vm.taxonomy.Children.length; i++) {
                                        if (vm.taxonomy.Children[i].Id == vm.selectedTaxonomy.Id) {
                                            vm.taxonomy.Children.splice(i, 1);
                                            break;
                                        }
                                    }
                                    $mdFeedbackService.reportInfo("delete");
                                    changeTab();
                                }, function (error) {
                                    $mdFeedbackService.reportError("delete", error);

                                })
                        },
                        function () {

                        });
                }
            }
        }

        //Order
        vm.orderDown = orderDown;
        vm.orderUp = orderUp;
        vm.disableUntilFinishedChange = false;

        //Function for change place of two elements of each other
        Array.prototype.move = function (old_index, new_index) {
            if (new_index >= this.length) {
                var k = new_index - this.length;
                while ((k--) + 1) {
                    this.push(undefined);
                }
            }
            this.splice(new_index, 0, this.splice(old_index, 1)[0]);
            return this;
        };

        //function for order down for one place
        function orderDown(index) {
            if (vm.currentTab == 0) {
                if (index + 1 == vm.pageSize) {
                    taxonomyController.GetByParentIdWithPagination({
                        path: vm.taxonomy.Id,
                        pageIndex: (vm.currentPage + 1) * vm.pageSize,
                        pageSize: 1,
                        sortingString: vm.sortingString,
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                        function (data) {
                            $scope.$apply(function () {
                                var taxonomyCopy = angular.copy(vm.taxonomy);
                                taxonomyCopy.Children.splice(index, 0, data.Items[0]);
                                vm.disableUntilFinishedChange = true;
                                taxonomyController.updateChildren(
                                    taxonomyCopy,
                                    vm.currentPage * vm.pageSize,
                                    function (data) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            vm.currentPage++;
                                            $mdFeedbackService.reportInfo("update");
                                        });
                                    },
                                    function (error) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            $mdFeedbackService.reportError("update", error);
                                        });
                                    });
                            });
                        }, function (error) {
                        });
                } else {
                    vm.taxonomy.Children.move(index, index + 1);
                    vm.disableUntilFinishedChange = true;
                    taxonomyController.updateChildren(
                        vm.taxonomy,
                        vm.currentPage * vm.pageSize,
                        function (data) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportInfo("update");
                            });
                        },
                        function (error) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportError("update", error);
                            });
                        });
                }
            } else {
                if (index + 1 == vm.pageSize) {
                    taxonomyContentController.paginationGetByTaxonomyId({
                        taxonomyId: vm.taxonomy.Id,
                        lcid: vm.lcid,
                        currentPageIndex: (vm.currentPage + 1) * vm.pageSize,
                        maxNumberOfRows: 1,
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                        function (data) {
                            $scope.$apply(function () {
                                var taxonomyCopy = angular.copy(vm.taxonomy);
                                taxonomyCopy.Contents.splice(index, 0, data.Items[0]);
                                vm.disableUntilFinishedChange = true;
                                taxonomyContentController.update(
                                    taxonomyCopy,
                                    vm.currentPage * vm.pageSize,
                                    function (data) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            vm.currentPage++;
                                            $mdFeedbackService.reportInfo("update");
                                        });
                                    }, function (error) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            $mdFeedbackService.reportError("update", error);
                                        });
                                    });
                            });
                        }, function (error) {

                        })
                } else {
                    vm.taxonomy.Items.move(index, index + 1);
                    vm.disableUntilFinishedChange = true;
                    taxonomyContentController.update(vm.taxonomy, vm.currentPage * vm.pageSize,
                        function (data) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportInfo("update");
                            });
                        }, function (error) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportError("update", error);
                            });
                        });
                }
            }
        }

        function orderUp(index) {
            if (vm.currentTab == 0) {
                if (index == 0 && vm.currentPage > 0) {
                    taxonomyController.GetByParentIdWithPagination({
                        path: vm.taxonomy.Id,
                        pageIndex: ((vm.currentPage - 1) * vm.pageSize) + vm.pageSize - 1,
                        pageSize: 1,
                        sortString: vm.sortingString,
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                        function (data) {
                            $scope.$apply(function () {
                                var taxonomyCopy = angular.copy(vm.taxonomy);
                                taxonomyCopy.Children.splice(index + 1, 0, data.Items[0]);
                                vm.disableUntilFinishedChange = true;
                                taxonomyController.updateChildren(
                                    taxonomyCopy,
                                    vm.currentPage * vm.pageSize - 1,
                                    function (data) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            vm.currentPage--;
                                            $mdFeedbackService.reportInfo("update");
                                        });
                                    },
                                    function (error) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            $mdFeedbackService.reportError("update", error);
                                        });
                                    });
                            });
                        }, function (error) {
                        });
                } else {
                    vm.taxonomy.Children.move(index, index - 1);
                    vm.disableUntilFinishedChange = true;
                    taxonomyController.updateChildren(
                        vm.taxonomy,
                        vm.currentPage * vm.pageSize,
                        function (data) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportInfo("update");
                            });
                        },
                        function (error) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportError("update", error);
                            });
                        });
                }
            } else {
                if (index == 0 && vm.currentPage > 0) {
                    taxonomyContentController.paginationGetByTaxonomyId({
                        taxonomyId: vm.taxonomy.Id,
                        lcid: vm.lcid,
                        currentPageIndex: ((vm.currentPage - 1) * vm.pageSize) + vm.pageSize - 1,
                        maxNumberOfRows: 1,
                        sort: vm.sortingString,
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                        function (data) {
                            $scope.$apply(function () {
                                var taxonomyCopy = angular.copy(vm.taxonomy);
                                taxonomyCopy.Contents.splice(index + 1, 0, data.Items[0]);
                                vm.disableUntilFinishedChange = true;
                                taxonomyContentController.update(
                                    taxonomyCopy,
                                    vm.currentPage * vm.pageSize - 1,
                                    function (data) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            vm.currentPage--;
                                            $mdFeedbackService.reportInfo("update");
                                        });
                                    }, function (error) {
                                        $scope.$apply(function () {
                                            vm.disableUntilFinishedChange = false;
                                            $mdFeedbackService.reportError("update", error);
                                        });
                                    });
                            });
                        }, function (error) {

                        })
                } else {
                    vm.taxonomy.Items.move(index, index - 1);
                    vm.disableUntilFinishedChange = true;
                    taxonomyContentController.update(vm.taxonomy, vm.currentPage * vm.pageSize,
                        function (data) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportInfo("update");
                            });
                        }, function (error) {
                            $scope.$apply(function () {
                                vm.disableUntilFinishedChange = false;
                                $mdFeedbackService.reportError("update", error);
                            });
                        });
                }
            }
        }

        function getNumberOfTaxonomiesToDisplay() {
            taxonomyController.getByParentIdCount(
                {
                    taxonomyId: vm.taxonomy.Id,
                    lcid: vm.lcid,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfTaxonomies = data;
                        vm.totalItems = vm.totalNumberOfTaxonomies;
                    });
                }, function (error) {
                });
        }

        function getNumberOfTaxonomyContentsToDisplay() {
            taxonomyContentController.getByTaxonomyIdCount(
                {
                    taxonomyId: vm.taxonomy.Id,
                    lcid: vm.lcid,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfTaxonomiesContents = data;
                        vm.totalItems = vm.totalNumberOfTaxonomiesContents;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                })
        }

        function getTaxonomyContents() {
            taxonomyContentController.paginationGetByTaxonomyId({
                taxonomyId: vm.taxonomy.Id,
                lcid: vm.lcid,
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize,
                sort: vm.sortingString,
                searchTerm: encodeURI(vm.searchTerm)
            },
                function (data) {
                    $scope.$apply(function () {
                        vm.taxonomy.Items = data.Items;
                        vm.taxonomy.ItemsTotalCount = data.TotalCount;
                        vm.totalItems = data.TotalCount;
                        vm.searchGotResponse = true;
                        vm.hasContents = checkContents();
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                })

        }

        function getTaxonomies() {
            taxonomyController.GetByParentIdWithPagination({
                parentId: vm.taxonomy.Id,
                pageIndex: vm.currentPage,
                pageSize: vm.pageSize,
                searchTerm: encodeURI(vm.searchTerm)
            },
                function (data) {
                    $scope.$apply(function () {
                        vm.taxonomy.Children = data.Items;
                        vm.taxonomy.ChildrenTotalCount = data.TotalCount;
                        vm.totalItems = data.TotalCount;
                        vm.searchGotResponse = true;
                        vm.hasChildren = checkChildren();
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                });
        }

        function changeTab() {
            vm.currentPage = 0;
            if (vm.searchTerm.length) {
                vm.searchTerm = "";
                if (vm.currentTab == 0) {
                    getTaxonomyContents();
                } else if (vm.currentTab == 1) {
                    getTaxonomies();
                }
            }
            if (vm.currentTab == 0) {
                vm.showOrder = true;
                vm.toggleView('grid');
                vm.displayTaxonomies = 1;
                vm.displayTaxonomyContents = 0;
                vm.totalItems = vm.taxonomy.ChildrenTotalCount
            } else if (vm.currentTab == 1) {
                vm.toggleView('list');
                vm.displayTaxonomies = 0;
                vm.displayTaxonomyContents = 1;
                vm.selectTaxonomy(vm.taxonomy);
                vm.totalItems = vm.taxonomy.ItemsTotalCount
            }
        }
    }
})();
