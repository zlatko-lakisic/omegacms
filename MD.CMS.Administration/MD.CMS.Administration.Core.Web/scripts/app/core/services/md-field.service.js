(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdFieldService', [mdFieldService]);

    /** @ngInject */
    function mdFieldService() {

        function transformField(field, folderPath) {
            if (folderPath === undefined || folderPath == null) {
                folderPath = '';
            }
            var numberValue = 0;
            try {
                numberValue = parseFloat(field.Value || 0)
            } catch (e) {
                console.log(e);
            }
            return {
                value: (field.Value || field.DefaultValue) || null,
                constraints: field.JsonField.constraints,
                numberValue: numberValue,
                type: field.AttributeTypeDefinition.InputType,
                required: field.IsRequired,
                listValue: field.ListValue,
                delimiter: field.Delimiter || ';',
                id: field.Id,
                order: field.Order,
                name: field.Name,
                uniqueId: field.UniqueId,
                defaultValue: field.DefaultValue,
                isReadOnly: field.IsReadOnly,
                jsonField: field.JsonField,
                friendlyName: field.FriendlyName,
                isSelector: [
                    mdBusinessLogic.dataAccess.entities.attributeTypeEnum.contentSelectorSingle,
                    mdBusinessLogic.dataAccess.entities.attributeTypeEnum.mediaContentSelectorSingle,
                    mdBusinessLogic.dataAccess.entities.attributeTypeEnum.taxonomySelectorMultiple,
                    mdBusinessLogic.dataAccess.entities.attributeTypeEnum.taxonomySelectorSingle,
                    mdBusinessLogic.dataAccess.entities.attributeTypeEnum.userSelectorSingle
                ].indexOf(field.AttributeTypeDefinition.InputType) >= 0,
                isYoutube: field.AttributeTypeDefinition.InputType == mdBusinessLogic.dataAccess.entities.attributeTypeEnum.youtube,
                folderPath: folderPath,
                dataBound: field.DataBound,
                contentTypeDefinitionId: field.ContentTypeDefinitionId,
                gridTileData: parseGridTileValues(field.AttributeTypeDefinition.InputType, field.JsonField.gridTileData)
            };
        }

        function transformOther(value, required, listValue, delimiter, id, name, uniqueId, defaultValue, jsonField, friendlyName, isSelector, folderPath) {
            var numberValue = 0;
            try {
                numberValue = parseFloat(value || 0)
            } catch (e) {
            }
            return {
                value: (value || defaultValue) || null,
                constraints: ((jsonField && jsonField.constraints) ? jsonField.constraints : {}),
                numberValue: numberValue,
                type: null,
                required: required,
                listValue: listValue || '',
                delimiter: delimiter || ';',
                id: id,
                order: 0,
                name: name || '',
                uniqueId: uniqueId,
                defaultValue: defaultValue,
                isReadOnly: false,
                jsonField: jsonField || null,
                friendlyName: friendlyName || name,
                isSelector: isSelector || false,
                isYoutube: false,
                folderPath: isSelector || '',
                gridTileData: parseGridTileValues(null)
            };
        }

        function parseGridTileValues(type, gridTileData) {
            if (gridTileData === undefined) {
                gridTileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();
            }

            if (type != null) {

                gridTileData.minHeight = 0;
                gridTileData.minWidth = 0;

                switch (type) {
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.input:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.trueFalse:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.selectSingle:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.selectMultiple:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.taxonomySelectorSingle:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.taxonomySelectorMultiple:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.date:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.contentSelectorSingle:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.section:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.mediaContentSelectorSingle:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.userSelectorSingle:
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.calculated:
                        gridTileData.minHeight = 1;
                        gridTileData.minWidth = 2;
                        break;
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.textarea:
                        gridTileData.minHeight = 3;
                        gridTileData.minWidth = 6;
                        break;
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.map:
                        gridTileData.minHeight = 5;
                        gridTileData.minWidth = 6;
                        break;
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.file:
                        gridTileData.minHeight = 3;
                        gridTileData.minWidth = 4;
                        break;
                    case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.youtube:
                        gridTileData.minHeight = 3;
                        gridTileData.minWidth = 5;
                        break;
                }

                if (parseInt(gridTileData.height) < gridTileData.minHeight) {
                    gridTileData.height = gridTileData.minHeight;
                }

                if (parseInt(gridTileData.width) == 0) {
                    gridTileData.width = 12;
                }

                if (parseInt(gridTileData.width) < gridTileData.minWidth) {
                    gridTileData.width = gridTileData.minWidth;
                }
            }

            return gridTileData;
        }

        return {
            transformField: transformField,
            transformOther: transformOther,
            parseGridTileValues: parseGridTileValues
        };

    }
}());
