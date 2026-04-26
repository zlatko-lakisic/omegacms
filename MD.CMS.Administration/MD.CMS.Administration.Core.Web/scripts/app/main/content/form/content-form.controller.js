
(function () {
    'use strict';

    angular
        .module('app.content.form')
        .controller('ContentFormController', ['$scope', '$state', '$rootScope', '$q', 'mdFeedbackService', 'folder', 'content', 'taxonomies', 'contentTypeDefinition', 'folderMetaData', 'mdFieldService', ContentFormController]);


    /** @ngInject */
    function ContentFormController($scope, $state, $rootScope, $q, $mdFeedbackService, folder, content, taxonomies, contentTypeDefinition, folderMetaData, mdFieldService) {
        var vm = this;

        //Controllers
        var controllers = {
            folderController: new mdBusinessLogic.dataAccess.controllers.folderController(),
            contentController: new mdBusinessLogic.dataAccess.controllers.contentController(),
            metaDataFieldController: new mdBusinessLogic.dataAccess.controllers.metaDataFieldController(),
            metaDataFieldValueController: new mdBusinessLogic.dataAccess.controllers.metaDataFieldValueController(),
            taxonomyController: new mdBusinessLogic.dataAccess.controllers.taxonomyController(),
            taxonomycontentController: new mdBusinessLogic.dataAccess.controllers.taxonomyContentController(),
            cultureController: new mdBusinessLogic.dataAccess.controllers.cultureController(),
            contentTypeDefinitionController: new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController(),
            contentTypeDefinitionFieldController: new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionFieldController(),
            contentTypeDefinitionFieldValueController: new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionFieldValueController(),
            contentAliasController: new mdBusinessLogic.dataAccess.controllers.contentAliasController(),
            templateController: new mdBusinessLogic.dataAccess.controllers.templateController(),
            profileTypeController: new mdBusinessLogic.dataAccess.controllers.profileTypeController(),
            userController: new mdBusinessLogic.dataAccess.controllers.userController(),
            mediaContentController: new mdBusinessLogic.dataAccess.controllers.mediaContentController(),
            permissionControllerProfileType: new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType(),
            permissionControllerUser: new mdBusinessLogic.dataAccess.controllers.permissionControllerUser()
        }

        //Page Variables
        vm.isNew = $state.params.action != 'edit';
        vm.folder = folder;
        vm.content = content;
        vm.contentTypeDefinition = contentTypeDefinition;
        if (vm.isNew) {
            vm.content.ContentType = contentTypeDefinition;
        }
        vm.lcid = $state.params.lcid || 2057;
        vm.formTitle = vm.isNew ? $rootScope.globals.resources.Titles.AddContent : $rootScope.globals.resources.Titles.EditContent;
        vm.tab = 1;
        vm.contentTaxonomies = {
            value: taxonomies.map(function (taxonomy) { return taxonomy.Id; }).join(';'),
            delimiter: ';'
        };
        vm.isHtml = vm.content.ContentType == null;
        vm.aliases = [];
        vm.contentUploadEvents = [];
        vm.contentMetaDataUploadEvents = [];
        vm.saveEvents = [];
        vm.contentAliasSaveEvent = function () { };
        vm.permissionSaveEvents = [];
        if (vm.isNew) {
            vm.content.ContentType = contentTypeDefinition;
            vm.content.MetaDataFieldValues = folderMetaData;
        }
        if (vm.isHtml) {
            vm.htmlField = mdFieldService.transformOther(vm.content.Html, true);
        }
        vm.templateScreenshot = '';



        //Public Methods
        vm.changeTab = changeTab;
        vm.registerContentUploadEvents = registerContentUploadEvents;
        vm.resigerContentMetaDataUploadEvents = resigerContentMetaDataUploadEvents;
        vm.registerContentAliasSaveEvent = registerContentAliasSaveEvent;
        vm.registerPermissionSaveEvents = registerPermissionSaveEvents;
        vm.onSave = onSave;
        vm.save = save;
        vm.goBack = goBack;
        vm.getScreenshot = getScreenshot;
        vm.registerContentSaveEvent = registerContentSaveEvent;
        vm.onContentPreSave = onContentPreSave;
        vm.onContentPostSave = onContentPostSave;
        vm.onError = onError;


        //Private Methods
        function onContentPreSave() {
            vm.contentAliasSaveEvent();
        }
        function onContentPostSave(data) {
            vm.content = data;
            var postSaveEvents = [];
            var taxonomyIdArray = vm.contentTaxonomies.value.split(vm.contentTaxonomies.delimiter).filter(function (item) { return item != ""; });
            postSaveEvents.push(assignContentToTaxonomies(taxonomyIdArray, data.Id));
            for (var i = 0; i < vm.permissionSaveEvents.length; i++) {
                var event = vm.permissionSaveEvents[i];
                postSaveEvents.push(event());
            }
            $q.all(postSaveEvents).then(function () {
                $mdFeedbackService.reportInfo("save");
                goBack();
            });
        }
        function onError(error) {
            $mdFeedbackService.reportError('save', error);
        }
        var contentSaveEvent = function () { };
        function registerContentSaveEvent(event) {
            contentSaveEvent = event;
        }

        function registerContentUploadEvents(event) {
            vm.contentUploadEvents.push(event);
        }
        function resigerContentMetaDataUploadEvents(event) {
            vm.contentMetaDataUploadEvents.push(event);
        }
        function registerContentAliasSaveEvent(event) {
            vm.contentAliasSaveEvent = event;
        }
        function registerPermissionSaveEvents(event) {
            vm.permissionSaveEvents.push(event);
        }
        function onSave(event) {
            vm.saveEvents.push(event);
        }

        function goBack() {
            var backtoFolder = $state.params.path;
            $state.go('app.content_list', { folderPath: backtoFolder, currentView: $state.params.currentView }, { reload: false });
        }

        function changeTab(tab) {
            vm.tab = tab;
            $scope.$broadcast('content-tabs-change');
        }

        function getScreenshot() {
            var screenshotUrl = window.location.href.split('/').filter(function (value, i) {
                return i < 3;
            }).join('/') + '/' + (vm.selectedAlias ? vm.selectedAlias : vm.content.ContentAliases[0].Alias);

            var screenshotObject = new mdBusinessLogic.dataAccess.entities.templateScreenshot();
            screenshotObject.ScreenshotUrl = screenshotUrl;
            screenshotObject.ScreenshotWidth = window.innerWidth;
            screenshotObject.ScreenshotHeight = window.innerHeight;
            screenshotObject.Template = vm.content.Template;

            controllers.templateController.getScreenshot(screenshotObject, function (data) {
                $scope.$apply(function () {
                    vm.templateScreenshot = 'uploads/screenshots/' + data.ScreenshotFile;
                });
            }, function (error) {
            })
        }

        function save($event) {
            var promiseArray = [];
            for (var i = 0; i < vm.contentMetaDataUploadEvents.length; i++) {
                var event = vm.contentMetaDataUploadEvents[i];
                promiseArray.push(event());
            }

            $q.all(promiseArray).then(function () {
                contentSaveEvent();
            });


            /*for (var i = 0; i < vm.contentUploadEvents.length; i++) {
                var event = vm.contentUploadEvents[i];
                promiseArray.push(event());
            }


            $q.all(promiseArray).then(function () {
                for (var i = 0; i < vm.saveEvents.length; i++) {
                    var event = vm.saveEvents[i];
                    event();
                }

                if (vm.isHtml) {
                    vm.content.Html = vm.htmlField.value;
                }

                controllers.contentController.save(vm.content, function (data) {
                }, function (error) {
                    $mdFeedbackService.reportError('save', error);
                });
            });*/
        }

        function assignContentToTaxonomies(taxonomyIds, contentId) {
            return $q(function (resolve, reject) {
                if (contentId === undefined || contentId == null || contentId.length == 0) {
                    resolve();
                } else {
                    controllers.taxonomyController.assignContentToTaxonomies(taxonomyIds, contentId, function (obj) {
                        resolve();
                    }, function (error) {
                        reject();
                    });
                }
            });
        }

        function init() {
        }

        init();
    }
})();
