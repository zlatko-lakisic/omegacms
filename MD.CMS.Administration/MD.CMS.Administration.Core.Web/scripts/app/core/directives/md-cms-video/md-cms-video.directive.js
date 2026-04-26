(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsVideo', ['$q', '$sce', '$document', '$timeout', mdCmsVideo]);
    /** @ngInject */
    function mdCmsVideo($q, $sce, $document, $timeout) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-video/md-cms-video.template.html',
            transclude: true,
            scope: {
                mdModel: "=",
                ngDisabled: "="
            },
            link: function (scope, element, attrs) {
                //Directive variables
                scope.youtuberegex = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;
                scope.iframeId = 'iframe_' + scope.mdModel.uniqueId;
                scope.videoLoaded = false;

                //Directive methods
                scope.getInnerHtml = getInnerHtml;
                scope.loadYoutubeIframe = loadYoutubeIframe;

                //Autocomplete query search
                function loadYoutubeIframe() {
                    function getUrl() {
                        if (scope.mdModel.value) {
                            var p = scope.youtuberegex;
                            var videoId = (scope.mdModel.value.match(p)) ? RegExp.$1 : false;
                            if (videoId) {
                                return $sce.trustAsResourceUrl('//www.youtube.com/embed/' + videoId);
                            }
                        }
                        return false;
                    }
                    var videoUrl = getUrl();
                    scope.videoLoaded = !videoUrl ? false : true;
                    if (videoUrl) {
                        $document.find('#' + scope.iframeId).attr('src', videoUrl);
                    }
                }

                function getInnerHtml() {
                    return element.find('md-inner-html').html() || '';
                }

                function init() {
                    $timeout(function () {
                        loadYoutubeIframe();
                    });
                }

                init();
            }
        };
    }
})();
