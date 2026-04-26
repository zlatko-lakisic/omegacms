(function () {
    'use strict';


    angular
        .module('app.folder.forms')
        .controller('FolderFormController', ['$mdDialog', '$rootScope', '$mdMedia', '$state', '$scope', '$timeout', '$q', 'mdFeedbackService', 'folder', FolderFormController]);

    /** @ngInject */
    function FolderFormController($mdDialog, $rootScope, $mdMedia, $state, $scope, $timeout, $q, $mdFeedbackService, folder) {
        var vm = this;

        // Controllers
        var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
        var metaDataFieldController = new mdBusinessLogic.dataAccess.controllers.metaDataFieldController();
        var folderMetaDataFieldController = new mdBusinessLogic.dataAccess.controllers.folderMetaDataFieldController();
        var folderMediaContentMetaDataFieldController = new mdBusinessLogic.dataAccess.controllers.folderMediaContentMetaDataFieldController();
        var contentTypeDefinitionController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController();
        var contentTypeDefinitionFolderController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionFolderController();
        var templateController = new mdBusinessLogic.dataAccess.controllers.templateController();
        var approvalChainController = new mdBusinessLogic.dataAccess.controllers.approvalChainController();
        var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var dataBoundConditionController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionFolderDataBoundConditionController();
        var dataBoundSyncController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionFolderDataBoundSyncController();
        var permissionControllerProfileType = new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType();
        var permissionControllerUser = new mdBusinessLogic.dataAccess.controllers.permissionControllerUser();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        // Variables
        vm.currentFolderPath = $state.params.path;
        var parentFolder = {};
        var dialogInfo = {};
        var stateInfo = {};
        var selectedField = {};
        vm.folder = {};
        vm.basicForm = {};
        vm.formWizard = {};
        vm.metaDataFields = [];
        vm.selectedFields = [];
        vm.addedContentTypes = [];
        vm.removedContentTypes = [];
        vm.contentType = [];
        vm.dataBoundConditions = [];
        vm.dataBoundSync = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionFolderDataBoundSync();
        vm.contentTypeReadOnly = false;
        vm.assignedTemplates = [];
        vm.allTemplates = [];
        vm.addedTemplates = [];
        vm.removedTemplates = [];
        vm.folderMediaContentMetaDataFields = [];
        vm.folderMetaDataFields = [];
        vm.folder.MetaDataFields = [];
        vm.folder.ContentTypeDefinitions = [];
        vm.folder.ParentArray = [];
        vm.notAuthorizedUsers = [];
        vm.profileTypes = [];
        vm.allProfileTypes = [];
        profileTypeController.getAll('', function (data) {
            vm.allProfileTypes = data;
        }, function (error) {

            });
        vm.profileTypePermissions = [];
        vm.userPermissions = [];
        vm.stepJoins = [];
        vm.selectedContentType;
        vm.contentTypeSearchText;
        vm.tab;
        vm.contentTypeDefinitionSyncFrequency = new Date(0, 0, 0, 12, 0, 0, 0);
        vm.comparerTypes = mdBusinessLogic.dataAccess.entities.ComparerType;
        for (var key in vm.comparerTypes) {
            if (!isNaN(key)) {
                delete vm.comparerTypes[key];
            }
        }
        vm.contentTypeDefinitionSyncTypes = mdBusinessLogic.dataAccess.entities.contentTypeDefinitionFolderDataBoundSyncType;
        for (var key in vm.contentTypeDefinitionSyncTypes) {
            if (!isNaN(key)) {
                delete vm.contentTypeDefinitionSyncTypes[key];
            }
        }
        var addOrEdit;
        $state.params.folderId ? addOrEdit = 'edit' : addOrEdit = 'add';
        var dialogInfoText = addOrEdit === 'add' ? $rootScope.globals.resources.Labels.AddedText : $rootScope.globals.resources.Labels.EditedText;
        vm.formTitle = addOrEdit === 'add' ? $rootScope.globals.resources.Titles.AddFolder : $rootScope.globals.resources.Titles.EditFolder;
        vm.isNew = addOrEdit === 'add';
        vm.createFolderEnded = true;
        vm.disableBtnsWhenSavingInProgress = false;
        vm.folderID = $state.params.folderId;
        vm.target = 1;
        vm.isFoundFolderWithTheSameName = false;
        vm.isFetchedFoldersByParent = false;
        vm.approvalChain = new mdBusinessLogic.dataAccess.entities.approvalChain({ FolderId: vm.folderID, IsActive: true, Steps: [] });
        vm.jsplumbInstance = jsPlumb.getInstance();
        vm.permissionSaveEvents = [];

        // Methods
        vm.changeTab = changeTab;
        vm.sendForm = sendForm;
        vm.Back = Back;
        vm.removeStep = removeStep;
        vm.toggleUserDialog = toggleUserDialog;
        vm.addStep = addStep;
        vm.addAnotherTableRow = addAnotherTableRow;
        vm.ContentTypeDefinitionExist = ContentTypeDefinitionExist;
        vm.addNewCondition = addNewCondition;
        vm.deleteCondition = deleteCondition;
        vm.getOnlyDataBoundFields = getOnlyDataBoundFields;
        vm.filterDataSyncDeltaFields = filterDataSyncDeltaFields;
        vm.filterDataSyncDeltaInfoDialog = filterDataSyncDeltaInfoDialog;
        vm.registerPermissionSaveEvents = registerPermissionSaveEvents;

        function registerPermissionSaveEvents(event) {
            vm.permissionSaveEvents.push(event);
        }

        function filterDataSyncDeltaInfoDialog(ev) {
            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#forms')))
                    .clickOutsideToClose(true)
                    .title('What is a delta field?')
                    .textContent('You can specify some description text in here.')
                    .ariaLabel('What is a delta field?')
                    .ok('Got it!')
                    .targetEvent(ev)
            );
        }

        function filterDataSyncDeltaFields(attributeTypeIds) {
            vm.dataBoundSyncDeltaFields = vm.contentType[0].Fields.filter(function (item) { return attributeTypeIds.includes(item.AttributeTypeDefinition.InputType); });
        }

        function Back() {
            var backtoFolder = $state.params.path;
            $state.go('app.content_list', { folderPath: backtoFolder, currentView: $state.params.currentView }, { reload: false });
        }

        function getOnlyDataBoundFields() {
            return vm.contentType[0].Fields.filter(function (field) {
                return true;
                //return field.DataBound;
            });
        }

        function addNewCondition() {
            var condition = new mdBusinessLogic.dataAccess.entities.contentTypeDefinitionFolderDataBoundCondition();
            condition.ContentTypeDefinitionId = vm.contentType[0].Id;
            vm.dataBoundConditions.push(condition);
        }

        function deleteCondition(index) {
            vm.dataBoundConditions.splice(index, 1);
        }

        function changeTab(tab) {
            vm.tab = tab;
            if (tab === 7) // if no connections are present it means that tab is clicked for the 1st time
            {
                drawJoins();
            }
        }

        function drawJoins() {
            $timeout(function () {
                jsPlumb.doWhileSuspended(function () {
                    for (var i = 0; i < vm.stepJoins.length; i++) {
                        vm.jsplumbInstance.connect(vm.stepJoins[i]);
                    }
                    vm.stepJoins.splice(0, vm.stepJoins.length);
                });
            }, 10);
        }

        function setContentTypeReadOnly() {
            vm.contentTypeReadOnly = vm.contentType.filter(function (contentType) {
                return contentType.Fields.filter(function (field) {
                    return field.DataBound;
                }).length > 0;
            }).length > 0;
            if (vm.folder.Inherit) {
                vm.contentTypeReadOnly = true;
            }
        }

        function assignMediaContentMetaDataFieldsToFolder() {
            var checked = new Array();
            for (var i = 0; i < vm.folderMediaContentMetaDataFields.length; i++) {
                if (vm.folderMediaContentMetaDataFields[i].Checked) {
                    if (vm.folderMediaContentMetaDataFields[i].IsRequired) {
                        vm.folderMediaContentMetaDataFields[i].IsRequired = true;
                    } else {
                        vm.folderMediaContentMetaDataFields[i].IsRequired = false;
                    }
                    checked.push(vm.folderMediaContentMetaDataFields[i]);
                }
            }
            vm.folder.FolderMediaContentMetaDataField = checked;
        }

        function assignMetaDataFieldsToFolder() {
            var checked = new Array();
            if (vm.folderMetaDataFields.length != 0) {
                for (var i = 0; i < vm.folderMetaDataFields.length; i++) {
                    if (vm.folderMetaDataFields[i].Checked) {
                        if (vm.folderMetaDataFields[i].IsRequired) {
                            vm.folderMetaDataFields[i].IsRequired = true;
                        } else {
                            vm.folderMetaDataFields[i].IsRequired = false;
                        }
                        checked.push(vm.folderMetaDataFields[i]);
                    }
                }
                vm.folder.MetaDataFields = checked;
            }
        }

        function assignProfilePermissionsToFolder() {
            vm.folder.ProfilePermissions = vm.profileTypes;
        }

        function assignUserPermissionsToFolder() {
            vm.folder.NotAuthorizedUsers = vm.notAuthorizedUsers;
        }

        ///////////////////////////content types autocomplete chips

        vm.queryAllContentTypes = queryAllContentTypes;
        vm.addContentType = addContentType;
        vm.removeContentType = removeContentType;

        function ContentTypeDefinitionExist(_contentTypes, _contentType) {
            var index = -1;
            for (var i in _contentTypes) {
                if (_contentTypes[i].Id == _contentType.Id) {
                    index = i;
                    break;
                }
            }

            return index;
        }

        function queryAllContentTypes(query) {
            var lowercaseQuery = query.toLowerCase();
            var results = query ? vm.allContentTypes.filter(function (query) {
                return function filterFn(contentType) {
                    return (contentType._lowertitle.indexOf(lowercaseQuery) === 0);
                };
            }) : [];
            var i = results.length;
            while (i--) {
                if (vm.ContentTypeDefinitionExist(vm.addedContentTypes, results[i]) >= 0 ||
                    vm.ContentTypeDefinitionExist(vm.contentType, results[i]) >= 0 ||
                    results[i]._lowertitle.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }

        function addContentType(contentType) {
            var contentTypeExist = vm.ContentTypeDefinitionExist(vm.addedContentTypes, contentType) >= 0 &&
                vm.ContentTypeDefinitionExist(vm.contentType, contentType) >= 0;

            if (!contentTypeExist) {
                var previouslyRemovedBeforeSaving = vm.removedContentTypes.indexOf(contentType);
                if (previouslyRemovedBeforeSaving != -1) {
                    vm.removedContentTypes.splice(previouslyRemovedBeforeSaving, 1);
                } else {
                    vm.addedContentTypes.push(contentType);
                    vm.folder.ContentTypeDefinitions = vm.addedContentTypes;
                }
            }
            vm.selectedContentType = null;
            vm.contentTypeSearchText = '';
            setContentTypeReadOnly();
        }

        function removeContentType(contentType) {
            var contentTypeExist = vm.ContentTypeDefinitionExist(vm.addedContentTypes, contentType) >= 0 &&
                vm.ContentTypeDefinitionExist(vm.contentType, contentType) >= 0;

            var index = vm.addedContentTypes.indexOf(contentType);
            vm.addedContentTypes.splice(index, 1);
            if (!contentTypeExist) {
                var previouslyAddedBeforeSaving = vm.removedContentTypes.indexOf(contentType);
                if (previouslyAddedBeforeSaving != -1) {
                    vm.removedContentTypes.splice(previouslyAddedBeforeSaving, 1);
                }
                else {
                    vm.removedContentTypes.push(contentType);
                    vm.folder.ContentTypeDefinitions = vm.removedContentTypes;

                }
            }
            setContentTypeReadOnly();
        }
        ///////////////////////////////////////////////////end content types autocomplete

        vm.queryAllTemplates = queryAllTemplates;
        vm.addTemplate = addTemplate;
        vm.removeTemplate = removeTemplate;
        vm.templateExist = templateExist;
        function templateExist(_templates, _template) {
            var index = -1;
            for (var i in _templates) {
                if (_templates[i].Id == _template.Id) {
                    index = i;
                    break;
                }
            }
            return index;
        }
        function queryAllTemplates(query) {
            var lowercaseQuery = query.toLowerCase();
            var results = query ? vm.allTemplates.filter(function (query) {
                return function filterFn(template) {
                    return (template._lowertitle.indexOf(lowercaseQuery) === 0);
                };
            }) : [];
            var i = results.length;
            while (i--) {
                if (vm.templateExist(vm.addedTemplates, results[i]) >= 0 ||
                    vm.templateExist(vm.assignedTemplates, results[i]) >= 0 ||
                    results[i]._lowertitle.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }

        function addTemplate(template) {
            var templateExist = vm.templateExist(vm.addedTemplates, template) >= 0 &&
                vm.templateExist(vm.assignedTemplates, template) >= 0;

            if (!templateExist) {
                var previouslyRemovedBeforeSaving = vm.removedTemplates.indexOf(template);
                if (previouslyRemovedBeforeSaving != -1) {
                    vm.removedTemplates.splice(previouslyRemovedBeforeSaving, 1);
                } else {
                    vm.addedTemplates.push(template);
                    vm.folder.Templates = vm.addedTemplates;
                }
            }
            vm.selectedTemplate = null;
            vm.assignedTemplatesSearchText = '';
        }

        function removeTemplate(template) {
            var templateExist = vm.templateExist(vm.addedTemplates, template) >= 0 &&
                vm.templateExist(vm.assignedTemplates, template) >= 0;
            var index = vm.addedTemplates.indexOf(template);
            vm.addedTemplates.splice(index, 1);
            if (!templateExist) {
                var previouslyAddedBeforeSaving = vm.removedTemplates.indexOf(template);
                if (previouslyAddedBeforeSaving != -1) {
                    vm.removedTemplates.splice(previouslyAddedBeforeSaving, 1);
                }
                else {
                    vm.removedTemplates.push(template);
                    vm.folder.Templates = vm.removedTemplates;

                }
            }
        }
        /////////////////////template autocomplete chips end

        function assignContentTypeDefinitionsToFolder() {
            vm.folder.ContentTypeDefinitions = vm.contentType;
        }

        function assignTemplatesToFolder() {
            vm.folder.Templates = vm.assignedTemplates;
        }

        function assignPropertiesToFolder() {
            if ($state.params.id > 0) {
                vm.folder.ParentId = $state.params.id;
            }
        }

        var ContentTypeOrFolderTemplate = "";

        function leftBehindContentTypesOrFolderTemplates() {
            var contentTypeChip = document.getElementById("contentTypeChip");
            var chipsLength = contentTypeChip.firstElementChild.children[0].getElementsByTagName("md-chip").length;
            if (chipsLength == 0 && contentTypeChip.firstElementChild.children[0].getElementsByTagName("input")[0].value.length != 0) {
                ContentTypeOrFolderTemplate = "ContentType";
            }
            var folderTemplateChip = document.getElementById("folderTemplateChip");
            chipsLength = folderTemplateChip.firstElementChild.children[0].getElementsByTagName("md-chip").length;
            if (chipsLength == 0 && folderTemplateChip.firstElementChild.children[0].getElementsByTagName("input")[0].value.length != 0) {
                if (ContentTypeOrFolderTemplate == "ContentType")
                    ContentTypeOrFolderTemplate = "Both";
                else
                    ContentTypeOrFolderTemplate = "FolderTemplate";
            }
        }

        /////////////////////approval chain
        $scope.$on('$destroy', function () {
            vm.jsplumbInstance.doWhileSuspended(function () {
                vm.jsplumbInstance.removeAllEndpoints();
            });
        });

        if (addOrEdit !== 'add') {
            approvalChainController.getByFolderId($state.params.folderId, function (apiData) {
                if (apiData === undefined || apiData === null || apiData.Steps === undefined || apiData.Steps === null) {
                    addStep();
                    addStep();
                    return;
                }
                vm.approvalChain = apiData;
                for (var i = 0; i < apiData.Steps.length; i++) {
                    if (i < apiData.Steps.length - 1) {
                        if (apiData.Steps[i].Actions && apiData.Steps[i].Actions[0]) {
                            vm.approvalChain.Steps[i].Actions[0].RedirectTo = apiData.Steps[i + 1].Order;
                            vm.stepJoins.push({
                                source: "entity" + vm.approvalChain.Steps[i].Order,
                                target: "entity" + apiData.Steps[i + 1].Order,
                                detachable: false,
                                anchors: ["Bottom", "Top"], // lines connecting steps sequentially should be straight and in middle
                                connector: "Straight",
                                endpoint: ["Rectangle", { width: 10, height: 10 }],
                                label: $rootScope.globals.resources.Labels.Approve
                            });
                        }

                    }

                    for (var j = i + 1; j < apiData.Steps.length; j++) {
                        //Change redirect to values to Order number
                        //check all steps after if redirecting to this step where ID = this step ID
                        if (apiData.Steps[j].Actions && apiData.Steps[j].Actions[1] && apiData.Steps[j].Actions[1].RedirectTo === apiData.Steps[i].Id) {
                            vm.approvalChain.Steps[j].Actions[1].RedirectTo = apiData.Steps[i].Order;
                            vm.stepJoins.push({
                                source: "entity" + vm.approvalChain.Steps[j].Order,
                                target: "entity" + apiData.Steps[i].Order,
                                endpoint: ["Rectangle", { width: 10, height: 10 }],
                                detachable: false,
                                label: $rootScope.globals.resources.Labels.Reject,
                                anchors: ["RightMiddle", "RightMiddle"] // lines used for redirect should be in right side
                            });
                        }
                    }
                }

            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });
        }
        function addStep() {
            //add base date automatically, only 2 actions allowed - approve type 1 and reject type 2
            var data = {
                Order: vm.approvalChain.Steps.length,
                ApprovalChainId: vm.approvalChain.ChainId,
                UserIds: [],
                Actions: [{ Type: 1, Action: 2 }, { Type: 2, Action: 1 }], // default action for approve is publish, for reject redirect
                ComboOperator: 1
            };
            var step = new mdBusinessLogic.dataAccess.entities.approvalChainStep(data);

            if (vm.approvalChain.Steps.length > 0) {
                //on approve action on before step redirect it to new step
                vm.approvalChain.Steps[vm.approvalChain.Steps.length - 1].Actions[0].Action = 1;
                vm.approvalChain.Steps[vm.approvalChain.Steps.length - 1].Actions[0].RedirectTo = step.Order;
                step.Actions[1].RedirectTo = vm.approvalChain.Steps[0].Order; //redirect to step 0 on reject by default

                vm.stepJoins.push({
                    source: "entity" + vm.approvalChain.Steps[vm.approvalChain.Steps.length - 1].Order,
                    target: "entity" + step.Order,
                    endpoint: ["Rectangle", { width: 10, height: 10 }],
                    anchors: ["Bottom", "Top"], // lines connecting steps sequentially should be straight and in middle
                    connector: "Straight",
                    detachable: false,
                    label: $rootScope.globals.resources.Labels.Approve
                });
                vm.stepJoins.push({
                    source: "entity" + step.Order,
                    target: "entity" + vm.approvalChain.Steps[0].Order,
                    endpoint: ["Rectangle", { width: 10, height: 10 }],
                    detachable: false,
                    anchors: ["RightMiddle", "RightMiddle"],
                    label: $rootScope.globals.resources.Labels.Reject
                });
            }
            vm.approvalChain.Steps.push(step);
            if (vm.tab === 7) {
                drawJoins();
            }

        }

        function removeStep($index) {
            jsPlumb.doWhileSuspended(function () {
                vm.jsplumbInstance.select({ target: "entity" + $index })['delete']();
                vm.jsplumbInstance.select({ source: "entity" + $index })['delete']();
            });
            vm.approvalChain.Steps.splice($index, 1);
            if ($index > 1) {
                //only last step can be deleted so it's safe to remove redirect to attribute on step before pointing to step being removed
                vm.approvalChain.Steps[$index - 1].Actions[0].RedirectTo = null;
            }
            $("#content").scrollTop($("#content").scrollTop() - 105);
        }
        function toggleUserDialog(event, item) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            var lastRedirect = item.Actions[1].RedirectTo;
            userController.getAll(function (data) {
                $mdDialog.show({
                    controller: "approvalChainUserFormController",
                    templateUrl: 'scripts/app/main/content/folderForm/approvalChain/dialogs/approval-chain-user-dialog.html',
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    fullscreen: useFullScreen,
                    resolve: {
                        step: function () {
                            return item;
                        },
                        users: function () {
                            return data;
                        },
                        otherSteps: function () {
                            return vm.approvalChain.Steps;
                        }
                    }
                })
                    .then(function (usersAdded) {
                        item.UserIds.splice(0, item.UserIds.length);
                        for (var i = 0; i < usersAdded.length; i++) {
                            item.UserIds.push(usersAdded[i].Id);
                        }
                        jsPlumb.doWhileSuspended(function () {
                            vm.jsplumbInstance.select({ source: "entity" + item.Order, target: "entity" + lastRedirect })['delete']();
                            vm.jsplumbInstance.connect({
                                source: "entity" + item.Order,
                                target: "entity" + item.Actions[1].RedirectTo,
                                endpoint: ["Rectangle", { width: 10, height: 10 }],
                                detachable: false,
                                anchors: ["RightMiddle", "RightMiddle"],
                                label: $rootScope.globals.resources.Labels.Reject
                            });
                        });
                    }, function () {
                    });
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });
        }
        function checkChain() {
            if (typeof vm.approvalChain === "undefined" || vm.approvalChain === null) {
                return false;
            }
            if (typeof vm.approvalChain.Steps === "undefined" || vm.approvalChain.Steps === null
                || vm.approvalChain.Steps.length === 0) {
                return false;
            }
            for (var step in vm.approvalChain.Steps) {
                if (vm.approvalChain.Steps[step].Order === 0) {
                    continue;
                }
                if (vm.approvalChain.Steps[step].UserIds.length === 0 || vm.approvalChain.Steps[step].Actions.length !== 2) {
                    return false;
                }
            }
            return true;
        }

        function saveApprovalChain() {
            if (checkChain()) {
                var pathoffolder = "";
                var pathofparentfolder = "";
                if ($state.params.path === "Root") {
                    pathoffolder = "Root";
                }
                else {
                    pathoffolder = $state.params.path;
                }

                var lastindex = pathoffolder.lastIndexOf("/");
                if (lastindex == -1) {
                    pathofparentfolder = pathoffolder;
                }
                else {
                    pathofparentfolder = pathoffolder.slice(0, lastindex);
                }

                var redirectionUrl = "";
                if (!vm.isNew) {
                    redirectionUrl = pathofparentfolder;
                } else {
                    redirectionUrl = pathoffolder;
                }
                approvalChainController.save(vm.approvalChain, function (data) {
                    $mdFeedbackService.reportInfo('save');
                    $state.go('app.content_list', { folderPath: vm.folder.FolderPath }, { reload: false })
                }, function (error) {

                    var confirm = $mdDialog.confirm()
                        .clickOutsideToClose(true)
                        .title($rootScope.globals.resources.Titles.SaveApprovalChainError)
                        .textContent($rootScope.globals.resources.Labels.SaveApprovalChainErrorText)
                        .ok($rootScope.globals.resources.Labels.SaveApprovalChainErrorYes)
                        .cancel($rootScope.globals.resources.Labels.SaveApprovalChainErrorNo);
                    $mdDialog.show(confirm).then(function () {
                        //stay on this page and fix
                    }, function () {
                        $mdFeedbackService.reportError('save', error);
                        $state.go('app.content_list', { folderPath: redirectionUrl }, { reload: false })
                    });
                });
            } else {
                var confirm = $mdDialog.confirm()
                    .clickOutsideToClose(true)
                    .title($rootScope.globals.resources.Titles.SaveApprovalChainError)
                    .textContent($rootScope.globals.resources.Labels.SaveApprovalChainErrorText)
                    .ok($rootScope.globals.resources.Labels.SaveApprovalChainErrorYes)
                    .cancel($rootScope.globals.resources.Labels.SaveApprovalChainErrorNo);
                $mdDialog.show(confirm).then(function () {
                    //stay on this page and fix
                }, function () {
                    $state.go('app.content_list', { folderPath: redirectionUrl }, { reload: false })
                });
            }
        }

        function saveConditions() {
            if (vm.contentTypeReadOnly) {
                dataBoundConditionController.deleteAll(vm.folder.Id, vm.contentType[0].Id, function (data) {
                    for (var i = 0; i < vm.dataBoundConditions.length; i++) {
                        vm.dataBoundConditions[i].FolderId = vm.folder.Id;
                    }
                    dataBoundConditionController.saveAll(vm.dataBoundConditions);
                });
            }
        }

        function saveDataBoundSync() {
            if (vm.contentTypeReadOnly) {
                vm.dataBoundSync.Frequency = vm.contentTypeDefinitionSyncFrequency.getHours().toString() + ':' + vm.contentTypeDefinitionSyncFrequency.getMinutes().toString() + ':' + vm.contentTypeDefinitionSyncFrequency.getSeconds().toString();
                vm.dataBoundSync.ContentTypeDefinitionId = vm.contentType[0].Id;
                vm.dataBoundSync.FolderId = vm.folder.Id;
                dataBoundSyncController.save(vm.dataBoundSync, function (data) {

                }, function (error) {
                });
            }
        }

        function saveFolder() {
            vm.createFolderEnded = false;
            vm.disableBtnsWhenSavingInProgress = true;

            folderController.save(vm.folder, function (data) {
                vm.saveApprovalActive = true;
                $scope.$apply(function () {
                    vm.folder = data;
                    var postSaveEvents = [];
                    for (var i in vm.permissionSaveEvents) {
                        var event = vm.permissionSaveEvents[i];
                        postSaveEvents.push(event());
                    }
                    $q.all(postSaveEvents).then(function () {
                        vm.createFolderEnded = true;
                        $mdFeedbackService.reportInfo('save');
                        //We need to comment this because this caused internal error 500 because it is called in business logic
                        //saveFolderProfileTypePermissions(vm.profileTypes, vm.folder);
                        // saveFolderUSerPermissions(vm.notAuthorizedUsers, vm.folder);
                        saveApprovalChain();
                        saveConditions();
                        saveDataBoundSync();
                        $scope.$emit('LoadNav', {
                            action: 'save',
                            type: mdBusinessLogic.dataAccess.entities.entitiesEnum.Content,
                            value: angular.copy(vm.folder)
                        });
                    });
                });

            }, function (error) {
                if (vm.saveApprovalActive) {
                    return; // prevent from redirecting if approval chain fails
                }
                var pathoffolder = "";
                var pathofparentfolder = "";
                if ($state.params.path === "Root") {
                    pathoffolder = "Root";
                }
                else {
                    pathoffolder = $state.params.path;
                }

                var lastindex = pathoffolder.lastIndexOf("/");
                if (lastindex == -1) {
                    pathofparentfolder = pathoffolder;
                }
                else {
                    pathofparentfolder = pathoffolder.slice(0, lastindex);
                }

                var redirectionUrl = "";
                if (!vm.isNew) {
                    redirectionUrl = pathofparentfolder;
                } else {
                    redirectionUrl = pathoffolder;
                }

                $mdFeedbackService.reportError('save', error);
                $state.go('app.content_list', { folderPath: redirectionUrl }, { reload: false })
                vm.createFolderEnded = true;
            });
        }

        function sendForm(ev) {
            assignMetaDataFieldsToFolder();
            assignMediaContentMetaDataFieldsToFolder();
            assignContentTypeDefinitionsToFolder();
            assignTemplatesToFolder();
            assignPropertiesToFolder();
            assignProfilePermissionsToFolder();
            assignUserPermissionsToFolder();
            validateFolderName();
            if (vm.isFoundFolderWithTheSameName === true) {
                return;
            }
            if (leftBehindItemsValidated() === false && vm.isFoundFolderWithTheSameName === false)
                saveFolder();
        }

        function validateFolderName() {
            if (!vm.isNew) {
                if (oldFolderName == vm.folder.Name) {
                    //leftBehindItemsValidated();
                }
                else {
                    checkFolderNames();
                }
            }
            else {
                checkFolderNames();
            }

        }


        function leftBehindItemsValidated() {

            leftBehindContentTypesOrFolderTemplates();
            if (ContentTypeOrFolderTemplate == "" || ContentTypeOrFolderTemplate == undefined) {
                return false;
            }
            else if (ContentTypeOrFolderTemplate == "Both") {
                $mdDialog.show($mdDialog.confirm()
                    .title($rootScope.globals.resources.Titles.ContentTypeTemplateNotFinished)
                    .targetEvent()
                    .ok($rootScope.globals.resources.Labels.Yes)
                    .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                        saveFolder();
                    }, function () {

                    });
                ContentTypeOrFolderTemplate = "";
                return true;
            } else if (ContentTypeOrFolderTemplate == "ContentType") {
                $mdDialog.show($mdDialog.confirm()
                    .title($rootScope.globals.resources.Titles.ContentTypeNotFinished)
                    .targetEvent()
                    .ok($rootScope.globals.resources.Labels.Yes)
                    .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                        saveFolder();
                    }, function () {

                    });
                ContentTypeOrFolderTemplate = "";
                return true;
            } else {
                $mdDialog.show($mdDialog.confirm()
                    .title($rootScope.globals.resources.Titles.FolderTemplateNotFinished)
                    .targetEvent()
                    .ok($rootScope.globals.resources.Labels.Yes)
                    .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                        saveFolder();
                    }, function () {

                    });
                ContentTypeOrFolderTemplate = "";
                return true;
            }
        }

        function getFoldeByParentId(parentid) {
            folderController.getByParentId(parentid, function (data) {
                vm.foldersByParentID = data;
                vm.isFetchedFoldersByParent = true;
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });
        }

        function checkFolderNames() {
            vm.isFoundFolderWithTheSameName = false;
            if (vm.isFetchedFoldersByParent === true) {
                for (var i = 0; i < vm.foldersByParentID.length; i++) {
                    if (vm.foldersByParentID[i].Name.toLowerCase() == vm.folder.Name.toLowerCase()) {
                        vm.isFoundFolderWithTheSameName = true;
                        $mdDialog.show($mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title($rootScope.globals.resources.Titles.Warning)
                            .textContent($rootScope.globals.resources.Labels.DuplicateFolderName)
                            .ok($rootScope.globals.resources.Labels.GotIt));
                        return;
                    }

                }
            } else {
                vm.isFoundFolderWithTheSameName = true;
                $mdDialog.show($mdDialog.alert()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title($rootScope.globals.resources.Titles.Warning)
                    .textContent($rootScope.globals.resources.Labels.ErrorTryAgain)
                    .ok($rootScope.globals.resources.Labels.GotIt));
            }

        }

        function saveFolderProfileTypePermissions(profileTypes, content) {
            var profileTypePermissionsData = {
                ValueArray: profileTypes,
                ValueName: content.Id + ';' + 1
            };
            if (!vm.isNew) {
                profileTypeController.update(profileTypePermissionsData,
                    function (data) {
                        $mdFeedbackService.reportInfo('update');
                    }, function (error) {
                        $mdFeedbackService.reportError('update', error);
                    });
            } else {
                profileTypeController.saveProfileTypePermissions(profileTypePermissionsData,
                    function (data) {
                        $mdFeedbackService.reportInfo('save');
                    }, function (error) {
                        $mdFeedbackService.reportError('save', error);
                    });
            }

        }

        function saveFolderUSerPermissions(users, content) {
            var usersPermissionsData = {
                ValueArray: users,
                ValueName: content.Id + ';' + 1
            };
            if (!vm.isNew) {
                userController.updateUserPermission(usersPermissionsData,
                    function (data) {
                        $mdFeedbackService.reportInfo('update');
                    }, function (error) {
                        $mdFeedbackService.reportError('update', error);
                    });
            } else {
                userController.saveUserPermissions(usersPermissionsData,
                    function (data) {
                        $mdFeedbackService.reportInfo('save');
                    }, function (error) {
                        $mdFeedbackService.reportError('save', error);
                    });
            }

        }

        //executing
        if (!vm.isNew) {
            vm.id = $state.params.folderId;
            vm.folder.Id = vm.id;
        }
        userController.getAll(function (data) {
            vm.selectedUser = new mdBusinessLogic.dataAccess.entities.user();
            vm.selectedUsers = [];
            vm.selectedUsers[0] = vm.selectedUser;
            vm.allUsers = data;

        }, function (error) {
            $mdFeedbackService.reportError('load', error);
        })

        function findUserId(selectedUser) {
            for (var i in vm.allUsers) {
                if (vm.allUsers[i].Username == selectedUser.Username) {
                    return vm.allUsers[i].Id;
                }
            }
        }

        function addAnotherTableRow(ev) {

            if (vm.notAuthorizedUsers != null) {

                vm.selectedUser.Id = findUserId(vm.selectedUser);

                for (var i = 0; i < vm.notAuthorizedUsers.length; i++) {
                    if (vm.notAuthorizedUsers[i].Username == vm.selectedUser.Username) {
                        vm.notAuthorizedUsers[i] = vm.selectedUser;
                        vm.selectedUser = null;
                        return;
                    }
                }
                if (vm.selectedUser.Username != null && vm.selectedUser.Username != "") {
                    vm.selectedUser.Id = findUserId(vm.selectedUser);
                    vm.notAuthorizedUsers.push(vm.selectedUser);
                    vm.selectedUser = null;
                }
            }
            else {
                if (vm.selectedUser.Username != null && vm.selectedUser.Username != "") {
                    vm.selectedUser.Id = findUserId(vm.selectedUser);
                    vm.notAuthorizedUsers.push(vm.selectedUser);
                    vm.selectedUser = null;
                }
            }
        }

        var oldFolderName;
        if (!vm.isNew) {
            if (!$state.params.id) {
                parentFolder = folder;
                vm.folder.ParentId = parentFolder.Id;
            } else {
                vm.folder = folder;
                oldFolderName = vm.folder.Name;
                //we call this method for using data when checked for same names in directory
                getFoldeByParentId(vm.folder.ParentId);
                if (vm.folder.Inherit == 0) {
                    folderMetaDataFieldController.getUsed(vm.folder.Id, function (data) {
                        $scope.$apply(function () {
                            vm.folderMetaDataFields = data;
                        });

                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    folderMediaContentMetaDataFieldController.getUsed(vm.folder.Id, function (data) {
                        $scope.$apply(function () {
                            vm.folderMediaContentMetaDataFields = data;

                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    contentTypeDefinitionController.getAll(function (data) {
                        vm.allContentTypes = data.map(function (contentType) {
                            contentType._lowertitle = contentType.Name.toLowerCase();
                            return contentType;
                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });

                    templateController.getAll("", function (data) {
                        vm.allTemplates = data.map(function (template) {
                            template._lowertitle = template.Name.toLowerCase();
                            return template;
                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });

                    contentTypeDefinitionController.getByFolder(vm.folder.Id, function (data) {
                        $scope.$apply(function () {
                            vm.folder.ContentTypeDefinitions = data;
                            vm.contentType = data;
                            setContentTypeReadOnly();
                            if (!vm.isNew && vm.contentTypeReadOnly) {
                                dataBoundConditionController.getByFolderAndContentTypeDefinitionId(vm.folder.Id, vm.contentType[0].Id, function (data) {
                                    $scope.$apply(function () {
                                        vm.dataBoundConditions = data;
                                    });
                                }, function (error) {
                                });

                                dataBoundSyncController.getByFolderAndContentTypeDefinitionId(vm.folder.Id, vm.contentType[0].Id, function (data) {
                                    $scope.$apply(function () {
                                        vm.dataBoundSync = data;
                                        var frequencyArray = vm.dataBoundSync.Frequency.split(':');
                                        vm.contentTypeDefinitionSyncFrequency = new Date(0, 0, 0, frequencyArray[0], frequencyArray[1], frequencyArray[2], 0);
                                    });
                                }, function (error) {
                                });

                                if (vm.Synchronisation == null || vm.Synchronisation === undefined) {
                                    vm.Synchronisation = new mdBusinessLogic.dataAccess.entities.actionSchedule();
                                    vm.Synchronisation.ActionType = vm.contentTypeDefinitionSyncTypes.None;
                                }
                            }
                        });

                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    templateController.getByFolder(vm.folder.Id, function (data) {
                        vm.folder.Templates = data;
                        vm.assignedTemplates = data;
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    /*profileTypeController.getAllProfileTypesWithPermissions({ target: vm.target, targetPrimaryKey: vm.folderID }, function (data) {
                        vm.profileTypes = data

                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });*/

                    /*userController.getOnlyNotAuthorizedUsers({ target: vm.target, targetPrimaryKey: vm.folderID }, function (data) {
                        vm.notAuthorizedUsers = data

                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });*/

                } else {
                    folderMetaDataFieldController.getUsed(vm.folder.ParentId, function (data) {
                        $scope.$apply(function () {
                            vm.folderMetaDataFields = data;

                        });

                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    contentTypeDefinitionController.getAll(function (data) {
                        vm.allContentTypes = data.map(function (contentType) {
                            contentType._lowertitle = contentType.Name.toLowerCase();
                            return contentType;
                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });

                    templateController.getAll("", function (data) {
                        vm.allTemplates = data.map(function (template) {
                            template._lowertitle = template.Name.toLowerCase();
                            return template;
                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });

                    folderMediaContentMetaDataFieldController.getUsed(vm.folder.ParentId, function (data) {
                        $scope.$apply(function () {
                            vm.folderMediaContentMetaDataFields = data;

                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    contentTypeDefinitionController.getByFolder(vm.folder.ParentId, function (data) {
                        vm.folder.ContentTypeDefinitions = data;
                        vm.contentType = data;
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    templateController.getByFolder(vm.folder.ParentId, function (data) {
                        vm.folder.Templates = data;
                        vm.assignedTemplates = data;
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    })

                    /*profileTypeController.getAllProfileTypesWithPermissions({ target: vm.target, targetPrimaryKey: vm.folder.ParentId }, function (data) {
                        vm.profileTypes = data;
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });

                    userController.getOnlyNotAuthorizedUsers({ target: vm.target, targetPrimaryKey: vm.folder.ParentId }, function (data) {
                        vm.notAuthorizedUsers = data;
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });*/
                }
            }
        } else {
            // add dio
            var parentId = $state.params.id;
            //we call this method for using data when checked for same names in directory
            getFoldeByParentId(parentId);
            vm.folder.ParentArray = folder.ParentArray;

            folderMetaDataFieldController.getUsed(parentId, function (data) {
                $scope.$apply(function () {
                    vm.folderMetaDataFields = data;
                    assignMetaDataFieldsToFolder();
                });
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            })

            folderMediaContentMetaDataFieldController.getUsed(parentId, function (data) {
                $scope.$apply(function () {
                    vm.folderMediaContentMetaDataFields = data;
                    assignMediaContentMetaDataFieldsToFolder();
                });
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            })

            contentTypeDefinitionController.getByFolder(parentId, function (data) {
                vm.folder.ContentTypeDefinitions = data;
                vm.contentType = data;
                assignContentTypeDefinitionsToFolder();
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            })

            templateController.getByFolder(parentId, function (data) {
                vm.folder.Templates = data;
                vm.assignedTemplates = data;
                assignTemplatesToFolder();
                assignPropertiesToFolder();
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            })

            templateController.getAll("", function (data) {
                vm.allTemplates = data.map(function (template) {
                    template._lowertitle = template.Name.toLowerCase();
                    return template;
                });
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });

            contentTypeDefinitionController.getAll(function (data) {
                vm.allContentTypes = data.map(function (contentType) {
                    contentType._lowertitle = contentType.Name.toLowerCase();
                    return contentType;
                });
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });

            /*profileTypeController.getAllProfileTypesWithPermissions({ target: vm.target, targetPrimaryKey: parentId }, function (data) {
                vm.profileTypes = data

            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });

            userController.getOnlyNotAuthorizedUsers({ target: vm.target, targetPrimaryKey: parentId }, function (data) {
                vm.notAuthorizedUsers = data

            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });*/

        }
    }
})();
