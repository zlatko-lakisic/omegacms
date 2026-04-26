(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsDatetimePicker', ['$q', mdCmsDatetimePicker]);

    function mdCmsDatetimePicker($q) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-datetime-picker/md-cms-datetime-picker.template.html',
            transclude: true,
            scope: {
                mdModel: "=",
                mdInputName: "@",
                mdFloatingLabel: "@",
                placeholder: "@",
                ngDisabled: "=?",
                ngPattern: "=?",
                mdTimezone: "=?",
                mdFormat: "@",
                mdStartView: "@",
                mdCompactMode: "=?",
                mdMinDate: "=?",
                mdMaxDate: "=?",
                mdSelectedRanges: "=?"
            },
            controller: 'mdCmsDatetimePickerController as vm',
            link: function (scope, element, attrs) {
                if (scope.ngPattern === undefined || scope.ngPattern == null) {
                    scope.ngPattern = '';
                }

                if (scope.mdFormat === undefined || scope.mdFormat == null) {
                    scope.mdFormat = 'YYYY-MM-DD HH:mm';
                }

                if (scope.mdStartView === undefined || scope.mdStartView == null) {
                    scope.mdStartView = "day";
                }

                if (scope.mdFloatingLabel === undefined || scope.mdFloatingLabel == null) {
                    scope.mdFloatingLabel = "";
                }

                if (scope.mdSelectedRanges === undefined || scope.mdSelectedRanges == null) {
                    scope.mdSelectedRanges = [];
                }
            }
        };
    }
})();
