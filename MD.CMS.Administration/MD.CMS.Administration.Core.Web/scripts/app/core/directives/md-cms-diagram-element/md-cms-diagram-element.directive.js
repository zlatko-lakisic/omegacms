(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsDiagramElement', ['$q', 'mdFeedbackService', '$compile', '$timeout', mdCmsDiagramElement]);

    function mdCmsDiagramElement($q, $mdFeedbackService, $compile, $timeout) {
        function setupTileData(tileData) {
            if (tileData === undefined || tileData == null) {
                tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();
            }
            return tileData;
        }

        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-diagram-element/md-cms-diagram-element.template.html',
            transclude: true,
            scope: {
                tileData: '=?',
                whiteframe: '@',
                layoutPadding: '@',
                layoutWrap: '@'
            },
            controller: 'mdCmsDiagramElementController as vm',
            link: function (scope, element, attributes) {
                if (scope.whiteframe === undefined) {
                    scope.whiteframe = 4;
                }

                var tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();

                if (scope.tileData !== undefined) {
                    tileData.construct(scope.tileData);
                }
                scope.tileData = tileData;

                if (scope.uniqueId === undefined) {
                    scope.tileData = setupTileData(scope.tileData);
                }
                angular.element(element).attr('id', scope.uniqueId);
            }
        }
    }
})();
