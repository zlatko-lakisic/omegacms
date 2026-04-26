(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsContentFormDialogController', ['$scope', '$mdDialog', 'mdFieldService', '$q', 'data', mdCmsContentFormDialogController]);
    /** @ngInject */
    function mdCmsContentFormDialogController($scope, $mdDialog, mdFieldService, $q, data) {

        //Private Attributes
        var vm = this;

        //Public Attributes
        vm.loaded = false;
        vm.newElementData = null;
        vm.data = data;
        vm.folder = null;
        vm.selectedTab = 0;
        vm.startFolder = null;
        vm.contentTypeDefinition = null;

        //Public Methods
        vm.onContentPreSave = onContentPreSave;
        vm.onContentPostSave = onContentPostSave;
        vm.onError = onError;
        vm.registerContentSaveEvent = registerContentSaveEvent;
        vm.save = save;
        vm.selectFolder = selectFolder;
        vm.cancelNewElementDialog = cancelNewElementDialog;

        //Private Methods
        function getFolderByPathPromise(path) {
            if (!path) {
                path = '';
            }

            if (!path.trim().length || !vm.data.constraints.folderPaths.filter(function (f) { return f.indexOf(path) >= 0; }).length) {
                path = vm.data.constraints.folderPaths[0];
                if (!path || !path.length) {
                    path = 'Root';
                }
            }
            return $q(function (resolve, reject) {
                (new mdBusinessLogic.dataAccess.controllers.folderController()).getByFolderPath(path, false, function (data) {
                    resolve(data);
                }, function (error) {
                    resolve(error);
                });
            });
        }
        function onContentPreSave() {

        }
        function onContentPostSave(data) {
            $mdDialog.hide(data);
        }
        function onError(error) {

        }
        var saveEvent = function () { }
        function registerContentSaveEvent(event) {
            saveEvent = event;
        }
        function save() {
            saveEvent();
        }
        function selectFolder(path) {
            getFolderByPathPromise(path).then(function (data) {
                vm.folder = data;
            });
        }
        function cancelNewElementDialog() {
            $mdDialog.cancel();
        }

        function init() {
            var promises = [
                getFolderByPathPromise(),
                $q(function (resolve, reject) {
                    if (vm.data.constraints.contentTypeId != '' && vm.data.constraints.contentTypeId != 0) {
                        (new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController()).getById(vm.data.constraints.contentTypeId, function (data) {
                            resolve(data);
                        }, function (error) {
                            resolve(null);
                        });
                    } else {
                        resolve(null);
                    }
                })
            ];

            $q.all(promises).then(function (data) {
                vm.contentTypeDefinition = data[1];
                vm.startFolder = data[0];
                vm.folder = data[0];
                vm.loaded = true;
                vm.newElementData = {
                    contentTypeDefinition: null,
                    folder: data[0],
                    onContentPreSave: function () {

                    },
                    onContentPostSave: function (data) {
                        vm.data.onContentPostSave(data);
                    },
                    onError: function (error) {

                    },
                    saveEvent: function () { },
                    registerContentSaveEvent: function (event) {
                        vm.newElementData.saveEvent = event;
                    },
                    save: function () {
                        vm.newElementData.saveEvent();
                        $mdDialog.hide();
                    },
                    selectedTab: 0,
                    selectFolder: function (path) {
                        getFolderByPathPromise(path).then(function (data) {
                            scope.newElementData.folder = data;
                        });
                    }
                };
            });
        }

        init();
    }
})();
