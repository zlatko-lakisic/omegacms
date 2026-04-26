(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsGrid', ['$q', 'mdFeedbackService', '$compile', '$timeout', mdCmsGrid])
        .directive('mdCmsGridList', [mdCmsGridList]);

    function mdCmsGrid($q, $mdFeedbackService, $compile, $timeout) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-grid/md-cms-grid.template.html',
            transclude: {
                'list': 'mdCmsGridList',
                'toolbar': '?mdCmsGridToolbar'
            },
            scope: {
                mdOptions: '=?',
                mdTitle: '@',
                gridId: '@',
                mdOnTileEvent: "&?",
                registerRemoveEvent: "&?",
                registerEditEvent: '&?',
                registerLoadedEvent: '&?',
                registerUpdateTileDataModelEvent: '&?',
                mdHideToolbar: '=?',
                uniqueId: '@?',
                reinitEventName: '@?',
                mdGridTileCount: '=?',
                mdIsNested: '=?',
                styleDependencies: '=?',
                gridActions: '=?',
                defailtGridTileData: '=?'
            },
            controller: 'mdCmsGridController as vm',
            link: function (scope, element, attributes) {
                angular.element(element).attr('id', scope.uniqueId);

                if (scope.reinitEventName !== undefined && scope.reinitEventName == '') {
                    scope.reinitEventName = undefined;
                }

                attributes.$addClass('md-cms-grid');
                if (scope.mdHideToolbar === undefined) {
                    scope.mdHideToolbar = false;
                }

                if (scope.reinitEventName !== undefined) {
                    scope.$on(scope.reinitEventName, function () {
                        $timeout(function () {
                            element.ready(function () {
                                scope.$apply(function () {
                                    scope.initGrid(true);
                                });
                            });
                        }, 1000);
                    });
                }

                if (scope.mdIsNested === undefined) {
                    scope.mdIsNested = false;
                }

                angular.element(element).toggleClass('md-cms-grid-nested', scope.mdIsNested);
            }
        }
    }

    function mdCmsGridList() {
        return {
            restrict: 'EA',
            template: '<div class="md-cms-grid-list" layout="row" layout-wrap flex="100" ng-transclude></div>',
            transclude: true,
            scope: {
                mdHideTrash: '=?',
                layout: '@',
                layoutWrap: '@',
                layoutPadding: '@'
            },
            link: function (scope, element, attributes) {
                if (scope.mdHideTrash === undefined) {
                    scope.mdHideTrash = false;
                }

                scope.mdLayout = '';

                if (scope.layout !== undefined) {
                    scope.mdLayout = scope.layout.toLowerCase();
                }

                scope.$watch(function () {
                    return scope.layout;
                }, function (layout) {
                        if (layout !== undefined) {
                            scope.mdLayout = layout.toLowerCase();
                        }
                });
            }
        }
    }
})();
