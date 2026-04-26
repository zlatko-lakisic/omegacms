/// <reference path="../globalVariables.ts" />
/// <reference path="../settings.ts" />
namespace mdBusinessLogic {
  export namespace helpers {
    export class dialog {
      public dialog: any;
      public state: any;

      constructor(dialog:any, state:any) {
        this.dialog = dialog;
        this.state = state;
      }

      public showSimpleDialogO(_dialogInfo, _stateInfo) {
        var parentElement = angular.element(document.querySelector('.' + this.state.current.bodyClass));
        this.dialog.show(
          this.dialog.alert()
            .parent(parentElement)
            .clickOutsideToClose(true)
            .parent(parentElement)
            .title(_dialogInfo.title || '')
            .textContent(_dialogInfo.text || '')
            .ariaLabel(_dialogInfo.title || '')
            .ok(_dialogInfo.okText || 'Got it!')
        );
        if (_stateInfo.changeState) {
          this.state.go(_stateInfo.stateToGo, _stateInfo.stateParams, { reload: true });
        }
      }

      public showCustomDialog(_onConfirm, _onDecline) {

        var show = this.dialog.show({
          templateUrl: 'scripts/app/main/settings/configuration/template/dialogTemplates.html',
          parent: angular.element(document.body),
          clickOutsideToClose: true
        }).then(_onConfirm, _onDecline);

      };

      public showSimpleDialog(_title, _text, _redirect, _state, _stateParams) {
        var parentElement = angular.element(document.querySelector('.' + this.state.current.bodyClass));
        this.dialog.show(
          this.dialog.alert()
            .parent(parentElement)
            .clickOutsideToClose(true)
            .parent(parentElement)
            .title(_title)
            .textContent(_text)
            .ariaLabel(_title)
            .ok('Got it!')
        );
        if (_redirect) {
          this.state.go(_state, _stateParams, { reload: true });
        }
      }

      public showConfirmDialogO(_dialogInfo, _onConfirm, _onDecline) {
        var confirm = this.dialog.confirm()
          .title(_dialogInfo.title || '')
          .textContent(_dialogInfo.text || '')
          .clickOutsideToClose(false)
          .parent(angular.element(document.body))
          .ok(_dialogInfo.ok || 'Ok')
          .cancel(_dialogInfo.cancelText || 'Cancel');
        this.dialog.show(confirm).then(function () {
          _onConfirm();
        }, function () {
          _onDecline() || function () { };
        }, function () {

        })
      }

      public showConfirmDialog(_title, _text, _ok, _cancel, _onConfirm, _onDecline) {
        var confirm = this.dialog.confirm()
          .title(_title)
          .textContent(_text)
          .clickOutsideToClose(false)
          .parent(angular.element(document.body))
          .ok(_ok)
          .cancel(_cancel);
        this.dialog.show(confirm).then(function () {
          _onConfirm();
        }, function () {
          _onDecline();
        });
      }

      public redirect(_state, _stateParams) {
        this.state.go(_state, _stateParams, { reload: true });
      }
    }
  }
}
