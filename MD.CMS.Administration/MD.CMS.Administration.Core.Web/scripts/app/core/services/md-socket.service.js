(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdSocketService', [mdSocketService]);

    /** @ngInject */
    function mdSocketService() {

        function MdSocket(socketDelayStart, socketMessageInterval, keepAlive) {
            if (keepAlive === undefined) {
                keepAlive = 0;
            }

            if (isNaN(keepAlive)) {
                keepAlive = 0;
            }

            this.socketDelayStart = socketDelayStart;
            this.socketMessageInterval = socketMessageInterval;
            this.id = null;
            this.socket = null;
            this.isRunning = false;
            this.intervalTimeoutId = null;
            this.keepAlive = keepAlive;
            this.callback = null;
        }

        MdSocket.prototype.run = function (callback) {
            this.callback = callback;
            function execute(obj, intervalTimeout) {
                if (intervalTimeout === undefined) {
                    intervalTimeout = function (isFirstRun, obj) { };
                }

                obj.id = obj.callback(function (socket) {
                    obj.socket = socket;
                    if (obj !== undefined && obj.socket != null) {
                        obj.isRunning = true;
                    }

                    intervalTimeout(false, obj);
                });
            }

            function intervalTimeout(isFirstRun, obj) {
                obj.intervalTimeoutId = setTimeout(function () {
                    execute(obj, intervalTimeout);
                }, isFirstRun ? obj.socketDelayStart : obj.socketMessageInterval);
            }

            if (this.socketMessageInterval == 0) {
                execute(this);
            } else {
                intervalTimeout(true, this);
            }
        }

        MdSocket.prototype.close = function () {
            if (this.socket !== undefined && this.socket != null) {
                this.socket.close();
            }
            if (this.intervalTimeoutId != null) {
                clearTimeout(this.intervalTimeoutId);
                this.intervalTimeoutId = null;
            }
            this.isRunning = false;
            if (this.keepAlive > 0) {
                this.keepAlive--;
                this.run(this.callback);
            }
        }

        return {
            create: function (socketDelayStart, socketMessageInterval) {
                return new MdSocket(socketDelayStart, socketMessageInterval);
            }
        };
    }
}());
