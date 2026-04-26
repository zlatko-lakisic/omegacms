/// <reference path="./helpers/Encoder.ts" />
namespace mdBusinessLogic.helpers {
  export var oopHelper = (child: any, parent: any): void => {
    child.prototype = Object.create(parent.prototype);
  }

  export function loadParentArray(obj: any, parentName: string, parentLinkName: string, parentArray?: Array<any>): Array<any> {
    if (parentName === undefined) {
      parentName = 'Name';
    }
    if (parentArray === undefined) {
      parentArray = new Array<any>()
    }
    if (obj !== undefined && obj !== null){
      if (obj[parentName] !== undefined && obj[parentName] != null) {
        if (parentLinkName !== undefined && obj[parentLinkName] !== undefined && obj[parentLinkName] != null) {
          var objInArray: any = {};
          objInArray[parentName] = obj[parentName];
          objInArray[parentLinkName] = obj[parentLinkName];
          parentArray.unshift(objInArray);
        } else {
          parentArray.unshift(obj[parentName]);
        }
      }
      if (obj.Parent !== undefined && obj.Parent != null) {
        loadParentArray(obj.Parent, parentName, parentLinkName, parentArray)
      }
    }
    return parentArray
  }
}
