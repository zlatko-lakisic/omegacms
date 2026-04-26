(function () {
    'use strict';

    angular
        .module('app.menu', [
            'app.menu.list',
            'app.menu.forms'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
    }
})();