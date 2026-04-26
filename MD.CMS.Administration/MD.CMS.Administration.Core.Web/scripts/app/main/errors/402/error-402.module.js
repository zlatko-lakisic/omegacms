(function ()
{
    'use strict';

    angular
        .module('app.errors.error-402', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msNavigationServiceProvider)
    {
        // State
        $stateProvider.state('app.errors_error-402', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/errors/error-402',
            views    : {
                'main@'                             : {
                    templateUrl: 'scripts/app/core/layouts/content-only.html',
                    controller : 'MainController as vm'
                },
                'content@app.errors_error-402': {
                    templateUrl: 'scripts/app/main/errors/402/error-402.html',
                    controller : 'Error402Controller as vm'
                }
            },
            bodyClass: 'error-402'
        });
    }

})();