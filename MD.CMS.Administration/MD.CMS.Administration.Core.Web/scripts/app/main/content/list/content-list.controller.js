(function () {
    'use strict';

    angular
        .module('app.content.list')
      .controller('ContentListController', ['$state', 'mdFeedbackService', '$rootScope', '$mdSidenav', '$mdDialog', '$scope', 'folder', 'mdCustomDialogs', ContentListController])
       

    /** @ngInject */
    function ContentListController($state, $mdFeedbackService, $rootScope, $mdSidenav, $mdDialog, $scope, folder, dialog) {
        var vm = this;

        // Controllers
        var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var contentTypeDefinitionController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController();     

        // Variables
        var initialLoad = true;
        vm.selectedtype = 1;
        vm.contentVersions = [];
        vm.selected = folder;
        vm.currentFolderPath = $state.params.folderPath || "Root";
        vm.currentView = {
            Name: 'grid',
            Label: 'Labels.GridView',
            Icon: 'icon-view-headline'
        };
        vm.displayFolders = 1;
        vm.displayContents = 0;
        vm.lcid = mdBusinessLogic.settings.lcid || 2057;
        vm.folder = folder;
        vm.contentTypes = vm.folder.ContentTypeDefinitions;
        vm.deleteEnabled = true;
        vm.searchTerm = "";
        vm.pagesBorder = 1;
        var pathoffolder = $state.params.folderPath;
        var pathofparentfolder = pathoffolder.slice(0, pathoffolder.lastIndexOf("/"));
        vm.currentPage = 0;
        vm.totalItems = folder.ChildrenCount;
        vm.pageSize = 10;
        vm.hasChildren = checkChildren();
        vm.hasContents = checkContents();
        vm.currentTab = vm.hasChildren || !vm.hasContents ? 0 : 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;

        // Methods
        vm.selectFolder = selectFolder;
        vm.selectContent = selectContent;
        vm.toggleContentDetails = toggleContentDetails;
        vm.toggleFolderDetails = toggleFolderDetails;
        vm.toggleSidenav = toggleSidenav;
        vm.toggleView = toggleView;
        vm.getSelectedType = getSelectedType;
        vm['delete'] = deleteItem;
        vm.goToForm = goToForm;
        vm.changeTab = changeTab;
        vm.validateEdit = validateEdit;
        vm.openMenu = openMenu;
        vm.checkContents = checkContents;
        vm.checkChildren = checkChildren;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openContent = openContent;
        vm.openFolder = openFolder;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            if (vm.currentTab == 0) {
                getFolders();
            } else if (vm.currentTab == 1) {
                getContents();
            }
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            if (vm.currentTab === 0) {
                getFolders();
            } else {
                getContents();
            }
            return true;
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
            return vm.folder.ChildrenTotalCount > 0 || ((vm.searchTerm || !vm.searchGotResponse) && vm.currentTab == 0);
        }

        function checkContents() {
          return vm.folder.ContentsTotalCount || ((vm.searchTerm || !vm.searchGotResponse) && vm.currentTab == 1);
        }

        function sort(sortingString) {
            vm.sortingString = sortingString;
            getContents();
        }

        function openMenu($mdOpenMenu, ev) {
            $mdOpenMenu(ev);
        };

        function validateEdit() {
            //when folder
            if (vm.selected.LCID == undefined) {
                folderController.paginationGetByFolderPath(
                    {
                        path: (vm.currentFolderPath == '') ? 'Root' : vm.selected.FolderPath,
                        pageIndex: vm.currentPage,
                        pageSize: vm.pageSize,
                        fillContents: true,
                        fillMediaContents: false,
                        searchTerm: encodeURI(vm.searchTerm)
                    }, function (data) {
                        $state.go('app.folder_forms', {
                            currentView: vm.currentView.Name,
                            path: vm.folder.FolderPath,
                            action: 'add',
                            folderId: vm.selected.Id,
                            id: vm.folder.ParentId
                        });
                    },
                   function (error) {
                       $mdFeedbackService.reportError("load", error);
                   });
            }
            else {
                if (vm.selectedContent != null) {
                    //when content
                    $state.go('app.content_form', {
                        currentView: vm.currentView.Name,
                        path: vm.folder.FolderPath,
                        action: 'edit',
                        folderId: vm.folder.Id,
                        id: vm.selectedContent.Id,
                        isDataBound: vm.selectedContent.IsDataBound,
                        contentTypeId: vm.selectedContent.ContentType ? vm.selectedContent.ContentType.Id : 0,
                        lcid: vm.lcid
                    });
                }
                else {
                    //folder else
                    folderController.paginationGetByFolderPath(
                        {
                            path: (vm.currentFolderPath == '') ? 'Root' : vm.folder.FolderPath,
                            pageIndex: vm.currentPage,
                            pageSize: vm.pageSize,
                            fillContents: true,
                            fillMediaContents: false,
                            searchTerm: encodeURI(vm.searchTerm)
                        }, function (data) {
                        $state.go('app.content_form', {
                            action: 'edit',
                            folderId: vm.folder.Id,
                            id: vm.selected.Id,
                            lcid: vm.selected.LCID,
                            path: vm.folder.FolderPath
                        });

                    }, function (error) {
                        $mdFeedbackService.reportError("load", error);
                    });
                }
            }
        }

        function goToForm(folderId) {
            $state.go('app.folder_forms', {
                folderId: folderId,
                folderPath: vm.folder.FolderPath
            });
        }

        function selectFolder(item) {
            vm.selectedContent = null;
            vm.selectedFolder = item;
            if (item.FolderPath != "Root") {
                vm.selectedtype = 2;
            }
            vm.folder.FolderPath = item.FolderPath;
            select(item);
        }

        function selectContent(item) {
            vm.selectedFolder = null;
            if (item.FolderPath != "Root")
                vm.selectedtype = 2;

            vm.selectedContent = item;
            select(item);
        }

        function select(item) {
            vm.selected = item;
        }

        function openFolder(folder) {
            folderController.getById(
                folder.Id,
                function (data) {
                    $state.go("app.content_list", { folderPath: folder.FolderPath, currentView: 'grid' });
                },
                function (error) {
                    $mdFeedbackService.reportError("auth", error);
                });
        }
                                                           
        function openContent(content) {
            validateEdit();
        }

        function toggleFolderDetails(item, event) {
            event.stopPropagation();
            selectFolder(item);
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
            } else if(view == 'grid' || view == undefined) {
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
                $mdDialog.show({
                    controller: ['$scope', '$mdDialog', function ($scope, $mdDialog) {
                        $scope.cancel = function () {
                            $mdDialog.cancel();
                        };

                        $scope.answer = function (answer) {
                            $mdDialog.hide(answer);
                        };
                    }],
                    templateUrl: 'scripts/app/main/content/list/views/content-delete-dialog.template.html',
                    clickOutsideToClose: true
                })
                            .then(function (answer) {
                                if (answer == "all") {
                                    contentController.deleteByAll(
                                        vm.selectedContent.Id,
                                        function (data) {
                                            $scope.$apply(function () {
                                                $mdFeedbackService.reportInfo("delete");
                                                for (var i = 0; i < vm.folder.Contents.length; i++) {
                                                    if (vm.folder.Contents[i].Id == vm.selected.Id) {
                                                        vm.folder.Contents.splice(i, 1);
                                                        break;
                                                    }
                                                }
                                                getContents();
                                                select(vm.folder);
                                                vm.selectedContent = null;
                                                vm.selected.FolderPath = pathoffolder;
                                                vm.selectedFolder = vm.folder;
                                                vm.selectedFolder.FolderPath = pathoffolder;
                                            });
                                        },
                                        function (error) {
                                            $mdFeedbackService.reportError("delete", error);
                                        });
                                } else if ("this") {
                                    contentController.del(
                                        vm.selectedContent.Id,
                                        function (data) {
                                            $scope.$apply(function () {
                                                $mdFeedbackService.reportInfo("delete");
                                                for (var i = 0; i < vm.folder.Contents.length; i++) {
                                                    if (vm.folder.Contents[i].Id == vm.selected.Id) {
                                                        vm.folder.Contents.splice(i, 1);
                                                        break;
                                                    }
                                                }
                                                getContents();
                                                select(vm.folder);
                                                vm.selectedContent = null;
                                                vm.selected.FolderPath = pathoffolder;
                                                vm.selectedFolder = vm.folder;
                                                vm.selectedFolder.FolderPath = pathoffolder;
                                            });
                                        },
                                        function (error) {
                                            $mdFeedbackService.reportError("delete", error);
                                        });
                                }
                            });
            }
            if (getSelectedType() === 1) {
                if (vm.selectedFolder.Id <= 1) {
                    $mdFeedbackService.reportError("403");
                } else {
                    //TODO: check if vm.selectedFolder.Contents.length is always 0
                    $mdDialog.show($mdDialog.confirm()
                                                .clickOutsideToClose(true)
                                                .title($rootScope.globals.resources.Titles.RemoveQuestion)
                                                .textContent(
                                                    (vm.selectedFolder.Contents.length > 0 || vm.selectedFolder.Children.length > 0)
                                                    ? $rootScope.globals.resources.Labels.DeleteFolderWithContents
                                                    : $rootScope.globals.resources.Labels.RemoveAnswer)
                                                .ok($rootScope.globals.resources.Labels.Yes)
                                                .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                                                    folderController.del(
                                                        vm.selectedFolder.Id,
                                                        function (data) {
                                                            $scope.$apply(function () {
                                                                $mdFeedbackService.reportInfo("delete");
                                                                for (var i = 0; i < vm.folder.Children.length; i++) {
                                                                    if (vm.folder.Children[i].Id == vm.selected.Id) {
                                                                        $scope.$emit('LoadNav', {
                                                                            action: 'remove',
                                                                            type: mdBusinessLogic.dataAccess.entities.entitiesEnum.Content,
                                                                            value: angular.copy(vm.folder.Children[i])
                                                                        });
                                                                        vm.folder.Children.splice(i, 1);
                                                                        break;
                                                                    }
                                                                }
                                                            });
                                                        },
                                                        function (error) {
                                                            $mdFeedbackService.reportError("delete", error);
                                                        });
                                                });
                    }
                }
        }

        vm.disableDeleteBtn = disableDeleteBtn;
        function disableDeleteBtn() {
            vm.selectedtype = 1;
            if (vm.selectedFolder != undefined)
                vm.selectedFolder.Id = vm.folder.Id;
            vm.deleteEnabled = true;
        }

        function getNumberOfContentsToDisplay() {
            contentController.getByFolderIdCount(
                {
                    folderId: vm.folder.Id || 0, 
                    lcid: vm.lcid,
                    searchTerm: vm.searchTerm
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfContents = data;
                        vm.totalItems = vm.totalNumberOfContents;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error, true);
                });
        }

        function getNumberOfFoldersToDisplay() {
            folderController.getByParentIdCount(
                    {
                        folderId: vm.folder.Id || 0,
                        searchTerm: vm.searchTerm
                    },
                    function (data) {
                        $scope.$apply(function () {
                            vm.totalNumberOfFolders = data.data;
                            vm.totalItems = vm.totalNumberOfFolders;
                        });
                    }, function (error) {
                        $mdFeedbackService.reportError("load", error, true);
                    });
        }

        function getFolders() {
            folderController.paginationGetByParentId(
                {
                    parentId: vm.folder.Id,
                    pageIndex: vm.currentPage,
                    pageSize: vm.pageSize,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                  $scope.$apply(function () {
                    vm.folder.Children = data.Items;
                    vm.folder.ChildrentTotalCount = data.TotalCount;
                    vm.totalItems = data.TotalCount;
                    vm.searchGotResponse = true;
                    vm.hasChildren = checkChildren();
                  })
                }, function (error) {
                    $mdFeedbackService.reportError("load", error, true);
                });
        }

        function getContents() {
            var contentTypes = vm.contentTypes.filter(function (type) {
                return type.Fields.filter(function (field) {
                    return field.DataBound;
                }).length > 0;
            });

            contentController.paginationGetByFolderId({
                folderId: vm.folder.Id || 0,
                lcid: vm.lcid,
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize,
                sort: vm.sortingString,
                searchTerm: encodeURI(vm.searchTerm),
                contentTypeDefinitionId: contentTypes.length > 0 ? contentTypes[0].Id : 0
            },
            function (data) {
                $scope.$apply(function () {
                  vm.folder.Contents = data.Items;
                  vm.folder.ContentsTotalCount = data.TotalCount;
                  vm.totalItems = data.TotalCount;
                  vm.searchGotResponse = true;
                  vm.hasContents = checkContents();
                });
            }, function (error) {
                $mdFeedbackService.reportError("load", error, true);
            })

        }
       
        function changeTab() {
            vm.currentPage = 0;
            if (vm.searchTerm.length) {
              vm.searchTerm = "";
              if (vm.currentTab == 0) {
                getContents();
              } else if (vm.currentTab == 1) {
                getFolders();
              }
            }
            if (vm.currentTab == 0) {
                vm.toggleView('grid');
                vm.displayFolders = 1;
                vm.displayContents = 0;
                vm.totalItems = vm.folder.ChildrenTotalCount;
            } else if (vm.currentTab == 1) {
                vm.toggleView('list');
                vm.displayFolders = 0;
                vm.displayContents = 1;
                vm.selectFolder(vm.folder);
                vm.totalItems = vm.folder.ContentsTotalCount;
            }
        }
    }
})();
