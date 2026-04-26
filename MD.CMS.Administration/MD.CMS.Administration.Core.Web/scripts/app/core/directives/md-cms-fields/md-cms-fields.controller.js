(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsFieldsController', ['$scope', 'mdFieldService', mdCmsFieldsController]);
    /** @ngInject */
    function mdCmsFieldsController($scope, mdFieldService) {

        //Private Attributes
        var vm = this;
        var internalUpdate = false;

        //Public Attributes
        vm.fields = [];
        vm.attributeTypeEnum = mdBusinessLogic.dataAccess.entities.attributeTypeEnum;
        vm.calculateEvents = [];
        vm.showSubSections = false;
        vm.layout = {};
        vm.currentDepth = 1;
        vm.nestedDepth = 0;
        vm.disabled = false;
        vm.mdFields = [];
        vm.parentId = undefined;
        vm.folderPath = '';
        vm.formName = '';
        vm.textAreaOptions = '';
        vm.registerUploadEvents = '';
        vm.reinitEventName = '';
        
        //Public Methods
        vm.registerUploadEvent = registerUploadEvent;
        vm.registerCalculateEvent = registerCalculateEvent;
        vm.onChangeEvent = onChangeEvent;
        vm.registerOnSaveEvent = registerOnSaveEvent;
        vm.getStyleForField = getStyleForField;
        vm.onSave = function () { };

        //Private Methods


        function onChangeEvent() {
            for (var i in vm.calculateEvents) {
                vm.calculateEvents[i]();
            }
        }

        function registerCalculateEvent(event) {
            if (vm.calculateEvents !== undefined) {
                vm.calculateEvents.push(event);
            }
        }

        function registerUploadEvent(event) {
            if (vm.registerUploadEvents !== undefined && angular.isFunction(vm.registerUploadEvents())) {
                vm.registerUploadEvents()(event);
            }
        }

        function transformFieldsBack() {
            internalUpdate = true;
            for (var i = 0; i < vm.fields.length; i++) {
                for (var f = 0; f < vm.mdFields.length; f++) {
                    if (vm.mdFields[f].Id == vm.fields[i].id) {
                        vm.mdFields[f].Value = vm.fields[i].value;
                        break
                    }
                }
            }
            $scope.mdFields = vm.mdFields;
            internalUpdate = false;
        }

        function registerOnSaveEvent(event) {
            if (vm.onSave !== undefined) {
                vm.onSave({
                    event: event
                });
            }
        }

        function getStyleForField(field, styleName, defaultValue) {
            var style = field.jsonField.style;
            if (style !== undefined && style != null) {
                var styleObject = style[vm.attributeTypeEnum[field.type]];
                if (styleObject !== undefined && styleObject != null) {
                    return styleObject[styleName];
                }
            }
            return defaultValue;
        }

        function parseFields(mdFields) {
            vm.fields = mdFields.filter(function (field) {
                return (
                    vm.parentId !== undefined &&
                    field.JsonField !== undefined &&
                    field.JsonField != null &&
                    field.JsonField.gridTileData !== undefined &&
                    field.JsonField.gridTileData != null &&
                    field.JsonField.gridTileData.parentId == vm.parentId
                ) || (
                    vm.parentId === undefined &&
                    field.JsonField !== undefined &&
                    field.JsonField != null &&
                    field.JsonField.gridTileData !== undefined &&
                    field.JsonField.gridTileData != null && (
                        field.JsonField.gridTileData.parentId === undefined ||
                        field.JsonField.gridTileData.parentId == null
                    )
                );
            }).map(function (field) {
                return mdFieldService.transformField(field, vm.mdFolderPath);
            });
        }

        function init() {

            $scope.$watchGroup([function () { return $scope.mdFields; }, function () { return $scope.mdParentId; }], function (data) {
                if (data[1] !== undefined) {
                    vm.parentId = data[1];
                }

                if (data[0] !== undefined && !internalUpdate) {
                    vm.mdFields = data[0];
                    parseFields(vm.mdFields);
                }
            });

            $scope.$watchGroup([
                function () { return $scope.mdNestedDepth; },
                function () { return $scope.mdCurrentDepth; },
                function () { return $scope.mdParentId; },
                function () { return $scope.mdFormName; },
                function () { return $scope.mdTextAreaOptions; },
                function () { return $scope.registerUploadEvents; },
                function () { return $scope.onSave; },
                function () { return $scope.reinitEventName; },
                function () { return $scope.layout; },
                function () { return $scope.mdFolderPath; },
                function () { return $scope.mdDisabled; }
            ], function (data) {
                    if (data[0] !== undefined) {
                        vm.nestedDepth = data[0];
                    }

                    if (data[1] !== undefined) {
                        vm.currentDepth = data[1];
                    }

                    if (data[2] !== undefined) {
                        vm.parentId = data[2];
                    }

                    if (data[3] !== undefined) {
                        vm.formName = data[3];
                    }

                    if (data[4] !== undefined) {
                        vm.textAreaOptions = data[4];
                    }

                    if (data[5] !== undefined) {
                        vm.registerUploadEvents = data[5];
                    }

                    if (data[6] !== undefined) {
                        vm.onSave = data[6];
                    }

                    if (data[7] !== undefined) {
                        vm.reinitEventName = data[7];
                    }

                    if (data[8] !== undefined) {
                        vm.layout = data[8];
                    }

                    if (data[9] !== undefined) {
                        vm.folderPath = data[9];
                    }

                    vm.disabled = data[10] === undefined ? false : data[10];
                    if (!vm.disabled) {
                        vm.onSave({
                            event: transformFieldsBack
                        });
                    }

                    if (vm.nestedDepth == 0 || vm.nestedDepth >= vm.currentDepth) {
                        vm.showSubSections = true;
                    }
            });
        } 

        init();
    }
})();
