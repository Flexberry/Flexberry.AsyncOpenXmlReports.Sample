import { moduleForModel, test } from 'ember-qunit';

moduleForModel('i-i-s-async-open-xml-reports-sample-spare-part', 'Unit | Model | i-i-s-async-open-xml-reports-sample-spare-part', {
  // Specify the other units that are required for this test.
  needs: [
    'model:i-i-s-async-open-xml-reports-sample-brand',
    'model:i-i-s-async-open-xml-reports-sample-car',
    'model:i-i-s-async-open-xml-reports-sample-producing-country',
    'model:i-i-s-async-open-xml-reports-sample-spare-part',
    'validator:ds-error',
    'validator:presence',
    'validator:number',
    'validator:date',
    'validator:belongs-to',
    'validator:has-many',
    'service:syncer',
  ],
});

test('it exists', function(assert) {
  let model = this.subject();

  // let store = this.store();
  assert.ok(!!model);
});
