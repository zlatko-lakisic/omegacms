(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler.form')
        .directive('actionForm', [actionForm]);

    /** @ngInject */
    function actionForm() {
        return {
            templateUrl: 'scripts/app/main/reporting/report-scheduler/form/actions/action-form-template.html',
            controller: 'ActionFormController',
            controllerAs: 'vm'
        }
    }
})();