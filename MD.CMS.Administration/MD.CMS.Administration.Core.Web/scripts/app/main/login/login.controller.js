(function ()
{
    'use strict';

    angular
        .module('app.login')
        .controller('LoginController', ['$state', '$scope', '$rootScope', 'mdSavedDataService', 'mdToastService', '$location', 'mdAuthenticationRegistryService', '$timeout', '$mdDialog', 'sessionTimeout', 'returnUrl', '$q', 'mdPermissionAuthenticateService', 'mdSavedDataKeys', '$http', LoginController]);

    /** @ngInject */
    function LoginController($state, $scope, $rootScope, mdSavedDataService, mdToastService, $location, mdAuthenticationRegistryService, $timeout, $mdDialog, sessionTimeout, returnUrl, $q, mdPermissionAuthenticateService, mdSavedDataKeys, $http)
    {
        //Private Attributes
        var vm = this;
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var permissionControllerProfileType = new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType();
        var permissionControllerUser = new mdBusinessLogic.dataAccess.controllers.permissionControllerUser();

        //Public Properties
        vm.authMode = mdBusinessLogic.dataAccess.providers.authentication.authMode;
        vm.IsForgotPassword = false;
        vm.loginLoading = false;
        vm.rememberMe = true;
        vm.background = mdBusinessLogic.settings.loginBackground;
        vm.loginProviders = mdAuthenticationRegistryService.getAll();

        //Public Methods
        vm.processLogin = processLogin
        vm.onRememberMe = onRememberMe
        vm.resetPassword = resetPassword;

        //Private Methods
        function onRememberMe(rememberMe) {
            vm.rememberMe = rememberMe;
        }

        function processLogin(data) {
            vm.loginLoading = true;
            data.AuthenticationProviderName = vm.selectedProvider.Key;

            var loginPromise = $q(function (resolve, reject) {
                userController.loginAuthData(data, function (data) {
                    $scope.$apply(function () {
                        if (data !== undefined && data != null && data.AdministrationAllowed) {
                            mdBusinessLogic.globals.loggedOnUser = data;
                            mdBusinessLogic.globals.loggedOnUserToken = mdBusinessLogic.helpers.encoder.base64.encode(data.Username + ':' + data.SessionId);

                            mdSavedDataService.storeData(mdSavedDataKeys.globals.loggedOnUserToken, mdBusinessLogic.globals.loggedOnUserToken.toString(), vm.rememberMe);
                            mdSavedDataService.storeData(mdSavedDataKeys.globals.loggedOnUser, JSON.stringify(mdBusinessLogic.globals.loggedOnUser), vm.rememberMe);
                            mdSavedDataService.storeData(mdSavedDataKeys.globals.selectedLanguage, mdBusinessLogic.globals.selectedLanguage.toString(), true);
                            mdSavedDataService.storeData(mdSavedDataKeys.settings.lcid, mdBusinessLogic.settings.lcid.toString(), true);

                            $q.all([
                                $q(function (resolve, reject) {
                                    permissionControllerProfileType.getLoggedOnProfileTypePermissions(function (data) {
                                        mdPermissionAuthenticateService.setLoggedOnProfileTypePermissions(data);
                                        resolve();
                                    }, function (error) {
                                        resolve();
                                    });
                                }),
                                $q(function (resolve, reject) {
                                    permissionControllerUser.getLoggedOnUserPermissions(function (data) {
                                        mdPermissionAuthenticateService.setLoggedOnUserPermissions(data);
                                        resolve();
                                    }, function (error) {
                                        resolve();
                                    });
                                }),
                                mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.onLogin, $scope)
                            ]).then(function () {
                                $timeout(function () {
                                    $location.search('');
                                    if (returnUrl != null && returnUrl.indexOf('/') >= 0) {
                                        var result = $location.path(returnUrl);
                                    } else {
                                        $state.go(mdBusinessLogic.settings.defaultState, {}, { reload: true, $retry: true });
                                    }
                                });
                            }, function () {
                                vm.loginLoading = false;
                                mdToastService.showBodyToast($rootScope.globals.resources.Labels.LoginError);
                                reject();
                            });
                        } else {
                            vm.loginLoading = false;
                            mdToastService.showBodyToast($rootScope.globals.resources.Labels.LoginError);
                            reject();
                        }
                    });
                }, function (data) {
                    $scope.$apply(function () {
                        vm.loginLoading = false;
                        mdToastService.showBodyToast($rootScope.globals.resources.Labels.LoginError);
                    });
                    reject(data);
                });
            });

            return loginPromise;
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
            for (var i = 0; i < vm.loginProviders.length; i++) {
                vm.selectedProvider = vm.loginProviders[i];
                break;
            }
            if (vm.background === undefined) {
                vm.background = {
                    type: 'video',
                    video: {
                        url: 'assets/omega_background.mp4'
                    },
                    image: {
                        url: '',
                        label: 'Labels.WelcomeOmegaCMS'
                    }
                };
            }

            if (vm.background.type == 'image') {
                vm.background.image.url = vm.background.image.url.replace(/\/\//g, '/');
            } else {
                vm.background.video.url = vm.background.video.url.replace(/\/\//g, '/');
            }

            $q.when(mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.onLogout, $scope)).then(function () {
                userController.logout(mdBusinessLogic.globals.loggedOnUser);
                mdSavedDataService.deleteData(mdSavedDataKeys.globals.loggedOnUser);
                mdSavedDataService.deleteData(mdSavedDataKeys.globals.loggedOnUserToken);
                mdSavedDataService.deleteData(mdSavedDataKeys.globals.selectedLanguage);
                mdSavedDataService.deleteData(mdSavedDataKeys.settings.lcid);
                mdPermissionAuthenticateService.setLoggedOnProfileTypePermissions([]);
                mdPermissionAuthenticateService.setLoggedOnUserPermissions([]);
                mdBusinessLogic.globals.loggedOnUser = null;
                mdBusinessLogic.globals.loggedOnUserToken = null;
                mdBusinessLogic.globals.selectedLanguage = "en-GB";
                mdBusinessLogic.settings.lcid = 0;

                $mdDialog.cancel();
            });

            if (sessionTimeout) {
                $mdDialog.show(
                    $mdDialog.alert()
                        .parent(angular.element(document.querySelector('body')))
                        .clickOutsideToClose(true)
                        .title('Logged out due to inactivity')
                        .textContent('You have been logged out due to inactivity')
                        .ariaLabel('Logged out due to inactivity')
                        .ok('Got it!')
                );
            }
        }

        init();
    }
})();
