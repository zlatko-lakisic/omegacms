(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsFileController', ['$scope', '$timeout', '$element', '$q', mdCmsFileController]);
    /** @ngInject */
    function mdCmsFileController($scope, $timeout, $element, $q) {

        //Private Attributes
        var vm = this;
        var forbiddenTypes = ['exe', 'apk', 'bat', 'bin', 'cgi', 'pl', 'com', 'gadget', 'jar', 'py', 'wsf'];
        var defaultMimeType = 'image/*,video/*,audio/*,application/*,text/*';
        var fileTypeRegex = null;

        //Public Attributes
        vm.mdModel = null;
        vm.disabled = false;
        vm.required = false;
        vm.folderPath = 'Root/';
        vm.inputName = null;
        vm.floatingLabel = null;
        vm.mimeType = defaultMimeType;
        vm.extension = '*';
        vm.anyFiles = false;
        vm.filesRemoved = false;
        vm.filesModified = false;
        vm.firstLoaded = true;
        vm.fileAllowed = false;
        vm.files = [];
        vm.firstLoad = true;
        vm.firstLoadAdd = true;
        
        //Public Methods
        vm.addRemoteFilesApi = null;
        vm.onFileEvent = onFileEvent;
        vm.getInnerHtml = getInnerHtml;


        //Private Methods
        function createlfFileObject(fileName, fileUrl, isRemote) {
            if (isRemote === undefined) {
                isRemote = true;
            }
            return {
                "key": "",
                "lfFile": {},
                "lfFileName": fileName,
                "lfDataUrl": fileUrl,
                "isRemote": isRemote
            };
        }
        function onFileEvent(event, files) {
            vm.files = files;
            vm.anyFiles = vm.files.length > 0;
            vm.filesRemoved = (event == 'removeFile' || event == 'removeAllFiles') && !vm.firstLoad;
            vm.filesModified = true;

            if (vm.anyFiles && vm.files[0].lfFile) {
                vm.firstLoaded = false;
                vm.fileAllowed = vm.files[0].lfFile.type.match(fileTypeRegex);
                vm.mdExtension = vm.files[0].lfFileName.split('.')[vm.files[0].lfFileName.split('.').length - 1].toLowerCase();
                if (!vm.fileAllowed) {
                    vm.files = [];
                    vm.mdModel.value = '';
                } else {
                    vm.mdModel.value = vm.files[0].lfFileName;
                }
            } else {
                vm.mdModel.value = '';
            }
        }
        function validateType(type) {
            type = type.split('/')[1];
            for (var i = 0; i < forbiddenTypes.length; i++) {
                if (type == forbiddenTypes[i]) {
                    return false;
                }
            }
            return true;
        }
        function upload() {
            if (vm.folderPath === undefined || vm.folderPath == null || vm.folderPath == '') {
                throw new mdBusinessLogic.helpers.mdException("The folder path must be set for the file upload directive!");
            }

            var fileController = new mdBusinessLogic.dataAccess.controllers.fileController();

            var deferred = $q.defer();

            if (vm.anyFiles) {

                var file = new mdBusinessLogic.dataAccess.entities.file();
                file.path = vm.folderPath;
                for (var i = 0; i < vm.files.length; i++) {
                    if (vm.files[i].lfFile && validateType(vm.files[i].lfFile.type)) {
                        file.data = vm.files[i].lfFile;
                    }
                }
                if (file.data != null) {
                    file.fileType = mdBusinessLogic.dataAccess.entities.fileTypeEnum[file.data.type.split('/')[0]];
                    fileController.upload(file, function (data) {
                        if (data) {
                            vm.mdModel.value = data.PathToSaveToDatabase;
                            deferred.resolve(true);
                        } else {
                            showDialog(
                                $rootScope.globals.resources.Titles.ActionNotCompleted,
                                $rootScope.globals.resources.Labels.InvalidFormat,
                                false);
                            deferred.resolve(false);
                        }
                    }, function (error) {
                        $mdFeedbackService.reportError('save', error);
                        deferred.resolve(false);
                    });
                } else {
                    deferred.resolve(true);
                }
            } else {
                deferred.resolve(true);
            }

            return deferred.promise;
        }
        function getInnerHtml() {
            return $element.find('md-inner-html').html() || '';
        }


        function init() {
            $scope.$watchGroup([
                function () { return $scope.mdModel; },
                function () { return $scope.ngDisabled; },
                function () { return $scope.mdFolderPath; },
                function () { return $scope.mdInputName; },
                function () { return $scope.mdMimeType; },
                function () { return $scope.mdRequired; },
                function () { return $scope.mdFloatingLabel; },
                function () { return $scope.mdExtension; },
                function () { return vm.addRemoteFilesApi; }
            ], function (data) {
                    if (data[0]) {
                        vm.mdModel = data[0];
                    }
                    if (data[1]) {
                        vm.disabled = data[1];
                    }
                    if (data[2]) {
                        vm.folderPath = data[2];
                    }
                    if (data[3]) {
                        vm.inputName = data[3];
                    }
                    if (data[4]) {
                        vm.mimeType = data[4];
                    }
                    if (data[5]) {
                        vm.required = data[5];
                    }
                    if (data[6]) {
                        vm.floatingLabel = data[6];
                    }
                    if (data[7]) {
                        vm.extension = data[7];
                    }
                    if (data[8]) {
                        vm.addRemoteFilesApi = data[8];
                    }

                    fileTypeRegex = new RegExp(vm.mimeType.replace(/,/g, '|'), "i");

                    if (vm.firstLoad = vm.mdModel && vm.mdModel.value != null) {

                        var fileUrl = vm.mdModel.value;
                        if (fileUrl.indexOf('http') < 0) {
                            fileUrl = mdBusinessLogic.settings.uploadsBase + fileUrl;
                        }
                        var fileName = vm.mdModel.value.split("/");
                        fileName = fileName[fileName.length - 1];
                        var fileType = vm.mdModel.FileType;
                        var fileTypeString = fileName.split('.');
                        fileTypeString = fileTypeString[fileTypeString.length - 1];

                        $timeout(function () {
                            if (vm.addRemoteFilesApi && vm.firstLoadAdd) {
                                vm.firstLoadAdd = false;
                                vm.addRemoteFilesApi.addRemoteFile(fileUrl, fileName, vm.mimeType.split('/')[0]);
                            }
                        });
                        vm.firstLoad = false;
                    }

            });

            $scope.$watch(function () { return $scope.registerUploadEvent; }, function (registerUploadEvent) {
                if (registerUploadEvent) {
                    registerUploadEvent()(upload);
                }
            });

            $scope.$watch('mediaContentForm.$error', function (form) {
                if (form) {

                }
            });
        } 

        init();
    }
})();
