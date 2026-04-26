(function () {
    'use strict';

    angular
        .module('app.settings.configuration.meta-data-field-list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        $stateProvider.state('app.meta-data-field-list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/meta-data-field/list',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/meta-data-field/list/meta-data-field-list.html',
                    controller: 'MetaDataFieldListController as vm'
                }
            },
            params: {
                currentView: 'list'
            },
            //resolve: {
            //    Documents: function (msApi) {
            //        return msApi.resolve('fileManager.documents@get');
            //    }
            //},
            bodyClass: 'file-manager',
            resolve: {
                metaDataFields: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var metaDataConfig = {
                        currentPageIndex: 0,
                        maxNumberOfRows: 10,
                        sort: "",
                        searchTerm: "",
                        searchColumn: "All"
                    };
                    (new mdBusinessLogic.dataAccess.controllers.metaDataFieldController()).paginationGetAll(
                        metaDataConfig,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    return defer.promise;
                }]
            }
        });

        //// Translation
        //$translatePartialLoaderProvider.addPart('scripts/app/main/settings/configuration/user/list/');

        //// Api
        //msApiProvider.register('fileManager.documents', ['scripts/app/data/file-manager/documents.json']);
    }

})();
