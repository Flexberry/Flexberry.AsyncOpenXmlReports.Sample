import SignalRConnection from '../utils/signalr';
import config from '../config/environment';

export function initialize(application) {
  let signalr = new SignalRConnection(config.APP.backendUrls.root + '/SignalRTest');
  application.register('realtime:signalr', signalr, { instantiate: false });
}