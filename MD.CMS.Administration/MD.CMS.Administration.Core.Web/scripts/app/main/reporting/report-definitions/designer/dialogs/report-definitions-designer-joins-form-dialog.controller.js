(function ()
{
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .controller('reportDesignerJoinFormController', ['$scope', '$mdDialog', 'action', 'join', 'entities', reportDesignerJoinFormController]);

    /** @ngInject */
    function reportDesignerJoinFormController($scope, $mdDialog, action, join, entities)
    {
        //Private attributes
        var vm = this;

        //Public properties
        vm.join = join;
        vm.action = action;
        vm.entities = entities;

        //Public methods
        vm.cancel = cancel;
        vm.save = save;
        vm.isValid = isValid;

        //Private methods
        function cancel() {
            $mdDialog.cancel();
        }

        function save() {
            $mdDialog.hide(vm.join);
        }
        function isValid() {
            return vm.join.Left.Property.Name.toString().length != 0 && vm.join.Right.Property.Name.toString().length != 0 != 0 && vm.join.Type.toString().length != 0;
        }
    }
}());
