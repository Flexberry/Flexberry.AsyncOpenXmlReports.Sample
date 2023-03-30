import { buildValidations } from 'ember-cp-validations';
import EmberFlexberryDataModel from 'ember-flexberry-data/models/model';

import {
  defineProjections,
  ValidationRules,
  Model as SparePartMixin
} from '../mixins/regenerated/models/i-i-s-async-open-xml-reports-sample-spare-part';

const Validations = buildValidations(ValidationRules, {
  dependentKeys: ['model.i18n.locale'],
});

let Model = EmberFlexberryDataModel.extend(SparePartMixin, Validations, {
});

defineProjections(Model);

export default Model;
