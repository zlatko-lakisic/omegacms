(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .service('TinyMceMailboxService', ['$mdMedia', TinyMceMailboxService]);


    /** @ngInject */
    function TinyMceMailboxService($mdMedia) {
        var service = this;
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();

        //vm.tinymceModel = '';
        service.searchCmsContent = function (searchTerm, callback, filetype) {
            if (searchTerm && searchTerm.length) {
                if (filetype == -2) {
                    var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
                    contentController.getBySearchTerm(searchTerm, false, mdBusinessLogic.settings.lcid || 2057,
                    function (data) {
                        callback(data);
                    },
                    function (error) {
                    });
                } else {
                    var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
                    mediaContentController.searchByFileType(searchTerm, filetype, mdBusinessLogic.settings.lcid || 2057,
                        function (data) {
                            callback(data);
                        },
                        function (error) {
                        });
                }
            } else {
                service.refreshCmsContent(callback, filetype);
            }
        };
        service.refreshCmsContent = function (callback, filetype) {
            if (filetype == -2) {
                var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
                contentController.getAll(
                    function (data) {
                        callback(data);
                    },
                    function (error) {
                    });
            } else {
                var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
                mediaContentController.getByFileType(filetype, mdBusinessLogic.settings.lcid || 2057,
                    function (data) {
                        callback(data);
                    },
                    function (error) {
                    });
            }
        };
        service.tinymceOptions = {
            plugins: [
                'advlist autolink lists link image charmap print preview hr anchor pagebreak',
                'searchreplace wordcount visualblocks visualchars code fullscreen cmsimage',
                'insertdatetime media nonbreaking save table contextmenu directionality cmslink',
                'emoticons template paste textcolor colorpicker textpattern imagetools codesample cmsmedia'
            ],
            toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent',
            toolbar2: 'print preview | forecolor backcolor emoticons | codesample | cmsmedia cmsimage cmslink',
            cmsimage: {
                search: function (searchTerm, callback) {
                    service.searchCmsContent(searchTerm, callback, 1);
                },
                refresh: function (callback) {
                    service.refreshCmsContent(callback, 1);
                },
                image_list: []
            },
            cmsmedia: {
                search: function (searchTerm, callback) {
                    service.searchCmsContent(searchTerm, callback, 2);
                },
                refresh: function (callback) {
                    service.refreshCmsContent(callback, 2);
                },
                video_list: []
            },
            cmslink: {
                searchmedia: function (searchTerm, callback) {
                    service.searchCmsContent(searchTerm, callback, -1);
                },
                searchcontent: function (searchTerm, callback) {
                    service.searchCmsContent(searchTerm, callback, -2);
                },
                refreshmedia: function (callback) {
                    service.refreshCmsContent(callback, -1);
                },
                refreshcontent: function (callback) {
                    service.refreshCmsContent(callback, -2);
                },
                mediacontent_list: []
            }
        }
    }
})();