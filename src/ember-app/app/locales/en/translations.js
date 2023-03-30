import $ from 'jquery';
import EmberFlexberryTranslations from 'ember-flexberry/locales/en/translations';

import IISAsyncOpenXmlReportsSampleBrandLForm from './forms/i-i-s-async-open-xml-reports-sample-brand-l';
import IISAsyncOpenXmlReportsSampleCarLForm from './forms/i-i-s-async-open-xml-reports-sample-car-l';
import IISAsyncOpenXmlReportsSampleProducingCountryLForm from './forms/i-i-s-async-open-xml-reports-sample-producing-country-l';
import IISAsyncOpenXmlReportsSampleBrandEForm from './forms/i-i-s-async-open-xml-reports-sample-brand-e';
import IISAsyncOpenXmlReportsSampleCarEForm from './forms/i-i-s-async-open-xml-reports-sample-car-e';
import IISAsyncOpenXmlReportsSampleProducingCountryEForm from './forms/i-i-s-async-open-xml-reports-sample-producing-country-e';
import IISAsyncOpenXmlReportsSampleBrandModel from './models/i-i-s-async-open-xml-reports-sample-brand';
import IISAsyncOpenXmlReportsSampleCarModel from './models/i-i-s-async-open-xml-reports-sample-car';
import IISAsyncOpenXmlReportsSampleProducingCountryModel from './models/i-i-s-async-open-xml-reports-sample-producing-country';
import IISAsyncOpenXmlReportsSampleSparePartModel from './models/i-i-s-async-open-xml-reports-sample-spare-part';

const translations = {};
$.extend(true, translations, EmberFlexberryTranslations);

$.extend(true, translations, {
  models: {
    'i-i-s-async-open-xml-reports-sample-brand': IISAsyncOpenXmlReportsSampleBrandModel,
    'i-i-s-async-open-xml-reports-sample-car': IISAsyncOpenXmlReportsSampleCarModel,
    'i-i-s-async-open-xml-reports-sample-producing-country': IISAsyncOpenXmlReportsSampleProducingCountryModel,
    'i-i-s-async-open-xml-reports-sample-spare-part': IISAsyncOpenXmlReportsSampleSparePartModel
  },

  'application-name': 'Async open xml reports sample',

  forms: {
    loading: {
      'spinner-caption': 'Loading stuff, please wait for a moment...'
    },
    index: {
      greeting: 'Welcome to ember-flexberry test stand!'
    },

    application: {
      header: {
        menu: {
          'sitemap-button': {
            title: 'Menu'
          },
          'user-settings-service-checkbox': {
            caption: 'Use service to save user settings'
          },
          'show-menu': {
            caption: 'Show menu'
          },
          'hide-menu': {
            caption: 'Hide menu'
          },
          'language-dropdown': {
            caption: 'Application language',
            placeholder: 'Choose language'
          }
        },
        login: {
          caption: 'Login'
        },
        logout: {
          caption: 'Logout'
        }
      },

      footer: {
        'application-name': 'Async open xml reports sample',
        'application-version': {
          caption: 'Addon version {{version}}',
          title: 'It is version of ember-flexberry addon, which uses in this dummy application ' +
          '(npm version + commit sha). ' +
          'Click to open commit on GitHub.'
        }
      },

      sitemap: {
        'application-name': {
          caption: 'Async open xml reports sample',
          title: 'Async open xml reports sample'
        },
        'application-version': {
          caption: 'Addon version {{version}}',
          title: 'It is version of ember-flexberry addon, which uses in this dummy application ' +
          '(npm version + commit sha). ' +
          'Click to open commit on GitHub.'
        },
        index: {
          caption: 'Home',
          title: ''
        },
        'async-open-xml-reports-sample': {
          caption: 'AsyncOpenXmlReportsSample',
          title: 'AsyncOpenXmlReportsSample',
          'i-i-s-async-open-xml-reports-sample-brand-l': {
            caption: 'Brand',
            title: ''
          },
          'i-i-s-async-open-xml-reports-sample-car-l': {
            caption: 'Car',
            title: ''
          },
          'i-i-s-async-open-xml-reports-sample-producing-country-l': {
            caption: 'Producing country',
            title: ''
          }
        }
      }
    },

    'edit-form': {
      'save-success-message-caption': 'Save operation succeed',
      'save-success-message': 'Object saved',
      'save-error-message-caption': 'Save operation failed',
      'delete-success-message-caption': 'Delete operation succeed',
      'delete-success-message': 'Object deleted',
      'delete-error-message-caption': 'Delete operation failed'
    },
    'i-i-s-async-open-xml-reports-sample-brand-l': IISAsyncOpenXmlReportsSampleBrandLForm,
    'i-i-s-async-open-xml-reports-sample-car-l': IISAsyncOpenXmlReportsSampleCarLForm,
    'i-i-s-async-open-xml-reports-sample-producing-country-l': IISAsyncOpenXmlReportsSampleProducingCountryLForm,
    'i-i-s-async-open-xml-reports-sample-brand-e': IISAsyncOpenXmlReportsSampleBrandEForm,
    'i-i-s-async-open-xml-reports-sample-car-e': IISAsyncOpenXmlReportsSampleCarEForm,
    'i-i-s-async-open-xml-reports-sample-producing-country-e': IISAsyncOpenXmlReportsSampleProducingCountryEForm
  },

});

export default translations;
