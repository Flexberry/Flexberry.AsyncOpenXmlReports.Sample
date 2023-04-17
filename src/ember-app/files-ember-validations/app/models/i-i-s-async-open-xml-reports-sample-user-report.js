import {
  defineNamespace,
  defineProjections,
  Model as UserReportMixin
} from '../mixins/regenerated/models/i-i-s-async-open-xml-reports-sample-user-report';

import EmberFlexberryDataModel from 'ember-flexberry-data/models/model';

let Model = EmberFlexberryDataModel.extend(UserReportMixin, {
});

defineNamespace(Model);
defineProjections(Model);

export default Model;
