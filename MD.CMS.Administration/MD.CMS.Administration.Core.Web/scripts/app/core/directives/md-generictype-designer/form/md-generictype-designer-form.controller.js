
(function () {
    'use strict';

    angular
        .module('app.core')
        .filter('mdGenerictypeDesignerFormFilter', mdGenerictypeDesignerFormFilter)
        .controller('mdGenerictypeDesignerFormController', ['$scope', '$mdDialog', 'mdGenerictypeDesignerElementConstatns', 'mdGenerictypeDesignerFormService', 'mdFieldService', '$q', '$timeout', mdGenerictypeDesignerFormController]);
    function mdGenerictypeDesignerFormFilter() {
        return function (fields, parentId) {
            if (fields === undefined || fields == null) {
                return [];
            }

            return fields.filter(function (field) {
                if (field.JsonField.gridTileData.parentId == null && parentId === undefined) {
                    return true;
                }
                return field.JsonField.gridTileData.parentId === parentId;
            });
        }
    }

    function mdGenerictypeDesignerFormController($scope, $mdDialog, mdGenerictypeDesignerElementConstatns, mdGenerictypeDesignerFormService, mdFieldService, $q, $timeout) {
        //Private Attributes
        var vm = this;
        var attributeController = new mdBusinessLogic.dataAccess.controllers.attributeTypeDefinitionController();
        var editEvent = null;
        var tileEditEvents = [];
        var updateTileDataModelEvent = null;
        var attributeList = {};

        //Public Attributes
        vm.gridTileLayout = mdBusinessLogic.dataAccess.entities.grid.gridTileLayout;
        vm.attributeTypeEnum = mdBusinessLogic.dataAccess.entities.attributeTypeEnum;
        vm.genericTypeObj = $scope.genericTypeObj
        vm.mdGenerictypeDesignerElementConstatns = mdGenerictypeDesignerElementConstatns;
        vm.editMode = false;
        vm.fields = {
            parent: []
        };
        vm.fieldsHierarchy = {
            root: {}
        };
        vm.options = {
            acceptWidgets: function (el) {
                return angular.element(el).hasClass('md-generictype-designer-element-parent-drop') || angular.element(el).hasClass('md-cms-grid-tile-new');
            }
        };
        vm.styleDependencies = ['scripts/app/core/directives/md-generictype-designer/form/md-generictype-designer-form-canvas.min.css'];
        vm.gridActions = [{
            id: 1,
            name: 'edit',
            icon: 'fa fa-pencil'
        }];
        vm.hideToolbar = { edit: true, save: true, cancel: true };
        vm.defailtGridTileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();
        vm.attributesLoaded = false;

        //Public Methods
        vm.querySearch = querySearch;
        vm.toggleSideNav = toggleSideNav;
        vm.showEditDialog = showEditDialog;
        vm.onTileEvent = onTileEvent;
        vm.registerToggleEditEvent = registerToggleEditEvent;
        vm.registerTileToggleEditEvent = registerTileToggleEditEvent;
        vm.registerUpdateTileDataModelEvent = registerUpdateTileDataModelEvent;
        vm.elementShowEditDialog = elementShowEditDialog;
        vm.showPreviewDialog = showPreviewDialog;

        //Private Methods
        function onTileEvent(event, data) {
            switch (event) {
                case 'add':
                    return addField(data);
                case 'remove':
                    return removeField(data);
                case 'moved':
                    return moveField(data);
                case 'render':
                    return $q(function (resolve, reject) {
                        var clone = $(data.blockTile.element).clone();
                        clone.find('*[data-value]').each(function (index, el) {
                            var key = $(el).attr('data-value');
                            $(el).text(data.obj.model.attributes.attributes.data[key]);
                        });
                        resolve(clone.html().replace('class="ng-hide"', ''));
                    });
                case 'resize':
                    return $q(function (resolve, reject) {
                        resolve(updateTileSize(data));
                    });
                case 'edit':
                    return showEditDialog(null, data, vm.genericTypeObj, $scope.databoundReady, $scope.databoundReady ? $scope.allDataBoundTypes : null);
                default:
                    return $q(function (resolve, reject) {
                        resolve();
                    });
            }
            return promise;
        }

        function array_move(arr, old_index, new_index) {
            if (new_index >= arr.length) {
                var k = new_index - arr.length + 1;
                while (k--) {
                    arr.push(undefined);
                }
            }
            arr.splice(new_index, 0, arr.splice(old_index, 1)[0]);
            return arr;
        };

        function sortFields() {
            function filter(parentId) {
                return vm.genericTypeObj.Fields.filter(function (field) {
                    if (parentId === undefined) {
                        return field.JsonField.gridTileData.parentId === undefined || field.JsonField.gridTileData.parentId == null;
                    }
                    return field.JsonField.gridTileData.parentId == parentId;
                });
            }


            for (var i = 0; i < vm.genericTypeObj.Fields.length; i++) {
                if (vm.genericTypeObj.Fields.filter(function (field) {
                    return field.JsonField.gridTileData.parentId == vm.genericTypeObj.Fields[i].JsonField.gridTileData.parentId;
                }).length == 0) {
                    vm.genericTypeObj.Fields[i].JsonField.gridTileData.parentId = null;
                }
            }

            vm.fields = {
                parent: filter()
            };
            for (var i = 0; i < vm.genericTypeObj.Fields.length; i++) {
                if (vm.genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId !== undefined) {
                    vm.fields[vm.genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId] = filter(vm.genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId);
                }
            }


            function sortHierarchy(data, parentId, level) {
                data.children = [];
                data.level = level === undefined ? 0 : level;
                for (var i = 0; i < vm.genericTypeObj.Fields.length; i++) {
                    if (vm.genericTypeObj.Fields[i].JsonField.gridTileData.parentId == parentId && vm.genericTypeObj.Fields[i].AttributeTypeDefinition.InputType == vm.attributeTypeEnum.section) {
                        var childObject = {};
                        childObject.id = vm.genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId;
                        childObject.name = vm.genericTypeObj.Fields[i].Name;
                        data.children.push(sortHierarchy(childObject, vm.genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId, data.level+1));
                    }
                }
                return data;
            }
            vm.fieldsHierarchy.root.id = 0;
            vm.fieldsHierarchy.root.name = 'Root';
            vm.fieldsHierarchy.root = sortHierarchy(vm.fieldsHierarchy.root);
        }

        function toggleEditMode(reinit) {

            function evt() {

            }

            if (!vm.attributesLoaded) {

            }
            $scope.$watch(function () {
                return vm.attributesLoaded;
            }, function (attributesLoaded) {
                if (attributesLoaded === true) {
                }
            });
            if (reinit === undefined) {
                reinit = false;
            }

            vm.editMode = !vm.editMode;
            editEvent(reinit);
            for (var i = 0; i < tileEditEvents.length; i++) {
                tileEditEvents[i](reinit);
            }
        }

        function querySearch(query) {
            var results = query ? $scope.icons.filter(createFilterFor(query)) : $scope.icons,
                deferred;
            return results;
        }

        function createFilterFor(query) {
            var lowercaseQuery = query.toLowerCase();

            return function filterFn(item) {
                return (item.indexOf(lowercaseQuery) === 0);
            };
        }

        function toggleSideNav() {
            $mdSidenav('controlToolBar').toggle();
        }

        function elementShowEditDialog(ev, id) {
            showEditDialog(ev, id, $scope.genericTypeObj, $scope.databoundReady, $scope.allDataBoundTypes);
        }

        function showEditDialog(ev, _data, genericTypeObj, databoundReady, allDataBoundTypes) {
            return $q(function (resolve, reject) {
                $mdDialog.show({
                    controller: 'mdGenerictypeDesignerFormEditDialogController as vm',
                    templateUrl: 'scripts/app/core/directives/md-generictype-designer/form/dialogs/edit/views/edit-dialog.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    locals: {
                        field: _data.data,
                        genericTypeObj: genericTypeObj,
                        databoundReady: databoundReady,
                        allDataBoundTypes: allDataBoundTypes,
                        fieldsHierarchy: vm.fieldsHierarchy
                    },
                    clickOutsideToClose: true,
                    fullscreen: true,
                    multiple: true
                }).then(function (data) {
                    for (var i = 0; i < genericTypeObj.Fields.length; i++) {
                        if (genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId == data.JsonField.gridTileData.uniqueId) {
                            genericTypeObj.Fields[i] = data;
                            //genericTypeObj.Fields[i].ListValue = vm.field.ListValue.join(vm.field.Delimiter);
                            break;
                        }
                    }
                    _data.data = data;
                    _data['data-name'] = data.Name;
                    _data['data-layout'] = vm.gridTileLayout[data.JsonField.gridTileData.layout];
                    sortFields(_data);
                    resolve(_data);
                }, function () { resolve(); });
            });
        }

        function showPreviewDialog(ev) {
            $mdDialog.show({
                controller: 'mdGenerictypeDesignerFormPreviewDialogController as vm',
                templateUrl: 'scripts/app/core/directives/md-generictype-designer/form/dialogs/preview/views/preview-dialog.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                locals: {
                    genericTypeObj: $scope.genericTypeObj
                },
                clickOutsideToClose: true,
                fullscreen: true,
                multiple: true
            }).then(function () {
            }, function () { });
        }

        function addField(_data) {
            var defer = $q.defer();
            var id = _data.element['data-type'];
            var index = _data.index;

            attributeController.getByInputTypeId(id, function (data) {
                var field = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionField();
                field.Id = -Math.floor(mdBusinessLogic.helpers.math.random() * 9999999);
                field.AttributeTypeDefinition = data;
                field.AttributeTypeDefinitionId = data.Id;
                field.Name = data.Name;
                field.DefaultValue = data.DefaultValue;
                field.ContentTypeDefinitionId = $scope.genericTypeObj.Id;
                var fieldValue = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionFieldValue(field);
                fieldValue.ContentTypeDefinitionId = $scope.genericTypeObj.Id;
                fieldValue.ContentTypeDefinitionFieldId = field.Id;
                fieldValue.LCID = $scope.genericTypeObj.LCID;
                fieldValue.JsonField.gridTileData.index = index;
                fieldValue.Order = index;
                fieldValue.JsonField.gridTileData.setWidth(mdGenerictypeDesignerElementConstatns.getWidth(mdGenerictypeDesignerElementConstatns.getControlByEnum(mdBusinessLogic.dataAccess.entities.attributeTypeEnum[data.InputType])));
                fieldValue.JsonField.gridTileData.setHeight(mdGenerictypeDesignerElementConstatns.getHeight(mdGenerictypeDesignerElementConstatns.getControlByEnum(mdBusinessLogic.dataAccess.entities.attributeTypeEnum[data.InputType])));
                fieldValue.JsonField.gridTileData.parentId = _data.parent.data === undefined ? null : _data.parent.data.JsonField.gridTileData.id;
                vm.genericTypeObj.Fields.push(fieldValue);
                sortFields();
                _data.element.data = fieldValue;
                _data.element['data-id'] = fieldValue.Id;
                _data.element['data-tiledata'] = fieldValue.JsonField.gridTileData;
                showEditDialog(null, _data.element, vm.genericTypeObj, $scope.databoundReady, $scope.databoundReady ? $scope.allDataBoundTypes : null).then(function (data) {
                    defer.resolve(data);
                }, function () {
                    defer.resolve();
                });
            }, function (error) {
                defer.reject(error);
            });

            /*if (element !== undefined && element.length > 0) {
                var inputType = element.attr('data-id');
                var uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                
            } else {
                defer.resolve(false);
            }*/
            return defer.promise;
        }

        function removeRecursive(parentId) {
            var array = vm.genericTypeObj.Fields.filter(function (field) { return field.JsonField.gridTileData.parentId == parentId; });
            for (var k = 0; k < array.length; k--) {
                var field = array[k];
                removeRecursive(field.Id);
                for (var i = vm.genericTypeObj.Fields.length - 1; i >= 0; i--) {
                    if (vm.genericTypeObj.Fields[i].Id == field.Id) {
                        vm.genericTypeObj.Fields.splice(i, 1);
                    }
                }
            }
        }

        function removeField(data) {
            return $q(function (resolve, reject) {
                for (var i = vm.genericTypeObj.Fields.length - 1; i >= 0; i--) {
                    if (vm.genericTypeObj.Fields[i].Id == data.element.data.Id) {
                        removeRecursive(vm.genericTypeObj.Fields[i].Id);
                        break;
                    }
                }

                for (var i = vm.genericTypeObj.Fields.length - 1; i >= 0; i--) {
                    if (vm.genericTypeObj.Fields[i].Id == data.element.data.Id) {
                        vm.genericTypeObj.Fields.splice(i, 1);
                        break;
                    }
                }
                sortFields();
                resolve();
            });
        }

        function moveField(data) {
            return $q(function (resolve, reject) {
                for (var i = vm.genericTypeObj.Fields.length - 1; i >= 0; i--) {
                    var field = vm.genericTypeObj.Fields[i];
                    if (field.JsonField.gridTileData.id == data.element['data-tiledata'].id) {
                        array_move(vm.genericTypeObj.Fields, i, data.index);
                        field.JsonField.gridTileData.index = data.index;
                        field.Order = data.index;
                        if (data.parent.data !== undefined) {
                            field.JsonField.gridTileData.parentId = data.parent['data-tiledata'].id;
                        } else {
                            field.JsonField.gridTileData.parentId = null;
                        }
                        data.element.data = field;
                        sortFields();
                        break;
                    }
                }

                resolve(data.element);
            });
        }

        function updateTileSize(data) {
            for (var i = vm.genericTypeObj.Fields.length - 1; i >= 0; i--) {
                var field = vm.genericTypeObj.Fields[i];
                if (field.Id == data.data['data-id']) {
                    var device = data.device == 'large' ? '' : data.device;
                    if (data.property == 'width') {
                        field.JsonField.gridTileData.setWidth(data.value, device);
                    } else {
                        field.JsonField.gridTileData.setHeight(data.value);
                    }
                    data.data.data = field;
                    return data.data;
                }
            }
            return null;
        }

        function updateOptionsField(items) {
            var defer = $q.defer();
            for (var it = 0; it < items.length; it++) {
                var id = items[it].el.id;
                for (var i = vm.genericTypeObj.Fields.length - 1; i >= 0; i--) {
                    if (vm.genericTypeObj.Fields[i].JsonField.gridTileData.id == id) {
                        vm.genericTypeObj.Fields[i].setOptions(vm.genericTypeObj.Fields[i].JsonField);
                    }
                }
            }
            sortFields();
            defer.resolve();
            return defer.promise;
        }

        function registerToggleEditEvent(event) {
            editEvent = event;
        }

        function registerTileToggleEditEvent(event) {
            tileEditEvents.push(event);
        }

        function registerUpdateTileDataModelEvent(event) {
            updateTileDataModelEvent = event;
        }

        function init() {
            mdGenerictypeDesignerFormService.loadControlAttributeData().then(function (data) {
                console.log(data);
                var arr = data.filter(function (item) { return item.success; });
                for (var i = 0; i < arr.length; i++) {
                    attributeList[arr[i].data.Id] = arr[i].data;
                }
                vm.attributesLoaded = true;
            }, function (error) {
                console.log(error);
            });

            $scope.$watch(function () {
                return $scope.genericTypeObj;
            }, function (newValue) {
                if (newValue !== undefined) {
                    vm.genericTypeObj = newValue;
                    for (var i = 0; i < vm.genericTypeObj.Fields.length; i++) {
                        vm.genericTypeObj.Fields[i].JsonField.gridTileData = mdFieldService.parseGridTileValues(vm.genericTypeObj.Fields[i].AttributeTypeDefinition.InputType, vm.genericTypeObj.Fields[i].JsonField.gridTileData);
                    }

                    sortFields();
                }
            });

            $scope.$watch(function () {
                return $scope.defaultTileWidth;
            }, function (defaultTileWidth) {
                if (!isNaN(defaultTileWidth)) {
                    vm.defailtGridTileData.setWidth(defaultTileWidth);
                }
            });

            $scope.$watch(function () {
                return $scope.defaultTileHeight;
            }, function (defaultTileHeight) {
                if (!isNaN(defaultTileHeight)) {
                    vm.defailtGridTileData.setHeight(defaultTileHeight);
                }
            });

            $scope.$watch(function () {
                return $scope.formName;
            }, function (formName) {
                if (formName !== undefined) {
                    vm.formName = formName;
                }
            });
            
            $scope.registerEditEvent()(toggleEditMode);

            $scope.$on('md-generictype-designer-element-updateindex', function (event, data) {
                for (var i = 0; i < vm.genericTypeObj.Fields.length; i++) {
                    if (vm.genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId == data.id) {
                        vm.genericTypeObj.Fields[i].JsonField.gridTileData.index = data.index;
                        vm.genericTypeObj.Fields[i].Order = data.index;
                    }
                }
            });

            /*$scope.$on('md-cms-grid-events-tile-added', function (event, data) {
                addField(data).then(function (result) {
                    $scope.$broadcast('md-cms-grid-reinit');
                });
            });*/

            /*$scope.$on('md-cms-grid-events-tile-removed', function (event, data) {
                removeField(data.draggedElement);
                $scope.$broadcast('md-cms-grid-events-tile-removed-cleanup');
            });*/

            $scope.$on('md-cms-grid-events-tile-query-data', function (event, parentData) {
                if (parentData.toolbar) {
                    var attr = attributeList[parentData.id];
                    if (attr !== undefined) {
                        var field = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionField();
                        field.Id = -Math.floor(mdBusinessLogic.helpers.math.random() * 9999999);
                        field.AttributeTypeDefinition = attr;
                        field.AttributeTypeDefinitionId = attr.Id;
                        field.Name = attr.Name;
                        field.DefaultValue = attr.DefaultValue;
                        field.ContentTypeDefinitionId = $scope.genericTypeObj.Id;
                        $scope.$broadcast('md-cms-grid-events-tile-query-data-toolbar-' + parentData.id, {
                            data: field,
                            tileData: field.JsonField.gridTileData
                        });
                    } else {
                        $scope.$broadcast('md-cms-grid-events-tile-query-data-toolbar-' + parentData.id, {
                            data: null,
                            tileData: null
                        });
                    }
                    /*attributeController.getByInputTypeId(parentData.id, function (data) {
                        var field = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionField();
                        field.Id = -Math.floor(mdBusinessLogic.helpers.math.random() * 9999999);
                        field.AttributeTypeDefinition = data;
                        field.AttributeTypeDefinitionId = data.Id;
                        field.Name = data.Name;
                        field.DefaultValue = data.DefaultValue;
                        field.ContentTypeDefinitionId = $scope.genericTypeObj.Id;
                        $scope.$broadcast('md-cms-grid-events-tile-query-data-toolbar-' + parentData.id, {
                            data: field,
                            tileData: field.JsonField.gridTileData
                        });
                    }, function (error) {
                        $scope.$broadcast('md-cms-grid-events-tile-query-data-toolbar-' + parentData.id, {
                            data: null,
                            tileData: null
                        });
                    });*/
                } else {
                    var field = vm.genericTypeObj.Fields.filter(function (field) { return field.Id == parentData.id; })[0];
                    $scope.$broadcast('md-cms-grid-events-tile-query-data-' + parentData.id, {
                        data: field,
                        tileData: field.JsonField.gridTileData
                    });
                }
            });
        }

        init();
    }
})();