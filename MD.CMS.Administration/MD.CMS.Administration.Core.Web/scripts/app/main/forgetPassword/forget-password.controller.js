(function () {
    'use strict';

    angular
        .module('app.forgetPassword')
        .controller('ForgetPasswordController', ['$state', 'mdToastService', '$mdDialog', ForgetPasswordController]);

    /** @ngInject */
    function ForgetPasswordController($state, mdToastService, $mdDialog) {
        var vm = this;
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        vm.passwordConfirm;
        vm.changePassword = changePassword;
        vm.token = $state.params.token;

        function showDialog(title, text, redirect) {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));
            $mdDialog.show(
                            $mdDialog.alert()
                              .parent(parentElement)
                              .clickOutsideToClose(true)
                              .parent(parentElement)
                              .title(title)
                              .textContent(text)
                              .ariaLabel(title)
                              .ok('Got it!')
                          );
            if (redirect) {
                $state.go('app.login', { reload: true });
            }
        }
        function changePassword() {
            if ( vm.passwordConfirm != vm.user.Password) {
                showDialog('Passwords doesn\'t match', 'Please try again', false);
               
            }
            else {
                vm.user.Token = vm.token;
                userController.updateUser(vm.user,
                    function (data) {
                        showDialog('User updated', 'You successfully updated user', true);
                    }, function (error) {
                        showDialog(error.errorData.statusText, error.errorData.responseText, false);
                    });
            }
        }
      
    }
})();
