(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdPaginationService', [mdPaginationService]);

    /** @ngInject */
    function mdPaginationService() {
        var size, total, page, show;

        function getTotalItems() {
            return ((page + 1) * size) > total ? total : ((page + 1) * size);
        }

        function checkVisibility() {
            show = total > 10 && total != 0;
        }

        return {
            setPage: function (currentPage) {
                page = currentPage;
            },
            getPage: function () {
                return page < 0 ? 0 : page || 0;
            },
            setTotal: function (totalItems) {
                total = totalItems;
                checkVisibility();
            },
            getTotal: function () {
                return total || 0;
            },
            setSize: function (pagingSize) {
                size = pagingSize;
                checkVisibility();
            },
            getSize: function () {
                return size || 0;
            },
            showPaging: function () {
                return show;
            },

            //0 - shown left
            //1 - show both
            //2 - show right
            //3 - show none
            showArrows: function () {
                if (total <= size) {
                    return 3;
                }
                if(page==0){
                    return 2;
                }
                if (page < (Math.ceil(total / size) - 1) && parseInt(total / size) > 1) {
                    return 1;
                }
                return 0
            },
            setVisibility: function (visibility) {
                checkVisibility();
                show = visibility;
            },
            nextPage: function () {
                page++;
            },
            previousPage: function () {
                page--;
            },
            getPaginationString: function () {
                return (page * size + 1) + " - " + getTotalItems() + " of " + (total || 0);
            },         
            isLastPage: function () {
                return page == Math.ceil(total / size) - 1;
            },
            isFirstPage: function () {
                return page == 0;
            }
        };
    }
}());