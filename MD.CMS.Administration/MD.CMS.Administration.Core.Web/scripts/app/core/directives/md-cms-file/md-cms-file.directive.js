(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsFile', ['$q', '$timeout', 'mdFeedbackService', '$http', '$state', mdCmsFile]);
    /** @ngInject */
    function mdCmsFile($q, $timeout, $mdFeedbackService, $http, $state) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-file/md-cms-file.template.html',
            transclude: true,
            scope: {
                mdModel: "=",
                ngDisabled: "=",
                mdFolderPath: "=",
                mdInputName: "@?",
                mdMimeType: "=?",
                mdRequired: "=?",
                mdFloatingLabel: "@",
                registerUploadEvent: "&",
                mdExtension: "=?"
            },
            controller: 'mdCmsFileController as vm',
            link: function (scope, element, attrs) {
                if (scope.mdRequired === undefined) {
                    scope.mdRequired = false;
                }

                if (scope.mdInputName === undefined) {
                    scope.mdInputName = 'omega-file-input-' + mdBusinessLogic.helpers.Guid.create().value;
                }

                //Directive methods

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
            }
        };
    }
})();
