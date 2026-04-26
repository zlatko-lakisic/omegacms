(function () {
    'use strict';

    angular
        .module('app.support', [
            'app.support.javascript_documentation',
            'app.support.typescript_documentation',
            'app.support.assembly_documentation',
            'app.support.webapi_documentation'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

        msNavigationServiceProvider.saveItem('support', {
            title: 'Menus.MainSupport',
            group: true,
            weight: 4
        });
    }
})();
