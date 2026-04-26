(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template-list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        $stateProvider.state('app.template-list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/template/list',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/template/list/template-list.html',
                    controller: 'TemplateListController as vm'
                }
            },
            params: {
                currentView: 'list'
            },
            bodyClass: 'file-manager',
            resolve: {
                templates: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var templatesConfig = {
                        sort: "",
                        pageIndex: 0,
                        pageSize: 10,
                        searchTerm: "",
                        searchColumn: "All"
                    };
                    (new mdBusinessLogic.dataAccess.controllers.templateController()).getAllWithPagination(
                        templatesConfig,
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
    }
})();
