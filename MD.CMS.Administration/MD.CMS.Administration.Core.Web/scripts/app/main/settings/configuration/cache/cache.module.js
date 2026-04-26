(function () {
    'use strict';

    angular
        .module('app.settings.configuration.cache', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider
            .state('app.settings_configuration_cache', {
                url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/configuration/cache',
                views: {
                    'content@app': {
                        templateUrl: 'scripts/app/main/settings/configuration/cache/cache.html',
                        controller: 'CacheController as vm'
                    }
                },
                bodyClass: 'cache',
                resolve: {
                    allDataCache: ['$q', function ($q) {
                        var defer = $q.defer();
                        (new mdBusinessLogic.dataAccess.controllers.cacheController()).getAllDataCache(function (data) {
                            defer.resolve(data);
                        }, function () {

                        });
                        return defer.promise;
                    }]
                }
            });
    }
})();
