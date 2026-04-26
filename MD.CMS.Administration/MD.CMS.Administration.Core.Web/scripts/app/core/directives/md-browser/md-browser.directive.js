(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdBrowser', ['$rootScope', mdBrowserDirective]);
    /** @ngInject */
    function mdBrowserDirective($rootScope) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-browser/md-browser.template.html',
            scope: {
                config: "="
            },
            link: function (scope, element, attrs) {
                scope.group = {};
                scope.item = {};
                scope.currentView = {};
                scope.ParentGroupPaths = [];
                scope.currentPage = 0;
                scope.totalItems = 0;
                scope.pageSize = 0;
                scope.showSearch = false;
                scope.searchTerm = "";
                scope.pagesBorder = 1;

                scope.toggleView = function () {
                    if ((scope.currentView.Name == null && scope.currentView.Name == undefined) || scope.currentView.Name == 'grid') {
                        scope.currentView = {
                            Name: 'list',
                            Label: 'Labels.ListView',
                            Icon: 'icon-view-module'
                        };
                    } else {
                        scope.currentView = {
                            Name: 'grid',
                            Label: 'Labels.GridView',
                            Icon: 'icon-view-headline'
                        };
                    }
                }
                scope.toggleView();

                scope.toggleSearchBar = function () {
                    scope.showSearch = !scope.showSearch;
                    if (!scope.showSearch) {
                        scope.searchTerm = "";
                        scope.search();
                    }
                }

                scope.updatePager = function(currentPage, pageSize, pagesBorder) {
                    scope.currentPage = currentPage;
                    scope.pageSize = pageSize;
                    scope.pagesBorder = pagesBorder;
                    if (scope.currentTab == 0) {
                        scope.getGroups();
                    } else {
                        scope.getItems();
                    }
                }

                scope.getGroupPath = function () {

                }

                scope.getViewPath = function () {
                }

                scope.changeTab = function () {

                }

                scope.search = function () {

                }

                scope.getGroups = function () {
                    scope.config.getGroups({
                        currentPage: scope.currentPage,
                        pageSize: scope.pageSize,
                        searchTerm: scope.searchTerm
                    },
                    function (group, children) {
                        scope.group = group;
                    },
                    function (error) {
                    });
                }

                scope.getItems = function () {
                }
            }
        };
    }
})();