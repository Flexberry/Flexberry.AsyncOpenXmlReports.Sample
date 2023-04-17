import FlexberryEnum from 'ember-flexberry-data/transforms/flexberry-enum';
import ReportStatusTypeEnum from '../enums/i-i-s-async-open-xml-reports-sample-report-status-type';

export default FlexberryEnum.extend({
  enum: ReportStatusTypeEnum,
  sourceType: 'IIS.AsyncOpenXmlReportsSample.ReportStatusType'
});
