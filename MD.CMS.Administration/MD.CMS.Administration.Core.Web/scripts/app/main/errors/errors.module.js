(function ()
{
    'use strict';

    angular
        .module('app.errors', [
            'app.errors.error-402',
            'app.errors.error-403',
            'app.errors.error-404',
            'app.errors.error-500'
        ])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msNavigationServiceProvider)
    {
    }

})();