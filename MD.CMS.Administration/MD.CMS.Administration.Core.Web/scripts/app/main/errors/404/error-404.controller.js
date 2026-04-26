(function ()
{
    'use strict';

    angular
        .module('app.errors.error-404')
        .controller('Error404Controller', ['$mdDialog', Error404Controller]);

    /** @ngInject */
    function Error404Controller($mdDialog)
    {
        // Data

        // Methods

        //////////

        $mdDialog.cancel();
    }
})();