(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler', [
            'app.reporting.report_scheduler.list',
            'app.reporting.report_scheduler.form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

        // Navigation
        msNavigationServiceProvider.saveItem('reporting.report_scheduler_list', {
            title: 'Menus.MainReportingScheduler',
            icon: 'icon-calendar-clock',
            state: 'app.report_scheduler_list',
            weight: 4
        });
    }
})();
