'use strict';

module.exports = function(environment) {
  // Replace this local address to remote when backed will be published.
  var backendUrl = 'http://localhost:80';
  var authorityUrl = 'http://localhost:8080';

  if (environment === 'development-loc') {
    // Use `ember s -e development-loc` command for local backend usage.
    backendUrl = 'http://localhost:6500';
  }
  
  if (environment === 'docker') {
    // Use `ember s -e development-loc` command for local backend usage.
    backendUrl = 'http://asyncopenxmlreportssample:80';
    authorityUrl = 'http://asyncopenxmlreportssample:8080';
  }

  let ENV = {
    modulePrefix: 'ember-app',
    environment,
    rootURL: '/',
    locationType: 'auto',
    EmberENV: {
      LOG_STACKTRACE_ON_DEPRECATION: false,

      FEATURES: {
        // Here you can enable experimental features on an ember canary build
        // e.g. 'with-controller': true
      },
      EXTEND_PROTOTYPES: {
        // Prevent Ember Data from overriding Date.parse.
        Date: false
      }
    },
    KeycloakAuthSettings: {
      ClientOptions: {
        url: authorityUrl,
        realm: 'master',
        clientId: 'ember-app'
      },
      checkLoginIframe: false, // Можно включить если будет https и авторизация на одном поддомене.
      responseMode: 'query', // Для работоспособности редиректа после логина.
    },

    APP: {
      // Application name. Used in `user-settings` service.
      name: 'ember-app',

      backendUrl: backendUrl,

      // It's a custom property, used to prevent duplicate backend urls in sources.
      backendUrls: {
        root: backendUrl,
        api: backendUrl + '/odata'
      },

      // Log service settings.
      log: {
        // Flag: indicates whether log service is enabled or not.
        enabled: true,

        // Flag: indicates whether to store error messages or not.
        storeErrorMessages: true,
        storeWarnMessages: false,
        storeLogMessages: true,
        storeInfoMessages: false,
        storeDebugMessages: false,
        storeDeprecationMessages: false,
        storePromiseErrors: true,
        showPromiseErrors: true,
        errorMessageFilterActive: false,
      },

      // Options for Perforator service that can be used to calculate performance of components rendering.
      perf: {
        enabled: false,
      },

      // Lock settings.
     lock: {
        enabled: true,
        openReadOnly: true,
        unlockObject: true,
      },

      // Flag: indicates whether to use user settings service or not.
      useUserSettingsService: true,

      // Flag: indicates whether to use adv limit service or not.
      useAdvLimitService: true,

      // Custom property with offline mode settings.
      offline: {
        // Flag that indicates whether offline mode in application is enabled or not.
        offlineEnabled: true,

        // Flag that indicates whether to switch to offline mode when got online connection errors or not.
        modeSwitchOnErrorsEnabled: false,

        // Flag that indicates whether to sync down all work with records when online or not.
        // This let user to continue work without online connection.
        syncDownWhenOnlineEnabled: false,
      },

      // Custom property with components settings.
      components: {
        // Settings for `flexberry-file` component.
        flexberryFile: {
          // URL of file upload controller.
          uploadUrl: backendUrl + '/api/File',

          // Max file size in bytes for uploading files.
          maxUploadFileSize: null,

          // Flag: indicates whether to upload file on controllers modelPreSave event.
          uploadOnModelPreSave: true,

          // Flag: indicates whether to show upload button or not.
          showUploadButton: true,

          // Flag: indicates whether to show modal dialog on upload errors or not.
          showModalDialogOnUploadError: true,

          // Flag: indicates whether to show modal dialog on download errors or not.
          showModalDialogOnDownloadError: true,
        },

        // For guideline theme
        // Settings for flexberry-objectlistview component.
        flexberryObjectlistview: {
          // Flag indicates whether to side page or usually mode.
          useSidePageMode: true,
        },

        // Settings for flexberry-lookup component.
        flexberryLookup: {
          // Flag: indicates whether to side page or usually mode.
          useSidePageMode: true,
        },

        flexberryGroupedit: {
          // Flag: indicates whether to side page or usually mode.
          useSidePageMode: true,
          // Flag: indicates whether to show asterisk icon in first column of every changed row.
          showAsteriskInRow: false,
        },

        flexberrySimpledatetime: {
          // The selector to get the element (using `jQuery`) for the `appendTo` flatpickr option, see https://flatpickr.js.org/options/.
          calendarContext: undefined
        }
,
        // Settings for `flexberryObjectlistview` component.
        flexberryObjectlistview: {
          // Default number of records on the list page.
          defaultPerPage: 5,
          // Flag indicates whether to use side page or usual mode.
          useSidePageMode: true,
        },
        // Settings for flexberry-lookup component.
        flexberryLookup: {
          // Flag: indicates whether to use side page or usual mode.
          useSidePageMode: true,
        },
        flexberryGroupedit: {
          // Flag: indicates whether to use side page or usual mode.
          useSidePageMode: true,
        },
        flexberrySimpledatetime: {
          // The selector to get the element (using `jQuery`) for the `appendTo` flatpickr option, see https://flatpickr.js.org/options/.
          calendarContext: undefined,
        }
      },

      // Here you can pass flags/options to your application instance
      // when it is created
    }
  };

  // Read more about CSP:
  // http://www.ember-cli.com/#content-security-policy
  // https://github.com/rwjblue/ember-cli-content-security-policy
  // http://content-security-policy.com
  ENV.contentSecurityPolicy = {
    'style-src': "'self' 'unsafe-inline' https://fonts.googleapis.com",
    'font-src': "'self' data: https://fonts.gstatic.com",
    'connect-src': "'self' " + ENV.APP.backendUrls.root
  };

  // Read more about ember-i18n: https://github.com/jamesarosen/ember-i18n.
  ENV.i18n = {
    // Should be defined to avoid ember-i18n deprecations.
    // Locale will be changed then to navigator current locale (in instance initializer).
    defaultLocale: null
  };

  // Read more about ember-moment: https://github.com/stefanpenner/ember-moment.
  // Locale will be changed then to same as ember-i18n locale (and will be changed every time when i18n locale changes).
  ENV.moment = {
    outputFormat: 'L'
  };


  if (environment === 'development') {
    // ENV.APP.LOG_RESOLVER = true;
    // ENV.APP.LOG_ACTIVE_GENERATION = true;
    // ENV.APP.LOG_TRANSITIONS = true;
    // ENV.APP.LOG_TRANSITIONS_INTERNAL = true;
    // ENV.APP.LOG_VIEW_LOOKUPS = true;
  }

  if (environment === 'test') {
    // Testem prefers this...
    ENV.locationType = 'none';

    // keep test console output quieter
    ENV.APP.LOG_ACTIVE_GENERATION = false;
    ENV.APP.LOG_VIEW_LOOKUPS = false;

    ENV.APP.rootElement = '#ember-testing';
    ENV.APP.autoboot = false;
  }

  if (environment === 'production') {
    // here you can enable a production-specific feature
  }

  return ENV;
};
