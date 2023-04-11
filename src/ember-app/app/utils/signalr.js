import { resolve } from 'rsvp';

class SignalRConnection {
  constructor(url) {

    // eslint-disable-next-line no-undef
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(url)

      // eslint-disable-next-line no-undef
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connected = false;
  }

  start() {
    if (!this.connected && this.connection) {
      let self = this;

      return self.connection.start().then(function () {
        self.connected = true;
      });
    } else {
      return resolve();
    }
  }
}

export default SignalRConnection;
