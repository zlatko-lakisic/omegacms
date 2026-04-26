(function () {
    'use strict';

    angular
        .module('app.settings.configuration.user.form')
        .controller('UserFormController', ['$state', '$rootScope', 'mdAuthenticationRegistryService', '$mdDialog', '$scope', '$q', 'mdFeedbackService', 'user', 'allProfileTypes', 'selectedProfileType', UserFormController]);

    /** @ngInject */
    function UserFormController($state, $rootScope, mdAuthenticationRegistryService, $mdDialog, $scope, $q, $mdFeedbackService, user, allProfileTypes, selectedProfileType) {
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

        //Page Variables
        var postSavePromiseArray = [];
        var authenticationPoviderPromise = null;

        vm.user = user;
        vm.isNew = $state.params.action != 'edit';
        vm.formTitle = vm.isNew ? $rootScope.globals.resources.Titles.AddUser : $rootScope.globals.resources.Titles.EditUser;
        vm.tab = 0;
        vm.profileTypeUploadEvents = [];
        vm.saveEvents = [];
        vm.allProfileTypes = allProfileTypes.map(function (profileType) {
            return {
                name: profileType.Name,
                id: profileType.Id,
                selected: user.ProfileTypes.filter(function (userProfileType) { return userProfileType.Id == profileType.Id; }).length == 1
            };
        });
        vm.changePass = vm.isNew
        vm.authenticationProvider = mdAuthenticationRegistryService.get(vm.user.AuthenticationProvider);
        if (vm.isNew) {
            vm.user.ProfileTypes.push(selectedProfileType);
            vm.authenticationProvider = mdAuthenticationRegistryService.get(mdBusinessLogic.dataAccess.providers.authentication.builtIn.getAuthenticationProviderId());
        }
        vm.authMode = mdBusinessLogic.dataAccess.providers.authentication.authMode;




        //Public Methods
        vm.processSave = processSave;
        vm.changeTab = changeTab;
        vm.registerProfileTypeUploadEvents = registerProfileTypeUploadEvents;
        vm.onSave = onSave;
        vm.save = save;
        vm.goBack = goBack;
        vm.toggleProfileTypes = toggleProfileTypes;
        vm.registerChangePassword = registerChangePassword;


        //Private Methods
        function processSave(_promise) {
            authenticationPoviderPromise = _promise;
        }

        function registerProfileTypeUploadEvents(event) {
            vm.profileTypeUploadEvents.push(event);
        }

        function registerChangePassword() {
            if (!vm.isNew) {
                vm.changePass = vm.user.Password !== undefined && vm.user.Password != '';
            }
        }

        function onSave(event) {
            vm.saveEvents.push(event);
        }

        function toggleProfileTypes() {
            for (var p = 0; p < vm.allProfileTypes.length; p++) {
                var profileType = vm.allProfileTypes[p];
                var exists = vm.user.ProfileTypes.filter(function (pt) {
                    return pt.Id == profileType.id;
                }).length == 1;
                if (exists) {
                    if (!profileType.selected) {
                        for (var i = vm.user.ProfileTypes.length - 1; i >= 0; i--) {
                            if (vm.user.ProfileTypes[i].Id == profileType.id) {
                                $q.all(assignProfileToUser(vm.user.ProfileTypes[i], false)).then(function () {
                                    vm.user.ProfileTypes.splice(i, 1);
                                    changeTab(1 + vm.user.ProfileTypes.length);
                                });
                                break;
                            }
                        }
                    }
                } else {
                    if (profileType.selected) {
                        var profileTypeToAssign = allProfileTypes.filter(function (pt) {
                            return pt.Id == profileType.id;
                        })[0];
                        $q.all(assignProfileToUser(profileTypeToAssign, true)).then(function () {
                            vm.user.ProfileTypes.push(profileTypeToAssign);
                            changeTab(1 + vm.user.ProfileTypes.length);
                        });
                    }
                }
            }
        }

        function goBack() {
            var backtoFolder = $state.params.path;
            $state.go('app.user_list', { folderPath: backtoFolder, currentView: $state.params.currentView }, { reload: false });
        }

        function changeTab(tab) {
            vm.tab = tab;
        }

        function save($event) {
            var promiseArray = [];
            for (var i = 0; i < vm.profileTypeUploadEvents.length; i++) {
                var event = vm.profileTypeUploadEvents[i];
                promiseArray.push(event());
            }

            $q.all(promiseArray).then(function () {
                for (var i = 0; i < vm.saveEvents.length; i++) {
                    var event = vm.saveEvents[i];
                    event();
                }

                function next() {
                    controllers.userController.save(vm.user, function (data) {
                        vm.user.Id = data.Id;
                        if (!vm.isNew) {
                            postSavePromiseArray.push(authenticationPoviderPromise());
                        }
                        for (var i = 0; i < vm.user.ProfileTypes.length; i++) {
                            var profileType = vm.user.ProfileTypes[i];
                            for (var p = 0; p < profileType.Fields.length; p++) {
                                profileType.Fields[p].UserId = vm.user.Id;
                            }
                            if (vm.isNew) {
                                postSavePromiseArray.push(assignProfileToUser(profileType, true));
                            }
                            postSavePromiseArray.push(createProfileTypeSave(profileType));
                        }
                        $q.all(postSavePromiseArray).then(function () {
                            $mdFeedbackService.reportInfo("save");
                            goBack();
                        });
                    }, function (error) {
                        $mdFeedbackService.reportError('save', error);
                    });
                }

                if (vm.isNew) {
                    $q.when(authenticationPoviderPromise(), function (user) {
                        vm.user.Username = user.Username;
                        vm.user.Password = user.Password;
                        next();
                    }, function (error) {
                        $mdFeedbackService.reportError('save');
                    });
                } else {
                    next();
                }
            });
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

        function assignProfileToUser(profileType, isAssigned) {
            return $q(function (resolve, reject) {
                controllers.profileController.assignProfileTypeToUser({
                    userId: vm.user.Id,
                    profileTypeId: profileType.Id,
                    assigned: isAssigned
                }, function (data) {
                    resolve(true);
                }, function (error) {
                    resolve(true);
                });
            });
        }

        function init() {
        }

        init();


        /*
        //helpers
        var postfixService = PostfixEvaluatorService;
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var profileController = new mdBusinessLogic.dataAccess.controllers.profileController();
        var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
        var profileTypeFieldController = new mdBusinessLogic.dataAccess.controllers.profileTypeFieldController();
        var profileTypeFieldValueController = new mdBusinessLogic.dataAccess.controllers.profileTypeFieldValueController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
        var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
        var permissionControllerUser = new mdBusinessLogic.dataAccess.controllers.permissionControllerUser();

        //variables
        vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;
        vm.currentView = $state.params.currentView;
        vm.isNew = $state.params.action == 'add';
        vm.id = $state.params.id;
        vm.userId;
        vm.mainProfileTypeId;
        vm.user = user;
        vm.mainProfileType = {};
        vm.markerPosition;
        var mapFieldsArray = [];
        vm.profileTypes = [];
        vm.selectedType = {};
        vm.passwordConfirm;
        vm.permissions = [];
        vm.selectedPermission = {};
        vm.index;
        vm.selected = [];
        vm.notBelongingProfileTypes = [];
        vm.oldPassword;
        vm.passwordConfirmm;
        var isOk = false;
        vm.contentPickerFieldValueToSave;
        vm.taxonomyPickerFieldValueToSave;
        vm.basicForm = {};
        vm.formWizard = {};
        vm.lcid = mdBusinessLogic.settings.lcid || 2057;
        //tinymce
        vm.tinymceModel = '';
        vm.getContent = getContentFromTextArea;
        vm.setContent = setContentToTextArea;
        var getAllMediaContentsCalled = false; //to call get all media contents only once
        var getAllContentsCalled = false; //to call get all media contents only once
        var getAllUsersCalled = false; //to call get all media contents only once
        var getAllTaxonomyCalled = false; //to call get all media contents only once
        //DISABLE PICKER UNTIL DATA IS FETCHED
        vm.disableContentPicker = true;
        vm.disableMediaContentPicker = true;
        vm.disableTaxonomyPicker   = true;
        vm.disableUserPicker = true;
        //Disable picker END

        //methods
        vm.assignProfileTypeToUser = assignProfileTypeToUser;
        

        //custom fields content, taxonomy, media content, user methods
        vm.chooseContent = chooseContent;
        vm.queryContents = queryContents;
        vm.chooseTaxonomy = chooseTaxonomy;
        vm.queryTaxonomies = queryTaxonomies;
        vm.chooseMediaContent = chooseMediaContent;
        vm.queryMediaContents = queryMediaContents;
        vm.chooseUser = chooseUser;
        vm.queryUsers = queryUsers;
        vm.sendForm = sendForm;

        vm.evaluatePostfixExpression = evaluatePostfixExpression;

        function evaluatePostfixExpression(ngProfileType) {
            //if new we don't use Object which we accept in function,we use object which we set in setCalculatedField function
            if (vm.isNew) {
                for (var i in vm.ngModelFields) {
                    if (vm.ngModelFields[i].AttributeTypeDefinitionId == 18) {
                        vm.formulaWithValues = getFormulaWithValues(vm.ngModelFields[i].DefaultValue, vm.ngModelFields);
                        vm.profileTypes[0].Fields[i].Value = postfixService.evaluatePostfixExpression(vm.formulaWithValues);
                    }
                }
            }
            //if edit we use object which we send from form on ng-change, because we have to know which profile type is curently edited
            else {
                for (var i in ngProfileType.Fields) {
                    if (ngProfileType.Fields[i].AttributeTypeDefinitionId == 18) {
                        vm.formulaWithValues = getFormulaWithValues(ngProfileType.Fields[i].DefaultValue, ngProfileType.Fields);
                        ngProfileType.Fields[i].Value = postfixService.evaluatePostfixExpression(vm.formulaWithValues);
                    }
                }
            }
        }

        function getFormulaWithValues(formula, ngProfileType) {
            var expressionParts = formula.split(',');
            for (var i = 0, length = expressionParts.length; i < length; i++) {
                if (expressionParts[i].substring(0, 6) == "field.") {
                    for (var key = 0; key < ngProfileType.length; key++) {
                        if (ngProfileType[key].Name == expressionParts[i].substring(6)) {
                            expressionParts[i] = ngProfileType[key].Value;
                            break;
                        }
                    }
                }
            }
            return expressionParts;
        }


        function showDialog(title, text, redirect) {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));
            $mdDialog.show(
                            $mdDialog.alert()
                              .parent(parentElement)
                              .clickOutsideToClose(true)
                              .parent(parentElement)
                              .title(title)
                              .textContent(text)
                              .ariaLabel(title)
                              .ok($rootScope.globals.resources.Labels.GotIt)
                          );
            if (redirect) {
                $state.go('app.user_list', { currentView: vm.currentView }, { reload: true });
            }
        }


        function assignProfileTypeToUser(profileType, $index) {
            profileController.assignProfileTypeToUser(
                {
                    userId: vm.user.Id,
                    profileTypeId: profileType.Id,
                    assigned: profileType.IsAssigned
                },
            function (data) {
                if (profileType.IsAssigned) {
                    $mdFeedbackService.reportInfo('update');
                } else {
                    $mdFeedbackService.reportInfo('update');
                }
            }, function (error) {
                $mdFeedbackService.reportError('update', error);
                profileType.IsAssigned = true;
            });
        }


        //tinymce
        function getContentFromTextArea(id, type) {
            // return vm.tinymceModel;
            if (id && type) {
                return vm["tinymceModel" + id + '' + 1];
            } else {
                return vm.tinymceModel;
            }
        }

        function setContentToTextArea(sadrzajForme, id, type) {
            if (id && type) {
                vm["tinymceModel" + id + '' + 1] = sadrzajForme;
            } else {
                vm.tinymceModel = sadrzajForme;
            }

        }

        vm.tinymceOptions = {
            plugins: 'link image code',
            toolbar: 'undo redo | bold italic | alignleft align center alignright | code'
        };
        //tinymce end

        //map
        function changeMapFieldValue(lat, lng, unicateValue) {
            vm["markerPosition" + unicateValue] = lat + ';' + lng;
        }
        function cloneMap(mapToClone, unicateValue) {
            return vm["map" + unicateValue] = JSON.parse(JSON.stringify(mapToClone));
        }
        function giveMapProperties(map, unicateValue, fieldName, fields, fieldType) {
            map.center = {
                latitude: 43.336995,
                longitude: 17.814417
            };

            map.zoom = 6;
            map.events = {
                click: function (map, eventName, originalEventArgs) {
                    var e = originalEventArgs[0];
                    var lat = e.latLng.lat(), lon = e.latLng.lng();
                    $scope.$apply(function () {
                        vm["map" + unicateValue].marker = new google.maps.Marker({
                            coords: {
                                latitude: lat,
                                longitude: lon
                            }
                        });
                        vm["map" + unicateValue].marker;
                        vm["map" + unicateValue].marker.coords;
                        changeMapFieldValue(lat, lon, unicateValue);
                    })
                }
            }
        }


        function setMap(mapFields, type) {
            uiGmapGoogleMapApi.then(function (map) {
                vm.map = map;
                for (var i in mapFields) {
                  if (!mdBusinessLogic.helpers.checkType.isFunction(mapFields[i])) {
                        var unicateValue = mapFields[i].Id + "" + 1;
                        var clonedMap = cloneMap(vm.map, unicateValue);
                        giveMapProperties(clonedMap, unicateValue, mapFields[i].Name, mapFields, 1);
                        
                        if (mapFields[i].Value) {
                            var latlong = mapFields[i].Value.split(';');
                            vm["map" + unicateValue].marker = new google.maps.Marker({
                                coords: {
                                    latitude: latlong[0],
                                    longitude: latlong[1]
                                }
                            })

                            vm["map" + unicateValue].center = {
                                latitude: vm["map" + unicateValue].marker.coords.latitude,
                                longitude: vm["map" + unicateValue].marker.coords.longitude
                            }
                        }
                    }
                }
              
            });
           
        }
        //map end

        //custom fields content and taxonomy//

        //content
        function setContentField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldContent();
            var assignedContentId = ngModelFields[fieldName].Value || 0;
            if (assignedContentId != " " && assignedContentId != null) {
                contentController.getById(assignedContentId, false, vm.lcid, false, false, 0,
                    function (contentData) {
                        vm["selectedContent" + ngModelFields[fieldName].Id + "" + 1] = contentData;
                    }, function () { });
            }
        }
        function setCustomFieldContent() {
            vm.chooseContent = chooseContent;
            vm.queryContents = queryContents;
            if (!getAllContentsCalled) {
                getAllContentsForAutocomplete();
            }
        }
        function chooseContent(selected, unicateValue) {
            if (selected) {
                vm["contentPickerFieldValueToSave" + unicateValue] = selected.Id;
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
        
        function getAllContentsForAutocomplete() {
            getAllContentsCalled = true;
                contentController.getAll(
            function (data) {
                vm.contentsByFolder = data.map(function (content) {
                    content._lowertitle = content.Title.toLowerCase();
                    return content;
                })
                $scope.$apply(function () {
                    vm.disableContentPicker = false;
                });
               
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            });
          
        }
        //end content
        //taxonomy
        function setTaxonomyField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldTaxonomy();
            var assignedTaxonomyId = ngModelFields[fieldName].Value || 0;
            if (assignedTaxonomyId != " " && assignedTaxonomyId != null) {
                taxonomyController.getById(assignedTaxonomyId, function (taxonomyData) {
                    vm["selectedTaxonomy" + ngModelFields[fieldName].Id + "" + 1] = taxonomyData;
                }, function (error) { })
            }
        }

        function setCustomFieldTaxonomy() {
            vm.chooseTaxonomy = chooseTaxonomy;
            vm.queryTaxonomies = queryTaxonomies;
            if (!getAllTaxonomyCalled) {
                getAllTaxonomiesForAutocomplete();
            }
        }
        function chooseTaxonomy(selected, unicateValue) {
            if (selected) {
                vm["taxonomyPickerFieldValueToSave" + unicateValue] = selected.Id;
            } else {
                vm["taxonomyPickerFieldValueToSave" + unicateValue] = null;
            }
        }

        function queryTaxonomies(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.allTaxonomies.filter(function (query) {
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

        function getAllTaxonomiesForAutocomplete() {
            getAllTaxonomyCalled = true;
            taxonomyController.getAll(vm.lcid,
           function (data) {
           vm.allTaxonomies = data.map(function (taxonomy) {
           taxonomy._lowertitle = taxonomy.Name.toLowerCase();
           return taxonomy;
           })
           $scope.$apply(function () {
               vm.disableTaxonomyPicker = false;
           });
          
           }, function (error) {
               $mdFeedbackService.reportError('load', error);
           });
        }
        //end taxonomy
        //media content
        function chooseMediaContent(selected, unicateValue) {
            if (selected) {
                vm["mediaContentPickerFieldValueToSave" + unicateValue] = selected.Id;
            } else {
                vm["mediaContentPickerFieldValueToSave" + unicateValue] = null;
            }
        }

        function queryMediaContents(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.allMediaContents.filter(function (query) {
                return function filterFn(mediaContent) {
                    return (mediaContent._lowertitle.indexOf(lowercaseQuery) === 0);
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
        function getAllMediaContentsForAutocomplete() {
            getAllMediaContentsCalled = true;
            mediaContentController.getAll(
           function (data) {
               vm.allMediaContents = data.map(function (mediaContent) {
                   mediaContent._lowertitle = mediaContent.Name.toLowerCase();
                   return mediaContent;
               })
               $scope.$apply(function () {
                   vm.disableMediaContentPicker = false;
               });
             
           }, function (error) {
               $mdFeedbackService.reportError('load', error);
           });
        }
        function setCustomFieldMediaContent() {
            vm.chooseMediaContent = chooseMediaContent;
            vm.queryMediaContents = queryMediaContents;
            if (!getAllMediaContentsCalled) {
            getAllMediaContentsForAutocomplete();
             }
        }

        function setMediaContentField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldMediaContent();
            var assignedMediaContentId = ngModelFields[fieldName].Value;
            if (assignedMediaContentId != " " && assignedMediaContentId != null) {
                assignedMediaContentId = assignedMediaContentId.split(';')[0];
                mediaContentController.getById(assignedMediaContentId, vm.lcid, function (mediaContentData) {
                    vm["selectedMediaContent" + ngModelFields[fieldName].Id + "" + 1] = mediaContentData;
                }, function (error) {
                });
            }
        }
        //end mediacontent
        //user
        function chooseUser(selected, unicateValue) {
           
            if (selected) {
                vm["userPickerFieldValueToSave" + unicateValue] = selected.Id;
            } else {
                vm["userPickerFieldValueToSave" + ""] = null;
            }
        }
        function setCustomFieldUser() {
            vm.chooseUser = chooseUser;
            vm.queryUsers = queryUsers;
            if (!getAllUsersCalled) {
                getAllUsersForAutocomplete();
            }
        }
        function setUserField(fieldName, ngModelFields, fieldsType) {
            setCustomFieldUser();
            var assignedUserId = ngModelFields[fieldName].Value || 0;
            if (assignedUserId != " " && assignedUserId != null) {
                userController.getById(assignedUserId, function (userData) {
                    vm["selectedUser" + ngModelFields[fieldName].Id + "" + 1] = userData;
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
            }
        }

        function queryUsers(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.allUsers.filter(function (query) {
                return function filterFn(user) {
                    return (user._lowerusername.indexOf(lowercaseQuery) === 0);
                };
            }) : [];

            var i = results.length;
            while (i--) {
                if (
                    results[i]._lowerusername.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }

        function getAllUsersForAutocomplete() {
            getAllUsersCalled = true;
           userController.getAll(
           function (data) {
               vm.allUsers = data.map(function (user) {
                   user._lowerusername = user.Username.toLowerCase();
                   return user;
               })
               $scope.$apply(function () {
                   vm.disableUserPicker = false;
               });
              
           }, function (error) {
               $mdFeedbackService.reportError('load', error);
           });
        }
        //end user
        //custom fields content and taxonomy and media content and user END

        function setTextareaField(fieldName, ngModelFields, fieldsType) {
            setContentToTextArea(ngModelFields[fieldName].Value, ngModelFields[fieldName].Id, fieldsType)
        }
        function setSelectMultipleField(fieldName, ngModelFields, fieldsType) {
            var unicateValue = ngModelFields[fieldName].Id + "" + 1;
            vm["selected" + unicateValue] = [];
            if (ngModelFields[fieldName].ListValue != "") {
                var listValue = ngModelFields[fieldName].ListValue;
                var delimiter = ';';

                var value = ngModelFields[fieldName].Value;
                vm["items" + unicateValue] = listValue.split(delimiter);
                if (value) {
                    vm["selected" + unicateValue] = value.split(delimiter);
                }
                vm["toggle" + unicateValue] = function (item, list) {
                    var idx = list.indexOf(item);
                    if (idx > -1) list.splice(idx, 1);
                    else list.push(item);
                };
                vm["exists" + unicateValue] = function (item, list) {
                    return list.indexOf(item) > -1;
                };
            }
        }
       

        function setAppropriateFields() {
            for (var profileType in vm.profileTypes) {
                for (var field in vm.profileTypes[profileType].Fields) {
                    switch (vm.profileTypes[profileType].Fields[field].AttributeTypeDefinitionId) {
                        case 3:
                            vm.profileTypes[profileType].Fields[field].Value = parseInt(vm.profileTypes[profileType].Fields[field].Value);
                            break;
                        case 4:
                            if (vm.profileTypes[profileType].Fields[field].Value == 'true') {
                                vm.profileTypes[profileType].Fields[field].Value = true;
                            }
                            break;
                        case 5:
                           //textarea
                            setTextareaField(field, vm.profileTypes[profileType].Fields, profileType);
                             
                            break;
                        case 12:
                            //map
                            mapFieldsArray.push(vm.profileTypes[profileType].Fields[field]);
                            vm["markerPosition" + vm.profileTypes[profileType].Fields[field].Id + '' + 1] = vm.profileTypes[profileType].Fields[field].Value;
                            break;
                        case 13:
                            //content
                            setContentField(field, vm.profileTypes[profileType].Fields, profileType);
                            break;
                        case 8:
                            //taxonomy
                            setTaxonomyField(field, vm.profileTypes[profileType].Fields, profileType);
                          
                            break;
                        case 16:
                            //mediacontent
                            setMediaContentField(field, vm.profileTypes[profileType].Fields, profileType);
                            break;
                        case 17:
                            //user
                            setUserField(field, vm.profileTypes[profileType].Fields, profileType);
                            break;
                        case 7:
                            setSelectMultipleField(field, vm.profileTypes[profileType].Fields, profileType);
                           
                            break;
                        case 11:
                            if (vm.profileTypes[profileType].Fields[field].Value == null){
                                vm.profileTypes[profileType].Fields[field].Value = new Date();
                            }
                               
                            else {
                                vm.profileTypes[profileType].Fields[field].Value = new Date(vm.profileTypes[profileType].Fields[field].Value);
                            }
                            break;
                            //calculated
                        case 18:
                            vm.profileTypes[profileType].Fields[field].Value = parseInt(vm.profileTypes[profileType].Fields[field].Value);
                            setCalculatedField(field, vm.profileTypes[profileType].Fields);
                            break;
                        default:
                            break;

                    }
                }
            }
            if (mapFieldsArray.length > 0) {
                setMap(mapFieldsArray, 1);
            }
        }

        function fillFields(ngModelFields) {
            var ngModelFieldss = {};
            for (var i in ngModelFields) {
              if (!mdBusinessLogic.helpers.checkType.isFunction(ngModelFields[i])) {
                    if (ngModelFields[i].AttributeTypeDefinitionId == 1 && ngModelFields[i].JsonField && ngModelFields[i].JsonField.validation && ngModelFields[i].JsonField.validation.repeatable) {
                        var values = [];
                        if (ngModelFields[i].Value != undefined) {
                            for (var f = 0; f < ngModelFields[i].Value.length; f++) {
                                values[f] = ngModelFields[i].Value[f] || null;
                            }
                        }
                        ngModelFieldss[ngModelFields[i].FriendlyName] = {
                            value: values || null,
                            type: ngModelFields[i].AttributeTypeDefinitionId,
                            listValue: ngModelFields[i].ListValue,
                            delimiter: ngModelFields[i].Delimiter || ';',
                            id: ngModelFields[i].Id,
                            order: ngModelFields[i].Order,
                            name: ngModelFields[i].FriendlyName,
                            repeatable: ngModelFields[i].JsonField && ngModelFields[i].JsonField.validation && ngModelFields[i].JsonField.validation.repeatable,
                            options: ngModelFields[i].JsonField,
                            defaultValue: ngModelFields[i].DefaultValue
                        }
                    } else {
                        ngModelFieldss[ngModelFields[i].FriendlyName] = {
                            value: ngModelFields[i].Value || null,
                            type: ngModelFields[i].AttributeTypeDefinitionId,
                            listValue: ngModelFields[i].ListValue,
                            delimiter: ngModelFields[i].Delimiter || ';',
                            id: ngModelFields[i].Id,
                            order: ngModelFields[i].Order,
                            name: ngModelFields[i].FriendlyName,
                            defaultValue: ngModelFields[i].DefaultValue
                        }
                    }
                }
            }
            return ngModelFieldss;
        }
        function setCalculatedField(fieldName, ngModelFields) {
            vm.ngModelFields = ngModelFields;
            //vm.ngModelFields = fillFields(ngModelFields);
           
        }

        function getUser() {
            userController.getById(vm.id,
               function (data) {
                   vm.user = data;
                   vm.profileTypes = vm.user.ProfileTypes;
                   for (var i in vm.profileTypes) {
                       vm.profileTypes[i].IsAssigned = true;
                   }
                   profileTypeController.getNotBelonging(vm.user.Id,
                  function (data) {
                      vm.notBelongingProfileTypes = data;
                      for (var i in vm.notBelongingProfileTypes) {
                          vm.notBelongingProfileTypes[i].IsAssigned = false;
                      }
                  },
                 function (error) {
                     $mdFeedbackService.reportError('load', error);
                 });


                   setAppropriateFields();
               }, function (error) {
                   $mdFeedbackService.reportError('load', error);
               });
        }

        function saveFields() {
            vm.mainProfileType.Fields = [];
            for (var i in vm.fields) {
                var value = "";
                var fieldName = vm.fields[i].FriendlyName;
                if (fieldName !== undefined) {
                    value = vm.fields[i].Value;
                }
                if (vm.isNew) {
                    var fieldValue = {
                        ValueProfileTypeFieldId: vm.fields[i].Id,
                        UserId: vm.userId,
                        Value: value,
                        ValueProfileTypeId: vm.mainProfileTypeId
                    }
                }
                switch (vm.fields[i].AttributeTypeDefinitionId) {

                    case 5:
                        fieldValue.Value = getContentFromTextArea(vm.fields[i].Id,1);
                        break;
                    case 12:
                        //map
                        fieldValue.Value = vm["markerPosition" + vm.fields[i].Id + '' + 1];
                        break;
                    case 13:
                        //content
                        fieldValue.Value = vm["contentPickerFieldValueToSave" + vm.fields[i].Id + "" + 1];
                        break;
                    case 8:
                        //taxonomy
                        fieldValue.Value = vm["taxonomyPickerFieldValueToSave" + vm.fields[i].Id + "" + 1];
                        break;
                    case 16:
                        //media content
                        fieldValue.Value = vm["mediaContentPickerFieldValueToSave" + vm.fields[i].Id + "" + 1];
                        break;
                    case 17:
                        //user
                        fieldValue.Value = vm["userPickerFieldValueToSave" + vm.fields[i].Id + "" + 1];
                        break;
                    case 7:
                       
                        var value = ""
                        var unicateValue = vm.fields[i].Id + "" + 1;
                        var friendlyName = vm.fields[i].FriendlyName;
                        for (var i in vm["selected" + unicateValue]) {
                          if (!mdBusinessLogic.helpers.checkType.isFunction(vm["selected" + unicateValue][i])) {
                                value += vm["selected" + unicateValue][i] + ';'
                            }
                        }
                        fieldValue.Value = value;
                        break;
                    case 11:
                        fieldValue.Value = new Date(value);
                        break;
                    default:
                        break;
                }
                vm.fields[i].value = fieldValue.Value;
                if (value !== "") {
                    vm.mainProfileType.Fields.push(fieldValue);
                }
            }
            profileTypeController.saveProfileTypeWithProfileTypeFieldValues(vm.mainProfileType, function (data) {
              vm.mainProfileType = data;
              $mdFeedbackService.reportInfo('save');
            }, function (error) {
              $mdFeedbackService.reportError('save', error);
            });

        }

        function updateFields(profileType) {
            for (var field in profileType.Fields) {
              if (!mdBusinessLogic.helpers.checkType.isFunction(profileType.Fields[field])) {
                    var myField = new mdBusinessLogic.dataAccess.entities.profileTypeFieldValue();
                    myField.Id = profileType.Fields[field].Id;
                    myField.UserId = vm.user.Id;
                    myField.ValueProfileTypeId = profileType.Id;
                    myField.ValueProfileTypeFieldId = profileType.Fields[field].Id;

                    switch (profileType.Fields[field].AttributeTypeDefinitionId) {
                        case 5:
                            profileType.Fields[field].Value = getContentFromTextArea(profileType.Fields[field].Id,1);
                            break;
                        case 12:
                            //map
                            profileType.Fields[field].Value = vm["markerPosition" + profileType.Fields[field].Id + '' + 1];
                            break;
                        case 13:
                            //content
                            profileType.Fields[field].Value = vm["contentPickerFieldValueToSave" + profileType.Fields[field].Id + "" + 1];
                            break;
                        case 8:
                            //taxonomy
                            profileType.Fields[field].Value = vm["taxonomyPickerFieldValueToSave" + profileType.Fields[field].Id + "" + 1];
                            break;
                        case 16:
                            //media content
                            profileType.Fields[field].Value = vm["mediaContentPickerFieldValueToSave" + profileType.Fields[field].Id + "" + 1];
                            break;
                        case 17:
                            //user
                            profileType.Fields[field].Value = vm["userPickerFieldValueToSave" + profileType.Fields[field].Id + "" + 1]
                            
                            break;
                        case 7:
                           
                            var value = ""
                            var friendlyName = profileType.Fields[field].FriendlyName;
                            var unicateValue = profileType.Fields[field].Id + "" + 1;
                            for (var i in vm["selected" + unicateValue]) {
                              if (!mdBusinessLogic.helpers.checkType.isFunction(vm["selected" + unicateValue][i])) {
                                    value += vm["selected" + unicateValue][i] + ';'
                                }
                            }
                            profileType.Fields[field].Value = value;
                            break;
                        case 11:
                            profileType.Fields[field].Value = new Date(profileType.Fields[field].Value);
                            break;
                    }
                    myField.Value = profileType.Fields[field].Value;
                    profileType.Fields[field] = myField;
                }
            }
           
        }

        function saveNewUser() {
            if (vm.isNew && vm.passwordConfirm != vm.user.Password) {
                showDialog($rootScope.globals.resources.Titles.PasswordMismatch,
                    $rootScope.globals.resources.Labels.ErrorTryAgain,
                    false);
            } else {
                userController.save(vm.user,
                       function (data) {
                           vm.changePass = false;
                           vm.userId = data.Id;
                           vm.user = data;
                           vm.user.ProfileType = vm.mainProfileType;
                           saveFields();
                           $state.go("app.user_list", { currentView: 'list' });
                       },
                       function (error) {
                           $mdFeedbackService.reportError('load', error);
                       });
            }
        }

        function updateUser() {
            if (!vm.isNew && vm.passwordConfirmm != vm.user.Password && vm.changePass == true) {
                showDialog(
                    $rootScope.globals.resources.Titles.PasswordMismatch,
                    $rootScope.globals.resources.Labels.ErrorTryAgain,
                    false);
            }
            else {
                userController.save(vm.user,
                    function (data) {
                      $mdFeedbackService.reportInfo('save');
                      $state.go("app.user_list", { currentView: 'list' });
                    }, function (error) {
                        $mdFeedbackService.reportError('save', error);
                    });
            }
        }

        function sendForm() {
            if (vm.isNew) {
                saveNewUser();
            }
            else {
                for (var profileType in vm.user.ProfileTypes) {
                    updateFields(vm.user.ProfileTypes[profileType]);
                }
                updateUser();
            }
        }

        if (!vm.isNew) {
            vm.userId = vm.id;
            getUser();
        }

        if (vm.isNew) {
            vm.mainProfileTypeId = vm.id;
            vm.user.ProfileTypeId = vm.mainProfileTypeId;
            vm.changePass = true;
            profileTypeController.getByIdAndTransformExpression(vm.mainProfileTypeId,false,
                function (data) {
                    vm.mainProfileType = data;
                    vm.profileTypes.push(data);
                    setAppropriateFields();
                    vm.fields = [];
                    vm.fields = vm.profileTypes[0].Fields;
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }*/
    }
})();
