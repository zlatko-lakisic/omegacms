/// <reference path="../globalVariables.ts" />
/// <reference path="../settings.ts" />
namespace mdBusinessLogic {
  export namespace helpers {
    export class mdException {
      public settings: any;
      public message: string;
      public errorData: any;
      public innerException: any;
      public stackTrace: any;

      constructor(message: string, errorData?: any, innerException?: any, stackTrace?: any) {
        this.settings = mdBusinessLogic.settings;
        this.errorData = errorData;
        this.innerException = innerException;
          this.stackTrace = stackTrace !== undefined ? stackTrace : (new Error()).stack;
        this.message = message;

        if (this.settings.debug) {
          console.log('Error occurred: ' + this.message + (this.errorData !== undefined && this.errorData != null ? ' data(' + JSON.stringify(this.errorData) + ')' + (this.stackTrace !== undefined ? ', stacktrace(' + this.stackTrace + ')' : '') : ''));
        }
      }
    }
  }
}
