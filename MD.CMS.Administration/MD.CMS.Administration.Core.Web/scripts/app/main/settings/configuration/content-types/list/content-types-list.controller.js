(function () {
    'use strict';

    angular
        .module('app.settings.configuration.content-types-list')
      .controller('ContentTypesListController', ['$rootScope', '$mdSidenav', '$scope', '$mdDialog', '$state', 'mdFeedbackService', 'contentTypes', ContentTypesListController]);

    /** @ngInject */
    function ContentTypesListController($rootScope, $mdSidenav, $scope, $mdDialog, $state, mdFeedbackService, contentTypes) {
        var vm = this;

        // Controllers
        var contentTypeDefinitionsController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        // Variables
        vm.contentTypes = contentTypes.Items;
        vm.totalNumberOfPages;
        var totalNumberOfUsers;
        var maxNumberOfRows = 20;
        vm.pages = [];
        vm.disablebtn = true;
        var stateToGo = 'app.content-types-list';
        vm.typeOfForm = $state.params.form || 'contentTypeForm';
        vm.numberOfSearchResults = 0;
        vm.currentView = {
            Name: 'list',
            Label: 'Labels.ListView',
            Icon: 'icon-view-module'
        };
        vm.currentPage = 0;
        vm.totalItems = contentTypes.TotalCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";
        vm.searchColumn = "All";

        // Methods
        vm.deleteItem = deleteItem;
        vm.select = select;
        vm.toggleView = toggleView;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openContentType = openContentType;
        vm.toggleContentTypeDetails = toggleContentTypeDetails;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getContentTypes();
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            getContentTypes();
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

        function sort(sortingString) {
            vm.sortingString = sortingString;
            getContentTypes();
        }

        function openMenu($mdOpenMenu, ev) {
            $mdOpenMenu(ev);
        };

        function openMenu($mdOpenMenu, ev) {
            ev.stopImmediatePropagation();
            $mdOpenMenu(ev);
        };

        function select(contentType, $index) {
            vm.selected = contentType;
            vm.index = $index;
            vm.disablebtn = false;
        }

        function openContentType(contentType) {
            $state.go("app.content-types-edit", {id:contentType.Id, currentView:'edit'});
        }

        function toggleContentTypeDetails(item, event) {
            event.stopPropagation();
            select(item)
            toggleSidenav('details-sidenav');
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
            dialog.showConfirmDialog(
                $rootScope.globals.resources.Titles.RemoveQuestion,
                $rootScope.globals.resources.Labels.RemoveAnswer,
                $rootScope.globals.resources.Labels.Yes,
                $rootScope.globals.resources.Labels.No,
                function () {
                    contentController.selectByContentTypeDefinitionCount(vm.selected.Id,
                       function (data) {
                           var contentCount = data;
                           if (contentCount > 0) {
                               dialog.showSimpleDialog($rootScope.globals.resources.Titles.ActionNotCompleted, $rootScope.globals.resources.Labels.ThereAreXContentsWithContentType);
                           } else {
                               contentTypeDefinitionsController.del(vm.selected.Id,
                                   function () {
                                       mdFeedbackService.reportInfo("delete");
                                       vm.contentTypes.splice(vm.index, 1);
                                       getContentTypes();
                                   },
                                   function (error) {
                                       mdFeedbackService.reportError("delete", error);
                                   });
                           }
                       },
                       function (error) {
                           mdFeedbackService.reportError("load", error);
                       });
                },
                function () {
                });
        }

        function getNumberOfContentTypes() {
            contentTypeDefinitionsController.getAllCount(
                {
                    searchTerm: encodeURI(vm.searchTerm),
                    searchColumn: encodeURI(vm.searchColumn)
                },
                function (data) {
                    vm.totalNumberOfContentTypes = data;
                    vm.totalItems = data;
                },
                function (error) {
                    mdFeedbackService.reportError("load", error);
                });
        }

        function getContentTypes() {
            contentTypeDefinitionsController.paginationGetAll({
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize,
                sort: vm.sortingString,
                searchTerm: encodeURI(vm.searchTerm),
                searchColumn: encodeURI(vm.searchColumn)
            }, function (data) {
                $scope.$apply(function () {
                    vm.contentTypes = data.Items;
                    vm.totalItems = data.TotalCount;
                    vm.searchGotResponse = true;
                });
            }, function (error) {
                vm.searchGotResponse = true;
                mdFeedbackService.reportError("load", error);
            });
        }
    }
})();
