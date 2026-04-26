(function ()
{
    'use strict';

    angular
        .module('app.errors.error-403')
        .controller('Error403Controller', ['$mdDialog', Error500Controller]);

    /** @ngInject */
    function Error500Controller($mdDialog)
    {
        //Private Attributes
        var vm = this;

        //Public Attributes
        vm.homeState = mdBusinessLogic.settings.defaultState;

        $mdDialog.cancel();
    }
})();