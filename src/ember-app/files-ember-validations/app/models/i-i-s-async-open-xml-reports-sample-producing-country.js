import {
  defineNamespace,
  defineProjections,
  Model as ProducingCountryMixin
} from '../mixins/regenerated/models/i-i-s-async-open-xml-reports-sample-producing-country';

import EmberFlexberryDataModel from 'ember-flexberry-data/models/model';

let Model = EmberFlexberryDataModel.extend(ProducingCountryMixin, {
});

defineNamespace(Model);
defineProjections(Model);

export default Model;
