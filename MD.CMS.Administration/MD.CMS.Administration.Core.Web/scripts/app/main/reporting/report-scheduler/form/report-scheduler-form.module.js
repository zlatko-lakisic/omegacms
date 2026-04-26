(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler.form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.report_scheduler_form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/reporting/report-scheduler/:action/:id',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/reporting/report-scheduler/form/report-scheduler-form.html',
                    controller: 'ReportinSchedulerFormController as vm'
                }
            },
            bodyClass: 'forms',
            resolve: {
                reportScheduler: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var reportSchedulerId = $stateParams.id;
                    if ($stateParams.action == "edit") {
                        (new mdBusinessLogic.dataAccess.controllers.reportSchedulerController()).getById(reportSchedulerId, function (data) {
                            defer.resolve(data);
                        }, function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    } else {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.reportScheduler());
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();