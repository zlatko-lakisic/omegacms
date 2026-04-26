(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsPlaceholder', [mdCmsPlaceholder])
        .directive('mdCmsPlaceholderCustomTemplate', [mdCmsPlaceholderCustomTemplate]);

    function mdCmsPlaceholder() {
        return {
            restrict: 'E',
            templateUrl: 'scripts/app/core/directives/md-cms-placeholder/md-cms-placeholder.template.html',
            transclude: true,
            scope: {
                uniqueId: '@?',
                mdTemplate: "@?",
                mdLoading: "=?",
                mdPromise: "&?"
            },
            controller: 'mdCmsPlaceholderController as vm',
            link: function (scope, element, attrs) {
                scope.hasCustomTemplate = false;
                scope.$on('hasCustomTemplate', function () {
                    scope.hasCustomTemplate = true;
                });

                /*function showHideSelf() {
                    if (!scope.mdCmsPlaceholder) {
                        element.hide();
                    } else {
                        element.show();
                    }
                }

                if (scope.mdCmsPlaceholder === undefined || scope.mdCmsPlaceholder == null) {
                    scope.mdCmsPlaceholder = true;
                }

                showHideSelf();

                scope.$watch(function () {
                    return scope.mdCmsPlaceholder;
                }, function (mdCmsPlaceholder) {
                        if (mdCmsPlaceholder !== undefined && mdCmsPlaceholder != null) {
                        showHideSelf();
                    }
                });*/
            }
        };
    }

    function mdCmsPlaceholderCustomTemplate() {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-transclude />',
            link: function (scope, element, attrs) {
                scope.$emit('hasCustomTemplate');
            }
        };
    }
})();
