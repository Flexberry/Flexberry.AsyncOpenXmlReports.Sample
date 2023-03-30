import {
  defineNamespace,
  defineProjections,
  Model as BrandMixin
} from '../mixins/regenerated/models/i-i-s-async-open-xml-reports-sample-brand';

import EmberFlexberryDataModel from 'ember-flexberry-data/models/model';

let Model = EmberFlexberryDataModel.extend(BrandMixin, {
});

defineNamespace(Model);
defineProjections(Model);

export default Model;
