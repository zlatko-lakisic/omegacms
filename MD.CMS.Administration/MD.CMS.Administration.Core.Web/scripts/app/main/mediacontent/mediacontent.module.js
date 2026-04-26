(function () {
    'use strict';

    angular
        .module('app.mediacontent', [
            'app.mediacontent.list',
            'app.mediacontent.form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
    }
})();
