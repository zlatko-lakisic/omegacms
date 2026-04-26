(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsContentFormController', ['$scope', '$q', 'mdFieldService', 'mdFeedbackService', mdCmsContentFormController]);
    /** @ngInject */
    function mdCmsContentFormController($scope, $q, mdFieldService, $mdFeedbackService) {

        //Private Attributes
        var vm = this;
        var contentUploadEvents = [];
        var saveEvents = [];

        //Public Attributes
        vm.content = null;
        vm.contentFormName = 'contentForm';
        vm.folder = null;
        vm.showVersions = false;
        vm.showLanguages = false;
        vm.contentVersions = [];
        vm.cultures = [];
        vm.currentContentVersion = null;
        vm.selectedCulture = null;

        //Public Methods
        vm.registerContentUploadEvents = registerContentUploadEvents;
        vm.onSave = onSave;
        vm.loadVersions = loadVersions;
        vm.loadLanguages = loadLanguages;

        //Private Methods
        var mdOnError = function (data) { };
        var mdOnPreSave = function () { };
        var mdOnPostSave = function (data) { };
        function loadVersions() {
            var promise = $q(function (resolve, reject) {
                if (vm.showVersions && !vm.content.IsNew) {
                    (new mdBusinessLogic.dataAccess.controllers.contentController()).getAllVersion(vm.content.Id, function (data) {
                        vm.contentVersions = data;
                        resolve();
                    }, function (error) {
                        reject();
                        $mdFeedbackService.reportError("load", error);
                    });
                } else {
                    resolve();
                }
            });
            return promise;
        }
        function loadLanguages() {
            var promise = $q(function (resolve, reject) {
                if (vm.showLanguages && !vm.content.IsNew) {
                    (new mdBusinessLogic.dataAccess.controllers.cultureController()).getAll(function (data) {
                        vm.cultures = data;
                        resolve();
                    }, function (error) {
                        $mdFeedbackService.reportError("load", error);
                        resolve();
                    });
                } else {
                    resolve();
                }
            });
            return promise;
        }
        function onSave(event) {
            saveEvents.push(event);
        }
        function registerContentUploadEvents(event) {
            contentUploadEvents.push(event);
        }
        function save() {
            var promise = $q(function (resolve, reject) {
                var promiseArray = [];

                for (var i = 0; i < contentUploadEvents.length; i++) {
                    var event = contentUploadEvents[i];
                    promiseArray.push(event());
                }

                $q.all(promiseArray).then(function () {
                    for (var i = 0; i < saveEvents.length; i++) {
                        var event = saveEvents[i];
                        event();
                    }

                    if (vm.isHtml) {
                        vm.content.Html = vm.htmlField.value;
                    }

                    if (vm.content.ContentType.hasLinkToTitle()) {
                        vm.content.Title = vm.content.ContentType.getLinkToTitle().Value;
                    }

                    mdOnPreSave({});

                    (new mdBusinessLogic.dataAccess.controllers.contentController()).save(vm.content, function (data) {
                        mdOnPostSave({ data: data });
                        resolve(data);
                    }, function (error) {
                        mdOnError({ error: error });
                        reject(error);
                    });
                });
            });
            promise.then();
            return promise;
        }
        function init() {
            $scope.$watch(function () {
                return $scope.mdContentFormName;
            }, function (mdContentFormName) {
                vm.contentFormName = mdContentFormName;
            });

            $scope.$watchGroup([function () {
                return $scope.mdFolder;
            }, function () {
                return $scope.mdContent;
            }, function () {
                return $scope.mdContentTypeDefinition;
            }, function () {
                return $scope.mdShowVersions;
            }, function () {
                return $scope.mdShowLanguages;
            }, function () {
                return $scope.mdFolderId;
            }], function (data) {
                if (data[1] !== undefined && data[1] != null) {
                    vm.content = data[1];
                    vm.currentContentVersion = vm.content.DateCreated;
                    vm.selectedCulture = vm.content.LCID;

                    if (data[0] !== undefined && data[0] != null) {
                        vm.folder = data[0];
                        vm.content.FolderId = vm.folder.Id;
                    }

                    if (data[5] !== undefined && data[5] != null) {
                        vm.content.FolderId = data[5];
                        (new mdBusinessLogic.dataAccess.controllers.folderController()).getById(vm.content.FolderId, function (data) {
                            vm.folder = data;
                        }, function (error) { });
                    }

                    if (data[2] !== undefined && data[2] != null) {
                        vm.content.ContentType = data[2];
                        vm.content.ContentTypeDefinitionId = data[2].Id;
                    }

                    vm.htmlField = mdFieldService.transformOther(vm.content.Html, true);

                    if (vm.content && vm.content.ContentType && vm.content.ContentType.hasLinkToTitle()) {
                        $scope.$watch(function () {
                            return vm.content.ContentType.getLinkToTitle().Value;
                        }, function (title) {
                            vm.content.Title = title;
                        });
                    }

                    if (data[3] !== undefined && data[3] != null) {
                        vm.showVersions = data[3];
                    }
                    
                    if (data[4] !== undefined && data[4] != null) {
                        vm.showLanguages = data[4];
                    }

                    $scope.$watch(function () { return vm.currentContentVersion; }, function (currentContentVersion) {
                        if (currentContentVersion !== undefined && currentContentVersion != null) {
                            var content = vm.contentVersions.filter(function (content) { return content.DateCreated == vm.currentContentVersion; })[0];
                            if (content) {
                                vm.content = content;
                            }
                        }
                    }, true);

                    $scope.$watch(function () { return vm.selectedCulture; }, function (selectedCulture) {
                        if (selectedCulture !== undefined && selectedCulture != null) {
                            
                        }
                    }, true);
                }
            });

            $scope.$watch(function () { return vm.htmlField; }, function (htmlField) {
                if (htmlField !== undefined && htmlField != null) {
                    vm.content.Html = htmlField.value;
                }
            }, true);

            $scope.$watch(function () {
                return $scope.mdOnError;
            }, function (_mdOnError) {
                mdOnError = _mdOnError;
            });

            $scope.$watch(function () {
                return $scope.mdOnPreSave;
            }, function (_mdOnPreSave) {
                mdOnPreSave = _mdOnPreSave;
            });

            $scope.$watch(function () {
                return $scope.mdOnPostSave;
            }, function (_mdOnPostSave) {
                mdOnPostSave = _mdOnPostSave;
            });

            $scope.$watch(function () {
                return $scope.mdSaveEvent;
            }, function (mdSaveEvent) {
                mdSaveEvent({ event: save });
            });
        }

        init();
    }
})();
