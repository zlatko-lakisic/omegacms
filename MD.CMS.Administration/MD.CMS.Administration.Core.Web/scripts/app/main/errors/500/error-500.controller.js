(function ()
{
    'use strict';

    angular
        .module('app.errors.error-500')
        .controller('Error500Controller', ['$mdDialog', Error500Controller]);

    /** @ngInject */
    function Error500Controller($mdDialog)
    {
        //Private Attributes
        var vm = this;

        //Public Attributes

        //Public Methods
        vm.reloadApp = reloadApp;

        //Private Methods
        function reloadApp() {
            window.location.href = mdBusinessLogic.settings.appBase;
        }

        $mdDialog.cancel();
    }
})();