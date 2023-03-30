import { buildValidations } from 'ember-cp-validations';
import EmberFlexberryDataModel from 'ember-flexberry-data/models/model';

import {
  defineProjections,
  ValidationRules,
  Model as BrandMixin
} from '../mixins/regenerated/models/i-i-s-async-open-xml-reports-sample-brand';

const Validations = buildValidations(ValidationRules, {
  dependentKeys: ['model.i18n.locale'],
});

let Model = EmberFlexberryDataModel.extend(BrandMixin, Validations, {
});

defineProjections(Model);

export default Model;
