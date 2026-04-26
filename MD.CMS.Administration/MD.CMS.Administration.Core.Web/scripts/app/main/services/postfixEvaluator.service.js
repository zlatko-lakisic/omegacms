(function () {
    'use strict';

    angular
        .module('app.services')
        .service('PostfixEvaluatorService', [PostfixEvaluatorService]);


    /** @ngInject */
    function PostfixEvaluatorService() {
        var service = this;

        var args = [];
        function checkArgumentsSize() {
            if (args.length < 2) {
                throw 'invalid arguments';
            }
        }

        function addArgs() {
            checkArgumentsSize();
            var arg2 = args.pop();
            var arg1 = args.pop();
            args.push(arg1 + arg2);
        }

        function subArgs() {
            checkArgumentsSize();
            var arg2 = args.pop();
            var arg1 = args.pop();
            args.push(arg1 - arg2);
        }

        function mulArgs() {
            checkArgumentsSize();
            var arg2 = args.pop();
            var arg1 = args.pop();
            args.push(arg1 * arg2);
        }

        function divArgs() {
            checkArgumentsSize();
            var arg2 = args.pop();
            var arg1 = args.pop();
            args.push(arg1 / arg2);
        }

        function povArgs() {
            checkArgumentsSize();
            var arg2 = args.pop();
            var arg1 = args.pop();
            args.push(Math.pow(arg1, arg2));
        }

        function processToken(token) {
            switch (token) {
                case "+":
                    addArgs();
                    break;
                case "-":
                    subArgs();
                    break;
                case "*":
                    mulArgs();
                    break;
                case "/":
                    divArgs();
                    break;
                case "^":
                    povArgs();
                    break;
                default:
                    try {
                        args.push(token)
                    } catch (e) {
                        throw '';
                    }
                    break;
            }
        }

        service.evaluatePostfixExpression = function (expression) {
            args = [];
            try {
                for (var i in expression) {
                  if (!mdBusinessLogic.helpers.checkType.isFunction(expression[i]) && expression[i] != "") {
                        processToken(expression[i]);
                    }
                }
                if (args.length == 1) {
                    return args.pop();
                } else {
                    return 0;
                }
            } catch (e) {
                return '';
            }
        }
    }
})();
