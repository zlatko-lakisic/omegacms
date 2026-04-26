(function () {
    'use strict';

    angular
        .module('app.approval.chain', [          
            'app.approval.chain.form'        
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
    }
})();