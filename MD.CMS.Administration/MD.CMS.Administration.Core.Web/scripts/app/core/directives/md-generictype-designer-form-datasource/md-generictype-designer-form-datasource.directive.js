(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdGenerictypeDesignerFormDatasource', ['$compile', mdGenerictypeDesignerFormDatasource]);

    /** @ngInject */
    function mdGenerictypeDesignerFormDatasource($compile) {
        return {
            require: 'ngModel',
            restrict: 'E',
            scope: {
                type: '=',
                onBeforeSave: '&',
                onAfterSave: '&',
                onBeforeCancel: '&',
                onAfterCancel: '&'
            },
            link: function (scope, elem, attrs, ngModel) {

                scope.modelData = {};
                scope.registerBeforeSaveEvent = registerBeforeSaveEvent;
                scope.registerBeforeCancelEvent = registerBeforeCancelEvent;
                scope.registerAfterSaveEvent = registerAfterSaveEvent;
                scope.registerAfterCancelEvent = registerAfterCancelEvent;

                function registerBeforeSaveEvent(event) {
                    scope.onBeforeSave()(event);
                }

                function registerBeforeCancelEvent(event) {
                    scope.onBeforeCancel()(event);
                }

                function registerAfterSaveEvent(event) {
                    scope.onAfterSave()(event);
                }

                function registerAfterCancelEvent(event) {
                    scope.onAfterCancel()(event);
                }

                scope.$watch(function () {
                    return ngModel.$modelValue;
                }, function (newValue) {
                    scope.modelData = newValue;
                    var template = '<md-datasource-' + scope.type.toLowerCase() + ' ng-model="modelData" on-before-save="registerBeforeSaveEvent" on-after-save="registerAfterSaveEvent" on-before-cancel="registerBeforeCancelEvent" on-after-cancel="registerAfterCancelEvent" />';
                    elem.replaceWith($compile(angular.element(template))(scope));
                });

            }
        };
    }
})();