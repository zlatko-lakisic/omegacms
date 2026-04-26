(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdGenerictypeDesignerForm', ['$mdSidenav', '$mdDialog', '$timeout', '$http', 'FriendlyNameMakerService', 'mdFeedbackService', mdGenerictypeDesignerForm]);

    /** @ngInject */
    function mdGenerictypeDesignerForm($mdSidenav, $mdDialog, $timeout, $http, FriendlyNameMakerService, mdFeedbackService) {
        return {
            require: 'ngModel',
            restrict: 'E',
            scope: {
                databoundReady: '=?',
                allDataBoundTypes: '=?',
                icons: '=?',
                genericTypeObj: '=?',
                registerEditEvent: '&',
                defaultTileWidth: '@',
                defaultTileHeight: '@',
                formName: '@'
            },
            templateUrl: 'scripts/app/core/directives/md-generictype-designer/form/md-generictype-designer-form.html',
            controller: 'mdGenerictypeDesignerFormController as vm',
            link: function ($scope, element, attrs, ngModel) {

                $scope.$watch(function () {
                    return ngModel.$modelValue;
                }, function (newValue) {
                    $scope.genericTypeObj = newValue;
                });

                if ($scope.databoundReady === undefined) {
                    $scope.databoundReady = true;
                }

                if (isNaN($scope.defaultTileWidth)) {
                    $scope.defaultTileWidth = 30;
                }

                if (isNaN($scope.defaultTileHeight)) {
                    $scope.defaultTileHeight = 150;
                }

                if ($scope.icons === undefined) {
                    $scope.icons = [];

                    $http.get('assets/icons/selection.json').then(function (response) {
                        $scope.icons = response.data.icons.map(function (icon) {
                            return icon.properties.name;
                        });
                    });
                }

                if ($scope.databoundReady && $scope.allDataBoundTypes === undefined) {
                    var contentTypeDataSourceController = new mdBusinessLogic.dataAccess.controllers.contentTypeDataSourceController();
                    contentTypeDataSourceController.getAllDatabaseTypes(function (data) {
                        $scope.allDataBoundTypes = data;
                    }, function (error) {
                    });
                }

                // Add class
                element.addClass('md-generictype-designer-form');
            }
        };
    }
})();
