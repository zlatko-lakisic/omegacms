(function () {
    'use strict';

    angular
        .module('app.settings.configuration.permissions', [
            'app.settings.configuration.permissions-list',
            'app.settings.configuration.permissions-form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

    }
})();
