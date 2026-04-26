namespace mdBusinessLogic.helpers {
    export module checkType {
        export function isFunction(functionToCheck: any): boolean {
            var getType = {};
            return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
        }
        export function isArray(obj: any): boolean {
            return (!!obj) && (obj.constructor === Array);
        }
        export function isObject(obj: any): boolean {
            return (!!obj) && (obj.constructor === Object);
        }
        export function getTypeName(obj: any): string {
            const funcNameRegex = /function (.{1,})\(/;
            let results = (funcNameRegex).exec((obj).constructor.toString());
            return (results && results.length > 1) ? results[1] : "";
        }
    }
}