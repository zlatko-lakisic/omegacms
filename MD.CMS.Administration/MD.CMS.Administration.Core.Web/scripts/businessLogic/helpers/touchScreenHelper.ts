namespace mdBusinessLogic.helpers {
  export module touchScreenHelper {
    export function isTouchDevice() {
      return 'ontouchstart' in window
        || navigator.maxTouchPoints;
    }
  }
};
