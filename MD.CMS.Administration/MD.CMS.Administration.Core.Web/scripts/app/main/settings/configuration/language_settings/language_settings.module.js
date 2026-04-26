(function () {
    'use strict';

    angular
        .module('app.settings.configuration.language_settings', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider
            .state('app.settings_configuration_language_settings', {
                url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/configuration/language_settings',
                views: {
                    'content@app': {
                        templateUrl: 'scripts/app/main/settings/configuration/language_settings/language_settings.html',
                        controller: 'LanguageSettingsController as vm'
                    }
                },
                bodyClass: 'language_settings',
                resolve: {
                  allCultures: ['$q', function ($q) {
                        var defer = $q.defer();
                        (new mdBusinessLogic.dataAccess.controllers.cultureController()).getAll(function (data) {
                            defer.resolve(data);
                        }, function () {

                        });
                        return defer.promise;
                    }]
                }
            });
    }
})();
