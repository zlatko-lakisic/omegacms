(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsGridToolbar', ['$q', 'mdFeedbackService', '$timeout', '$mdSticky', mdCmsGridTile]);
    /** @ngInject */
    function mdCmsGridTile($q, $mdFeedbackService, $timeout, $mdSticky) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-grid-toolbar/md-cms-grid-toolbar.template.html',
            transclude: true,
            require: '^mdCmsGrid',
            scope: {
                mdTitle: '@',
                addableClass: '@',
                uniqueId: '@',
                parent: '@',
                ngClass: '='
            },
            controller: 'mdCmsGridToolbarController as vm',
            link: function (scope, element, attrs) {
                if (scope.uniqueId === undefined) {
                    scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                }
                angular.element(element).attr('id', scope.uniqueId);

                attrs.$addClass('md-cms-grid-toolbar');
                if (scope.addableClass === undefined) {
                    scope.addableClass = 'md-cms-grid-tile-new';
                }

                if (scope.parent !== undefined && scope.parent != null && scope.parent != '') {
                    $timeout(function () {
                        angular.element(element).detach().appendTo(scope.parent);
                    });
                }

                if (scope.ngClass !== undefined) {
                    if (typeof scope.ngClass === 'string' || scope.ngClass instanceof String) {
                        attrs.$addClass(scope.ngClass);
                    } else {
                        for (var cl in scope.ngClass) {
                            attrs.$addClass(scope.ngClass[cl]);
                        }
                    }
                }
            }
        }
    }
})();
