(function ()
{
    'use strict';

    angular
        .module('app.errors.error-403', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msNavigationServiceProvider)
    {
        // State
        $stateProvider.state('app.errors_error-403', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/errors/error-403',
            views    : {
                'main@'                             : {
                    templateUrl: 'scripts/app/core/layouts/content-only.html',
                    controller : 'MainController as vm'
                },
                'content@app.errors_error-403': {
                    templateUrl: 'scripts/app/main/errors/403/error-403.html',
                    controller : 'Error403Controller as vm'
                }
            },
            bodyClass: 'error-403'
        });
    }

})();