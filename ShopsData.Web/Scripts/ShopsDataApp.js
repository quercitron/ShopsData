var ShopsDataApp = angular.module('ShopsDataApp', ['ngRoute']);

ShopsDataApp.controller('LandingPageController', LandingPageController);
ShopsDataApp.controller('CurrentDataController', CurrentDataController);
ShopsDataApp.factory('CurrentDataService', CurrentDataService);

var configFunction = function ($routeProvider) {
    $routeProvider.
        when('home/currentData/:locationId/:productTypeId', {
            templateUrl: function (params) { return '/home/currentData/' + params.locationId + '/' + params.productTypeId },
            controller: 'CurrentDataController'
        });
}
configFunction.$inject = ['$routeProvider'];

ShopsDataApp.config(configFunction);