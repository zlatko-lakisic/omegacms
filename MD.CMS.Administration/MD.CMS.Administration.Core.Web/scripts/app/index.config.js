(function () {
    'use strict';

    angular
        .module('omega')
        .config(['cfpLoadingBarProvider', '$compileProvider', config]);

    function config(cfpLoadingBarProvider, $compileProvider) {

        $compileProvider.debugInfoEnabled(mdBusinessLogic.globals.uiDebugMode);
        $compileProvider.imgSrcSanitizationWhitelist(/^\s*(https?|ftp|file|chrome-extension):|data:image\//);
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|chrome-extension):/);

        mdBusinessLogic.settings.apiAllowCrossOrigin = true;

        if (mdBusinessLogic.settings.defaultState === undefined) {
            mdBusinessLogic.settings.defaultState = 'app.content_list';
        }

        tinyMCE.baseURL = '/js/ext/tinymce-dist';
        tinyMCE.suffix = '.min';

        cfpLoadingBarProvider.includeSpinner = false;
        cfpLoadingBarProvider.includeBar = false;

        mdBusinessLogic.settings.admin.registerAdminEvent(new mdBusinessLogic.settings.adminEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnJsonSerialize, function (nonSerializedRequest) {
            return new Promise(function (resolve, reject) {
                var result = {}.toString.call(nonSerializedRequest) === '[object Function]' ? nonSerializedRequest : JSON.parse(angular.toJson(nonSerializedRequest));
                resolve(result);
            });
        }));

        /*mdBusinessLogic.settings.ajax.onJsonSerialize = function (nonSerializedRequest) {
            var result = {}.toString.call(nonSerializedRequest) === '[object Function]' ? nonSerializedRequest : JSON.parse(angular.toJson(nonSerializedRequest));
            return result;
        }*/
    }

})();
