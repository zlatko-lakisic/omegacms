(function () {
    'use strict';

    angular
        .module('app.personal.profile')
        .controller('ProfileController', ['$state', '$scope', '$q', 'mdFeedbackService', 'user', ProfileController]);


    /** @ngInject */
    function ProfileController($state, $scope, $q, $mdFeedbackService, user) {
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
            permissionControllerUser: new mdBusinessLogic.dataAccess.controllers.permissionControllerUser(),
            profileController: new mdBusinessLogic.dataAccess.controllers.profileController()
        }

        //Private Attributes
        var saveEvents = [];
        var profileTypeUploadEvents = [];

        //Public Attributes
        vm.user = user;
        vm.originalUser = angular.copy(vm.user);
        vm.changePass = false;

        //Private Methods
        function onSave(event) {
            saveEvents.push(event);
        }
        function registerProfileTypeUploadEvents(event) {
            profileTypeUploadEvents.push(event);
        }
        function save($event) {
            var promiseArray = [];
            for (var i = 0; i < profileTypeUploadEvents.length; i++) {
                var event = profileTypeUploadEvents[i];
                promiseArray.push(event());
            }

            if (promiseArray.length > 0) {
                $q.all(promiseArray).then(saveUserAfterPromise);
            } else {
                saveUserAfterPromise();
            }

            function saveUserAfterPromise() {
                for (var i = 0; i < saveEvents.length; i++) {
                    var event = saveEvents[i];
                    event();
                }
                controllers.userController.updateUser(vm.user, function () {
                    var postSavePromiseArray = [];
                    for (var i = 0; i < vm.user.ProfileTypes.length; i++) {
                        var profileType = vm.user.ProfileTypes[i];
                        for (var p in profileType.Fields) {
                            profileType.Fields[p].UserId = vm.user.Id;
                        }
                        postSavePromiseArray.push(createProfileTypeSave(profileType));
                    }
                    $q.all(postSavePromiseArray).then(function () {
                        $mdFeedbackService.reportInfo("save");
                        reload();
                    });
                }, function (error) {
                    $mdFeedbackService.reportError('save', error);
                });
            }
        }
        function createProfileTypeSave(profileType) {
            return $q(function (resolve, reject) {
                controllers.profileTypeController.saveProfileTypeWithProfileTypeFieldValues(profileType, function (data) {
                    resolve(true);
                }, function (error) {
                    resolve(false);
                });
            });
        }
        function registerChangePassword() {
            vm.changePass = vm.user.Password !== undefined && vm.user.Password != '';
        }
        function reload() {
            $state.go($state.current.name, $state.params, { reload: true });
        }
        function cancel() {
            $state.go(mdBusinessLogic.settings.defaultState, {}, { reload: true, $retry: true });
        }

        //Public Methods
        vm.onSave = onSave;
        vm.registerProfileTypeUploadEvents = registerProfileTypeUploadEvents;
        vm.save = save;
        vm.registerChangePassword = registerChangePassword;
        vm.reload = reload;
        vm.cancel = cancel;
    }
})();
