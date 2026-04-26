(function () {
    'use strict';

    angular
        .module('omega')
        .run(['$rootScope', '$timeout', '$state', 'cfpLoadingBar', 'mdFeedbackService', 'mdSavedDataService', 'mdSocketService', '$transitions', '$q', '$location', 'mdAuthenticationRegistryService', 'mdPermissionAuthenticateService', 'mdSavedDataKeys', runBlock]);

    /** @ngInject */
    function runBlock($rootScope, $timeout, $state, cfpLoadingBar, $mdFeedbackService, mdSavedDataService, mdSocketService, $transitions, $q, $location, mdAuthenticationRegistryService, mdPermissionAuthenticateService, mdSavedDataKeys) {

        mdAuthenticationRegistryService.add({
            id: 'BuiltInAuthenticationProvider',
            name: 'Built In',
            shortcode: '<md-cms-authentication-provider-builtin />',
            icon: 'icon-omega-logo'
        });

        var loginStateUrl = $state.href('app.login', {}, { absolute: false });;
        var loginResetStateUrl = $state.href('app.login-reset', {}, { absolute: false });;
        var isLogin = false;

        function loadResources() {
            $rootScope.globals = {
                selectedLanguage: mdBusinessLogic.globals.selectedLanguage,
                resources: mdBusinessLogic.globals.resources
            };

        }

        loadResources();

        $rootScope.$watch("$root.globals.selectedLanguage", function handleFooChange(newValue, oldValue) {
            if (newValue !== undefined && newValue != oldValue) {
                var urlArray = window.location.href;
                window.location.href = window.location.href.split('/').map(function (element) {
                    if (element == oldValue) {
                        element = newValue;
                    }
                    return element;
                }).join('/');
            }
        });

        $rootScope.NumberOfDaRequests = 0;

        function loadingBarController(loadingType) {
            var oldValue = $rootScope.NumberOfDaRequests;
            if (loadingType == 'add') {
                if ($rootScope.NumberOfDaRequests < 0) {
                    $rootScope.NumberOfDaRequests = 0;
                }
                $rootScope.NumberOfDaRequests++;
            } else {
                if ($rootScope.NumberOfDaRequests <= 0) {
                    $rootScope.NumberOfDaRequests = 0;
                } else {
                    $rootScope.NumberOfDaRequests--;
                }
            }
        }

        $rootScope.$on('cfpLoadingBar:completed', function (event, data) {
            $rootScope.loadingProgress = false;
        });

        $rootScope.$on('cfpLoadingBar:started', function (event, data) {
            $rootScope.loadingProgress = true;
        });

        $rootScope.$on('$viewContentLoaded', function () {
            loadingBarController('remove');
        });

        mdBusinessLogic.settings.admin.registerAdminEvent(new mdBusinessLogic.settings.adminEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnBeforeSend, function () {
            return new Promise(function (resolve, reject) {
                loadingBarController('add');
                resolve();
            });
        }));

        mdBusinessLogic.settings.admin.registerAdminEvent(new mdBusinessLogic.settings.adminEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnComplete, function () {
            return new Promise(function (resolve, reject) {
                loadingBarController('remove');
                resolve();
            });
        }));

        mdBusinessLogic.settings.admin.registerAdminEvent(new mdBusinessLogic.settings.adminEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnUnauthorized, function (exception) {
            return new Promise(function (resolve, reject) {
                clearLogin();
                resolve();
            });
        }));

        mdBusinessLogic.settings.admin.registerAdminEvent(new mdBusinessLogic.settings.adminEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnForbidden, function (exception) {
            return new Promise(function (resolve, reject) {
                resolve();
            });
        }));

        $rootScope.pluginJobsInfo = {
            currentPluginStatus: 'idle',
            pluginJobs: []
        };

        $rootScope.unreadMessages = 0;
        $rootScope.mdBusinessLogic = mdBusinessLogic;


        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var systemInfoController = new mdBusinessLogic.dataAccess.controllers.systemInfoController();
        var messageController = new mdBusinessLogic.dataAccess.controllers.messageController();
        var permissionControllerUser = new mdBusinessLogic.dataAccess.controllers.permissionControllerUser();
        var permissionControllerProfileType = new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType();
        var loginSocket = mdSocketService.create(5000, 5000);
        var pluginJobSocket = mdSocketService.create(5000, 0);
        var unreadMessageSocket = mdSocketService.create(5000, 0);
        var userPermissionsSocket = mdSocketService.create(5000, 5000);
        var profileTypePermissionsSocket = mdSocketService.create(5000, 5000);



        mdBusinessLogic.settings.admin.registerAdminEvent(new mdBusinessLogic.settings.adminEvent(mdBusinessLogic.settings.adminEventTypes.onBeforeUnload, function (ev) {
            return new Promise(function (resolve, reject) {
                if (loginSocket.socket != null) {
                    loginSocket.close();
                }
                if (pluginJobSocket.socket != null) {
                    pluginJobSocket.close();
                }
                if (unreadMessageSocket.socket != null) {
                    unreadMessageSocket.close();
                }
                if (userPermissionsSocket.socket != null) {
                    userPermissionsSocket.close();
                }
                if (profileTypePermissionsSocket.socket != null) {
                    profileTypePermissionsSocket.close();
                }
                resolve();
            });
        }));

        // Login Checked
        function clearLogin(_options) {
            var options = {
                promisesToResolve: [],
                sessionTimeout: false,
                returnUrl: ''
            };
            options = angular.extend(options, _options);

            mdSavedDataService.deleteData(mdSavedDataKeys.globals.loggedOnUser);
            mdSavedDataService.deleteData(mdSavedDataKeys.globals.loggedOnUserToken);
            mdSavedDataService.deleteData(mdSavedDataKeys.settings.lcid);
            mdBusinessLogic.globals.loggedOnUser = null;
            mdBusinessLogic.globals.loggedOnUserToken = null;
            if (mdBusinessLogic.settings.lcid == undefined) {
                mdBusinessLogic.settings.lcid = 0;
            }
            if (options.promisesToResolve.length > 0) {
                optionspromisesToResolve.push($q(function (resolve, reject) {
                    reject(401);
                }));
            } else {
                if (!isLogin) {
                    $state.go('app.login', {
                        sessionTimeout: options.sessionTimeout,
                        returnUrl: options.returnUrl
                    });
                }
            }
        }

        $transitions.onError({}, function ($transition$) {
            var toState = $transition$.$to();
            if ($transition$.error()) {
                switch ($transition$.error().type) {
                    case 3:
                    case 4:
                        $mdFeedbackService.reportError("load", new mdBusinessLogic.helpers.mdException($transition$.error().message, $transition$.error(), new Error()))
                        break;
                    case 6:
                        switch ($transition$.error().detail) {
                            case 401:
                                clearLogin({
                                    sessionTimeout: false,
                                    returnUrl: encodeURI($location.url())
                                });
                                break;
                            case 403:
                                $state.go('app.errors_error-403');
                                break;
                            default:
                                $state.go('app.errors_error-500');
                        }
                        break;
                }
            }

            loadingBarController('remove');
            $timeout(function () {
                cfpLoadingBar.complete();
            });
        });

        $transitions.onBefore({}, function ($transition$) {
            var toState = $transition$.$to();
            var fromState = $transition$.$from();
            loadingBarController('add');

            cfpLoadingBar.start();

            isLogin = toState.name === "app.login" || toState.name === "app.forgetPassword" || toState.name === "app.login-reset";

            var loggedOnUserToken = mdSavedDataService.getData(mdSavedDataKeys.globals.loggedOnUserToken);
            if (loggedOnUserToken) {
                mdBusinessLogic.globals.loggedOnUserToken = loggedOnUserToken;
            }
            var loggedOnUser = mdSavedDataService.getData(mdSavedDataKeys.globals.loggedOnUser);
            if (loggedOnUser) {
                loggedOnUser = JSON.parse(loggedOnUser);
                mdBusinessLogic.globals.loggedOnUser = loggedOnUser;
            }
            var savedLcid = mdSavedDataService.getData(mdSavedDataKeys.settings.lcid);
            if (loggedOnUserToken && savedLcid != 0) {
                mdBusinessLogic.settings.lcid = savedLcid;
            }

            if (!isLogin) {

                if (mdBusinessLogic.globals.loggedOnUser !== undefined &&
                    mdBusinessLogic.globals.loggedOnUser != null &&
                    mdBusinessLogic.globals.loggedOnUserToken !== undefined &&
                    mdBusinessLogic.globals.loggedOnUserToken != null &&
                    mdBusinessLogic.globals.loggedOnUserToken != '' &&
                    mdBusinessLogic.globals.loggedOnUser.AdministrationAllowed) {
                    if (!loginSocket.isRunning) {
                        loginSocket.run(function (callback) {
                            return userController.validateTokenSocket(loginSocket.id, mdBusinessLogic.globals.loggedOnUserToken, function (data, socket) {
                                if (data !== undefined && data != null && data.AdministrationAllowed) {
                                    mdBusinessLogic.globals.loggedOnUser = data;
                                    callback(socket);
                                } else {
                                    loginSocket.close();
                                    clearLogin({
                                        sessionTimeout: true,
                                        returnUrl: encodeURI($location.url())
                                    });
                                }
                            }, function (socket) {
                                loginSocket.close();
                                clearLogin({
                                    sessionTimeout: true,
                                    returnUrl: encodeURI($location.url())
                                });
                            }, function (error, socket) {
                                loginSocket.close();
                                clearLogin({
                                    sessionTimeout: true,
                                    returnUrl: encodeURI($location.url())
                                });
                            });
                        });
                    }
                    if (!pluginJobSocket.isRunning) {
                        pluginJobSocket.run(function (callback) {
                            return systemInfoController.getPluginJobs(pluginJobSocket.id, function (data, socket) {
                                if (data !== undefined && data != null) {
                                    $rootScope.$apply(function () {
                                        $rootScope.pluginJobsInfo = {
                                            currentPluginStatus: data.length > 0 ? 'working' : 'idle',
                                            pluginJobs: data
                                        };
                                    });
                                    callback(socket);
                                } else {
                                    pluginJobSocket.close();
                                }
                            }, function (error, socket) {
                                pluginJobSocket.close();
                            });
                        });
                    }
                    if (!unreadMessageSocket.isRunning) {
                        unreadMessageSocket.run(function (callback) {
                            return messageController.getUnreadByUser(unreadMessageSocket.id, function (data, socket) {
                                if (data !== undefined && data != null) {
                                    $rootScope.$apply(function () {
                                        $rootScope.unreadMessages = data.length;
                                    });
                                    callback(socket);
                                } else {
                                    unreadMessageSocket.close();
                                }
                            }, function (error, socket) {
                                unreadMessageSocket.close();
                            });
                        });
                    }
                    if (!userPermissionsSocket.isRunning) {
                        userPermissionsSocket.run(function (callback) {
                            return permissionControllerUser.getLoggedOnUserPermissionsSocket(userPermissionsSocket.id, mdBusinessLogic.globals.loggedOnUserToken, function (data, socket) {
                                if (data !== undefined && data != null) {
                                    $rootScope.$apply(function () {
                                        mdPermissionAuthenticateService.setLoggedOnUserPermissions(data);
                                    });
                                    callback(socket);
                                } else {
                                    userPermissionsSocket.close();
                                }
                            }, function (error, socket) {
                                userPermissionsSocket.close();
                            });
                        });
                    }
                    if (!profileTypePermissionsSocket.isRunning) {
                        profileTypePermissionsSocket.run(function (callback) {
                            return permissionControllerProfileType.getLoggedOnProfileTypePermissionsSocket(profileTypePermissionsSocket.id, mdBusinessLogic.globals.loggedOnUserToken, function (data, socket) {
                                if (data !== undefined && data != null) {
                                    $rootScope.$apply(function () {
                                        mdPermissionAuthenticateService.setLoggedOnProfileTypePermissions(data);
                                    });
                                    callback(socket);
                                } else {
                                    profileTypePermissionsSocket.close();
                                }
                            }, function (error, socket) {
                                profileTypePermissionsSocket.close();
                            });
                        });
                    }
                } else {
                    loginSocket.close();
                    pluginJobSocket.close();
                    unreadMessageSocket.close();
                    userPermissionsSocket.close();
                    profileTypePermissionsSocket.close();
                    clearLogin();
                }
            }
        });

        // De-activate loading indicator
        $transitions.onSuccess({}, function ($transition$) {
            loadingBarController('remove');
            $timeout(function () {
                cfpLoadingBar.complete();
            });
        });

        // Store state in the root scope for easy access
        $rootScope.state = $state;
    }
})();
