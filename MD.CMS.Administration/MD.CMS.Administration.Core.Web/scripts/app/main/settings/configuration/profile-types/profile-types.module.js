(function () {
    'use strict';

    angular
        .module('app.settings.configuration.user_management.profile_type', [
            'app.settings.configuration.profile-types-list',
            'app.settings.configuration.profile-types-form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

    }
})();