(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsFieldvalidation', ['$q', mdCmsFieldvalidation]);
    /** @ngInject */
    function mdCmsFieldvalidation($q) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-fieldvalidation/md-cms-fieldvalidation.template.html',
            transclude: true,
            scope: {
                mdField: "=",
                mdFormName: "@"
            },
            link: function (scope, element, attrs) {

                //Directive Methods
                function init() {
                }

                init();
            }
        };
    }
})();
