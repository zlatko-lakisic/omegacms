(function () {
    'use strict';

    angular
        .module('app.mediacontent.form')
        .controller('MediaContentFormController', ['$state', '$timeout', '$rootScope', '$mdSidenav', '$mdDialog', '$scope', '$http', '$document', '$sce', '$window', '$q', 'uiGmapGoogleMapApi', 'mdFeedbackService', 'mediaContent', 'metaDataFields', 'mdFieldService', 'mdPromiseResolverService', MediaContentFormController]);

    function MediaContentFormController($state, $timeout, $rootScope, $mdSidenav, $mdDialog, $scope, $http, $document, $sce, $window, $q, uiGmapGoogleMapApi, $mdFeedbackService, mediaContent, metaDataFields, mdFieldService, mdPromiseResolverService) {
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

        var enums = {
            mediaContentInputType: mdBusinessLogic.dataAccess.entities.mediaContentInputType
        };


        //Page Variables
        vm.mediaContent = mediaContent;
        vm.folderPath = $state.params.path;
        vm.folderId = $state.params.folderId;
        vm.isNew = $state.params.action != 'edit';
        vm.lcid = $state.params.lcid || 2057;
        vm.formTitle = vm.isNew ? $rootScope.globals.resources.Titles.AddContent : $rootScope.globals.resources.Titles.EditContent;
        vm.tab = 1;
        vm.contentUploadEvents = [];
        vm.contentMetaDataUploadEvents = [];
        vm.saveEvents = [];
        vm.permissionSaveEvents = [];
        vm.description = mdFieldService.transformOther(vm.mediaContent.Description, true);
        vm.file = mdFieldService.transformOther(vm.mediaContent.FullNameFile, true);
        vm.fileType = vm.isNew ? $state.params.fileType : vm.mediaContent.FileType;
        vm.mediaContent.Type = vm.fileType;
        vm.fileExtension = vm.isNew ? '' : enums.mediaContentInputType[vm.mediaContent.InputType]
        setFileTypeParams(vm.fileType);
        


        //Public Methods
        vm.changeTab = changeTab;
        vm.registerContentUploadEvents = registerContentUploadEvents;
        vm.resigerContentMetaDataUploadEvents = resigerContentMetaDataUploadEvents;
        vm.registerPermissionSaveEvents = registerPermissionSaveEvents;
        vm.onSave = onSave;
        vm.save = save;
        vm.goBack = goBack;


        //Private Methods
        function registerContentUploadEvents(event) {
            vm.contentUploadEvents.push(event);
        }
        function resigerContentMetaDataUploadEvents(event) {
            vm.contentMetaDataUploadEvents.push(event);
        }
        function registerPermissionSaveEvents(event) {
            vm.permissionSaveEvents.push(event);
        }
        function onSave(event) {
            vm.saveEvents.push(event);
        }

        function goBack() {
            var backtoFolder = $state.params.path;
            $state.go('app.mediacontent_list', { folderPath: backtoFolder, currentView: $state.params.currentView }, { reload: false });
        }

        function changeTab(tab) {
            vm.tab = tab;
        }

        function save($event) {

            mdPromiseResolverService.resolve([
                vm.contentUploadEvents,
                vm.contentMetaDataUploadEvents
            ], function (data) {
                for (var i in vm.saveEvents) {
                    var event = vm.saveEvents[i];
                    event();
                }

                vm.mediaContent.FullNameFile = vm.file.value;
                vm.mediaContent.Description = vm.description.value;
                vm.mediaContent.InputType = enums.mediaContentInputType[vm.fileExtension];
                vm.mediaContent.FolderId = vm.folderId;
                    
                controllers.mediaContentController.save(vm.mediaContent, function (data) {
                    vm.mediaContent = data;

                    mdPromiseResolverService.resolve([vm.permissionSaveEvents], function () {
                        $mdFeedbackService.reportInfo("save");
                        goBack();
                    }, function (e) {
                        console.log(e);
                    });

                }, function (error) {
                    $mdFeedbackService.reportError('save', error);
                });
            }, function (e) {
                console.log(e);
            });
        }

        function setFileTypeParams(fileType) {
            fileType = parseInt(fileType);
            switch (fileType) {
                case 1:
                    vm.mimeType = 'image/*';
                    vm.fileTypeString = 'image';
                    break;
                case 2:
                    vm.mimeType = 'video/*';
                    vm.fileTypeString = 'video';
                    break;
                case 3:
                    vm.mimeType = 'audio/*';
                    vm.fileTypeString = 'audio';
                    break;
                case 4:
                    vm.mimeType = 'application/*,text/*';
                    vm.fileTypeString = 'document'
                    break;
                default:
                    break;
            }

            vm.validationMessage = 'Only ' + vm.fileTypeString + 's allowed';
            vm.formTitle = $state.params.action + " " + vm.fileTypeString;
        }

        function init() {
        }

        init();




































        /*
        // Controllers
        var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
        var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
        var metaDataFieldController = new mdBusinessLogic.dataAccess.controllers.metaDataFieldController();
        var folderMediaContentMetaDataFieldController = new mdBusinessLogic.dataAccess.controllers.folderMediaContentMetaDataFieldController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
        var taxonomycontentController = new mdBusinessLogic.dataAccess.controllers.taxonomyContentController();
        var metaDataFieldController = new mdBusinessLogic.dataAccess.controllers.metaDataFieldController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        vm.folder = new mdBusinessLogic.dataAccess.entities.folder();
        vm.dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        // Variables
        vm.createMediaContentEnded = true;
        vm.disableSubmitBtn = false;
        vm.mediacontent = mediaContent;
        vm.mimeType;
        vm.fileTypeString;
        vm.validationMessage;
        vm.changeFile = false;
        var maxNumberOfRows = 10;
        vm.isNew = $state.params.action != 'edit';
        vm.folderId = $state.params.folderId;
        vm.id = 0;
        vm.currentFolderPath = $state.params.path;
        vm.currentView = $state.params.currentView;
        vm.folder = {};
        var addOrEdit = $state.params.action;
        vm.formTitle = addOrEdit === 'add' ? $rootScope.globals.resources.Titles.AddMediaContent : $rootScope.globals.resources.Titles.EditMediaContent;
        vm.folderMediaContent;
        vm.save = save;
        vm.mediacontent.FolderId = vm.folderId;
        vm.lcid = mdBusinessLogic.settings.lcid || 2057;
        vm.fields = {};
        vm.metaDataFields = metaDataFields;
        vm.metaDataFieldVals = {};
        vm.addMetaDataFieldsToMediaContent = addMetaDataFieldsToMediaContent;
        vm.files = [];
        $scope.SelectItem = false;
        vm.upload = [];
        vm.contentsByFolder = [];
        vm.fieldValues = [];
        vm.fieldsLoaded = false;
        vm.map = {};
        vm.mainPart;
        vm.marker = {};
        var mapFieldsArray = [];
        vm.notAuthorizedUsers = [];
        var parentContent = {};
        vm.profileTypes = [];
        vm.userPermissions = [];
        vm.removedtaxonomy = [];
        vm.selected = [];
        vm.selectedMeta = [];
        vm.selectedtaxonomyItem;
        vm.selectedContent;
        vm.selectedContentMeta;
        vm.selectedTaxonomy;
        vm.selectedTaxonomyMeta;
        vm.tab = 1;
        vm.taxonomies = [];
        vm.taxonomy = [];
        vm.targetEnumeration = 3;
        vm.taxonomySearchText;
        vm.tinymceModel = '';
        vm.tinymceOptions = {
            plugins: 'link image code',
            toolbar: 'undo redo | bold italic | alignleft align center alignright | code'
        }
        var getAllCountentsCounter = 0;
        var unicateMapValues = [];
        vm.forbiddenTypes = ['exe', 'apk', 'bat', 'bin', 'cgi', 'pl', 'com', 'gadget', 'jar', 'py', 'wsf'];
        vm.allowedExtension;
        vm.youtubeRegex = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;

        vm.permissionSaveEvents = [];
        vm.saveEvents = [];
        
        var isValid = true;
        // Methods  
        vm.getContent = getContentFromTextArea;
        vm.setContent = setContentToTextArea;
        vm.addAnotherTableRow = addAnotherTableRow;
        vm.goBack = goBack;
        vm.toggleChangeFile = toggleChangeFile;
        vm.YoutubeVideoUrl = "";
        vm.redirectToYoutube = redirectToYoutube
        vm.extractMediaContentPrimaryKey = extractMediaContentPrimaryKey;
        vm.youtubeVideo = youtubeVideo;

        vm.ValidationCheck = ValidationCheck;
        vm.showValidationMessage = showValidationMessage;

        vm.resigerContentMetaDataUploadEvents = resigerContentMetaDataUploadEvents;
        vm.registerPermissionSaveEvents = registerPermissionSaveEvents;
        vm.onSave = onSave;


        function resigerContentMetaDataUploadEvents(event) {
            vm.contentMetaDataUploadEvents.push(event);
        }

        function registerPermissionSaveEvents(event) {
            vm.permissionSaveEvents.push(event);
        }

        function onSave(event) {
            vm.saveEvents.push(event);
        }

        getFolderByPath();

        function init() {
            if (vm.metaDataFields) {
                if (vm.mediacontent.MediaContentMetaDataFieldValues) {
                    for (var j in vm.metaDataFields) {
                        for (var i in vm.mediacontent.MediaContentMetaDataFieldValues) {
                            if(vm.mediacontent.MediaContentMetaDataFieldValues[i].MetaDataFieldId == vm.metaDataFields[j].Id){
                                vm.metaDataFields[j].Value = vm.mediacontent.MediaContentMetaDataFieldValues[i].Value;
                            }
                        }
                    }
                }
                
                vm.metaDataFieldVals = fillNgModelFields(vm.metaDataFieldVals, vm.metaDataFields);
                setAppropriateFields(vm.metaDataFieldVals, 2);
            }
            if (vm.isNew) {
                vm.fileType = $state.params.fileType;  //image = 1, video = 2, audio = 3, document = 4
                setFileTypeParams(vm.fileType);
            } else {
                if ($state.params.id) {
                    vm.id = $state.params.id;
                    vm.mediacontent.Id = vm.id;
                    vm.fileUrl = mdBusinessLogic.settings.uploadsBase + vm.mediacontent.FullNameFile;
                    vm.fileName = vm.mediacontent.FullNameFile.split("/");
                    vm.fileName = vm.fileName[vm.fileName.length-1];
                    vm.fileType = vm.mediacontent.FileType;
                    setFileTypeParams(vm.fileType);

                    $timeout(function () {
                        $scope.addRemoteFilesApi.addRemoteFile(vm.fileUrl, vm.fileName, vm.fileTypeString);
                    });
                }
            }
        }
        init();
   
        function getUniqueName(field, fieldType) {
            return field.FriendlyName + field.Id + "" + (fieldType || "");
        }

        function extractMediaContentPrimaryKey(MediaContent) {
            var MediacontentId = MediaContent.Id;
            var LCID = MediaContent.LCID;
            var DateCreated = MediaContent.DateCreated;

            var MediaContentPrimaryKey = MediacontentId + ';' + LCID + ';' + moment(DateCreated).format("YYYY-MM-DD HH:mm:ss");
            return MediaContentPrimaryKey;
        }

        function redirectToYoutube() {
            $window.open(vm.mediacontent.PreviewUrl, '_blank');
        }

        // Dynamic form generate funcions

        //Map functions
        function makeUnicateMapValue(fieldName, ngModelFields, fieldsType) {
            return ngModelFields[fieldName].id + "" + fieldsType;
        }

        function changeMapFieldValue(lat, lng, fieldName, fields, fieldType) {
            if (fields) {
              if (!fields[fieldName] || mdBusinessLogic.helpers.checkType.isFunction(fields[fieldName])) {
                    fields[fieldName] = {};
                }
                fields[fieldName].value = lat + ';' + lng;
                if (fieldType == 1) {
                    vm.fields[fieldName].value = fields[fieldName].value;
                } else if (fieldType == 2) {
                    vm.metaDataFieldVals[fieldName].value = fields[fieldName].value;
                }
            }
        }

        function cloneMap(mapToClone, uniqueId) {
            return vm["map" + uniqueId] = JSON.parse(JSON.stringify(mapToClone));
        }

        function giveMapProperties(map, uniqueId, fieldName, fields, fieldType) {
            map.center = {
                latitude: 43.336995,
                longitude: 17.814417
            };

            map.zoom = 17;
            map.events = {
                click: function (map, eventName, originalEventArgs) {
                    var e = originalEventArgs[0];
                    var lat = e.latLng.lat(), lon = e.latLng.lng();
                    $scope.$apply(function () {
                        vm["map" + uniqueId].marker = new google.maps.Marker({
                            coords: {
                                latitude: lat,
                                longitude: lon
                            }
                        });
                        vm["map" + uniqueId].marker;
                        vm["map" + uniqueId].marker.coords;
                        changeMapFieldValue(lat, lon, fieldName, fields, fieldType);
                    })
                }
            }
        }

        function setMaps(mapFields, fieldType) {
            uiGmapGoogleMapApi.then(function (map) {
                vm.map = map;
                for (var i in mapFields) {
                  if (!mdBusinessLogic.helpers.checkType.isFunction(mapFields[i])) {
                        var uniqueId = mapFields[i].id + "" + fieldType;
                        var clonedMap = cloneMap(vm.map, uniqueId);
                        giveMapProperties(clonedMap, uniqueId, mapFields[i].name, mapFields, fieldType);

                        if (mapFields[i].value) {
                            var latlong = mapFields[i].value.split(';');
                            vm["map" + uniqueId].marker = new google.maps.Marker({
                                coords: {
                                    latitude: latlong[0],
                                    longitude: latlong[1]
                                }
                            })

                            vm["map" + uniqueId].center = {
                                latitude: vm["map" + uniqueId].marker.coords.latitude,
                                longitude: vm["map" + uniqueId].marker.coords.longitude
                            }
                        }
                    }
                }
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });
        }
        //END Map functions

        // Youtube functions
        function youtubeVideo(url) {
            if (url) {
                var p = vm.youtubeRegex;
                var videoId = (url.match(p)) ? RegExp.$1 : false;
                if (videoId) {
                    return $sce.trustAsResourceUrl("//www.youtube.com/embed/" + videoId);
                } else {
                    return "";
                }
            } else {
                return "";
            }
        }

        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
        }

        function createEmbeded(mainPart) {
            return "<iframe width='560' height='315' src='https://www.youtube.com/embed/"
                + mainPart + "' frameborder='0' allowfullscreen></iframe>"
        }

        function setYoutubeField(fieldName) {
            if (vm.fields[fieldName]) {
                vm.mainPart = getParameterByName('v', vm.fields[fieldName].value);
                var embeded = createEmbeded(vm.mainPart);
                vm.fields[fieldName].embeded = embeded;
                vm.fields[fieldName].mainPart = vm.mainPart;
            }
        }
        //END Youtube function

        // Text area functions
        function getContentFromTextArea(id, type) {
            if (id && type) {
                return vm["tinymceModel" + id + '' + type];
            } else {
                return vm.tinymceModel;
            }

        }

        function setContentToTextArea(sadrzajForme, id, type) {
            if (id && type) {
                vm["tinymceModel" + id + '' + type] = sadrzajForme;
            } else {
                vm.tinymceModel = sadrzajForme;
            }

        }

        function setTextareaField(fieldName, ngModelFields, fieldsType) {
            setContentToTextArea(ngModelFields[fieldName].value, ngModelFields[fieldName].id, fieldsType)
        }
        //END Text area functions

        // Content select functions
        function setContentField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldContent();
            var assignedContentId = ngModelFields[fieldName].value || 0;
            if (assignedContentId) {
                contentController.getById(assignedContentId, false, vm.lcid, false, false, 0,
                    function (contentData) {
                        vm["contentPickerFieldValueToSave" + ngModelFields[fieldName].id + "" + fieldsType] = contentData;
                    }, function () { });
            }
        }

        function setCustomFieldContent() {
            vm.chooseContent = chooseContent;
            vm.queryContents = queryContents;
            if (getAllCountentsCounter == 0) {
                getAllCountentsCounter++;
                contentController.getAll(function (data) {
                    vm.contentsByFolder = data.map(function (content) {
                        content._lowertitle = content.Title.toLowerCase();
                        return content;
                    });
                }, function (error) {
                });
            }
        }

        function chooseContent(selected, unicateValue) {
            if (selected) {
                vm["contentPickerFieldValueToSave" + unicateValue] = selected;
            } else {
                vm["contentPickerFieldValueToSave" + unicateValue] = null;
            }
        }

        function queryContents(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.contentsByFolder.filter(function (query) {
                return function filterFn(content) {
                    return (content._lowertitle.indexOf(lowercaseQuery) === 0);
                };
            }) : [];

            var i = results.length;
            while (i--) {
                if (
                    results[i]._lowertitle.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }
        //END Content select functions

        // Taxonomy select functions
        function setTaxonomyField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldTaxonomy();
            var assignedTaxonomyId = ngModelFields[fieldName].value || 0;
            if (assignedTaxonomyId) {
                taxonomyController.getById(assignedTaxonomyId, function (taxonomyData) {
                    vm["taxonomyPickerFieldValueToSave" + ngModelFields[fieldName].id + "" + fieldsType] = taxonomyData;
                }, function (error) { })
            }
        }

        function setCustomFieldTaxonomy() {
            taxonomyController.getAll(vm.lcid, function (data) {
                vm.taxonomies = data.map(function (taxonomy) {
                    taxonomy._lowertitle = taxonomy.Name.toLowerCase();
                    return taxonomy;
                });
            });
            vm.chooseTaxonomy = chooseTaxonomy;
            vm.queryTaxonomies = queryTaxonomies;
        }

        function chooseTaxonomy(selected, unicateValue) {
            if (selected) {
                vm["taxonomyPickerFieldValueToSave" + unicateValue] = selected;
            } else {
                vm["taxonomyPickerFieldValueToSave" + unicateValue] = null;
            }
        }

        function queryTaxonomies(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.taxonomies.filter(function (query) {
                return function filterFn(taxonomy) {
                    return (taxonomy._lowertitle.indexOf(lowercaseQuery) === 0);
                };
            }) : [];
            var i = results.length;
            while (i--) {
                if (
                    results[i]._lowertitle.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }
        //END Taxonomy select functions

        // User select functions
        function setUserField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldUser();
            var assignedUserId = ngModelFields[fieldName].value || 0;
            if (assignedUserId) {
                userController.getById(assignedUserId, function (userData) {
                    vm["userPickerFieldValueToSave" + ngModelFields[fieldName].id + "" + fieldsType] = userData;
                }, function (error) { })
            }
        }

        function setCustomFieldUser() {
            vm.chooseUser = chooseUser;
            vm.queryUsers = queryUsers;
        }

        function chooseUser(selected, unicateValue) {
            if (selected) {
                vm["userPickerFieldValueToSave" + unicateValue] = selected;
            } else {
                vm["userPickerFieldValueToSave" + unicateValue] = null;
            }
        }

        function queryUsers(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.allUsers.filter(function (query) {
                return function filterFn(user) {
                    return (user._lowerUsername.indexOf(lowercaseQuery) === 0);
                };
            }) : [];

            var i = results.length;
            while (i--) {
                if (
                    results[i]._lowerUsername.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }
        //END User select functions

        // Checkbox functions
        function setTrueFalseField(fieldName, ngModelFields) {
            ngModelFields[fieldName].value = ngModelFields[fieldName].value == 'true';
        }
        //END Checkbox functions

        // Checkbox group functions
        function setSelectMultipleField(fieldName, ngModelFields, fieldsType) {
            if (!ngModelFields[fieldName].value) {
                ngModelFields[fieldName].value = {
                    items: JSON.parse(ngModelFields[fieldName].listValue || "[]").map(function (value, index) {
                        return {
                            value: false,
                            name: value
                        }
                    }),
                    checked: false,
                    indeterminate: false
                }
            } else {
                ngModelFields[fieldName].value = JSON.parse(ngModelFields[fieldName].value);
            }
            var toggledItems = 0;

            var checkList = function () {
                ngModelFields[fieldName].value.indeterminate = (toggledItems > 0 && toggledItems < ngModelFields[fieldName].value.items.length) || ngModelFields[fieldName].value.items.length == 0;
                ngModelFields[fieldName].value.checked = toggledItems == ngModelFields[fieldName].value.items.length && ngModelFields[fieldName].value.items.length > 0;
            }
            var toggle = function (index) {
                ngModelFields[fieldName].value.items[index].value = !ngModelFields[fieldName].value.items[index].value;
                if (ngModelFields[fieldName].value.items[index].value) {
                    toggledItems++;
                } else {
                    toggledItems--;
                }
                checkList();
            }
            for (var item in ngModelFields[fieldName].value.items) {
                ngModelFields[fieldName].value.items[item].toggle = toggle;
                if (ngModelFields[fieldName].value.items[item].value) {
                    toggledItems++;
                }
            }
            if (ngModelFields[fieldName].value && ngModelFields[fieldName].value.items && ngModelFields[fieldName].value.items.length) {
                ngModelFields[fieldName].value.toggleAll = function () {
                    if (!ngModelFields[fieldName].value.checked) {
                        toggledItems = 0;
                        for (var item in ngModelFields[fieldName].value.items) {
                            ngModelFields[fieldName].value.items[item].value = true;
                            toggledItems++;
                        }
                        ngModelFields[fieldName].value.indeterminate = false;
                        ngModelFields[fieldName].value.checked = true;
                    } else {
                        for (var item in ngModelFields[fieldName].value.items) {
                            ngModelFields[fieldName].value.items[item].value = false;
                        }
                        ngModelFields[fieldName].value.indeterminate = false;
                        ngModelFields[fieldName].value.checked = false;
                        toggledItems = 0;
                    }
                }
            }
            checkList();
        }
        //END Checkbox group functions

        // Date field functions
        function setDateField(fieldName, ngModelFields) {
            ngModelFields[fieldName].value = new Date(ngModelFields[fieldName].value)
        }
        //END Date field functions

        function setSectionField() {
        }

        function setAppropriateFields(ngModelFields, fieldsType) {
            for (var property in ngModelFields) {
              if (ngModelFields.hasOwnProperty(property) && !mdBusinessLogic.helpers.checkType.isFunction(ngModelFields[property])) {
                    switch (ngModelFields[property].type) {
                        case 12:
                            mapFieldsArray.push(ngModelFields[property]);
                            break;
                        case 13:
                            setContentField(property, ngModelFields, fieldsType);
                            break;
                        case 8:
                            setTaxonomyField(property, ngModelFields, fieldsType);
                            break;
                        case 14:
                            setYoutubeField(property, ngModelFields, fieldsType);
                            break;
                        case 17:
                            setUserField(property, ngModelFields, fieldsType);
                            break;
                        case 5:
                            setTextareaField(property, ngModelFields, fieldsType);
                            break;
                        case 4:
                            setTrueFalseField(property, ngModelFields);
                            break;
                        case 7:
                            setSelectMultipleField(property, ngModelFields, fieldsType);
                            break;
                        case 11:
                            setDateField(property, ngModelFields);
                            break;
                        case 15:
                            setSectionField(property, ngModelFields);
                        default:
                            break;
                    }
                }
            }

            if (mapFieldsArray.length > 0) {
                setMaps(mapFieldsArray, fieldsType);
            }
        }
        function fillNgModelFields(ngModelFields, baseFields) {
            for (var i in baseFields) {
              if (!mdBusinessLogic.helpers.checkType.isFunction(baseFields[i])) {
                    ngModelFields[getUniqueName(baseFields[i], 2)] = {
                        value: baseFields[i].Value || null,
                        type: baseFields[i].AttributeTypeDefinitionId,
                        listValue: baseFields[i].ListValue,
                        delimiter: baseFields[i].Delimiter || ';',
                        id: baseFields[i].Id,
                        name: getUniqueName(baseFields[i], 2)
                    }
                }
            }
            return ngModelFields;
        }
        //function for add meta data for mediacontent
        function addMetaDataFieldsToMediaContent() {
            if (vm.metaDataFields) {
                for (var i in vm.metaDataFields) {
                    var name = getUniqueName(vm.metaDataFields[i], 2);
                    switch (vm.metaDataFields[i].AttributeTypeDefinitionId) {
                        case 5:
                            vm.metaDataFieldVals[name] = {
                                value: getContentFromTextArea(vm.metaDataFields[i].Id, 2)
                            }
                            break;
                        case 8:
                            vm.metaDataFieldVals[name] = {
                                value: (vm["taxonomyPickerFieldValueToSave" + vm.metaDataFields[i].Id + "" + 2] || { Id: null }).Id
                            }
                            break;
                        case 13:
                            vm.metaDataFieldVals[name] = {
                                value: (vm["contentPickerFieldValueToSave" + vm.metaDataFields[i].Id + "" + 2] || { Id: null }).Id
                            }
                            break;
                        case 17:
                            vm.metaDataFieldVals[name] = {
                                value: (vm["userPickerFieldValueToSave" + vm.metaDataFields[i].Id + "" + 2] || { Id: null }).Id
                            }
                            break;
                        case 7:
                            vm.metaDataFieldVals[name] = {
                                value: JSON.stringify(vm.metaDataFieldVals[name].value),
                                type: 7
                            }
                            break;
                    }
                }
            }

            vm.mediacontent.MediaContentMetaDataFieldValues = [];

            for (var key in vm.metaDataFieldVals) {
              if (vm.metaDataFieldVals.hasOwnProperty(key) && key != 'undefined' && !mdBusinessLogic.helpers.checkType.isFunction(vm.metaDataFieldVals[key])) {
                    var fieldValue = {
                        Value: vm.metaDataFieldVals[key].value,
                        Name: key,
                        MetaDataFieldId: vm.metaDataFieldVals[key].metaDataFieldId
                    }

                    //adding matching field id to field value
                    for (var j in vm.metaDataFields) {
                        if (getUniqueName(vm.metaDataFields[j], 2) == fieldValue.Name) {
                            fieldValue.MetaDataFieldId = vm.metaDataFields[j].Id;
                            vm.mediacontent.MediaContentMetaDataFieldValues.push(fieldValue);
                            break;
                        }
                    }
                }
            }
        }



        function getFileType(extension) {
            extension = extension.toLowerCase();
            switch (extension) {
                case 'jpg':
                case 'jpeg':
                case 'gif':
                case 'png':
                case 'ai':
                case 'bmp':
                case 'ico':
                case 'ps':
                case 'psd':
                case 'svg':
                case 'tif':
                case 'tiff':
                    return 1;
                case 'mp4':
                case '3g2':
                case '3gp':
                case 'avi':
                case 'flv':
                case 'h264':
                case 'm4v':
                case 'mkv':
                case 'mov':
                case 'mpg':
                case 'rm':
                case 'swf':
                case 'vob':
                case 'wmv':
                    return 2;
                case 'mp3':
                case 'aif':
                case 'mid':
                case 'midi':
                case 'mpa':
                case 'wav':
                case 'wma':
                case 'm4a':
                    return 3;
                case 'txt':
                case 'doc':
                case 'docx':
                case 'pdf':
                case 'odt':
                case 'rtf':
                case 'tex':
                case 'wpd':
                case 'wks':
                case 'csv':
                case 'xlsx':
                    return 4;
                case 'apk':
                case 'bat':
                case 'bin':
                case 'cgi':
                case 'pl':
                case 'com':
                case 'exe':
                case 'gadget':
                case 'jar':
                case 'py':
                case 'wsf':
                    return 5;
                    break;
                default:
                    return 0;
            }
        }
        function createMediaContent(uploadResponse) {
            var ext = $scope.file[0].lfFileName.split('.').pop();
            vm.mediacontent.FileType = getFileType(ext);
            vm.mediacontent.Path = $state.params.path;
            vm.mediacontent.PreviewUrl = uploadResponse.YoutubeVideoUrl;
            vm.mediacontent.FullNameFile = uploadResponse.PathToSaveToDatabase;
            vm.mediacontent.Size = $scope.file[0].lfFile.size;
        }

        function saveMediaContent() {
            mediaContentController.save(vm.mediacontent, function (data) {
                vm.mediacontent = data;
                var postSaveEvents = [];
                for (var i in vm.permissionSaveEvents) {
                    var event = vm.permissionSaveEvents[i];
                    postSaveEvents.push(event());
                }
                $q.all(postSaveEvents).then(function () {
                    vm.mediacontent.Id = data.Id;
                    vm.createMediaContentEnded = true;
                    $mdFeedbackService.reportInfo('save');
                    $state.go('app.mediacontent_list', { folderPath: $state.params.path }, { reload: false });
                });
            }, function (error) {
                $mdFeedbackService.reportError('save', error);
                $state.go('app.mediacontent_list', { folderPath: $state.params.path }, { reload: false })
                vm.createMediaContentEnded = true;             
            })
        }
        
        function validateType(type) {
            type = type.split('/')[1];
            for (var i in vm.forbiddenTypes) {
                if (type == vm.forbiddenTypes[i]) {
                    return false;
                }
            }
            return true;
        }

        
        function showValidationMessage(type) {
            $mdDialog.show(
                            $mdDialog.alert()
                               .parent(angular.element(document.querySelector('#popupContainer')))
                               .clickOutsideToClose(true)
                               .title($rootScope.globals.resources.Titles.Warning)
                               .textContent($rootScope.globals.resources.Labels.InvalidFormat)
                               .ok($rootScope.globals.resources.Labels.GotIt)
                             );
        }
        function ValidationCheck() {
            if ($scope.file){
                if ($scope.file.length > 0) {
                    var filetypewithextension = ($scope.file[0].lfFile || { type: vm.mimeType }).type;
                    var index = filetypewithextension.indexOf("/");
                    var type = filetypewithextension.substring(0, index);
                    switch ($state.params.fileType) {
                        case "1":
                            if (type !== "image") {
                                showValidationMessage("image")
                                $scope.file.splice(0, 1);
                                return false;
                            }
                            else {
                                return true;
                            }
                            break;
                        case "2":
                            if (type !== "video") {
                                showValidationMessage("video")
                                $scope.file.splice(0, 1);
                                return false;
                            }
                            else {
                                return true;
                            }
                            break;
                        case "3":
                            if (type !== "audio") {
                                showValidationMessage("audio")
                                $scope.file.splice(0, 1);
                                return false;
                            }
                            else {
                                return true;
                            }
                            break;
                        case "4":
                            if (type !== "application" && type !== "text") {
                                showValidationMessage("document")
                                $scope.file.splice(0, 1);
                                return false;
                            }
                            else {
                                return true;
                            }
                            break;

                    }
                }
        }
                
        }

        function save() {
            if (($scope.file.length == 0 && vm.isNew) || ($scope.file.length == 0 && !vm.isNew && vm.changeFile)) {
                $mdDialog.show(
                  $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title($rootScope.globals.resources.Titles.Warning)
                    .textContent($rootScope.globals.resources.Labels.MissingMediaContent)
                    .ok($rootScope.globals.resources.Labels.GotIt)
                );
                return;
            }
            if($scope.file.length > 0){
                isValid = ValidationCheck();
               
            }
            if(isValid){

            
            vm.createMediaContentEnded = false;
            vm.disableSubmitBtn = true;
            addMetaDataFieldsToMediaContent();


            var formData = new FormData();
            if (vm.changeFile || vm.isNew) {
                angular.forEach($scope.file, function (obj) {
                    if (validateType(obj.lfFile.type)) {
                        formData.append('file', obj.lfFile);
                    }
                });
                formData.append('path', $state.params.path);
                formData.append('fileType', vm.fileType);
                formData.append('mediaContentName', vm.mediacontent.Name)
                formData.append('mediaContentDescription', vm.mediacontent.Description)

                $http.post('/ws/Upload/PostFormData/', formData, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function onSuccess(result) {
                    if (result.data) {
                        createMediaContent(result.data);
                        vm.YoutubeVideoUrl = result.data.YoutubeVideoUrl;                        
                        if (vm.mediacontent.FileType == 0 || vm.mediacontent.FileType == 5) {
                            showDialog(
                                $rootScope.globals.resources.Titles.ActionNotCompleted,
                                $rootScope.globals.resources.Labels.InvalidFormat,
                                false);
                        } else {
                            saveMediaContent();
                        }
                    } else {
                        showDialog(
                            $rootScope.globals.resources.Titles.ActionNotCompleted,
                                $rootScope.globals.resources.Labels.InvalidFormat,
                            false);
                    }

                }, function onError(error) {
                    $mdFeedbackService.reportError('save', error);
                })
            } else {
                saveMediaContent();
            }
            }
        }

        function getFolderByPath() {
            folderController.getByFolderPath(vm.currentFolderPath,
                function (data) {
                    $scope.$apply(function () {
                        vm.folder = data;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function showDialog(title, text, redirect) {
            var alertInfo = {
                title: title || 'successfull',
                redirect: redirect
            }
            $mdDialog.show({
                controller: function ($scope, $mdDialog, formWizardData) {
                    $scope.formWizardData = formWizardData;
                    $scope.closeDialog = function () {
                        $mdDialog.hide();
                    }
                },
                template: '<md-dialog>' +
                '  <md-dialog-content><h1>' + text + '</h1></md-dialog-content>' +
                '  <md-dialog-actions>' +
                '    <md-button ng-click="closeDialog()" class="md-primary">' +
                $rootScope.globals.resources.Labels.Close +
                '    </md-button>' +
                '  </md-dialog-actions>' +
                '</md-dialog>',
                parent: angular.element('body'),
                locals: {
                    formWizardData: vm.mediacontent
                },
                clickOutsideToClose: true,
                title: alertInfo.title,
                ariaLabel: alertInfo.title,
                ok: $state.go('app.mediacontent_list', { folderPath: $state.params.path, currentView: vm.currentView }, { reload: true })
            })
        };
        function goBack() {
            $state.go('app.mediacontent_list', { folderPath: $state.params.path, currentView: vm.currentView }, { reload: true });
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
        function findUserId(selectedUser) {
            for (var i in vm.allUsers) {
                if (vm.allUsers[i].Username == selectedUser.Username) {
                    return vm.allUsers[i].Id;
                }
            }
        }
        function toggleChangeFile() {
            vm.changeFile = !vm.changeFile;
        }

        function setFileTypeParams(fileType) {
            fileType = parseInt(fileType);
            switch (fileType) {
                case 1:
                    vm.mimeType = 'image/*';
                    vm.fileTypeString = 'image';
                    break;
                case 2:
                    vm.mimeType = 'video/*';
                    vm.fileTypeString = 'video';
                    break;
                case 3:
                    vm.mimeType = 'audio/*';
                    vm.fileTypeString = 'audio';
                    break;
                case 4:
                    vm.mimeType = 'application/*|text/*';
                    vm.fileTypeString = 'document'
                    break;
                default:
                    break;
            }

            vm.validationMessage = 'Only ' + vm.fileTypeString + 's allowed';
            vm.formTitle = addOrEdit + " " + vm.fileTypeString;
           


        }*/
    }
})();
