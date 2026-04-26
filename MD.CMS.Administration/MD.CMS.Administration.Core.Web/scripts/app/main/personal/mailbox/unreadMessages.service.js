(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .service('$UnreadMessagesService', [UnreadMessagesService]);


    /** @ngInject */
    function UnreadMessagesService() {
        var service = this;

        service.unreadMessages = [];

    }
})();