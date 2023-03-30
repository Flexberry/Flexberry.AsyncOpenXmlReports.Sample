import Mixin from '@ember/object/mixin';
import $ from 'jquery';
import DS from 'ember-data';
import { validator } from 'ember-cp-validations';
import { attr, belongsTo, hasMany } from 'ember-flexberry-data/utils/attributes';

export let Model = Mixin.create({
  name: DS.attr('string'),
  quantity: DS.attr('number'),
  used: DS.attr('boolean', { defaultValue: false }),
  producingCountry: DS.belongsTo('i-i-s-async-open-xml-reports-sample-producing-country', { inverse: null, async: false }),
  car: DS.belongsTo('i-i-s-async-open-xml-reports-sample-car', { inverse: 'sparePart', async: false })
});

export let ValidationRules = {
  name: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-spare-part.validations.name.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  quantity: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-spare-part.validations.quantity.__caption__',
    validators: [
      validator('ds-error'),
      validator('number', { allowString: true, allowBlank: true, integer: true }),
    ],
  },
  used: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-spare-part.validations.used.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  producingCountry: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-spare-part.validations.producingCountry.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  car: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-spare-part.validations.car.__caption__',
    validators: [
      validator('ds-error'),
      validator('presence', true),
    ],
  },
};

export let defineProjections = function (modelClass) {
  modelClass.defineProjection('SparePartE', 'i-i-s-async-open-xml-reports-sample-spare-part', {
    name: attr('Name', { index: 0 }),
    quantity: attr('Quantity', { index: 1 }),
    used: attr('Used', { index: 2 }),
    producingCountry: belongsTo('i-i-s-async-open-xml-reports-sample-producing-country', 'Producing country', {
      name: attr('Name', { index: 4, hidden: true })
    }, { index: 3, displayMemberPath: 'name' })
  });
};
