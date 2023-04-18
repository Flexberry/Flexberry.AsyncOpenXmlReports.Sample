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

  keycloakSession: service(),

  actions: {
    BuildCarListReport() {
      const authToken = this.get('keycloakSession.token');

      $.ajax({
        headers: {
          Authorization: `Bearer ${authToken}`
        },
        async: true,
        cache: false,
        type: 'GET',
        url: `${config.APP.backendUrls.root}/api/CarListReport/Build`,
        dataType: 'json',
      });
    },

    DownloadCarListTemplate() {
      const authToken = this.get('keycloakSession.token');
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
          const link = document.createElement('a');
          link.href = window.URL.createObjectURL(response);
          link.download = "CarListTemplate.docx";
          link.style.display = 'none';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        },
        complete() {
          appState.reset();
        }
      });
    },

    UploadCarListTemplate() {
      const input = document.getElementById('file-upload');
      input.click();
    },

    SendCarListTemplateToServer() {
      const inputFile = document.getElementById("file-upload");
      const file = inputFile.files[0];
      var formData = new FormData();
      formData.append("file", file);

      if (!file) {
        alert('Не выбран файл');
        return;
      }

      const authToken = this.get('keycloakSession.token');
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
        success() {
          alert('Шаблон успешно загружен');
        },
        complete() {
          appState.reset();
          inputFile.value='';
        }
      });
    }
  }
});
