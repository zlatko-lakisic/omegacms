(function () {
    'use strict';

    angular
        .module('app.settings.configuration.profile-types-list')
        .controller('ProfileTypesListController', ['$scope', '$rootScope', '$state', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'profileTypes', 'profileTypesCount', ProfileTypesListController]);


    /** @ngInject */
    function ProfileTypesListController($scope, $rootScope, $state, $mdSidenav, $mdDialog, $mdFeedbackService, profileTypes, profileTypesCount) {

        var vm = this;

        // Controllers
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);
        var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();

        // Variables
        var initialLoad = true;
        vm.profileTypes = profileTypes;
        vm.selected;
        vm.disablebtn = true;
        vm.currentView = {
            Name: 'list',
            Label: 'Labels.GridView',
            Icon: 'icon-view-headline'
        };
        vm.index;
        vm.numberOfSearchResults = 0;
        vm.currentPage = 0;
        vm.totalItems = profileTypesCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";

        // Methods
        vm.select = select;
        vm.toggleView = toggleView;
        vm.deleteProfileType = deleteProfileType;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openProfileType = openProfileType;
        vm.toggleContentDetails = toggleContentDetails;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getProfileTypes();
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            getProfileTypes();
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
            if(!initialLoad){
                getProfileTypes();
            } else {
                getNumberOfProfileType();
            }
            initialLoad = false;
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

        function toggleContentDetails(item, event) {
            event.stopPropagation();
            select(item);
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function getNumberOfProfileType() {
            profileTypeController.getAllCount(
                {
                    searchTerm: vm.searchTerm
                }, function (data) {
                    $scope.$apply(function () {
                        vm.numberOfProfileTypes = data;
                        vm.totalItems = vm.numberOfProfileTypes;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function getProfileTypes() {
            profileTypeController.getAllWithPagination(
                {
                    pageIndex: vm.currentPage,
                    pageSize: vm.pageSize,
                    sort: vm.sortingString,
                    searchTerm: vm.searchTerm
                }, function (data) {
                    $scope.$apply(function () {
                        vm.profileTypes = data;
                        vm.searchGotResponse = true;
                        getNumberOfProfileType();
                    });
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function select(profileType) {
            vm.selected = profileType;
            vm.disablebtn = false;
        }

        function openProfileType(profileType) {
            $state.go("app.profile-types-form", { id: profileType.Id, currentView: 'edit' });
        }

        function deleteProfileType(profileType, index) {
            select(profileType);
            if (vm.selected.Id > 1) {
                dialog.showConfirmDialog(
                    $rootScope.globals.resources.Titles.RemoveQuestion,
                    $rootScope.globals.resources.Labels.RemoveAnswer,
                    $rootScope.globals.resources.Labels.Yes,
                    $rootScope.globals.resources.Labels.No,
                    function () {
                        profileTypeController.del(vm.selected.Id,
                            function (data) {
                                getProfileTypes();
                                $mdFeedbackService.reportInfo('delete');
                            }, function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    },
                    function () {
                    });
            }
        }
    }
})();
