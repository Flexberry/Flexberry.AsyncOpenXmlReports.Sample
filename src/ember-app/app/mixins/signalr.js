import { getOwner } from '@ember/application';
import Mixin from '@ember/object/mixin';

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

      signalr.connection.on('NotifyUser', _this._notifyUser.bind(_this));
    }).catch(function (err) {

      // eslint-disable-next-line no-console
      console.log("SignalR NOT Connected." + err);
    });
  },

  modalSignalRMessage: undefined,

  callSignarRTestNotificationModalOpen: false,

  getModalSignalRMessage() {
    return this.modalSignalRMessage;
  },

  _notifyUser(message) {
    const modalSignalRMessage = "SignalR notify User and send message - " + message;
    this.set('modalSignalRMessage', modalSignalRMessage);
    this.set('callSignarRTestNotificationModalOpen', true);
  }
});