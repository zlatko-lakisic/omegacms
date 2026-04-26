(function () {
    'use strict';

    angular
        .module('app.menu.list')
        .controller('MenuListController', ['$state', '$rootScope', '$scope', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'mdCustomDialogs', 'menu', MenuListController]);


    /** @ngInject */
    function MenuListController($state, $rootScope, $scope, $mdSidenav, $mdDialog, $mdFeedbackService, dialog, menu) {
        var vm = this;

        // Controllers
        var menuController = new mdBusinessLogic.dataAccess.controllers.menuController();
        var menuContentController = new mdBusinessLogic.dataAccess.controllers.menuContentController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();    

        // Vaiables
        vm.currentMenuPath = $state.params.menuPath || 'Root';
        vm.currentView = {
            Name: 'grid',
            Label: 'Labels.GridView',
            Icon: 'icon-view-headline'
        };
        var numberOfMenusToDisplay;
        var numberOfMenuContentsToDisplay;
        var initPager = true;
        vm.displayMenus = 1;
        vm.displayMenuContents = 0;
        vm.lcid = mdBusinessLogic.settings.lcid || 2057;
        vm.menu = menu;
        vm.selected = menu;
        var pathoffolder = $state.params.menuPath;
        var pathofparentfolder = pathoffolder.slice(0, pathoffolder.lastIndexOf("/"));
        vm.disableUntilFinishedChange = false;
        vm.redirectoContentLocation = redirectoContentLocation;
        vm.currentTab = 0;
        vm.currentPage = 0;
        vm.totalItems = menu.ChildrenTotalCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";
        vm.showOrder = true;
        vm.contentCount = 0;
        vm.se = "se";
        vm.me = "me";
        vm.is = false;
        vm.hasChildren = checkChildren();
        vm.hasContent = checkContents();
        vm.currentTab = vm.hasChildren || !vm.hasContent ? 0 : 1;

        // Methods
        vm.selectMenu = selectMenu;
        vm.selectContent = selectContent;
        vm.toggleMenuDetails = toggleMenuDetails;
        vm.toggleMenuContentDetails = toggleMenuContentDetails;
        vm.toggleSidenav = toggleSidenav;
        vm.toggleView = toggleView;
        vm.getSelectedType = getSelectedType;
        vm.selectChildren = selectChildren;
        vm.deleteItem = deleteItem;
        vm.goToForm = goToForm;
        vm.changeTab = changeTab;
        vm.orderDown = orderDown;
        vm.orderUp = orderUp;
        vm.showLocation = showLocation;
        vm.openMenu = openMenu;
        vm.checkContents = checkContents;
        vm.checkChildren = checkChildren;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openContent = openContent;
        vm.openMenuFolder = openMenuFolder;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            if (vm.currentTab == 0) {
                getMenus();
            } else if (vm.currentTab == 1) {
                getMenuContents();
            }
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            if (vm.currentTab === 0) {
                getMenus();
            } else {
                getMenuContents();
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
            return vm.menu.Children.length > 0 || ((vm.searchTerm.length != 0 || !vm.searchGotResponse) && vm.currentTab == 1);
        }

        function checkContents() {
            return vm.menu.Items.length > 0 || vm.contentCount > 0 || ((vm.searchTerm.length != 0 || !vm.searchGotResponse) && vm.currentTab == 2);
        }

        function sort(sortingString) {
            vm.sortingString = sortingString;
            if (sortingString != "Order ASC") {
                vm.showOrder = false;
            } else {
                vm.showOrder = true;
            }
            getMenuContents();
        }

        function openMenu($mdMenu, ev) {
            ev.stopImmediatePropagation();
            $mdMenu.open(ev);
        };

        function redirectoContentLocation() {
            if (vm.getSelectedType() == 2) {
                $state.go('app.content_list', {
                    folderPath: vm.selectedContent.MenuContentPath
                });
            }
        }

        function showLocation() {
            return (vm.selectedContent != null ? 2 : 1);
        }

        function goToForm(menuId) {
            $state.go('app.menu_forms', {
                menuId: menuId,
                menuPath: vm.menu.MenuPath
            });
        }
        function selectMenu(item) {
            vm.selectedContent = null;
            vm.selectedMenu = item;
            select(item);
        }
        function selectContent(item) {
            vm.selectedContent = item;
            select(item);
        }
        function select(item) {
            vm.selected = item;
        }

        function openMenuFolder(menu) {
            $state.go("app.menu_list", {menuPath:menu.MenuPath,currentView:vm.currentView.Name});
        }

        function openContent(content) {
            $state.go("app.menu_forms", { path: vm.menu.MenuPath, currentView: vm.currentView.Name, id: vm.selectedMenu.ParentId, action: 'edit', menuId: vm.selectedMenu.Id });
        }

        function toggleMenuDetails(item, event) {
            event.stopPropagation();
            selectMenu(item);
            toggleSidenav('details-sidenav');
        }

        function toggleMenuContentDetails(item, event) {
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
        function deleteItem(ev) {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));
            $mdDialog.show($mdDialog.confirm()
                                    .clickOutsideToClose(true)
                                    .title($rootScope.globals.resources.Titles.RemoveQuestion)
                                    .textContent($rootScope.globals.resources.Labels.RemoveAnswer)
                                    .ok($rootScope.globals.resources.Labels.Yes)
                                    .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                                        if (getSelectedType() === 2) {
                                            menuController.delContent(vm.selectedContent.Id, vm.currentMenuPath,
                                                function (data) {
                                                    $mdFeedbackService.reportInfo("delete");
                                                    for (var i = 0; i < vm.menu.Items.length; i++) {
                                                        if (vm.menu.Items[i].Id == vm.selected.Id) {
                                                            vm.menu.Items.splice(index, 1);
                                                            break;
                                                        }
                                                    }
                                                    vm.selectMenu(vm.menu);
                                                    getMenuContents();

                                                }, function (error) {
                                                    $mdFeedbackService.reportError("delete", error);
                                                });
                                        } else if (getSelectedType() === 1) {
                                            if (vm.selected.Id <= 1) {
                                                $mdFeedbackService.reportError("403");
                                            } else {
                                                menuController.del(vm.selectedMenu.Id, function (data) {
                                                    for (var i = 0; i < vm.menu.Children.length; i++) {
                                                        if (vm.menu.Children[i].Id == vm.selected.Id) {
                                                            vm.menu.Children.splice(i, 1);
                                                        }
                                                    }
                                                    $scope.$emit('LoadNav', {
                                                        action: 'remove',
                                                        type: mdBusinessLogic.dataAccess.entities.entitiesEnum.Menu,
                                                        value: angular.copy(vm.selectedMenu)
                                                    });
                                                    $mdFeedbackService.reportInfo("delete");
                                                }, function (error) {
                                                    $mdFeedbackService.reportError("delete", error);
                                                });
                                            }
                                        }
                                    });
        }

        function selectChildren(item) {
            menuController.getByParentId(item.Id, 1, function (data) {
                vm.menus = [];
                for (var i = 0, length = data.length; i < length; i++) {
                    vm.menus.push(data[i]);
                }
            });
            vm.selectedMenu = item;
            select(item);
        }

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

        // Function for order down for one place
        function orderDown(index) {
            if(vm.currentTab == 0){
                if (index + 1 == vm.pageSize) {
                    menuController.paginationGetMenuByPath({
                        path: vm.currentMenuPath,
                        pageIndex: (vm.currentPage + 1) * vm.pageSize,
                        pageSize: 1,
                        sortString: $SortService.getSortString(),
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                        function (data) {
                            $scope.$apply(function () {
                                var menuCopy = angular.copy(vm.menu);
                                menuCopy.Children.splice(index, 0, data.Children[0]);
                                vm.disableUntilFinishedChange = true;
                                menuController.updateChildren(
                                    menuCopy,
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
                            $mdFeedbackService.reportError("load", error);
                        });
                } else {
                    vm.menu.Children.move(index, index + 1);
                    vm.disableUntilFinishedChange = true;
                    menuController.updateChildren(
                        vm.menu,
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
            }else{
                if (index + 1 == vm.pageSize) {
                    menuContentController.paginationGetByMenuId({
                        menuId: vm.menu.Id,
                        lcid: vm.lcid,
                        currentPageIndex: (vm.currentPage + 1) * vm.pageSize,
                        maxNumberOfRows: 1,
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                     function (data) {
                         $scope.$apply(function () {
                             var menuCopy = angular.copy(vm.menu);
                             menuCopy.Contents.splice(index, 0, data[0]);
                             vm.disableUntilFinishedChange = true;
                             menuContentController.update(menuCopy, vm.currentPage * vm.pageSize,
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
                         $mdFeedbackService.reportError("load", error);
                     });
                } else {
                    vm.menu.Items.move(index, index + 1);
                    vm.disableUntilFinishedChange = true;
                    menuContentController.update(vm.menu, vm.currentPage * vm.pageSize,
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
                    menuController.paginationGetMenuByPath({
                        path: vm.currentMenuPath,
                        pageIndex: ((vm.currentPage - 1) * vm.pageSize) + vm.pageSize - 1,
                        pageSize: 1,
                        sortString: $SortService.getSortString(),
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                        function (data) {
                            $scope.$apply(function () {
                                var menuCopy = angular.copy(vm.menu);
                                menuCopy.Children.splice(index + 1, 0, data.Children[0]);
                                vm.disableUntilFinishedChange = true;
                                menuController.updateChildren(
                                    menuCopy,
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
                    vm.menu.Children.move(index, index - 1);
                    vm.disableUntilFinishedChange = true;
                    menuController.updateChildren(
                        vm.menu,
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
                    menuContentController.paginationGetByMenuId({
                        menuId: vm.menu.Id,
                        lcid: vm.lcid,
                        currentPageIndex: ((vm.currentPage - 1) * vm.pageSize) + vm.pageSize - 1,
                        maxNumberOfRows: 1,
                        searchTerm: encodeURI(vm.searchTerm)
                    },
                   function (data) {
                       $scope.$apply(function () {
                           var menuCopy = angular.copy(vm.menu);
                           menuCopy.Contents.splice(index + 1, 0, data[0]);
                           vm.disableUntilFinishedChange = true;
                           menuContentController.update(menuCopy, (vm.currentPage - 1) * vm.pageSize,
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
                       $mdFeedbackService.reportError("load", error);
                   })
                } else {
                    vm.menu.Items.move(index, index - 1);
                    vm.disableUntilFinishedChange = true;
                    menuContentController.update(vm.menu, vm.currentPage * vm.pageSize,
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

        function getNumberOfMenuContentsToDisplay() {
            menuContentController.getByMenuIdCount(
                {
                    menuId: vm.menu.Id,
                    lcid: vm.lcid,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfMenuContents = data;
                        vm.totalItems = vm.totalNumberOfMenuContents;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                })
        }

        function getNumberOfMenusToDisplay() {
            menuController.getByParentIdCount(
                {
                    menuId: vm.menu.Id,
                    lcid: vm.lcid,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfMenus = data;
                        vm.totalItems = vm.totalNumberOfMenus;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                });
        }

        function getMenus() {
                menuController.GetByParentIdWithPagination({
                    parentId: vm.menu.Id,
                    pageIndex: vm.currentPage,
                    pageSize: vm.pageSize,
                    sortString: vm.sortingString,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                   $scope.$apply(function () {
                       vm.menu.Children = data.Items;
                       vm.menu.ChildrenTotalCount = data.TotalCount;
                       vm.totalItems = data.TotalCount;
                       vm.searchGotResponse = true;
                       vm.hasChildren = vm.checkChildren();
                   })
               }, function (error) {
                   $mdFeedbackService.reportError("load", error);
               });
        }

        function getMenuContents() {
            menuContentController.paginationGetByMenuId({
                menuId: vm.menu.Id,
                lcid: vm.lcid,
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize,
                sort: vm.sortingString,
                searchTerm: encodeURI(vm.searchTerm)
            },
            function (data) {
                $scope.$apply(function () {
                    vm.menu.Items = data.Items;
                    vm.searchGotResponse = true;
                    vm.hasContent = vm.checkContents();
                    vm.totalItems = data.TotalCount;
                });
            }, function (error) {
                $mdFeedbackService.reportError("load", error);
            })
        }
         
        function changeTab() {
            if (vm.searchTerm.length) {
              vm.searchTerm = "";
              if (vm.currentTab == 0) {
                getMenuContents();
              } else if (vm.currentTab == 1) {
                getMenus();
              }
            }
            vm.currentPage = 0;
            if (vm.currentTab == 0) {
                vm.toggleView('grid');
                vm.displayMenus = 1;
                vm.displayMenuContents = 0;
                vm.totalItems = vm.menu.ChildrenTotalCount;
            } else if (vm.currentTab == 1) {
                vm.toggleView('list');
                vm.displayMenus = 0;
                vm.displayMenuContents = 1;
                vm.selectMenu(vm.menu);
                vm.totalItems = vm.menu.ItemsTotalCount;
            }
            vm.isenable = true;
        }
    }
})();
