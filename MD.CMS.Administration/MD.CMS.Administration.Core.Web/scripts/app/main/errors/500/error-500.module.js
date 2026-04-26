(function ()
{
    'use strict';

    angular
        .module('app.errors.error-500', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msNavigationServiceProvider)
    {
        // State
        $stateProvider.state('app.errors_error-500', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/errors/error-500',
            views    : {
                'main@'                             : {
                    templateUrl: 'scripts/app/core/layouts/content-only.html',
                    controller : 'MainController as vm'
                },
                'content@app.errors_error-500': {
                    templateUrl: 'scripts/app/main/errors/500/error-500.html',
                    controller : 'Error500Controller as vm'
                }
            },
            bodyClass: 'error-500'
        });
    }

})();