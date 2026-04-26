(function () {
    'use strict';

    angular
        .module('app.personal', [
            'app.personal.mailbox',
            'app.personal.profile'
        ])
        .config([config]);

    /** @ngInject */
    function config() {
    }
})();
