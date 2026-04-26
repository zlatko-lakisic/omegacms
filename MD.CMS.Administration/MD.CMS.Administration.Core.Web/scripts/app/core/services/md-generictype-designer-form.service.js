(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdGenerictypeDesignerFormService', ['$q', 'mdGenerictypeDesignerElementConstatns', mdGenerictypeDesignerFormService]);

    /** @ngInject */
    function mdGenerictypeDesignerFormService($q, mdGenerictypeDesignerElementConstatns) {
        var attributeController = new mdBusinessLogic.dataAccess.controllers.attributeTypeDefinitionController();

        function sortFields(genericTypeObj) {
            function filter(parentId) {
                return genericTypeObj.Fields.filter(function (field) {
                    if (parentId === undefined) {
                        return field.JsonField.gridTileData.parentId === undefined || field.JsonField.gridTileData.parentId == null;
                    }
                    return field.JsonField.gridTileData.parentId == parentId;
                });
            }

            var fields = {
                parent: filter()
            };
            for (var i = 0; i < genericTypeObj.Fields.length; i++) {
                if (genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId !== undefined) {
                    fields[genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId] = filter(genericTypeObj.Fields[i].JsonField.gridTileData.uniqueId);
                }
            }

            return fields;
        }

        function switchFieldType(field, inputType) {
            return $q(function (resolve, reject) {
                attributeController.getByInputTypeId(inputType, function (data) {
                    field.AttributeTypeDefinition = data;
                    field.AttributeTypeDefinitionId = data.Id;
                    resolve(field);
                }, function (error) {
                    reject(error);
                });
            });
        }

        function addField(options, fields, genericTypeObj, x, y, element) {
            var defer = $q.defer();
            if (x !== undefined && y !== undefined && element !== undefined && element.length > 0) {
                var inputType = element.attr('data-id');
                var uniqueId = element.attr('data-gs-unique-id');
                var parentId = element.attr('data-parent-id');
                if (parentId !== undefined) {
                    for (var i = 0; i < genericTypeObj.Fields.length; i++) {
                        var field = genericTypeObj.Fields[i];
                        if (field.JsonField.gridTileData.uniqueId == uniqueId) {
                            field.JsonField.gridTileData.parentId = parentId;
                            break;
                        }
                    }
                    fields = sortFields(genericTypeObj);
                    $timeout(function () {
                        defer.resolve({
                            id: uniqueId
                        });
                    }, 500);
                } else {
                    attributeController.getByInputTypeId(inputType, function (data) {
                        var field = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionField();
                        field.AttributeTypeDefinition = data;
                        field.AttributeTypeDefinitionId = data.Id;
                        field.Name = mdBusinessLogic.globals.resources.Labels['Input-' + mdBusinessLogic.dataAccess.entities.attributeTypeEnum[data.InputType]];
                        field.DefaultValue = data.DefaultValue;
                        field.ContentTypeDefinitionId = genericTypeObj.Id;
                        var fieldValue = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionFieldValue(field);
                        fieldValue.ContentTypeDefinitionId = genericTypeObj.Id;
                        fieldValue.ContentTypeDefinitionFieldId = field.Id;
                        fieldValue.LCID = genericTypeObj.LCID;
                        fieldValue.JsonField.gridTileData.x = x;
                        fieldValue.JsonField.gridTileData.y = y;
                        fieldValue.JsonField.gridTileData.setWidth(mdGenerictypeDesignerElementConstatns.getWidth(mdGenerictypeDesignerElementConstatns.getControlByEnum(mdBusinessLogic.dataAccess.entities.attributeTypeEnum[data.InputType])));
                        fieldValue.JsonField.gridTileData.setHeight(mdGenerictypeDesignerElementConstatns.getHeight(mdGenerictypeDesignerElementConstatns.getControlByEnum(mdBusinessLogic.dataAccess.entities.attributeTypeEnum[data.InputType])));
                        genericTypeObj.Fields.push(fieldValue);
                        if (fieldValue.JsonField.gridTileData.parentId === undefined) {
                            options.showEditDialog(null, genericTypeObj.Fields.length - 1, genericTypeObj, options.databoundReady, options.databoundReady ? options.allDataBoundTypes : null);
                        }
                        fields = sortFields(genericTypeObj);
                        options.$scope.$watch(function () {
                            return fieldValue.JsonField.gridTileData.uniqueId;
                        }, function (uniqueId) {
                            if (uniqueId !== undefined) {
                                defer.resolve({
                                    id: uniqueId
                                });
                            }
                        });
                    }, function (error) {
                    });
                }
            } else {
                defer.resolve(false);
            }
            return defer.promise;
        }

        function removeField(fields, genericTypeObj, data) {
            var defer = $q.defer();
            for (var i = genericTypeObj.Fields.length - 1; i >= 0; i--) {
                if (genericTypeObj.Fields[i].JsonField.gridTileData.id == data.id) {
                    genericTypeObj.Fields.splice(i, 1);
                    break;
                }
            }
            fields = sortFields(genericTypeObj);
            defer.resolve();
            return defer.promise;
        }

        function updateOptionsField(fields, genericTypeObj, items) {
            var defer = $q.defer();
            for (var it = 0; it < items.length; it++) {
                var id = items[it].el.id;
                for (var i = genericTypeObj.Fields.length - 1; i >= 0; i--) {
                    if (genericTypeObj.Fields[i].JsonField.gridTileData.id == id) {
                        genericTypeObj.Fields[i].setOptions(genericTypeObj.Fields[i].JsonField);
                    }
                }
            }
            fields = sortFields(genericTypeObj);
            defer.resolve();
            return defer.promise;
        }

        function onTileEvent(options, fields, genericTypeObj, event, data) {
            var promise = null;
            switch (event) {
                case 'added':
                    for (var i = 0; i < data.items.length; i++) {
                        promise = addField(options, fields, genericTypeObj, data.items[i].x, data.items[i].y, angular.element(data.items[i].el));
                    }
                    break;
                case 'removed':
                    promise = removeField(fields, genericTypeObj, data);
                    break;
                case 'change':
                    promise = updateOptionsField(fields, data.items);
                    break;
                default:
                    var defer = $q.defer();
                    defer.resolve();
                    promise = defer.promise;
            }
            return promise;
        }

        function loadControlAttributeData() {
            var promises = [];
            for (var groupKey in mdGenerictypeDesignerElementConstatns.controlLabels) {
                for (var key in mdGenerictypeDesignerElementConstatns.controls[groupKey]) {
                    var item = mdGenerictypeDesignerElementConstatns.controls[groupKey][key];
                    promises.push($q(function (resolve, reject) {
                        attributeController.getByInputTypeId(item.id, function (data) {
                            resolve({
                                data: data,
                                success: true
                            });
                        }, function (error) {
                            resolve({
                                data: null,
                                error: error,
                                success: false
                            });
                        });
                    }));
                }
            }
            return $q.all(promises);
        }

        var service = {
            sortFields: sortFields,
            onTileEvent: onTileEvent,
            switchFieldType: switchFieldType,
            loadControlAttributeData: loadControlAttributeData
        };

        return service;
    }
}());