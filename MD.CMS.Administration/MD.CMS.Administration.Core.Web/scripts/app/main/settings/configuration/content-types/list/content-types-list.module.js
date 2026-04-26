(function () {
    'use strict';

    angular
        .module('app.settings.configuration.content-types-list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider.state('app.content-types-list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/content-types/',
                views: {
                    'content@app': {
                        templateUrl: 'scripts/app/main/settings/configuration/content-types/list/content-types-list.html',
                        controller: 'ContentTypesListController as vm'
                    }
                },
                params: {
                    currentView: ''
                },
                resolve: {
                    contentTypes: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                        var defer = $q.defer();
                        var contentTypesConfig = {
                            currentPageIndex: 0,
                            maxNumberOfRows: 10,
                            sort: "Name ASC",
                            searchTerm: "",
                            searchColumn: "All"
                        };
                        (new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController()).paginationGetAll(
                            contentTypesConfig,
                            function (data) {
                                defer.resolve(data);
                            }, function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                        return defer.promise;
                    }]

                }
            });
    }
})();
