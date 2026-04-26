(function () {
    'use strict';
    angular
        .module('app.core')
        .directive('mdCmsDynamicDirective', ['$compile', mdCmsDynamicDirective]);

    function mdCmsDynamicDirective($compile) {
        return {
            restrict: 'E',
            scope: {
                dynamicShortcode: '@',
                dynamicData: '=?'
            },
            link: function (scope, element, attributes) {
                scope.$watch(function () {
                    return scope.dynamicShortcode;
                }, function (newValue) {
                    if (newValue !== undefined) {
                        var newElem = angular.element(scope.dynamicShortcode);
                        for (var prop in attributes.$attr) {
                            if (prop != 'dynamic-shortcode' && prop != 'dynamicShortcode' && prop != 'dynamic-data') {
                                newElem.attr(prop.replace(/([A-Z])/g, '-$1').trim().toLowerCase(), attributes[prop]);
                            }
                            if (scope.dynamicData !== undefined) {
                                newElem.attr('data', 'dynamicData');
                            }
                        }
                        element.replaceWith($compile(newElem)(scope));
                    }
                });
            }
        };
    }
})();
