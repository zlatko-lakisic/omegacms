(function () {
    'use strict';

    angular
        .module('app.settings.configuration.permissions-form', [])
        .config(['$stateProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($stateProvider, msApiProvider) {
        $stateProvider.state('app.permissions-form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/permissions/edit/:id?',
            params: {
                id: {
                    type: 'string',
                    value: ''
                }
            },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/permissions/form/permissions-form.html',
                    controller: 'PermissionsFormController as vm'
                }
            },
            resolve: {
                entityGroupId: ['$stateParams', function ($stateParams) {
                    return $stateParams.id;
                }]
            },
            bodyClass: 'forms'
        });
        // Api for all Icons from fuse
        msApiProvider.register('icons', ['./assets/icons/selection.min.json']);
    }
})();
