(function () {
    'use strict';

    angular
        .module('app.searchResults.module')
        .controller('SearchResultsController', ['$state', '$location', 'searchResults', '$rootScope', SearchResultsController]);


    /** @ngInject */
    function SearchResultsController($state, $location, searchResults, $rootScope) {
        var vm = this;

        //variables
        vm.noResults = false;
        vm.searchResults = searchResults;
        vm.currentView = 'list'
        $rootScope.globalSearchTerm = $state.params.searchTerm;

        //methods      
        vm.select = select;
        vm.redirect = redirect;
        vm.changeIcon = changeIcon;
        vm.toggleView = toggleView;
        vm.headers = {};

        function isEmpty(obj) {
            var empty = true;
            for (var key in obj) {
                empty = false;
                break;
            }
            return empty;
        }

        function select(obj) {
            vm.selected = obj;
        }

        function redirect(table, obj) {
            switch (table) {
                case 'Folders':
                    $state.go("app.content_list", { folderPath: obj.Path });
                    break;
                case 'Taxonomies':
                    $state.go('app.taxonomy_list', { taxonomyPath: obj.Path });
                    break;
                case 'Menus':
                    $state.go('app.menu_list', { menuPath: obj.Path });
                    break;
                case 'Contents':
                    $state.go("app.content_form", { action: 'edit', path: obj.Path, folderId: obj.FolderId, id: obj.Id });
                    break;
                case 'Content types':
                    $state.go('app.content-types-form', { id: obj.Id });
                    break;
                case 'Users':
                    $state.go('app.user_form', { action: 'edit', id: obj.Id });
                    break;
                case 'Profile types':
                    $state.go('app.profile-types-form', { id: obj.Id });
                    break;
                case 'Media contents':
                    $state.go("app.mediacontent_form", { action: 'edit', path: obj.Path, folderId: obj.FolderId, id: obj.Id, fileType: obj.FileType });
                    break;
                default:
                    break;
            }
        }

        function changeIcon(table) {
            switch (table) {
                case 'Folders':
                    vm.icon = 'icon-folder';
                    break;
                case 'Taxonomies':
                    vm.icon = 'icon-bookmark';
                    break;
                case 'Menus':
                    vm.icon = 'icon-menu';
                    break;
                case 'Contents':
                    vm.icon = 'icon-document';
                    break;
                case 'Content types':
                    vm.icon = 'icon-document';
                    break;
                case 'Users':
                    vm.icon = 'icon-account-box-outline';
                    break;
                case 'Profile types':
                    vm.icon = 'icon-account';
                    break;
                case 'Media contents':
                    vm.icon = 'icon-document';
                    break;
                default:
                    vm.icon = 'icon-document';
                    break;
            }
        }

        function toggleView() {
            vm.currentView == 'list' ? vm.currentView = 'grid' : vm.currentView = 'list';
        }

        function isFunction(functionToCheck) {
            return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
        }

        function constructHeaders() {
            for (var key in vm.searchResults) {
                var headersArray = [];
                if (vm.searchResults[key].length) {
                    for (var header in vm.searchResults[key][0]) {
                        if (!isFunction(vm.searchResults[key][0][header]) && header != 'TableName') {
                            headersArray.push(header);
                        }
                    }
                }
                vm.headers[key] = headersArray;
            }
        }

        //executing
        if (isEmpty(vm.searchResults)) {
            vm.noResults = true;
        }
        changeIcon(Object.keys(vm.searchResults)[0]);
        constructHeaders();
    }
})();