import { getOwner } from '@ember/application';
import Mixin from '@ember/object/mixin';
import { later } from '@ember/runloop';

export default Mixin.create({
  getSignalR() {
    const app = getOwner(this);
    const signalr = app.lookup('realtime:signalr');

    return signalr;
  },
  
  signalRConnect() {
    this.signalRTryToConnect();
  },

  signalRTryToConnect() {
    const _this = this;
    const signalr = _this.getSignalR();

    signalr.start().then(function () {

      // eslint-disable-next-line no-console
      console.log("SignalR Connected.");

      signalr.connection.onclose( _this._signalROnDisconnected.bind(_this));
      signalr.connection.on('NotifyUser', _this._notifyUser.bind(_this));
    }).catch(function (err) {

      // eslint-disable-next-line no-console
      console.log("SignalR NOT Connected." + err);
      setTimeout(_this.signalRTryToConnect(), 5000);
    });
  },

  _signalRTryToReconnect() {
    const _this = this;
    const signalR = _this.getSignalR();        

    if (!signalR.connected) {
      this.signalRTryToConnect();
      later((function() {
        _this._signalRTryToReconnect();
      }), 60000);
    }
  },

  _signalROnDisconnected() {
    const _this = this;
    const signalR = _this.getSignalR();

    if (signalR.connected) {
      signalR.connected = false;
    }
    later((function() {
      _this._signalRTryToReconnect();
    }), 5000);
  },

  _notifyUser(message) {
    const modalSignalRMessage = "SignalR notify User and send message - " + message;
    this.set('modalSignalRMessage', modalSignalRMessage);
    this.set('callSignarRTestNotificationModalOpen', true);
  }
});