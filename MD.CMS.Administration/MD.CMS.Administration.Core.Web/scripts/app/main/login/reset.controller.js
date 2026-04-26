(function ()
{
    'use strict';

    angular
        .module('app.login')
        .controller('ResetController', ['$state', '$scope', '$rootScope', 'mdToastService', '$location', '$window', '$timeout', '$mdDialog', ResetController]);

    /** @ngInject */
    function ResetController($state, $scope, $rootScope, mdToastService, $location, $window, $timeout, $mdDialog)
    {
        var vm = this;
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        vm.background = mdBusinessLogic.settings.loginBackground;
        vm.email = '';
        vm.password = '';
        vm.passwordConfirm = ''
        vm.token = $state.params.token;

        vm.changePassword = function () {
            return $q(function (resolve, reject) {
                userController.passwordReset(vm.token, vm.email, vm.password, function (data) {
                    $scope.$apply(function () {
                        resolve();
                        $state.go('app.login');
                    });
                }, function (error) {
                    resolve();
                    $mdFeedbackService.reportError('save', error);
                });
            });
        }
    }
})();
