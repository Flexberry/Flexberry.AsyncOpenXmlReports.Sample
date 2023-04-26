import Mixin from '@ember/object/mixin';
import $ from 'jquery';
import DS from 'ember-data';
import { validator } from 'ember-cp-validations';
import { attr, belongsTo, hasMany } from 'ember-flexberry-data/utils/attributes';

export let Model = Mixin.create({
  file: DS.attr('file'),
  reportId: DS.attr('guid'),
  reportTaskStartTime: DS.attr('date'),
  status: DS.attr('i-i-s-async-open-xml-reports-sample-report-status-type'),
  userEmail: DS.attr('string'),
  userName: DS.attr('string')
});

export let ValidationRules = {
  file: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-user-report.validations.file.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  reportId: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-user-report.validations.reportId.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  reportTaskStartTime: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-user-report.validations.reportTaskStartTime.__caption__',
    validators: [
      validator('ds-error'),
      validator('date'),
    ],
  },
  status: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-user-report.validations.status.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  userEmail: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-user-report.validations.userEmail.__caption__',
    validators: [
      validator('ds-error'),
    ],
  },
  userName: {
    descriptionKey: 'models.i-i-s-async-open-xml-reports-sample-user-report.validations.userName.__caption__',
    validators: [
      validator('ds-error'),
      validator('presence', true),
    ],
  },
};

export let defineProjections = function (modelClass) {
  modelClass.defineProjection('UserReportE', 'i-i-s-async-open-xml-reports-sample-user-report', {
    userName: attr('', { index: 0 }),
    userEmail: attr('', { index: 1 }),
    reportId: attr('', { index: 2, hidden: true }),
    reportTaskStartTime: attr('', { index: 3 }),
    status: attr('', { index: 4 }),
    file: attr('', { index: 5 })
  });

  modelClass.defineProjection('UserReportL', 'i-i-s-async-open-xml-reports-sample-user-report', {
    userName: attr('', { index: 0 }),
    userEmail: attr('', { index: 1 }),
    reportId: attr('', { index: 2, hidden: true }),
    reportTaskStartTime: attr('', { index: 3 }),
    status: attr('', { index: 4 }),
    file: attr('', { index: 5 })
  });
};
