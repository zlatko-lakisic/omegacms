(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsTextarea', ['$q', 'mdFeedbackService', mdCmsTextarea]);
    /** @ngInject */
    function mdCmsTextarea($q, $mdFeedbackService) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-textarea/md-cms-textarea.template.html',
            transclude: true,
            scope: {
                mdInputName: "@",
                mdFloatingLabel: "@",
                mdModel: "=?",
                ngDisabled: "="
            },
            controller: ['$scope', function ($scope) {
                //Directive variables
                $scope.tinymceOptions = {
                    plugins: [
                        'advlist autolink lists charmap print preview hr anchor pagebreak',
                        'searchreplace wordcount visualblocks visualchars code fullscreen cmsimage',
                        'insertdatetime nonbreaking save table contextmenu directionality cmslink',
                        'emoticons template paste textcolor colorpicker textpattern imagetools codesample cmsmedia'
                    ],
                    toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent',
                    toolbar2: 'print preview | forecolor backcolor emoticons | codesample | cmsmedia cmsimage cmslink',
                    cmsimage: {
                        search: function (searchTerm, callback) {
                            searchCmsContent(searchTerm, callback, 1);
                        },
                        refresh: function (callback) {
                            refreshCmsContent(callback, 1);
                        },
                        image_list: []
                    },
                    cmsmedia: {
                        search: function (searchTerm, callback) {
                            searchCmsContent(searchTerm, callback, 2);
                        },
                        refresh: function (callback) {
                            refreshCmsContent(callback, 2);
                        },
                        video_list: []
                    },
                    cmslink: {
                        searchmedia: function (searchTerm, callback) {
                            searchCmsContent(searchTerm, callback, -1);
                        },
                        searchcontent: function (searchTerm, callback) {
                            searchCmsContent(searchTerm, callback, -2);
                        },
                        refreshmedia: function (callback) {
                            refreshCmsContent(callback, -1);
                        },
                        refreshcontent: function (callback) {
                            refreshCmsContent(callback, -2);
                        },
                        mediacontent_list: []
                    },
                    baseURL: mdBusinessLogic.settings.appBase + 'scripts/plugins/tinymce-dist'
                };


                //Directive methods
                $scope.getInnerHtml = getInnerHtml;

                //Autocomplete query search
                function searchCmsContent(searchTerm, callback, filetype) {
                    if (searchTerm && searchTerm.length) {
                        if (filetype == -2) {
                            var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
                            contentController.getBySearchTerm(searchTerm, false, mdBusinessLogic.settings.lcid || 2057,
                                function (data) {
                                    callback(data);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                        } else {
                            var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
                            mediaContentController.searchByFileType(searchTerm, filetype, mdBusinessLogic.settings.lcid || 2057,
                                function (data) {
                                    callback(data);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                        }
                    } else {
                        refreshCmsContent(callback, filetype);
                    }
                }

                function refreshCmsContent(callback, filetype) {
                    if (filetype == -2) {
                        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
                        contentController.getAll(
                            function (data) {
                                callback(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    } else {
                        var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
                        mediaContentController.getByFileType(filetype, mdBusinessLogic.settings.lcid || 2057,
                            function (data) {
                                callback(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    }
                }

                function getInnerHtml() {
                    return element.find('md-inner-html').html() || '';
                }

                function init() {

                }

                init();
            }]
        };
    }
})();
