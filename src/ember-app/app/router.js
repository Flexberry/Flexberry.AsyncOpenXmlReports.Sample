import EmberRouter from '@ember/routing/router';
import config from './config/environment';

const Router = EmberRouter.extend({
  location: config.locationType
});

Router.map(function () {
  this.route('i-i-s-async-open-xml-reports-sample-brand-l');
  this.route('i-i-s-async-open-xml-reports-sample-brand-e',
  { path: 'i-i-s-async-open-xml-reports-sample-brand-e/:id' });
  this.route('i-i-s-async-open-xml-reports-sample-brand-e.new',
  { path: 'i-i-s-async-open-xml-reports-sample-brand-e/new' });
  this.route('i-i-s-async-open-xml-reports-sample-car-l');
  this.route('i-i-s-async-open-xml-reports-sample-car-e',
  { path: 'i-i-s-async-open-xml-reports-sample-car-e/:id' });
  this.route('i-i-s-async-open-xml-reports-sample-car-e.new',
  { path: 'i-i-s-async-open-xml-reports-sample-car-e/new' });
  this.route('i-i-s-async-open-xml-reports-sample-producing-country-l');
  this.route('i-i-s-async-open-xml-reports-sample-producing-country-e',
  { path: 'i-i-s-async-open-xml-reports-sample-producing-country-e/:id' });
  this.route('i-i-s-async-open-xml-reports-sample-producing-country-e.new',
  { path: 'i-i-s-async-open-xml-reports-sample-producing-country-e/new' });
  this.route('i-i-s-async-open-xml-reports-sample-user-report-l');
  this.route('i-i-s-async-open-xml-reports-sample-user-report-e',
  { path: 'i-i-s-async-open-xml-reports-sample-user-report-e/:id' });
  this.route('i-i-s-async-open-xml-reports-sample-user-report-e.new',
  { path: 'i-i-s-async-open-xml-reports-sample-user-report-e/new' });
});

export default Router;
