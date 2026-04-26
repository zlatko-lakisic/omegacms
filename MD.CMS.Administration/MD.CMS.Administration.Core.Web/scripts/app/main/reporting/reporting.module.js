(function () {
    'use strict';

    angular
        .module('app.reporting', [
            'app.reporting.report_definitions',
            'app.reporting.report_scheduler'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

        msNavigationServiceProvider.saveItem('reporting', {
            title: 'Menus.MainReporting',
            group: true,
            weight: 3
        });
    }
})();