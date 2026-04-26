(function ()
{
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .controller('reportDesignerCustomizeDialogController', ['$scope', '$mdDialog', 'entity', reportDesignerCustomizeDialogController]);

    /** @ngInject */
    function reportDesignerCustomizeDialogController($scope, $mdDialog, entity)
    {
        //Private attributes
        var vm = this;

        //Public properties
        vm.entity = entity;

        //Public methods
        vm.cancel = cancel;
        vm.save = save;
        vm.updateFieldEnabled = updateFieldEnabled;

        //Private methods
        function updateFieldEnabled(field, value) {
            var found = false;
            for (var bf = 0; bf < vm.entity.BaseFields.length; bf++) {
                if (!found && vm.entity.BaseFields[bf].Name == field.Name) {
                    vm.entity.BaseFields[bf].Enabled = value;
                    found = true;
                }
            }

            for (var f = 0; f < vm.entity.Fields.length; f++) {
                if (!found && vm.entity.Fields[f].Name == field.Name) {
                    vm.entity.Fields[f].Enabled = value;
                    found = true;
                }
            }

            if (!found) {
                for (var ef = 0; ef < vm.entity.ExtendedFields.length; ef++) {
                    if (vm.entity.ExtendedFields[ef].Name == field.Name) {
                        vm.entity.ExtendedFields[ef].Enabled = value;
                    }
                }
            }
        }

        function cancel() {
            $mdDialog.cancel();
        }

        function save() {
            $mdDialog.hide(vm.entity);
        }
    }
}());
