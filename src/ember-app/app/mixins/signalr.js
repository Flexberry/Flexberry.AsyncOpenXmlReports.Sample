import { getOwner } from '@ember/application';
import Mixin from '@ember/object/mixin';
import { later } from '@ember/runloop';
import { computed } from '@ember/object';
import { inject as service } from '@ember/service';

export default Mixin.create({
  keycloakSession: service(),

  userName: computed('keycloakSession.tokenParsed.preferred_username', function() {
    try {
      return this.keycloakSession.tokenParsed.preferred_username;
    } catch (e) {
      return null;
    }
  }),

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
      signalr.connection.onclose( _this._signalROnDisconnected.bind(_this));
      signalr.connection.on('NotifyUser', _this._notifyUser.bind(_this));
      signalr.connection.on('ReportComplete', _this._reportComplete.bind(_this));
      _this._signalRInitUserName(_this, signalr);

      // eslint-disable-next-line no-console
      console.log("SignalR Connected.");
    }).catch(function (err) {

      // eslint-disable-next-line no-console
      console.log("SignalR NOT Connected." + err);
    });
  },

  _signalRInitUserName(_this, signalr) {
    // Ждем пока keycloak вернет имя пользователя. Делает это он не сразу.
    if (_this.userName != null) {
      signalr.connection.invoke("AddUser", _this.userName);

      // eslint-disable-next-line no-console
      console.log("SignalR: UserName initialized.");
    } else {

      // eslint-disable-next-line no-console
      console.log("SignalR: No UserName.");

      later((function() {
        _this._signalRInitUserName(_this, signalr);
      }), 5000);
    }
  },

  _signalRTryToReconnect() {
    const _this = this;
    const signalR = _this.getSignalR();        

    if (!signalR.connected) {
      this.signalRTryToConnect();
      later((function() {
        _this._signalRTryToReconnect();
      }), 5000);
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
  },

  _reportComplete(message) {
    const modalSignalRMessage = message;
    this.set('modalSignalRTitle', "Формирование отчета");
    this.set('modalSignalRMessage', modalSignalRMessage);
    this.set('callSignarRTestNotificationModalOpen', true);
  }
});