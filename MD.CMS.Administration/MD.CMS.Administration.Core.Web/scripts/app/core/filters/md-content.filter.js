(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('mdContent', ['mdContentService', 'mdFeedbackService', mdContent]);

    /** @ngInject */
    function mdContent(mdContentService, $mdFeedbackService)
    {
        var result = {};
        var serviceInvoked = false;
        var filter = function (contentId, expression, defaultValue) {
            if (!result[contentId]) {
                if (!serviceInvoked) {
                    serviceInvoked = true;
                    var contentRequest = {
                        ContentIds: [contentId],
                        LoadAuthor: false,
                        LoadFields: true
                    };
                    if (contentId.indexOf('-') >= 0) {
                        contentRequest.ContentIds = [contentId.split('-')[0]];
                        contentRequest.LCID = contentId.split('-')[1];
                    }
                    mdContentService.parse(contentRequest, expression, defaultValue).then(function (data) {
                        if (data !== undefined && data != null) {
                            result[contentId] = data;
                        }
                        serviceInvoked = false;
                    }, function (error) {
                        $mdFeedbackService.reportError('read', error);
                        serviceInvoked = false;
                    });
                }
                return '-';
            } else {
                return result[contentId];
            }
        };
        filter.$stateful = true;
        return filter;
    }

})();