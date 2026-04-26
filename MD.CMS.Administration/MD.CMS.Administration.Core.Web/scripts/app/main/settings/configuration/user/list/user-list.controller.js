(function () {
    'use strict';
    angular
        .module('app.settings.configuration.user.list')
        .controller('UserListController', ['$scope', '$rootScope', '$state', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'users', 'profileTypes', UserListController]);


    /** @ngInject */
    function UserListController($scope, $rootScope, $state, $mdSidenav, $mdDialog, $mdFeedbackService, users, profileTypes) {
        var vm = this;

        // Controllers      
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);
        var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();

        // Variables
        vm.profileTypes = [];
        for (var i in profileTypes) {
          if (!mdBusinessLogic.helpers.checkType.isFunction(profileTypes[i])) {
                vm.profileTypes.push(profileTypes[i]);
            }
        }
        vm.users = users.Items;
        vm.selected;
        vm.disablebtn = true;
        vm.index;
        vm.currentView = {
            Name: 'list',
            Label: 'Labels.GridView',
            Icon: 'icon-view-headline'
        };
        var totalNumberOfUsers;
        vm.pages = [];
        vm.numberOfSearchResults = 0;
        vm.currentPage = 0;
        vm.totalItems = users.TotalCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";

        // Methods
        vm.toggleView = toggleView;
        vm.select = select;
        vm.deleteItem = deleteItem;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openUser = openUser;
        vm.toggleContentDetails = toggleContentDetails;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getResults();
        }

        function search(searchTerm) {
            vm.searchTerm = searchTerm;
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

        function toggleContentDetails(item, event) {
            event.stopPropagation();
            select(item);
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function select(user, $index) {
            vm.selected = user;
            vm.index = $index;
            vm.disablebtn = false;
        }

        function openUser(user) {
            $state.go("app.user_form", { id: user.Id, action: 'edit' });
        }

        function deleteItem() {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));

            if (vm.selected.Id > 1) {

                dialog.showConfirmDialog(
                    $rootScope.globals.resources.Titles.RemoveQuestion,
                    $rootScope.globals.resources.Labels.RemoveAnswer,
                    $rootScope.globals.resources.Labels.Yes,
                    $rootScope.globals.resources.Labels.No,
                    function () {
                        userController.del(vm.selected.Id,
                            function (data) {
                                $mdFeedbackService.reportInfo("deleted");
                                getResults();
                            }, function (error) {
                                $mdFeedbackService.reportError("delete", error);
                            });
                },
                function () {
                });
            }
        }

        function getTotalNumberOfResults() {
            userController.getAllCount(
                {
                    searchTerm: encodeURI(vm.searchTerm)
                },
                function (data) {
                    vm.totalNumberOfResults = data.data;
                    vm.totalItems = vm.totalNumberOfResults;
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                })
        }

        function getResults() {
            userController.paginationGetAll(
                {
                    sort: vm.sortingString,
                    currentPageIndex: vm.currentPage,
                    maxNumberOfRows: vm.pageSize,
                    searchTerm: encodeURI(vm.searchTerm)
                }, function (data) {
                    $scope.$apply(function () {
                        vm.users = data.Items;
                        vm.totalItems = data.TotalCount;
                        vm.searchGotResponse = true;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError("load", error);
                });
        }
    }
})();
