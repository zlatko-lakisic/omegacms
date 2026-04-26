(function () {
    'use strict';

    angular
        .module('app.settings.configuration.content-types-edit', [])
        .config(['$stateProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($stateProvider, msApiProvider) {
        $stateProvider.state('app.content-types-edit', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/content-types/:currentView/:id?',
            params: {
                id: {
                    type: 'string',
                    value: ''
                }
            },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/content-types/edit/content-types-edit.html',
                    controller: 'ContentTypeEditController as vm'
                }
            },
            resolve: {
                Icons: ['msApi', function (msApi) {
                    return msApi.resolve('icons@get');
                }],
                contentTypeDefinition: ['$q', '$stateParams', function ($q, $stateParams) {
                    var defer = $q.defer();

                    var contentTypeDefinition = new mdBusinessLogic.dataAccess.entities.contentTypeDefinition();
                    if ($stateParams.id !== undefined && $stateParams.id != '' && !isNaN($stateParams.id)) {
                        (new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController()).getById($stateParams.id, function (data) {
                            contentTypeDefinition = data;
                            defer.resolve(contentTypeDefinition);
                        }, function (error) {

                        });
                    } else {
                        defer.resolve(contentTypeDefinition);
                    }

                    return defer.promise;
                }]
            },
            bodyClass: 'forms'
        });
        // Api for all Icons from fuse
        msApiProvider.register('icons', ['./assets/icons/selection.min.json']);
    }
})();
