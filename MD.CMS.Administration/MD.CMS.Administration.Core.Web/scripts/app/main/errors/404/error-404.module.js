(function ()
{
    'use strict';

    angular
        .module('app.errors.error-404', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msNavigationServiceProvider)
    {
        // State
        $stateProvider.state('app.errors_error-404', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/errors/error-404',
            views    : {
                'main@'                             : {
                    templateUrl: 'scripts/app/core/layouts/content-only.html',
                    controller : 'MainController as vm'
                },
                'content@app.errors_error-404': {
                    templateUrl: 'scripts/app/main/errors/404/error-404.html',
                    controller : 'Error404Controller as vm'
                }
            },
            bodyClass: 'error-404'
        });
    }

})();