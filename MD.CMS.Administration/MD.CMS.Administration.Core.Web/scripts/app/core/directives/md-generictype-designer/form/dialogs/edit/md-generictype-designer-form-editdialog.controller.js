
(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdGenerictypeDesignerFormEditDialogController', ['$scope', '$mdDialog', '$timeout', 'field', 'genericTypeObj', 'databoundReady', 'allDataBoundTypes', 'fieldsHierarchy', '$q', 'mdFeedbackService', 'mdGenerictypeDesignerFormService', mdGenerictypeDesignerFormEditDialogController]);
    function mdGenerictypeDesignerFormEditDialogController($scope, $mdDialog, $timeout, field, genericTypeObj, databoundReady, allDataBoundTypes, fieldsHierarchy, $q, mdFeedbackService, mdGenerictypeDesignerFormService) {

        //Private Attributes
        var vm = this;
        var contentTypeDataSourceController = new mdBusinessLogic.dataAccess.controllers.contentTypeDataSourceController();
        var attributeController = new mdBusinessLogic.dataAccess.controllers.attributeTypeDefinitionController();
        var onBeforeSaveEvent = function () { return null; };
        var onBeforeCancelEvent = function () { return null; };
        var onAfterSaveEvent = function () { return null; };
        var onAfterCancelEvent = function () { return null; };
        var originalDataSourceObjectCopy = {};

        //Public Attributes
        vm.attributeTypeEnum = mdBusinessLogic.dataAccess.entities.attributeTypeEnum;
        vm.attributeTypeEnumStrings = {};
        vm.databoundReady = databoundReady;
        vm.selectedField = '';
        vm.searchText = '';
        vm.formula = '';
        vm.field = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionField(angular.copy(field));
        vm.field.Delimiter = ';';
        vm.field.Id = vm.field.Id == 0 ? Math.round(-1 * (mdBusinessLogic.helpers.math.random() * 100)) : vm.field.Id;
        vm.fields = genericTypeObj.Fields.filter(function (field) { return field.IsDataBoundPrimaryKey; });
        vm.rightJoinFieldsLoaded = false;
        vm.rightJoinFields = {};
        vm.rightDataSource = {};
        vm.regex = buildRegex(vm.field.JsonField.validation);
        vm.listValues = [];
        vm.oldListValue = '';
        vm.isRepeatable = vm.field.AttributeTypeDefinition.InputType == 1;
        vm.hasMaxLength = ([1, 3]).indexOf(vm.field.AttributeTypeDefinition.InputType) != -1;
        vm.hasMinLength = ([1, 3]).indexOf(vm.field.AttributeTypeDefinition.InputType) != -1;
        vm.hasCharacterTypes = ([1, 3]).indexOf(vm.field.AttributeTypeDefinition.InputType) != -1;
        vm.hasListValues = ([4, 5]).indexOf(vm.field.AttributeTypeDefinition.InputType) != -1;
        vm.isCalculated = vm.field.AttributeTypeDefinition.Name == 'Calculated';
        vm.selectedDataBoundType = '';
        vm.originalDelimiter = vm.field.Delimiter;
        vm.fieldsHierarchy = [];
        vm.parentId = 0;
        vm.originalParentId = 0;
        vm.defaultConstraint = null;



        //Public Methods
        vm.registerBeforeSaveEvent = registerBeforeSaveEvent;
        vm.registerBeforeCancelEvent = registerBeforeCancelEvent;
        vm.registerAfterSaveEvent = registerAfterSaveEvent;
        vm.registerAfterCancelEvent = registerAfterCancelEvent;
        vm.save = save;
        vm.cancel = cancel;
        vm.queryDroppedFields = queryDroppedFields;
        vm.newChip = newChip;
        vm.toggleAddEditMode = toggleAddEditMode;
        vm.onListValuesChanged = onListValuesChanged;
        vm.onListValueChanged = onListValueChanged;
        vm.onListDelimiterChanged = onListDelimiterChanged;
        vm.changeSelectedDataSource = changeSelectedDataSource;
        vm.editDataSource = editDataSource;
        vm.toggleAddJoin = toggleAddJoin;
        vm.saveDataSourceJoin = saveDataSourceJoin;
        vm.datasourceJoinExists = datasourceJoinExists;
        vm.saveDataSource = saveDataSource;
        vm.cancelDataSource = cancelDataSource;
        vm.deleteDataSource = deleteDataSource;
        vm.changeSelectedDataBoundType = changeSelectedDataBoundType;
        vm.addMetadata = addMetadata;
        vm.removeMetaData = removeMetaData;
        vm.getStyleWhiteframeClass = getStyleWhiteframeClass;


        //Private Methods
        function getStyleWhiteframeClass(whiteFrameWidth) {
            var resultClass = {
                'layout-padding': vm.field.JsonField.gridTileData.layoutPadding,
                'layout-margin': vm.field.JsonField.gridTileData.layoutMargin,
                'layout-wrap': vm.field.JsonField.gridTileData.layoutWrap
            };
            resultClass['md-whiteframe-' + whiteFrameWidth + 'dp'] = true
            var gridTileLayout = mdBusinessLogic.dataAccess.entities.grid.gridTileLayout[vm.field.JsonField.gridTileData.layout];
            if (gridTileLayout) {
                resultClass['layout-' + gridTileLayout.toLowerCase()] = true;
            }
            return resultClass;
        }

        function addMetadata() {
            if (vm.field.JsonField.metadata == null) {
                vm.field.JsonField.metadata = [];
            }
            vm.field.JsonField.metadata.push({
                Key: 'New Metadata Key',
                Value: 'New Metadata Value'
            });
        }
        function removeMetaData(index) {
            vm.field.JsonField.metadata.splice(index, 1);
        }

        function registerBeforeSaveEvent(event) {
            onBeforeSaveEvent = event;
        }

        function registerBeforeCancelEvent(event) {
            onBeforeCancelEvent = event;
        }

        function registerAfterSaveEvent(event) {
            onAfterSaveEvent = event;
        }

        function registerAfterCancelEvent(event) {
            onAfterCancelEvent = event;
        }

        function save() {
            vm.field.JsonField.setDefaultConstraint({
                folderPaths: vm.defaultConstraint.folderPaths.value.split(vm.defaultConstraint.folderPaths.delimiter),
                contentIds: vm.defaultConstraint.contentIds.value.split(vm.defaultConstraint.contentIds.delimiter),
                userIds: vm.defaultConstraint.userIds.value.split(vm.defaultConstraint.userIds.delimiter),
                profileId: vm.defaultConstraint.profileId.value,
                contentTypeId: vm.defaultConstraint.contentTypeId.value,
                taxonomyIds: vm.defaultConstraint.taxonomyIds.value.split(vm.defaultConstraint.taxonomyIds.delimiter),
                menuPaths: vm.defaultConstraint.menuPaths.value.split(vm.defaultConstraint.menuPaths.delimiter)
            });
            vm.field.JsonField.gridTileData.parentId = vm.parentId == 0 ? null : vm.parentId;
            vm.field.JsonField.validation.Regex = vm.regex;
            $mdDialog.hide(vm.field);
        }

        function cancel() {
            $mdDialog.cancel();
        }

        function queryDroppedFields(searchText) {
            var lowerSearch = searchText.toLowerCase();
            if (lowerSearch.substring(0, 6) != 'field.') {
                return [];
            }

            lowerSearch = lowerSearch.substring(7);

            return genericTypeObj.Fields.filter(function (element) {
                return element.Name && element.Name.toLowerCase().indexOf(lowerSearch) !== -1 && element.Name != vm.field.Name;
            });

        }

        function newChip(chip) {
            return {
                name: chip.Name ? chip.Name : chip,
                unicateValue: new Date(),
                type: getCalculatedType(chip)
            };
        }

        function toggleAddEditMode(isNew) {
            if (isNew === undefined) {
                isNew = false;
            }
            if (isNew && databoundReady) {
                vm.dataSourceModel = new mdBusinessLogic.dataAccess.entities.contentTypeDataSource();
            }
            vm.isAddEditMode = !vm.isAddEditMode;
            if (vm.isAddEditMode && vm.dataSourceModel.DbType != '') {
                vm.selectedDataBoundType = vm.dataSourceModel.DbType;
            }
        }

        function onListValuesChanged(scope, $event) {
            vm.field.ListValue = vm.listValues.join(vm.field.Delimiter);
        }

        function onListValueChanged(scope, $event) {
            vm.listValues = vm.field.ListValue.split(vm.field.Delimiter);
        }

        function onListDelimiterChanged(scope, $event) {
            if (vm.field.Delimiter != '') {
                vm.field.ListValue = vm.field.ListValue.replace(vm.originalDelimiter, vm.field.Delimiter);
                vm.originalDelimiter = vm.field.Delimiter;
                onListValuesChanged(scope, $event);
            }
        }

        function getCalculatedType(chip) {
            if (chip.Name) {
                return "field"
            } else {
                return "notField"
            }
        }

        function getCalculatedValue() {
            var value = "";
            for (var i in vm.formula) {
                if (mdBusinessLogic.helpers.checkType.isObject(vm.formula[i])) {
                    if (vm.formula[i].type == 'field' && !vm.formula[i].old) {
                        value += "field." + vm.formula[i].name + ",";
                    } else if (vm.formula[i].type == 'notField') {
                        value += vm.formula[i].name + ","
                    }
                }
            }
            vm.field.DefaultValue = value;
        }

        function getFields(dataSource, success, error) {
            if (dataSource !== undefined && dataSource != null) {
                vm.fieldsLoading = true;
                contentTypeDataSourceController.getDataStructure(dataSource, function (data) {
                    $scope.$apply(function () {
                        if (success !== undefined) {
                            success(data);
                        }
                        vm.fieldsLoading = false;
                    });
                }, function (_error) {
                    $scope.$apply(function () {
                        vm.fieldsLoading = false;
                        if (error !== undefined) {
                            error(_error);
                        }
                    });
                });
            }
        }

        function changeSelectedDataSource() {
            vm.selectedDataSource.selected = genericTypeObj.DataSources.filter(function (ds) { if (ds.Id == vm.selectedDataSource.selectedId) { return ds; } })[0];
            getFields(vm.selectedDataSource.selected, function (data) {
                vm.selectedDataSource.fields = {};
                for (var key in data) {
                    vm.selectedDataSource.fields[key] = data[key].map(function (item) {
                        return {
                            raw: item,
                            friendly: item.split('.').join(' - ')
                        }
                    });
                }
                vm.field.DataSourceId = vm.selectedDataSource.selected.Id;
            }, function (error) {
                mdFeedbackService.reportError('load', error);
            });
        }

        function editDataSource(datasource) {
            vm.toggleAddEditMode();
            vm.dataSourceModel = datasource;
            if (vm.dataSourceModel.DbType != '') {
                vm.selectedDataBoundType = vm.dataSourceModel.DbType;
            }
        }

        function toggleAddJoin(dataSource, isEdit) {
            vm.selectedJoin = new mdBusinessLogic.dataAccess.entities.contentTypeDataSourceJoin();
            if (isEdit === undefined) {
                isEdit = false;
            }
            if (isEdit) {
                var join = genericTypeObj.Joins.map(function (join) {
                    if (join.RightDataSourceId = dataSource.Id) {
                        return join;
                    }
                })[0];
                vm.selectedJoin.LeftFieldId = join.LeftFieldId;
                vm.selectedJoin.RightFieldId = join.RightFieldId;
            }
            vm.rightDataSource = dataSource;
            vm.isJoinAddMode = true;
        }

        function saveDataSourceJoin() {
            vm.selectedJoin.RightDataSourceId = vm.rightDataSource.Id;
            vm.selectedJoin.LeftRightDataSourceJoinType = '==';
            var isEdit = false;
            genericTypeObj.Joins.forEach(function (join) {
                if (join.RightDataSourceId == vm.rightDataSource.Id) {
                    isEdit = true;
                    join.LeftFieldId = vm.selectedJoin.LeftFieldId;
                    join.RightFieldId = vm.selectedJoin.RightFieldId;
                }
            });
            if (!isEdit) {
                genericTypeObj.Joins.push(JSON.parse(JSON.stringify(vm.selectedJoin)));
            }
            vm.allDataSources = JSON.parse(JSON.stringify(vm.allDataSources));
            vm.isJoinAddMode = false;
        }

        function datasourceJoinExists(datasource) {
            return datasource !== undefined && genericTypeObj.Joins.map(function (join) {
                return join.RightDataSourceId = datasource.Id;
            }).length > 0;
        }

        function saveDataSource() {
            if (onBeforeSaveEvent !== undefined && onBeforeSaveEvent != null) {
                try {
                    var result = onBeforeSaveEvent();
                    vm.dataSourceModel = result;
                }
                catch (error) {
                    mdFeedbackService.reportError('load', error);
                }
            }

            vm.dataSourceModel.ConnectionString = JSON.stringify(vm.dataSourceModel.ConnectionStringObject);
            var dataSource = new mdBusinessLogic.dataAccess.entities.contentTypeDataSource(vm.dataSourceModel);
            getFields(dataSource, function (data) {
                if (dataSource.Id != 0) {
                    vm.allDataSources.forEach(function (ds) {
                        if (ds.Id == dataSource) {
                            ds = dataSource;
                        }
                    });
                } else {
                    dataSource.Id = Math.round(-1 * (mdBusinessLogic.helpers.math.random() * 100));
                    genericTypeObj.DataSources.push(dataSource);
                    vm.allDataSources = genericTypeObj.DataSources;
                }

                var canClose = true;
                if (onAfterSaveEvent !== undefined && onAfterSaveEvent != null) {
                    try {
                        onAfterSaveEvent();
                    }
                    catch (error) {
                        canClose = false;
                        mdFeedbackService.reportError('load', error);
                    }
                }

                if (canClose) {
                    vm.toggleAddEditMode();
                }

                vm.changeSelectedDataSource();
            }, function (error) {
                mdFeedbackService.reportError('load', error);
            });
        }

        function cancelDataSource() {
            if (onBeforeCancelEvent !== undefined && onBeforeCancelEvent != null) {
                try {
                    onBeforeCancelEvent();
                }
                catch (error) {
                    mdFeedbackService.reportError('load', error);
                }
            }

            vm.dataSourceModel = originalDataSourceObjectCopy;

            vm.toggleAddEditMode();

            if (onAfterCancelEvent !== undefined && onAfterCancelEvent != null) {
                try {
                    onAfterCancelEvent();
                }
                catch (error) {
                    mdFeedbackService.reportError('load', error);
                }
            }
        }

        function deleteDataSource(dataSource) {
            vm.allDataSources = vm.allDataSources.filter(function (ds) {
                return ds.Id != dataSource.Id;
            });
        }

        function changeSelectedDataBoundType() {
            if (vm.dataSourceModel.DbType != '') {
                vm.selectedDataBoundType = vm.dataSourceModel.DbType;
                vm.dataSourceModel.ConnectionStringObject = new mdBusinessLogic.dataAccess.entities[vm.dataSourceModel.DbType + 'ConnectionString']();
                if (vm.dataSourceModel.ConnectionString != '') {
                    try {
                        vm.dataSourceModel.ConnectionStringObject = new mdBusinessLogic.dataAccess.entities[vm.dataSourceModel.DbType + 'ConnectionString'](JSON.parse(vm.dataSourceModel.ConnectionString));
                    } catch (e) {
                        //Do nothing
                    }
                }
                vm.selectedDataBoundType = vm.dataSourceModel.DbType;
            }
        }

        function configureData() {

            vm.listValues = vm.field.getListValueAsArray();

            if (databoundReady) {
                vm.allDataBoundTypes = allDataBoundTypes;
                vm.allDataSources = genericTypeObj.DataSources;
                vm.fieldsLoading = false;
                vm.isAddEditMode = false;
                vm.isJoinAddMode = false;
                vm.selectedJoin = new mdBusinessLogic.dataAccess.entities.contentTypeDataSourceJoin();
                vm.dataSourceModel = getSelectedDataSource(genericTypeObj.DataSources, genericTypeObj.Instance.DataSourceId);
                if (vm.dataSourceModel == undefined || vm.dataSourceModel == null) {
                    vm.dataSourceModel = new mdBusinessLogic.dataAccess.entities.contentTypeDataSource();
                }
                var selectedDataSources = JSON.parse(JSON.stringify(genericTypeObj.DataSources.filter(function (ds) { if (ds.Id == vm.field.DataSourceId) { return ds; } })));
                vm.selectedDataSource = {
                    selected: selectedDataSources.length > 0 ? selectedDataSources[0] : null,
                    selectedId: selectedDataSources.length > 0 ? selectedDataSources[0].Id : null,
                    fields: null
                };

                if (vm.dataSourceModel.ConnectionString != '') {
                    try {
                        vm.dataSourceModel.ConnectionStringObject = new mdBusinessLogic.dataAccess.entities[vm.dataSourceModel.DbType + 'ConnectionString'](JSON.parse(vm.dataSourceModel.ConnectionString));
                    } catch (e) {
                        //Do nothing
                    }
                }

                if (selectedDataSources.length > 0) {
                    vm.changeSelectedDataSource();
                }

                vm.selectedDataBoundType = vm.dataSourceModel.DbType;

                originalDataSourceObjectCopy = new mdBusinessLogic.dataAccess.entities.contentTypeDataSource(JSON.stringify(vm.dataSourceModel));
            }

            vm.defaultConstraint = {
                folderPaths: {
                    value: vm.field.JsonField.getDefaultConstraint().folderPaths.join(';'),
                    delimiter: ';'
                },
                contentIds: {
                    value: vm.field.JsonField.getDefaultConstraint().contentIds.join(';'),
                    delimiter: ';'
                },
                userIds: {
                    value: vm.field.JsonField.getDefaultConstraint().userIds.join(';'),
                    delimiter: ';'
                },
                taxonomyIds: {
                    value: vm.field.JsonField.getDefaultConstraint().taxonomyIds.join(';'),
                    delimiter: ';'
                },
                menuPaths: {
                    value: vm.field.JsonField.getDefaultConstraint().menuPaths.join(';'),
                    delimiter: ';'
                },
                contentTypeId: {
                    value: vm.field.JsonField.getDefaultConstraint().contentTypeId,
                    delimiter: ';'
                },
                profileId: {
                    value: vm.field.JsonField.getDefaultConstraint().profileId,
                    delimiter: ';'
                }
            };

            $scope.$watch(function () { return vm.field.JsonField.validation; }, function (validation) {
                vm.regex = buildRegex(vm.field.JsonField.validation);
            }, true);

            function hierarchyRecursiveFlattener(data) {
                if (data !== undefined) {
                    vm.fieldsHierarchy.push({
                        name: data.name,
                        id: data.id,
                        level: data.level
                    });
                    for (var i = 0; i < data.children.length; i++) {
                        hierarchyRecursiveFlattener(data.children[i]);
                    }
                }
            }

            hierarchyRecursiveFlattener(fieldsHierarchy.root);

            vm.parentId = vm.field.JsonField.gridTileData.parentId === undefined || vm.field.JsonField.gridTileData.parentId == null ? 0 : vm.field.JsonField.gridTileData.parentId;
            vm.originalParentId = vm.parentId;


            if (vm.isCalculated) {
                $timeout(function () {

                    var editor = ace.edit('calculated-formula', {
                        mode: "ace/mode/javascript",
                        selectionStyle: "text",
                        maxLines: 1, // make it 1 line
                        autoScrollEditorIntoView: true,
                        highlightActiveLine: false,
                        printMargin: false,
                        showGutter: false,
                        enableBasicAutocompletion: true,
                        enableSnippets: true,
                        enableLiveAutocompletion: false,
                        fontSize: 16
                    });

                    editor.on("paste", function (e) {
                        e.text = e.text.replace(/[\r\n]+/g, " ");
                    });

                    editor.renderer.screenToTextCoordinates = function (x, y) {
                        var pos = this.pixelToScreenCoordinates(x, y);
                        return this.session.screenToDocumentPosition(
                            Math.min(this.session.getScreenLength() - 1, Math.max(pos.row, 0)),
                            Math.max(pos.column, 0)
                        );
                    };

                    editor.commands.bindKey("Enter|Shift-Enter", "null");

                    editor.getSession().on('changeAnnotation', function () {
                        vm.field.DefaultValue = editor.getValue();

                        var hasErrors = editor.getSession().getAnnotations().filter(function (annotation) {
                            return annotation.type == 'error';
                        }).length > 0;

                        $scope.editForm.$setValidity('calculatedFormulaInput', !hasErrors);
                    });

                    function fieldListCompletor(editor, session, pos, prefix, callback) {
                        callback(null, genericTypeObj.Fields.map(function (field) {
                            return {
                                caption: field.FriendlyName,
                                value: field.FriendlyName,
                                meta: genericTypeObj.Name
                            };
                        }));
                    }

                    editor.commands.addCommand({
                        name: "fieldSelectorCommand",
                        bindKey: { win: ".", mac: "." },
                        exec: function () {
                            var pos = editor.selection.getCursor();
                            var session = editor.session;

                            var curLine = (session.getDocument().getLine(pos.row)).trim();
                            var curTokens = curLine.slice(0, pos.column).split(/\s+/);
                            var curCmd = curTokens[0];
                            if (!curCmd) return;
                            var lastToken = curTokens[curTokens.length - 1];

                            editor.insert(".");

                            if (lastToken === 'field' && lastToken.indexOf('.') < 0) {
                                var completer = {
                                    getCompletions: fieldListCompletor
                                }

                                var langTools = ace.require("ace/ext/language_tools");
                                langTools.setCompleters([completer]);

                                editor.execCommand("startAutocomplete");
                            }
                        }
                    });

                    function contentTypeFieldListCompletor(fieldName) {
                        return function (editor, session, pos, prefix, callback) {
                            editor.setReadOnly(true);
                            $q(function (resolve, reject) {
                                var field = genericTypeObj.Fields.filter(function (field) { return field.FriendlyName == fieldName; })[0];
                                if (field) {
                                    switch (field.AttributeTypeDefinition.InputType) {
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.userSelectorSingle:
                                            resolve(['Username'].map(function (field) {
                                                return {
                                                    caption: field,
                                                    value: field,
                                                    meta: 'User'
                                                };
                                            }));
                                            break;
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.taxonomySelectorSingle:
                                            resolve(['Name', 'TaxonomyPath'].map(function (field) {
                                                return {
                                                    caption: field,
                                                    value: field,
                                                    meta: 'Taxonomy'
                                                };
                                            }));
                                            break;
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.contentSelectorSingle: {
                                            var promiseArray = [];
                                            for (var i = 0; i < field.JsonField.constraints.Collection.length; i++) {
                                                var constraint = field.JsonField.constraints.Collection[i];
                                                if (constraint.Value.contentTypeId != '') {
                                                    promiseArray.push($q(function (resolve, reject) {
                                                        (new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController()).getById(constraint.Value.contentTypeId, function (data) {
                                                            resolve(data);
                                                        }, function (error) {
                                                            reject(error);
                                                        });
                                                    }));
                                                }
                                            }
                                            $q.all(promiseArray).then(function (data) {
                                                var fieldArray = [];
                                                for (var i = 0; i < data.length; i++) {
                                                    fieldArray = fieldArray.concat(data[i].Fields.map(function (field) {
                                                        return {
                                                            caption: field.FriendlyName,
                                                            value: field.FriendlyName,
                                                            meta: data[i].Name
                                                        };
                                                    }));
                                                }
                                                resolve(fieldArray);
                                            }, function (error) {
                                                reject(error);
                                            });
                                        }
                                            break;
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.mediaContentSelectorSingle: 
                                            resolve(['Name', 'Description', 'Path', 'UniqueId'].map(function (field) {
                                                return {
                                                    caption: field,
                                                    value: field,
                                                    meta: 'Media Content'
                                                };
                                            }));
                                            break;
                                        default: {
                                            reject();
                                            break;
                                        }
                                    }
                                } else {
                                    reject();
                                }
                            }).then(function (fields) {
                                callback(null, fields);
                                editor.setReadOnly(false);
                            }, function (error) {
                                callback(null, []);
                                editor.setReadOnly(false);
                            });
                        }
                    }

                    editor.commands.addCommand({
                        name: "fieldReferenceSelectorCommand",
                        bindKey: { win: "[", mac: "[" },
                        exec: function () {
                            var pos = editor.selection.getCursor();
                            var session = editor.session;

                            var curLine = (session.getDocument().getLine(pos.row)).trim();
                            var curTokens = curLine.slice(0, pos.column).split(/\s+/);
                            var curCmd = curTokens[0];
                            if (!curCmd) return;
                            var lastToken = curTokens[curTokens.length - 1];

                            editor.insert("[");

                            var searchToken = 'field.';

                            if (lastToken.indexOf(searchToken) == 0 && lastToken.length > searchToken.length && lastToken.indexOf('[') < 0) {
                                var completer = {
                                    getCompletions: contentTypeFieldListCompletor(lastToken.replace(searchToken, ''))
                                }

                                var langTools = ace.require("ace/ext/language_tools");
                                langTools.setCompleters([completer]);
                                editor.execCommand("startAutocomplete");
                            }
                        }
                    });
                }, 500);
            }
        }

        function init() {

            configureData();

            $scope.$watch(function () {
                return vm.listValues;
            }, function (listValues) {
                vm.field.ListValue = listValues.join(vm.field.Delimiter);
            });

            $scope.$watch(function () {
                return vm.field.AttributeTypeDefinition.InputType;
            }, function (InputType, InputTypeOld) {
                if (InputType !== undefined && InputType != InputTypeOld) {
                    mdGenerictypeDesignerFormService.switchFieldType(vm.field, vm.field.AttributeTypeDefinition.InputType).then(function (data) {
                        configureData();
                    }, function (error) {
                    });
                }
            });

            for (var key in vm.attributeTypeEnum) {
                if (isNaN(key)) {
                    vm.attributeTypeEnumStrings[key] = vm.attributeTypeEnum[key];
                }
            }
        }

        init();
    }

    function getSelectedDataSource(dataSourceArray, dataSourceId) {
        return dataSourceArray.filter(function (dataSource) {
            return dataSource.Id == dataSourceId;
        })[0];
    }

    function buildRegex(fieldValidation) {
        var regexString = '';

        if (regexString == '') {
            regexString = '(.*)';
        }

        if (fieldValidation.CharacterTypes.Edit) {
            regexString = '([';
            if (fieldValidation.CharacterTypes.Letters) {
                if (fieldValidation.CharacterTypes.Casing.Edit) {
                    if (fieldValidation.CharacterTypes.Casing.UpperCase) {
                        regexString = regexString + 'A-Z';
                    }

                    if (fieldValidation.CharacterTypes.Casing.LowerCase) {
                        regexString = regexString + 'a-z';
                    }
                } else {
                    regexString = regexString + 'A-Z';
                }
            }

            if (fieldValidation.CharacterTypes.SpecialCharacters.Edit) {
                for (var i = 0; i < fieldValidation.CharacterTypes.SpecialCharacters.Included.Length; i++) {
                    regexString = regexString + fieldValidation.CharacterTypes.SpecialCharacters.Included[i];
                }
            }

            if (fieldValidation.CharacterTypes.Numbers.Edit) {
                if (fieldValidation.CharacterTypes.Numbers.From < fieldValidation.CharacterTypes.Numbers.To) {
                    regexString = regexString + fieldValidation.CharacterTypes.Numbers.From.toString() + '-' + fieldValidation.CharacterTypes.Numbers.To.toString();
                }
            }

            regexString = regexString + '\\s])';
        }

        if (fieldValidation.MinLength.Edit || fieldValidation.MaxLength.Edit) {
            regexString = regexString + '{' + (fieldValidation.MinLength.Edit ? fieldValidation.MinLength.Length : 1) + ',' + (fieldValidation.MaxLength.Edit && fieldValidation.MaxLength.Length && fieldValidation.MaxLength.Length > fieldValidation.MinLength.Length ? fieldValidation.MaxLength.Length : '') + '}';
        }

        regexString = '/^' + regexString + '$/';

        return regexString;
    }
})();