
(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdGenerictypeDesignerFormPreviewDialogController', ['$scope', '$mdDialog', '$interval', 'genericTypeObj', mdGenerictypeDesignerFormPreviewDialogController]);
    function mdGenerictypeDesignerFormPreviewDialogController($scope, $mdDialog, $interval, genericTypeObj) {

        //Private Attributes
        var vm = this;

        //Public Attributes
        vm.genericTypeObj = genericTypeObj;


        //Public Methods
        vm.cancel = cancel;


        //Private Methods
        function cancel() {
            $mdDialog.cancel();
        }
    }
})();