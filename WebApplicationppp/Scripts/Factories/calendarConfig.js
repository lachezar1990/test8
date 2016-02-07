'use strict';

angular
  .module('mwl.calendar')
  .provider('calendarConfig', function () {

      var defaultDateFormats = {
          hour: 'HH:mm',
          day: 'D MMM',
          month: 'MMMM',
          weekDay: 'dddd'
      };

      var defaultTitleFormats = {
          day: 'dddd D MMMM, YYYY',
          week: 'Седмица {week} от {year}',
          month: 'MMMM YYYY',
          year: 'YYYY'
      };

      var i18nStrings = {
          eventsLabel: 'Събития',
          timeLabel: 'Време'
      };

      var configProvider = this;

      configProvider.setDateFormats = function (formats) {
          angular.extend(defaultDateFormats, formats);
          return configProvider;
      };

      configProvider.setTitleFormats = function (formats) {
          angular.extend(defaultTitleFormats, formats);
          return configProvider;
      };

      configProvider.setI18nStrings = function (strings) {
          angular.extend(i18nStrings, strings);
          return configProvider;
      };

      configProvider.$get = function () {
          return {
              dateFormats: defaultDateFormats,
              titleFormats: defaultTitleFormats,
              i18nStrings: i18nStrings
          };
      };

  });