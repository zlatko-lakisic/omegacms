(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdFeedbackService', ['mdToastService', '$templateRequest', '$compile', '$document', '$rootScope', '$mdDialog', mdFeedbackService]);

    /** @ngInject */
    function mdFeedbackService($mdToastService, $templateRequest, $compile, $document, $rootScope, $mdDialog) {
        var showToast = function (toastText, action, actionPromise) {
            $mdToastService.showToast(toastText, $document[0].querySelector('body'), 'bottom right', 5000, action, actionPromise);
        }
        var possibleTypes = ['save', 'delete', 'load', 'update', 'download', 'upload'];
        var possibleErrorTypesCodes = ['400', '401', '403', '404', '500'];
        var possibleErrorTypes = ['bad', 'auth', 'forbidden', 'notfound', 'internal'];

        var getTransformedString = function (string) {
            string = string.toLowerCase();
            return string.charAt(0).toUpperCase() + string.slice(1)
        }

        var getTypeFromErrorCode = function (exception) {
            if (exception.errorData != undefined || exception.errorData != null) {
                var codeIndex = possibleErrorTypesCodes.indexOf(exception.errorData.status + "");
                if (codeIndex != -1) {
                    return possibleErrorTypes[codeIndex];
                }
            }
            return null;
        }
        var getToastText = function (type, type2) {
            if (type.toLowerCase() === "error" || type.toLowerCase() === "info") {
                return $rootScope.globals.resources.Labels[getTransformedString(type2) + getTransformedString(type) + "Feedback"];
            } else {
                return "";
                console.log("Unsupported toast type");
            }
        }

        var reportErrorCode = function(errorCode) {
            $templateRequest("scripts/app/main/feedback/feedback-error.template.html").then(function (html) {
                if(html){
                    var template = angular.element(html);
                    var scope = $rootScope.$new();
                    scope.errorCode = errorCode;
                    var element = $document.find(".feedback-content-view");
                    if (element != undefined && element != null && element.length) {
                        element.html($compile(template)(scope));
                    }
                }
            });
        }

        return {
            reportError: reportError,
            reportInfo: reportInfo,
            reportCustomInfo: reportCustomInfo,
            reportJsonValue: reportJsonValue
        };
        
        /**
        * Reports error in form of toast
        * @param {string} type - Type of error we want to report (save, delete, load, update, download, upload)
        * @param {Excpetion} exception - Exception that we want to report
        * @param {boolean} errorRedirect - Variable that tells wether we should replace content with error message
        * @param {function(response)} promise - Function that will be called when toast is dissmised
        */
        function reportError(type, exception, errorRedirect, promise) {
            if (possibleTypes.indexOf(type.toLowerCase()) != -1 || possibleErrorTypes.indexOf(type.toLowerCase()) != -1) {
                if (exception == undefined || exception == null) {
                    exception = {};
                } else {
                    var errorCodeType = getTypeFromErrorCode(exception);
                    if (errorCodeType != null && errorRedirect != undefined && errorRedirect != null && errorRedirect) {
                        reportErrorCode(getToastText('error', errorCodeType));
                    }
                    type = errorCodeType || type;
                }
                if (mdBusinessLogic.settings.debug) {
                    if (exception != {}) {
                        console.log(exception);
                    }
                }
                if (false) {
                    //TODO: auto reporting system which sends bug report automatically
                }
                var showInfoDialog = function (response) {
                    if (response == 'ok') {
                        $mdDialog.show(
                            {
                                templateUrl: "scripts/app/main/feedback/feedback-dialog.template.html",
                                controller: ['$scope', '$mdDialog', function ($scope, $mdDialog) {
                                    $scope.exceptionMessage = exception.message;
                                    $scope.exceptionMessageInner = '';
                                    if (exception.innerException !== undefined && exception.innerException.ExceptionMessage !== undefined) {
                                        $scope.exceptionMessageInner = exception.innerException.ExceptionMessage.trim();
                                    }
                                    $scope.exceptionError = exception;
                                    $scope.cancel = function () {
                                        $mdDialog.cancel();
                                    };
                                }],
                                parent: angular.element(document.body),
                                clickOutsideToClose: true,
                                multiple: true
                            }
                            );
                    }
                };
                var action = $rootScope.globals.resources.Labels.Info
                var toast = showToast(getToastText('error', type), action, showInfoDialog);
                if (promise != undefined && promise != null) {
                    toast.then(promise);
                }
            } else {
                console.log("Unsupported feedback type: " + type);
            }
        }

        /**
        * Reports info in form of toast
        * @param {string} type - Type of info we want to report (save, delete, load, update, download, upload)
        */
        function reportInfo(type, promise) {
            if (possibleTypes.indexOf(type.toLowerCase()) != -1) {
                showToast(getToastText('info', type), '', promise);
            } else {
                console.log("Unsupported feedback type: " + type);
            }
        }

        /**
        * Reports info in form of toast
        * @param {string} jsonValue - Text to show in toast
        */
        function reportJsonValue(jsonValue) {
            $mdDialog.show(
                {
                    templateUrl: "scripts/app/main/feedback/feedback-code-dialog.template.html",
                    controller: ['$scope', '$mdDialog', function ($scope, $mdDialog) {
                        $scope.jsonValue = (typeof jsonValue === 'object' && jsonValue !== null) ? jsonValue : JSON.parse(jsonValue);
                        $scope.cancel = function () {
                            $mdDialog.cancel();
                        };
                    }],
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    multiple: true,
                    fullscreen: true
                }
            );
        }

        /**
        * Shows custom toast text
        * @param {string} text - Text to show in toast
        */
        function reportCustomInfo(text, promise) {
            showToast(text, '', promise);
        }
    }
}());
