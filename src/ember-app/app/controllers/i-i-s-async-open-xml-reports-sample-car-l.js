import ListFormController from 'ember-flexberry/controllers/list-form';
import $ from 'jquery';
import config from '../config/environment';

export default ListFormController.extend({
  /**
    Name of related edit form route.

    @property editFormRoute
    @type String
    @default 'i-i-s-async-open-xml-reports-sample-car-e'
   */
  editFormRoute: 'i-i-s-async-open-xml-reports-sample-car-e',

  actions: {
    PrintCarList() {
      //let appState = this.get('appState');
      //appState.loading();

      $.ajax({
        async: true,
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: `${config.APP.backendUrls.root}/api/reports`,
        dataType: 'blob',
        success(response) {
          let link = document.createElement('a');
          link.href = window.URL.createObjectURL(response);
          link.download = "Car list.docx";
          link.style.display = 'none';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        },
        complete() {
          //appState.reset();
        }
      });
    },
  }
});
