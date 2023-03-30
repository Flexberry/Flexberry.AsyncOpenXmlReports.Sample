import $ from 'jquery';
import EmberFlexberryTranslations from 'ember-flexberry/locales/ru/translations';

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
      'spinner-caption': 'Данные загружаются, пожалуйста подождите...'
    },
    index: {
      greeting: 'Добро пожаловать на тестовый стенд ember-flexberry!'
    },

    application: {
      header: {
        menu: {
          'sitemap-button': {
            title: 'Меню'
          },
          'user-settings-service-checkbox': {
            caption: 'Использовать сервис сохранения пользовательских настроек'
          },
          'show-menu': {
            caption: 'Показать меню'
          },
          'hide-menu': {
            caption: 'Скрыть меню'
          },
          'language-dropdown': {
            caption: 'Язык приложения',
            placeholder: 'Выберите язык'
          }
        },
        login: {
          caption: 'Вход'
        },
        logout: {
          caption: 'Выход'
        }
      },

      footer: {
        'application-name': 'Async open xml reports sample',
        'application-version': {
          caption: 'Версия аддона {{version}}',
          title: 'Это версия аддона ember-flexberry, которая сейчас используется в этом тестовом приложении ' +
          '(версия npm-пакета + хэш коммита). ' +
          'Кликните, чтобы перейти на GitHub.'
        }
      },

      sitemap: {
        'application-name': {
          caption: 'Async open xml reports sample',
          title: 'Async open xml reports sample'
        },
        'application-version': {
          caption: 'Версия аддона {{version}}',
          title: 'Это версия аддона ember-flexberry, которая сейчас используется в этом тестовом приложении ' +
          '(версия npm-пакета + хэш коммита). ' +
          'Кликните, чтобы перейти на GitHub.'
        },
        index: {
          caption: 'Главная',
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
      'save-success-message-caption': 'Сохранение завершилось успешно',
      'save-success-message': 'Объект сохранен',
      'save-error-message-caption': 'Ошибка сохранения',
      'delete-success-message-caption': 'Удаление завершилось успешно',
      'delete-success-message': 'Объект удален',
      'delete-error-message-caption': 'Ошибка удаления'
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
