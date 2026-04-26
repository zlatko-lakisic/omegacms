(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsAuthenticationProviderBuiltinController', ['$scope', '$q', '$parse', 'mdToastService', '$rootScope', mdCmsAuthenticationProviderBuiltinController]);
    /** @ngInject */
    function mdCmsAuthenticationProviderBuiltinController($scope, $q, $parse, mdToastService, $rootScope) {

        //Private Attributes
        var vm = this;
        var onSave = function () { throw 'Process save reference not set!'; };
        var processAuthentication = function (data) { throw 'Process Authentication reference not set!'; };
        var onRememberMe = function (data) { throw 'remember me reference not set!'; };
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var user = new mdBusinessLogic.dataAccess.entities.user();

        //Public Attributes
        vm.authMode = mdBusinessLogic.dataAccess.providers.authentication.authMode;
        vm.IsForgotPassword = false;
        vm.IsNotForgotPassword = false;
        vm.loginLoading = false;
        vm.form = {
            username: '',
            password: '',
            passwordConfirm: ''
        };
        vm.rememberMe = false;
        vm.mode = vm.authMode.login;

        //Public Methods
        vm.processSave = processSave;
        vm.processLogin = processLogin;
        vm.resetPassword = resetPassword;

        //Private Methods
        function processSave() {
            user.Username = vm.form.username;
            user.Password = vm.form.password;
            return $q(function (resolve, reject) {
                if (user.Id == '') {
                    resolve(user);
                } else {
                    (new mdBusinessLogic.dataAccess.controllers.userController()).updateAuthData(user, function (data) {
                        resolve(data);
                    }, function (error) {
                        reject();
                    });
                }
            });
        }

        function processLogin(e) {
            vm.loginLoading = true;
            
            var data = new mdBusinessLogic.dataAccess.providers.authentication.authData();
            data.SetData('username', vm.form.username);
            data.SetData('password', vm.form.password);
            data.SetData('token', mdBusinessLogic.helpers.Guid.create().toString());

            var resultPromise = processAuthentication(data);

            $q.when(resultPromise).then(function (data) {
                vm.loginLoading = false;
            }, function (error) {
                vm.loginLoading = false;
            });

            return resultPromise;
        }

        function resetPassword() {
            userController.resetAccount(vm.form.username, function (data) {
                mdToastService.showBodyToast($rootScope.globals.resources.Labels.EmailSent);

                vm.IsNotForgotPassword = false;
                vm.IsForgotPassword = true;

            }, function (data) {
                mdToastService.showBodyToast($rootScope.globals.resources.Labels.EmailIncorrectFormat);
            });
        }

        function init() {
            $scope.$watch('processAuthentication', function (_processAuthentication) {
                if (_processAuthentication !== undefined) {
                    processAuthentication = $parse(_processAuthentication)($scope.$parent.$parent.$parent);
                }
            });
            $scope.$watch('onSave', function (_onSave) {
                if (_onSave !== undefined) {
                    var onsave = $parse(_onSave)($scope.$parent.$parent);
                    onsave(processSave);
                }
            });
            $scope.$watch('onRememberMe', function (_onRememberMe) {
                if (_onRememberMe !== undefined) {
                    onRememberMe = $parse(_onRememberMe)($scope.$parent.$parent.$parent);
                }
            });
            $scope.$watch(function () { return vm.rememberMe; }, function (rememberMe) {
                if (rememberMe !== undefined) {
                    onRememberMe(rememberMe);
                }
            });
            $scope.$watch('mode', function (_mode) {
                if (_mode !== undefined) {
                    vm.mode = $parse(_mode)($scope.$parent.$parent);
                }
            });
            $scope.$watch('referenceId', function (_referenceId) {
                var referemceId = $parse(_referenceId)($scope.$parent.$parent);
                if (referemceId !== undefined && referemceId != '') {
                    (new mdBusinessLogic.dataAccess.controllers.userController()).getAuthData(referemceId, function (data) {
                        $scope.$apply(function () {
                            vm.form.username = data.GetData('username', '');
                        });
                    }, function (error) { });
                    (new mdBusinessLogic.dataAccess.controllers.userController()).getById(referemceId, function (data) {
                        $scope.$apply(function () {
                            user = data;
                        });
                    }, function (error) { });
                }
            });
        }

        init();
    }
})();
