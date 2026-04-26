(function () {
    'use strict';

    angular
        .module('app.content', [
            'app.content.list',
            'app.content.form',
            'app.folder.forms',          
            'ngMessages'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {
    }
})();
