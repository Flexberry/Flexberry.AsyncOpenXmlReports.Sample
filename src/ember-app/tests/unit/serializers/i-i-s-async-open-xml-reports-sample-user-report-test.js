import { moduleForModel, test } from 'ember-qunit';

moduleForModel('i-i-s-async-open-xml-reports-sample-user-report', 'Unit | Serializer | i-i-s-async-open-xml-reports-sample-user-report', {
  // Specify the other units that are required for this test.
  needs: [
    'serializer:i-i-s-async-open-xml-reports-sample-user-report',
    'service:syncer',
    'transform:file',
    'transform:decimal',
    'transform:guid',

    'transform:i-i-s-async-open-xml-reports-sample-car-type',
    'transform:i-i-s-async-open-xml-reports-sample-report-status-type',

    'model:i-i-s-async-open-xml-reports-sample-brand',
    'model:i-i-s-async-open-xml-reports-sample-car',
    'model:i-i-s-async-open-xml-reports-sample-producing-country',
    'model:i-i-s-async-open-xml-reports-sample-spare-part',
    'model:i-i-s-async-open-xml-reports-sample-user-report',
    'validator:ds-error',
    'validator:presence',
    'validator:number',
    'validator:date',
    'validator:belongs-to',
    'validator:has-many',
  ],
});

// Replace this with your real tests.
test('it serializes records', function(assert) {
  let record = this.subject();

  let serializedRecord = record.serialize();

  assert.ok(serializedRecord);
});
