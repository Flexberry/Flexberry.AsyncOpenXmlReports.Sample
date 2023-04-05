import AdapterMixin from 'ember-flexberry-data/mixins/adapter';
import OdataAdapter from 'ember-flexberry-data/adapters/odata';
import config from '../config/environment';
import KeycloakAdapterMixin from 'ember-keycloak-auth/mixins/keycloak-adapter';

export default OdataAdapter.extend(AdapterMixin, KeycloakAdapterMixin, {
  host: config.APP.backendUrls.api,
});
