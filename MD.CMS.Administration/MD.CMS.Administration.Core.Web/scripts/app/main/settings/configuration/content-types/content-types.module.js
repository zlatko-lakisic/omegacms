(function () {
    'use strict';

    angular
        .module('app.settings.configuration.content-types', [
            'app.settings.configuration.content-types-list',
            'app.settings.configuration.content-types-edit'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

    }
})();
