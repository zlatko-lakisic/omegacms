(function () {
    'use strict';

    angular
        .module('app.taxonomy', [
            'app.taxonomy.list',
            'app.taxonomy.forms'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
    }
})();
