(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdContentHeader', ['$rootScope', mdPagerDirective]);
    /** @ngInject */
    function mdPagerDirective($rootScope) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-content-header/md-content-header.template.html',
            transclude: true,
            scope: {
                onSearch: "&?",
                hasSearchBox: "=?",
                isLarge: "=?"
            },
            link: function (scope, element, attrs) {
                scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                scope.isSearchVisible = false;
                scope.searchGotResponse = true;
                scope.data = {
                    searchTerm: ''
                };
                scope.hasSearchBox = scope.hasSearchBox === undefined ? true : scope.hasSearchBox;
                scope.isLarge = scope.isLarge === undefined ? false : scope.isLarge;

                scope.cancelSearch = function () {
                    scope.isSearchVisible = false;
                    scope.data.searchTerm = '';
                    scope.onSearch({ searchTerm: scope.data.searchTerm });
                }

                scope.search = function () {
                    scope.searchGotResponse = scope.onSearch({ searchTerm: scope.data.searchTerm });
                }

                scope.toggleSearch = function () {
                    scope.isSearchVisible = !scope.isSearchVisible;
                    if (!scope.isSearchVisible) {
                        scope.data.searchTerm = '';
                        scope.onSearch({ searchTerm: scope.data.searchTerm });
                    }
                }
            }
        };
    }
})();
