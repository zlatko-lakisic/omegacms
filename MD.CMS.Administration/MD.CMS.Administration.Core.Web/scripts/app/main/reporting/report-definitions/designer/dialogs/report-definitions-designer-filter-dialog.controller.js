(function ()
{
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .controller('reportDesignerFilterDialogController', ['$scope', '$mdDialog', 'reportDesignerService', 'reportDefinition', 'index', reportDesignerFilterDialogController]);

    /** @ngInject */
    function reportDesignerFilterDialogController($scope, $mdDialog, reportDesignerService, reportDefinition, index)
    {
        var controller = new mdBusinessLogic.dataAccess.controllers.reportDefinitionController();

        $scope.reportDesignerService = reportDesignerService;

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.formWizard = {};
        $scope.fieldEnabled = {
          Enabled: true
        }

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.save = function () {
            if (index >= 0) {
                $scope.reportDefinition.Definition.Filters[index] = $scope.filter;
            } else {
                $scope.reportDefinition.Definition.Filters.push($scope.filter);
            }
            $mdDialog.hide($scope.reportDefinition);
        };

        $scope.reportDefinition = reportDefinition;
        if (index >= 0) {
            $scope.filter = $scope.reportDefinition.Definition.Filters[index];
            for (var field in $scope.filter.Entity.Fields) {
                if ($scope.filter.Entity.Fields[field].Name == $scope.filter.Property.Name) {
                    $scope.filter.Property = $scope.filter.Entity.Fields[field];
                }
            }
        } else {
            $scope.filter = new mdBusinessLogic.dataAccess.entities.innerReportDefinitionFilter();
            $scope.filter.Type = undefined;
        }
    }
}());
