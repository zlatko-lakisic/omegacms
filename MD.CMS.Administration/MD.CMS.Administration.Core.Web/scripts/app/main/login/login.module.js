(function ()
{
    'use strict';

    angular
        .module('app.login', [])
        .config(['$stateProvider', 'mdPermissionAuthenticateProvider', config]);

    /** @ngInject */
    function config($stateProvider, mdPermissionAuthenticateProvider)
    {
        // State
        $stateProvider.state('app.login', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/login/?return=:returnUrl?',
            params: {
                sessionTimeout: false,
                returnUrl: ''
            },
            views: {
                'main@': {
                    templateUrl: 'scripts/app/core/layouts/content-only.html',
                    controller: 'MainController as vm'
                },
                'content@app.login': {
                    templateUrl: 'scripts/app/main/login/login.html',
                    controller: 'LoginController as vm'

                }
            },
            resolve: {
                sessionTimeout: ['$stateParams', function ($stateParams) {
                    return ($stateParams.sessionTimeout === undefined || $stateParams.sessionTimeout == null ? false : $stateParams.sessionTimeout);
                }],
                returnUrl: ['$stateParams', function ($stateParams) {
                    return ($stateParams.returnUrl !== undefined && $stateParams.returnUrl != null && $stateParams.returnUrl != '' ? decodeURI($stateParams.returnUrl) : null);
                }]
            },
            bodyClass: 'login',
            onEnter: mdPermissionAuthenticateProvider.onStateEnter(false)
        });

        $stateProvider.state('app.login-reset', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/login/reset/:token',
            params: {
                token: ''
            },
            views: {
                'main@': {
                    templateUrl: 'scripts/app/core/layouts/content-only.html',
                    controller: 'MainController as vm'
                },
                'content@app.login-reset': {
                    templateUrl: 'scripts/app/main/login/reset.html',
                    controller: 'ResetController as vm'

                }
            },
            bodyClass: 'reset-password'
        });
    }

})();