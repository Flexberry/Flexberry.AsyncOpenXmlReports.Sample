import ListFormRoute from 'ember-flexberry/routes/list-form';
import { computed } from '@ember/object';
import { inject as service } from '@ember/service';
import { SimplePredicate } from 'ember-flexberry-data/query/predicate';
import FilterOperator from 'ember-flexberry-data/query/filter-operator';

export default ListFormRoute.extend({

  keycloakSession: service(),

  userName: computed('keycloakSession.tokenParsed.preferred_username', function() {
    return this.keycloakSession.tokenParsed.preferred_username;
  }),

  /**
    Name of model projection to be used as record's properties limitation.

    @property modelProjection
    @type String
    @default 'UserReportL'
  */
  modelProjection: 'UserReportL',

  /**
    Name of model to be used as list's records types.

    @property modelName
    @type String
    @default 'i-i-s-async-open-xml-reports-sample-user-report'
  */
  modelName: 'i-i-s-async-open-xml-reports-sample-user-report',

  objectListViewLimitPredicate() {
    const userName = this.get('userName');

    return new SimplePredicate('userName', FilterOperator.Eq, userName);
  },

  /**
    Defined user settings developer.
    For default userSetting use empty name ('').
    Property `<componentName>` may contain any of properties: `colsOrder`, `sorting`, `colsWidth` or being empty.

    ```javascript
    {
      <componentName>: {
        <settingName>: {
          colsOrder: [ { propName :<colName>, hide: true|false }, ... ],
          sorting: [{ propName: <colName>, direction: "asc"|"desc" }, ... ],
          colsWidths: [ <colName>:<colWidth>, ... ],
        },
        ...
      },
      ...
    }
    ```

    @property developerUserSettings
    @type Object
  */
  developerUserSettings: computed(function() {
    return { IISAsyncOpenXmlReportsSampleUserReportL: {} }
  }),
});
