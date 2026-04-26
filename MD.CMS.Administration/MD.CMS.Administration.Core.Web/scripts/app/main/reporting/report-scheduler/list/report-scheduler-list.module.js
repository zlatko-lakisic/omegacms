(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler.list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State       
        $stateProvider.state('app.report_scheduler_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/reporting/report-scheduler/list/:currentView',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/reporting/report-scheduler/list/report-scheduler-list.html',
                    controller: 'ReportinSchedulerListController as vm'
                }
            },
            params: {
                currentView: ''
            },
            bodyClass: 'file-manager',
            resolve: {
                reportSchedulerData: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var reportSchedulerDataConfig = {
                        sort: "",
                        searchTerm: "",
                        searchColumn: "All",
                        pageIndex: 0,
                        pageSize: 10
                    };
                    (new mdBusinessLogic.dataAccess.controllers.reportSchedulerController()).getAllWithPagination(reportSchedulerDataConfig, function (data) {
                        defer.resolve(data);
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });
                    return defer.promise;
                }],
                reportDefinitions: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.reportDefinitionController()).getAll({sort: "Name ASC"}, function (data) {
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
