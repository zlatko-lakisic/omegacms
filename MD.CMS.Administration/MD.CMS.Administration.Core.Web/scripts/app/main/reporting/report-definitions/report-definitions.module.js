(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions', [
            'app.reporting.report_definitions.list',
            'app.reporting.report_definitions.designer'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

        // Navigation
        msNavigationServiceProvider.saveItem('reporting.report_definitions_list', {
            title: 'Menus.MainReportingDefinitions',
            icon: 'icon-file-document-box',
            state: 'app.report_definitions_list',
            weight: 4
        });
    }
})();
