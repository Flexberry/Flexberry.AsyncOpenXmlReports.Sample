import ListFormController from 'ember-flexberry/controllers/list-form';
import $ from 'jquery';
import config from '../config/environment';
import { inject as service } from '@ember/service';

export default ListFormController.extend({
  /**
    Name of related edit form route.

    @property editFormRoute
    @type String
    @default 'i-i-s-async-open-xml-reports-sample-car-e'
   */
  editFormRoute: 'i-i-s-async-open-xml-reports-sample-car-e',

  appState: service(),

  actions: {
    BuildCarListReport() {
      let authToken = this.get('keycloakSession.token');
      let appState = this.get('appState');
      appState.loading();

      $.ajax({
        headers: {
          Authorization: `Bearer ${authToken}`
        },
        async: true,
        cache: false,
        type: 'GET',
        url: `${config.APP.backendUrls.root}/api/CarListReport/Build`,
        dataType: 'blob',
        success(response) {
          let link = document.createElement('a');
          link.href = window.URL.createObjectURL(response);
          link.download = "CarListReport.docx";
          link.style.display = 'none';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        },
        error(response) {
          console.log(response);
        },
        complete() {
          appState.reset();
        }
      });
    },

    DownloadCarListTemplate() {
      let authToken = this.get('keycloakSession.token');
      let appState = this.get('appState');
      appState.loading();

      $.ajax({
        headers: {
          Authorization: `Bearer ${authToken}`
        },
        async: true,
        cache: false,
        type: 'GET',
        url: `${config.APP.backendUrls.root}/api/CarListReport/DownloadTemplate`,
        dataType: 'blob',
        success(response) {
          let link = document.createElement('a');
          link.href = window.URL.createObjectURL(response);
          link.download = "CarListTemplate.docx";
          link.style.display = 'none';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        },
        error(response) {
          console.log(response);
        },
        complete() {
          appState.reset();
        }
      });
    },

    UploadCarListTemplate() {
      let input = document.getElementById('file-upload');
      input.click();
    },

    SendCarListTemplateToServer() {
      const inputFile = document.getElementById("file-upload");
      let file = inputFile.files[0];
      var formData = new FormData();
      formData.append("file", file);

      if (!file) {
        alert('Не выбран файл');
        return;
      }

      let authToken = this.get('keycloakSession.token');
      let appState = this.get('appState');
      appState.loading();

      $.ajax({
        headers: {
          Authorization: `Bearer ${authToken}`
        },
        async: true,
        cache: false,
        type: 'POST',
        url: `${config.APP.backendUrls.root}/api/CarListReport/`,
        dataType: 'blob',
        data: formData,
        contentType: false,
        processData: false,
        success(response) {
          alert('Шаблон успешно загружен');
          inputFile.value='';
        },
        error(response) {
          console.log(response);
        },
        complete() {
          appState.reset();
        }
      });
    }
  }
});
