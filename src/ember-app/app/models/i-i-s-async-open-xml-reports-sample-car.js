import { buildValidations } from 'ember-cp-validations';
import EmberFlexberryDataModel from 'ember-flexberry-data/models/model';

import {
  defineProjections,
  ValidationRules,
  Model as CarMixin
} from '../mixins/regenerated/models/i-i-s-async-open-xml-reports-sample-car';

const Validations = buildValidations(ValidationRules, {
  dependentKeys: ['model.i18n.locale'],
});

let Model = EmberFlexberryDataModel.extend(CarMixin, Validations, {
});

defineProjections(Model);

export default Model;
