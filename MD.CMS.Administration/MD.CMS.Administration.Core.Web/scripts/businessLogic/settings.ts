namespace mdBusinessLogic {
    export module settings {
        export var debug: boolean = false;
        export var code: string = '';
        export var lcid: number = 0;
        export var apiBase: string = '';
        export var apiBaseSeparator: string = '/';
        export var appBase: string = '';
        export var uploadsBase: string = '';
        export var apiAllowCrossOrigin: boolean = false;
        export var isAdministration: boolean = false;
        export var packageWebSocketInBody: boolean = false;
        export var authorizationHeader: string = 'authorization';
    }
}
