(function () {
    'use strict';

    angular
        .module('app.mediacontent.list')
        .controller('MediaContentListController', ['$state', '$rootScope', '$mdSidenav', '$mdDialog', '$scope', 'mdFeedbackService', 'mdCustomDialogs', 'folder', MediaContentListController]);

    function MediaContentListController($state, $rootScope, $mdSidenav, $mdDialog, $scope, $mdFeedbackService, dialog, folder) {

        var vm = this;

        // Controllers
        var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
        var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();

        // Variables
        vm.currentFolderPath = $state.params.folderPath || "Root";
        vm.currentView = {
            Name: 'grid',
            Label: 'Labels.GridView',
            Icon: 'icon-view-headline'
        };
        var numberOfFoldersToDisplay;
        var numberOfMediaContentsToDisplay;
        var pagerInit = true;
        var initialLoad = true;
        vm.displayFolders = 1;
        vm.displayMediaContents = 0;
        vm.lcid = mdBusinessLogic.settings.lcid || 2057;
        vm.folder = folder;
        vm.selected = folder;
        vm.hidePreviewUrl = true;
        var pathoffolder = $state.params.folderPath;
        var pathofparentfolder = pathoffolder.slice(0, pathoffolder.lastIndexOf("/"));
        vm.currentPage = 0;
        vm.totalItems = vm.folder.ChildrenTotalCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.searchTerm = "";
        vm.hasChildren = checkChildren();
        vm.hasContents = checkContents();
        vm.currentTab = vm.hasChildren || !vm.hasContents ? 0 : 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.disableBtn = true;
        vm.contentType = mdBusinessLogic.dataAccess.entities.mediaContentInputType;
        vm.uploadsBase = mdBusinessLogic.settings.uploadsBase;

        // Methods
        vm.selectFolder = selectFolder;
        vm.selectContent = selectContent;
        vm.toggleContentDetails = toggleContentDetails;
        vm.togglefolderdetails = togglefolderdetails;
        vm.toggleSidenav = toggleSidenav;
        vm.toggleView = toggleView;
        vm.getSelectedType = getSelectedType;
        vm.deleteItem = deleteItem;
        vm.goToForm = goToForm;
        vm.validateEdit = validateEdit;
        vm.changeTab = changeTab;
        vm.checkContents = checkContents;
        vm.checkChildren = checkChildren;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openContent = openContent;
        vm.openFolder = openFolder;

        vm.getIcon = getIcon;

        function getIcon(inputType) {
            var result = 'file';
            switch (inputType) {
                case 1:
                case 4:
                case 5:
                case 6:
                case 9:
                case 10:
                    result = 'file-image';
                    break;
                case 3:
                case 7:
                case 8:
                    result = 'file-video';
                    break;
            }
            return result;
        }


        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            if (vm.currentTab == 0) {
                getFolders();
            } else if (vm.currentTab == 1) {
                getMediaContents();
            }
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            if (vm.currentTab === 0) {
                getFolders();
            } else {
                getMediaContents();
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

        function sort(sortingString) {
            vm.sortingString = sortingString;
            getMediaContents();
        }

        function checkChildren() {
            return vm.folder.ChildrenTotalCount || ((vm.searchTerm || !vm.searchGotResponse) && vm.currentTab == 0);
        }

        function checkContents() {
            return vm.folder.MediaContentTotalCount || ((vm.searchTerm || !vm.searchGotResponse) && vm.currentTab == 1);
        }

        function goToForm(folderId) {
            $state.go('app.mediacontent_form', {
                folderId: folderId,
                folderPath: vm.folder.FolderPath
            });
        }

        function validateEdit() {
            mediaContentController.getById(vm.selectMediaContent.Id, vm.lcid, function (data) {
                $state.go('app.mediacontent_form', {
                    path: vm.folder.FolderPath,
                    action: 'edit',
                    folderId: vm.selected.FolderId,
                    id: vm.selected.Id,
                    currentView: vm.currentView,
                    fileType: vm.selectMediaContent.FileType
                });
            },function (error) {
                $mdFeedbackService.reportError('load', error);
            });

        }

        function selectFolder(item) {
            vm.selectedFolder = item;
            vm.selectMediaContent == null;
            //disable button for edit and delete
            vm.disableBtn = true;
            select(item);
        }

        function selectContent(item) {
            vm.selectedFolder = null;
            vm.selectMediaContent = item;
            //enable button for edit and delete
            vm.disableBtn = false;
            select(item);
        }

        function select(item) {
            vm.selected = item;
        }

        function openFolder(folder) {
            folderController.getById(
                folder.Id,
                function (data) {
                    $state.go("app.mediacontent_list", { folderPath: folder.FolderPath, currentView: 'grid' });
                },
                function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function openContent(content) {
            validateEdit();
        }

        function toggleContentDetails(item, event) {
            event.stopPropagation();
            selectContent(item);
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function getSelectedType() {

            return (vm.selectMediaContent != null ? 2 : 1);

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

        function togglefolderdetails(item, event) {
            event.stopPropagation();
            selectFolder(item);
            toggleSidenav('details-sidenav');
        }

        //edin
        function deleteItem(ev) {
            $mdDialog.show($mdDialog.confirm()
                .clickOutsideToClose(true)
                .title($rootScope.globals.resources.Titles.RemoveQuestion)
                .textContent($rootScope.globals.resources.Labels.RemoveAnswer)
                .ok($rootScope.globals.resources.Labels.Yes)
                .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                    mediaContentController.del(vm.selectMediaContent.Id, function (data) {
                        getMediaContents();
                        selectFolder(vm.folder);
                    }, function (error) {
                        $mdFeedbackService.reportError("delete", error);
                    });
                });
        }

        function getNumberOfMediaContentsToDisplay() {
            mediaContentController.getByFolderIdCount(
                {
                    folderId: vm.folder.Id,
                    lcid: vm.lcid,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfMediaContents = data;
                        vm.totalItems = vm.totalNumberOfMediaContents;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                });
        }

        function getNumberOfFoldersToDisplay() {
            folderController.getByParentIdCount(
                {
                    folderId: vm.folder.Id,
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfFolders = data.data;
                        vm.totalItems = vm.totalNumberOfFolders;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
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
                        vm.folder.ChildrenTotalCount = data.TotalCount;
                        vm.totalItems = data.TotalCount;
                        vm.searchGotResponse = true;
                        vm.hasChildren = vm.checkChildren();
                        vm.contentTypes = vm.folder.ContentTypeDefinitions;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                });
        }

        function getMediaContents() {
            mediaContentController.paginationGetByFolderId({
                folderId: vm.folder.Id,
                lcid: vm.lcid,
                pageIndex: vm.currentPage,
                pageSize: vm.pageSize,
                sort: vm.sortingString,
                searchTerm: encodeURI(vm.searchTerm)
            },
                function (data) {
                    $scope.$apply(function () {
                        vm.folder.MediaContent = data.Items;
                        vm.folder.MediaContentTotalCount = data.TotalCount;
                        vm.totalItems = data.TotalCount;
                        vm.searchGotResponse = true;
                        vm.hasContents = vm.checkContents();
                        for (var i in vm.folder.MediaContent) {
                            vm.folder.MediaContent[i].PreviewUrl = mdBusinessLogic.settings.uploadsBase + vm.folder.MediaContent[i].FullNameFile;
                            switch (vm.folder.MediaContent[i].FileType) {
                                case 1:
                                    vm.folder.MediaContent[i].Type = 'Image';
                                    break;
                                case 2:
                                    vm.folder.MediaContent[i].Type = 'Video';
                                    break;
                                case 3:
                                    vm.folder.MediaContent[i].Type = 'Audio';
                                    break;
                                case 4:
                                    vm.folder.MediaContent[i].Type = 'Document';
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                })
        }

        function changeTab() {
            if (vm.searchTerm.length) {
                vm.searchTerm = "";
                if (vm.currentTab == 0) {
                    getMediaContents();
                } else if (vm.currentTab == 1) {
                    getFolders();
                }
            }
            if (vm.currentTab == 0) {
                vm.toggleView('grid');
                vm.displayFolders = 1;
                vm.displayMediaContents = 0;
                vm.totalItems = vm.folder.ChildrenTotalCount;
            } else if (vm.currentTab == 1) {
                vm.toggleView('list');
                vm.displayFolders = 0;
                vm.displayMediaContents = 1;
                vm.selectFolder(vm.folder);
                vm.totalItems = vm.folder.MediaContentTotalCount;
            }
        }

    }
})();
