
(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdGenerictypeDesignerElementController', ['$scope', '$timeout', 'mdGenerictypeDesignerElementConstatns', 'mdFieldService', mdGenerictypeDesignerFormController]);
    function mdGenerictypeDesignerFormController($scope, $timeout, mdGenerictypeDesignerElementConstatns, mdFieldService) {

        //Private Attributes
        var vm = this; 

        //Public Attributes
        vm.gridTileLayout = mdBusinessLogic.dataAccess.entities.grid.gridTileLayout;
        vm.editMode = $scope.mdEditMode;
        vm.index = $scope.mdIndex;
        vm.field = $scope.mdField;
        vm.field.JsonField.gridTileData = mdFieldService.parseGridTileValues(vm.field.AttributeTypeDefinition.InputType, vm.field.JsonField.gridTileData);
        vm.mdGenerictypeDesignerElementConstatns = mdGenerictypeDesignerElementConstatns;
        vm.attributeTypeEnum = mdBusinessLogic.dataAccess.entities.attributeTypeEnum;
        vm.maxHeight = undefined;
        vm.options = {
            acceptWidgets: false
        };
        vm.collapsed = false;


        //Public Methods
        vm.showEditDialog = showEditDialog;
        vm.registerToggleEditEvent = registerToggleEditEvent;
        vm.mdOnTileEvent = mdOnTileEvent;
        vm.showElementEditDialog = showElementEditDialog;

        function mdOnTileEvent(event, data) {
            if ($scope.mdOnTileEvent !== undefined && data.items !== undefined) {

                if (data.items[0].el !== undefined) {
                    var inputType = angular.element(data.items[0].el).data('inputType');
                    angular.element(data.items[0].el).attr('data-id', inputType);

                    angular.element(data.items[0].el).attr('data-parent-id', vm.field.JsonField.gridTileData.uniqueId);
                }

                return $scope.mdOnTileEvent({ event: event, data: data });
            }
            return null;
        }

        function showEditDialog(ev, id) {
            $scope.mdShowEditDialog({
                ev: ev,
                id: id
            });
        }

        function showElementEditDialog(ev, id) {
            $scope.mdShowEditDialog({
                ev: ev,
                id: id
            });
        }

        function registerToggleEditEvent(event) {
            $scope.mdOnRegisterEditEvent()(event);
        }

        function mdCmsGridOptions_onDragStart(grid, callback) {
            function action(event, ui) {
                callback(event, ui);
            }
            return action;
        }

        function mdCmsGridOptions_onDragEnd(grid, callback) {
            function action(event, ui) {
                var placeholder = ui.helper.siblings('.grid-stack-placeholder.grid-stack-item');
                ui.helper.attr('data-gs-y', placeholder.attr('data-gs-y'));
                ui.helper.attr('data-gs-x', placeholder.attr('data-gs-x'));
                placeholder.remove();
                callback(event, ui);
                $timeout(function () {
                    //ui.helper.attr('style', '');
                }, 500);
            }
            return action;
        }

        function mdCmsGridOptions_onResize(grid, callback) {
            function action(event, ui) {
                callback(event, ui);
            }
            return action;
        }

        function mdCmsGridOptions_onDragOrResize(grid, callback) {
            function action(event, ui) {

                function outOfBounds() {
                    var gridWidth = angular.element(grid.el).width();
                    var gridHeight = angular.element(grid.el).height();
                    return ui.position.left < -50 ||
                        ui.position.top < -50 ||
                        ui.position.right > gridWidth + 50 ||
                        ui.position.bottom > gridHeight + 50;
                }

                if (outOfBounds()) {
                    ui.helper.removeClass('md-generictype-designer-element-nested');
                    ui.helper.addClass('md-generictype-designer-element-parent');
                } else {
                    ui.helper.addClass('md-generictype-designer-element-nested');
                    ui.helper.removeClass('md-generictype-designer-element-parent');
                }
                callback(event, ui);
            }
            return action;
        }

        function init() {
            $scope.$watch(function () {
                return $scope.mdField;
            }, function (field) {
                if (field !== undefined) {
                    vm.field = field;
                }
            });

            $scope.$watch(function () {
                return vm.field;
            }, function (field) {
                if (field !== undefined) {
                    if (vm.field.AttributeTypeDefinition.InputType == vm.attributeTypeEnum.section) {
                        angular.element('#' + $scope.uniqueId).addClass('md-generictype-designer-element-nested-grid');
                    }
                }
            });

            $scope.$watch(function () {
                return $scope.mdFields;
            }, function (mdFields) {
                if (mdFields !== undefined) {
                    vm.fields = mdFields;
                }
            });

            $scope.$watch(function () {
                return $scope.mdEditMode;
            }, function (mdEditMode) {
                 if (mdEditMode !== undefined) {
                     vm.editMode = mdEditMode;
                }
            });

            $scope.$watch(function () {
                return vm.field.JsonField.gridTileData;
            }, function (tileData) {
                    if (tileData !== undefined) {
                        vm.field.Order = tileData.index;
                }
            }, true);

            $scope.$on('md-cms-grid-events-tile-updateindex', function (event, data) {
                if (data.id == vm.field.JsonField.gridTileData.uniqueId) {
                    vm.field.JsonField.gridTileData.index = data.index;
                    $scope.$emit("md-generictype-designer-element-updateindex", data); 
                }
            });
        }

        init();
    }
})();