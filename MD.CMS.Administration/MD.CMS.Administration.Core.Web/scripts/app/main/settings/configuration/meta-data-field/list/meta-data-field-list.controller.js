(function () {
    'use strict';

    angular
        .module('app.settings.configuration.meta-data-field-list')
        .controller('MetaDataFieldListController', ['$rootScope','$state', '$scope', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'metaDataFields', MetaDataFieldListController]);

    /** @ngInject */
    function MetaDataFieldListController($rootScope, $state, $scope, $mdSidenav, $mdDialog, $mdFeedbackService, metaDataFields) {
        var vm = this;

        // Controllers
        var metaDataFieldController = new mdBusinessLogic.dataAccess.controllers.metaDataFieldController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state)

        // Variables
        vm.allMetaDataFields = metaDataFields.Items;
        vm.selected;
        vm.index;     
        vm.disablebtn = true;
        vm.numberOfSearchResults = 0;
        vm.currentPage = 0;
        vm.totalItems = metaDataFields.TotalCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";
        vm.searchColumn = "All";

        // Methods
        vm.select = select;
        vm.deleteItem = deleteItem;
        vm.toggleView = toggleView;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openMetaDataField = openMetaDataField;
        vm.toggleContentDetails = toggleContentDetails;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getResults();
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchColumn = 'All'
            vm.searchGotResponse = false;
            getResults();
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
            getResults();
        }

        function openMenu($mdOpenMenu, ev) {
            ev.stopImmediatePropagation();
            $mdOpenMenu(ev);
        };

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

        function select(item, $index) {
            vm.selected = item;
            vm.index = $index;
            vm.disablebtn = false;
        }

        function openMetaDataField() {
            $state.go("app.meta-data-field-form", {id: vm.selected.Id});
        }

        function deleteItem() {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));
            dialog.showConfirmDialog(
                $rootScope.globals.resources.Titles.RemoveQuestion,
                $rootScope.globals.resources.Labels.RemoveAnswer,
                $rootScope.globals.resources.Labels.Yes,
                $rootScope.globals.resources.Labels.No,
                function () {
                    metaDataFieldController.del(vm.selected.Id,
                        function (data) {
                            $mdFeedbackService.reportInfo('delete');
                            $state.go($state.current, {}, { reload: true });
                        }, function (error) {
                            $mdFeedbackService.reportError('delete', error);
                        })
                },
                function () {
                });
        }

        function toggleContentDetails(item, event) {
            event.stopPropagation();
            select(item);
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function getTotalNumberOfResults() {
            metaDataFieldController.getAllCount(
                {
                    searchTerm: encodeURI(vm.searchTerm),
                    searchColumn: encodeURI(vm.searchColumn)
                },
                function (data) {
                    vm.totalNumberOfResults = data;
                    vm.totalItems = vm.totalNumberOfResults;
                },
                function (error) {
                    vm.searchGotResponse = true;
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function getResults() {
            metaDataFieldController.paginationGetAll({
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize,
                sort: vm.sortingString,
                searchTerm: encodeURI(vm.searchTerm),
                searchColumn: encodeURI(vm.searchColumn)
            }, function (data) {
                $scope.$apply(function () {
                    vm.allMetaDataFields = data.Items;
                    vm.totalItems = data.TotalCount;
                    vm.searchGotResponse = true;
                });
            }, function (error) {
                vm.searchGotResponse = true;
                $mdFeedbackService.reportError('load', error);
            });
        }

    }
})();
