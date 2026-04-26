(function ()
{
    'use strict';

    angular
        .module('app.core')
        .directive('msSearchBar', ['$document', '$rootScope', msSearchBarDirective]);

    /** @ngInject */
    function msSearchBarDirective($document, $rootScope)
    {
        return {
            restrict   : 'E',
            scope      : true,
            templateUrl: 'scripts/app/core/directives/ms-search-bar/ms-search-bar.html',
            controller: 'SearchController',
            controllerAs: 'vm',
            compile    : function (tElement)
            {
                // Add class
                tElement.addClass('ms-search-bar');

                return function postLink(scope, iElement)
                {
                    var expanderEl,
                        collapserEl;

                    // Initialize
                    init();

                    function init()
                    {
                        expanderEl = iElement.find('#ms-search-bar-expander');
                        collapserEl = iElement.find('#ms-search-bar-collapser');

                        expanderEl.on('click', expand);
                        collapserEl.on('click', collapse);


                        $rootScope.$watch('globalSearchTerm', function (oldValue, newValue) {
                            if (newValue !== undefined) {
                                expand();
                            } else {
                                collapse();
                            }
                        });
                    }

                    /**
                     * Expand
                     */
                    function expand()
                    {
                        iElement.addClass('expanded');

                        // Esc key event
                        $document.on('keyup', escKeyEvent);
                    }

                    /**
                     * Collapse
                     */
                    function collapse()
                    {
                        iElement.removeClass('expanded');
                    }

                    /**
                     * Escape key event
                     *
                     * @param e
                     */
                    function escKeyEvent(e)
                    {
                        if ( e.keyCode === 27 )
                        {
                            collapse();
                            $document.off('keyup', escKeyEvent);
                        }
                    }
                };
            }
        };
    }
})();