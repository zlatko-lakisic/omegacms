/// <reference path="../globalVariables.ts" />
namespace mdBusinessLogic.settings.secureApi {
  var forge;
  export var enabled: boolean = false;
  export var rsaKeys: Object = new Object();
  export var aesKey: string = '';
  export var aesIV: string = '';
  export var crypto: any = {
    rsa: (forge !== undefined ? forge.rsa : null),
    aes: null
  };
  export var token: string = '';
}
