(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsCalculated', ['$q', '$timeout', 'mdFeedbackService', 'PostfixEvaluatorService', mdCmsCalculated]);
    /** @ngInject */
    function mdCmsCalculated($q, $timeout, $mdFeedbackService, PostfixEvaluatorService) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-calculated/md-cms-calculated.template.html',
            transclude: true,
            scope: {
                mdModel: "=",
                mdFields: "=",
                ngDisabled: "=",
                mdFloatingLabel: "@",
                calculate:  "&"
            },
            link: function (scope, element, attrs) {
                //Directive variables
                var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
                var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
                var userController = new mdBusinessLogic.dataAccess.controllers.userController();
                var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
                var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
                var contentTypeDefinitionController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController();

                scope.hasParseError = false;

                //Directive methods
                function getFormulaWithValues() {
                    var regexp = /\bfield\.[a-zA-Z1-9\[\]_]+/g;
                    var match = null;
                    var expressionPromiseArray = [];
                    while ((match = regexp.exec(scope.mdModel.defaultValue)) !== null) {
                        var currentExpressionPart = match[0];
                        expressionPromiseArray.push($q(function (resolve, reject) {
                            for (var key = 0; key < scope.mdFields.length; key++) {
                                var fieldName = currentExpressionPart.replace('field.', '');
                                var hasPropertyName = fieldName.split('[').length > 1;
                                var propertyName = '';
                                if (hasPropertyName) {
                                    propertyName = fieldName.split('[')[1].split(']')[0];
                                    fieldName = fieldName.split('[')[0];
                                }
                                if (scope.mdFields[key].friendlyName !== undefined && scope.mdFields[key].friendlyName.toLowerCase() == fieldName.toLowerCase()) {

                                    function parseValue(value) {

                                        if (value === undefined || value == null) {
                                            value = '';
                                        }

                                        var isOperator = false;
                                        switch (value) {
                                            case '+':
                                            case '-':
                                            case '*':
                                            case '/':
                                            case '(':
                                            case ')':
                                                isOperator = true;
                                                break;
                                        }

                                        if ((value == '' || isNaN(value)) && !isOperator) {
                                            value = "'" + value + "'";
                                        }

                                        return value;
                                    }

                                    switch (scope.mdFields[key].type) {
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.userSelectorSingle: {
                                            if (scope.mdFields[key].value == null) {
                                                resolve(parseValue(''));
                                                break;
                                            }
                                            var id = 0;
                                            if (scope.mdFields[key].value.split(scope.mdFields[key].delimiter).length) {
                                                id = scope.mdFields[key].value.split(scope.mdFields[key].delimiter)[0];
                                            }
                                            if (id > 0) {
                                                userController.getById(id, function (data) {
                                                    if (hasPropertyName) {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data[propertyName])
                                                        });
                                                    } else {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data.Username)
                                                        });
                                                    }
                                                }, function (error) {
                                                    reject(error);
                                                });
                                            }
                                        }
                                            break;
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.taxonomySelectorSingle: {
                                            function processTaxonomy(id, callback) {
                                                if (!isNaN(id) && id > 0) {
                                                    taxonomyController.getById(id, function (data) {
                                                        callback(data);
                                                    }, function (error) {
                                                        reject(error);
                                                    });
                                                }
                                            }
                                            if (scope.mdFields[key].value == null) {
                                                resolve(parseValue(''));
                                                break;
                                            }
                                            if (scope.isMultiple) {
                                                var ids = scope.mdFields[key].value.split(scope.mdFields[key].delimiter);
                                                for (var i in ids) {
                                                    var id = ids[i];
                                                    processTaxonomy(id, function (data) {
                                                        if (hasPropertyName) {
                                                            resolve({
                                                                field: currentExpressionPart,
                                                                data: parseValue(data[propertyName])
                                                            });
                                                        } else {
                                                            resolve({
                                                                field: currentExpressionPart,
                                                                data: parseValue(data.Name)
                                                            });
                                                        }
                                                    });
                                                }
                                            } else {
                                                processTaxonomy(scope.mdFields[key].value, function (data) {
                                                    if (hasPropertyName) {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data[propertyName])
                                                        });
                                                    } else {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data.Name)
                                                        });
                                                    }
                                                });
                                            }
                                        }
                                            break;
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.contentSelectorSingle: {
                                            if (scope.mdFields[key].value == null) {
                                                resolve(parseValue(''));
                                                break;
                                            }
                                            var id = 0;
                                            if (scope.mdFields[key].value.split(scope.mdFields[key].delimiter).length) {
                                                id = scope.mdFields[key].value.split(scope.mdFields[key].delimiter)[0];
                                            }
                                            if (id == scope.mdFields[key].value && scope.mdFields[key].value.split('-').length) {
                                                id = scope.mdFields[key].value.split('-')[0];
                                            }
                                            if (id > 0) {
                                                contentController.getById(id, true, mdBusinessLogic.settings.lcid, true, scope.mdFields[key].dataBound, scope.mdFields[key].jsonField.getRelevantConstraint().contentTypeId, function (data) {
                                                    if (hasPropertyName && data.ContentType) {
                                                        var value = data.ContentType.getFieldValue(propertyName);
                                                        if (!value || value.length == 0) {
                                                            value = data[propertyName];
                                                        }
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(value)
                                                        });
                                                    } else {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data.Title)
                                                        });
                                                    }
                                                }, function (error) {
                                                    reject(error);
                                                });
                                            }
                                        }
                                            break;
                                        case mdBusinessLogic.dataAccess.entities.attributeTypeEnum.mediaContentSelectorSingle: {
                                            if (scope.mdFields[key].value == null) {
                                                resolve(parseValue(''));
                                                break;
                                            }
                                            var id = 0;
                                            if (scope.mdFields[key].value.split(scope.mdFields[key].delimiter).length) {
                                                id = scope.mdFields[key].value.split(scope.mdFields[key].delimiter)[0];
                                            }
                                            if (id == scope.mdFields[key].value && scope.mdFields[key].value.split('-').length) {
                                                id = scope.mdFields[key].value.split('-')[0];
                                            }
                                            if (id > 0) {
                                                mediaContentController.getById(id, mdBusinessLogic.settings.lcid, function (data) {
                                                    if (hasPropertyName) {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data[propertyName])
                                                        });
                                                    } else {
                                                        resolve({
                                                            field: currentExpressionPart,
                                                            data: parseValue(data.Title)
                                                        });
                                                    }
                                                }, function (error) {
                                                    reject(error);
                                                });
                                            }
                                        }
                                            break;
                                        default: {
                                            resolve({
                                                field: currentExpressionPart,
                                                data: parseValue(scope.mdFields[key].value)
                                            });
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }));
                    }
                    return $q(function (resolve, reject) {
                        $q.all(expressionPromiseArray).then(function (data) {
                            var formula = scope.mdModel.defaultValue;
                            for (var i = 0; i < data.length; i++) {
                                formula = formula.replace(data[i].field, data[i].data);
                            }
                            resolve(formula);
                        }, reject);
                    });
                }

                function evaluatePostfixExpression() {
                    try {
                        var round = Math.round;
                        Math.round = function (val, places) {
                            if (places === undefined) {
                                places = 0;
                            }
                            if (places == 0) {
                                return round(val);
                            } else {
                                try {
                                    var parsed = parseFloat(val.toString());
                                    return parsed.toFixed(places);
                                } catch (e) {
                                    console.warn(e);
                                }
                            }
                            return val;
                        }

                        getFormulaWithValues().then(function (formulaWithValues) {
                            formulaWithValues = formulaWithValues.replace(new RegExp('(Math.Round\\()', 'gi'), 'Math.round(');
                            formulaWithValues = formulaWithValues.replace(new RegExp('(\\((double|int|long)\\))+', 'gi'), '');
                            scope.mdModel.value = eval(formulaWithValues);
                        });
                    } catch (e) {
                        console.warn(e);
                        scope.hasParseError = true;
                    }
                }

                function init() {
                    scope.calculate()(evaluatePostfixExpression);
                    evaluatePostfixExpression();
                }

                init();
            }
        };
    }
})();
