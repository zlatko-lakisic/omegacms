(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsDiagramToolbar', ['$q', 'mdFeedbackService', '$compile', '$timeout', mdCmsDiagramToolbar]);

    function mdCmsDiagramToolbar($q, $mdFeedbackService, $compile, $timeout) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-diagram-toolbar/md-cms-diagram-toolbar.template.html',
            transclude: true,
            scope: {
                mdTitle: '@',
                uniqueId: '@',
                parent: '@',
                ngClass: '='
            },
            controller: 'mdCmsDiagramToolbarController as vm',
            link: function (scope, element, attributes) {
                if (scope.uniqueId === undefined) {
                    scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                }
                angular.element(element).attr('id', scope.uniqueId);

                attributes.$addClass('md-cms-diagram-toolbar');

                if (scope.parent !== undefined && scope.parent != null && scope.parent != '') {
                    $timeout(function () {
                        angular.element(element).detach().appendTo(scope.parent);
                    });
                }

                if (scope.ngClass !== undefined) {
                    if (typeof scope.ngClass === 'string' || scope.ngClass instanceof String) {
                        attributes.$addClass(scope.ngClass);
                    } else {
                        for (var cl in scope.ngClass) {
                            attributes.$addClass(scope.ngClass[cl]);
                        }
                    }
                }
            }
        }
    }
})();
