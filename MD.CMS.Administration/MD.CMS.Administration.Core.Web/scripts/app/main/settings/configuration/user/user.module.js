(function () {
    'use strict';

    angular
        .module('app.settings.configuration.user_management.user', [
             'app.settings.configuration.user.list',
             'app.settings.configuration.user.form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
       
    }
})();