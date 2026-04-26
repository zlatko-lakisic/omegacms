(function ()
{
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .controller('reportDesignerPreviewController', ['$scope', '$mdDialog', 'reportDefinition', reportDesignerPreviewController]);

    /** @ngInject */
    function reportDesignerPreviewController($scope, $mdDialog, reportDefinition)
    {
        var controller = new mdBusinessLogic.dataAccess.controllers.reportDefinitionController();

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.reportData = [];
        $scope.tableDataReady = false;
        $scope.tableDataValid = true;

        $scope.reportDefinition = reportDefinition;
        $scope.hasDynamicFilters = false;
        for (var i = 0; i < reportDefinition.Definition.Filters.length; i++) {
            if (reportDefinition.Definition.Filters[i].IsDynamic == true) {
                $scope.hasDynamicFilters = true;
                break;
            }
        }

        $scope.dataTableOptions = {
            dom: '<"top"f>rt<"bottom"<"left"<"length"l>><"right"<"info"i><"pagination"p>>>',
            pagingType: 'simple',
            pageLength: 10,
            lengthMenu: [10, 20, 50, 100],
            autoWidth: false,
            responsive: true
        };

        $scope.generateSampleData = function () {
            //before sending make sure JSON represents edited/new definition
            $scope.reportDefinition.Json = JSON.stringify($scope.reportDefinition.Definition)
            controller.getReportPreview($scope.reportDefinition, function (data) {
                $scope.$apply(function () {
                    $scope.reportData = data;
                    $scope.tableDataReady = true;
                    if ($scope.reportData != undefined && $scope.reportData != null) {
                        if ($scope.reportData.columns != undefined && $scope.reportData.columns != null) {
                            $scope.tableDataValid = $scope.reportData.columns.length > 0;
                        } else {
                            $scope.tableDataValid = false;
                        }
                    } else {
                        $scope.tableDataValid = false;
                    }
                });
            }, function (error) {
            });
        }

        if (!$scope.hasDynamicFilters) {
            $scope.generateSampleData();
        }
    }
}());
