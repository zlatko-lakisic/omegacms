(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions.list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State       
        $stateProvider.state('app.report_definitions_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/reporting/report-definitions/list/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/reporting/report-definitions/list/report-definitions-list.html',
                    controller: 'ReportinDefinitionsListController as vm'
                }
            },
            params: {
                currentView: ''
            },
            bodyClass: 'file-manager',
            resolve: {
                reportDefinitions: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var reportDefinitionsConfig = {
                        sort: "",
                        searchTerm: "",
                        searchColumn: "All",
                        pageIndex: 0,
                        pageSize: 10
                    };
                    (new mdBusinessLogic.dataAccess.controllers.reportDefinitionController()).getAllWithPagination(reportDefinitionsConfig, function (data) {
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
