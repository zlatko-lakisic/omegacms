(function () {
  'use strict';

  angular
    .module('app.core')
    .factory('mdCustomDialogs', ['$mdDialog', mdCustomDialogs]);

  /** @ngInject */
  function mdCustomDialogs($mdDialog) {
    var showSimpleDialogO = function showSimpleDialogO(_dialogInfo, _stateInfo) {
      var parentElement = angular.element(document.querySelector('.' + this.state.current.bodyClass));
      $mdDialog.show(
        $mdDialog.alert()
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

    var showSimpleDialog = function showSimpleDialog(_title, _text, _redirect, _state, _stateParams) {
      var parentElement = angular.element(document.querySelector('.' + this.state.current.bodyClass));
      $mdDialog.show(
        $mdDialog.alert()
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

    //same method as one bellow, but optimized (all is kept due to not all changes are applied everywhere)
    var showConfirmDialogO = function showConfirmDialog(_dialogInfo, _onConfirm, _onDecline) {
      var confirm = $mdDialog.confirm()
        .title(_dialogInfo.title || '')
        .textContent(_dialogInfo.text || '')
        .clickOutsideToClose(false)
        .parent(angular.element(document.body))
        .ok(_dialogInfo.ok || 'Ok')
        .cancel(_dialogInfo.cancelText || 'Cancel');
      $mdDialog.show(confirm).then(function () {
        _onConfirm();
      }, function () {
        _onDecline() || function () { };
      }, function () {

      })
    }

    var showConfirmDialog = function showConfirmDialog(_title, _text, _ok, _cancel, _onConfirm, _onDecline) {
      var confirm = $mdDialog.confirm()
        .title(_title)
        .textContent(_text)
        .clickOutsideToClose(false)
        .parent(angular.element(document.body))
        .ok(_ok)
        .cancel(_cancel);
      $mdDialog.show(confirm).then(function () {
        _onConfirm();
      }, function () {
        _onDecline();
      });
    }

    var redirect = function (_state, _stateParams) {
      this.state.go(_state, _stateParams, { reload: true });
    }

    return {
      showSimpleDialog0: showSimpleDialogO,
      showSimpleDialog: showSimpleDialog,
      showConfirmDialogO: showConfirmDialogO,
      showConfirmDialog: showConfirmDialog,
      redirect: redirect
    };
  }
}());
