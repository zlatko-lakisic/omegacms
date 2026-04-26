(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsPassword', ['$compile', mdCmsPassword]);

    function mdCmsPassword($compile) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                if (angular.element(element).attr('type') == 'password') {

                    var passwordToggled = false;

                    function afterPasswordToggle() {
                        if (passwordToggled) {
                            angular.element(element).attr('type', 'text');
                            scope.passwordToggleIcon = 'icon-eye-off';
                            scope.passwordToggleTooltip = 'Hide Password';
                        } else {
                            angular.element(element).attr('type', 'password');
                            scope.passwordToggleIcon = 'icon-eye';
                            scope.passwordToggleTooltip = 'Show Password';
                        }
                    }

                    afterPasswordToggle();

                    scope.togglePassword = function () {
                        passwordToggled = !passwordToggled;
                        afterPasswordToggle();
                    }

                    var icon = angular.element('<md-icon ng-click="togglePassword()" md-font-icon="{{passwordToggleIcon}}" style="cursor: pointer"><md-tooltip md-direction="top">{{passwordToggleTooltip}}</md-tooltip></md-icon>');

                    icon.insertAfter(element);

                    $compile(icon)(scope);

                    if (element.parent().is('md-input-container')) {
                        element.parent().addClass('md-icon-right');
                    }
                }
            }
        };
    }
})();
