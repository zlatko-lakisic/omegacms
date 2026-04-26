(function () {
    'use strict';

    angular
        .module('app.settings.configuration.permissions-list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider
            .state('app.settings_configuration_permissions', {
                url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/configuration/permissions',
                views: {
                    'content@app': {
                        templateUrl: 'scripts/app/main/settings/configuration/permissions/list/permissions-list.html',
                        controller: 'PermissionsListController as vm'
                    }
                },
                bodyClass: 'permissions',
                resolve: {
                    auth: ['mdPermissionAuthenticateService', function (mdPermissionAuthenticateService) {
                        return mdPermissionAuthenticateService.authenticateByUser('Configuration');
                    }]
                }
            });
    }
})();
