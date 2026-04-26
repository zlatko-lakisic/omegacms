(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsWatchChange', [mdCmsWatchChange]);
    /** @ngInject */
    function mdCmsWatchChange() {
        return {
            scope: {
                onchange: '=mdCmsWatchChange'
            },
            link: function (scope, element, attrs) {
                element.on('input', function () {
                    scope.$apply(function () {
                        scope.onchange();
                    });
                });
            }
        };
    }
})();
