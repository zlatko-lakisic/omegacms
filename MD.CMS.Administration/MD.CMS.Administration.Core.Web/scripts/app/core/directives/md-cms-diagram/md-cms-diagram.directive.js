(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsDiagram', ['$q', 'mdFeedbackService', '$compile', '$timeout', mdCmsDiagram])
        .directive('mdCmsDiagramCanvas', [mdCmsDiagramCanvas]);

    function mdCmsDiagram($q, $mdFeedbackService, $compile, $timeout) {
        return {
            restrict: 'E',
            templateUrl: 'scripts/app/core/directives/md-cms-diagram/md-cms-diagram.template.html',
            transclude: {
                'toolbar': '?mdCmsDiagramToolbar',
                'canvas': '?mdCmsDiagramCanvas'
            },
            scope: {
                mdOnTileEvent: "&?",
            },
            controller: 'mdCmsDiagramController as vm',
            link: function (scope, element, attributes) {
                angular.element(element).attr('id', scope.uniqueId);
            }
        }
    }

    function mdCmsDiagramCanvas() {
        return {
            restrict: 'E',
            template: '<div ng-transclude></div>',
            transclude: true
        }
    }
})();
