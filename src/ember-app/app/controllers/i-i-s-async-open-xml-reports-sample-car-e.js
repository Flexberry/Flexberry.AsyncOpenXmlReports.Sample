import EditFormController from 'ember-flexberry/controllers/edit-form';

export default EditFormController.extend({
  parentRoute: 'i-i-s-async-open-xml-reports-sample-car-l',

  getCellComponent(attr, bindingPath, model) {
    let cellComponent = this._super(...arguments);
    if (attr.kind === 'belongsTo') {
      switch (`${model.modelName}+${bindingPath}`) {
        case 'i-i-s-async-open-xml-reports-sample-spare-part+producingCountry':
          cellComponent.componentProperties = {
            choose: 'showLookupDialog',
            remove: 'removeLookupValue',
            displayAttributeName: 'name',
            required: false,
            relationName: 'producingCountry',
            projection: 'ProducingCountryL',
            autocomplete: true,
          };
          break;

      }
    }

    return cellComponent;
  },
});
