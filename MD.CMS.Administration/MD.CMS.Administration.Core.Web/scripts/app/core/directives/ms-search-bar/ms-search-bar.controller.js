(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('SearchController', ['$state', '$rootScope', SearchBarController]);


    /** @ngInject */
    function SearchBarController($state, $rootScope) {
        var vm = this;
        var cultureController = new mdBusinessLogic.dataAccess.controllers.cultureController();

        vm.showButton = $rootScope.globalSearchTerm !== undefined;      
        vm.searchCms = searchCms;
        vm.showSearchButton = showSearchButton;
        vm.hideSearchButton = hideSearchButton;
        vm.searchTerm = $rootScope.globalSearchTerm;

        $rootScope.$watch('globalSearchTerm', function (oldValue, newValue) {
            vm.searchTerm = newValue;
            vm.showButton = vm.searchTerm !== undefined;
        });

        function searchCms(searchTerm) {
            $state.go('app.search_results', { searchTerm: searchTerm });
        }

        function showSearchButton() {
            vm.showButton = true;
        }

        function hideSearchButton() {
            vm.showButton = false;
        }

        $("#ms-search-bar-input").keyup(function (event) {
            if (event.keyCode == 13) {
                searchCms(vm.searchTerm);
            }
        });
    }
})();