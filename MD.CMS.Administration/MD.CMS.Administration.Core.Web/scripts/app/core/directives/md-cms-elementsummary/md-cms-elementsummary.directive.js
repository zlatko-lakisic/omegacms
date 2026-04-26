(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsElementsummary', [mdCmsElementsummary]);
    /** @ngInject */
    function mdCmsElementsummary() {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-elementsummary/md-cms-elementsummary.template.html',
            transclude: true,
            scope: {
                mdType: "=",
                mdSelectedItem: "=?"
            },
            link: function (scope, element, attrs) {
                scope.typeString = mdBusinessLogic.dataAccess.entities.entitiesEnum[scope.mdType];
                scope.uploadsBase = mdBusinessLogic.settings.uploadsBase;
            }
        };
    }
})();
