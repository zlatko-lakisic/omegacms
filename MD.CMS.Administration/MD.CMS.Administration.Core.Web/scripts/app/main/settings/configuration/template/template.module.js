(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template', [
            'app.settings.configuration.template-list',
            'app.settings.configuration.template-form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
       
    }
})();