var ShopsDataApp = angular.module('ShopsDataApp', ['ngRoute']);

ShopsDataApp.controller('LandingPageController', LandingPageController);
ShopsDataApp.controller('CurrentDataController', CurrentDataController);
ShopsDataApp.factory('CurrentDataService', CurrentDataService);
ShopsDataApp.factory('GeneralDataService', GeneralDataService);

var configFunction = function ($routeProvider, $locationProvider) {
    /* todo: hashPefix? */
    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/CurrentData/:locationId/:productTypeId', {
            templateUrl: function (params) { return 'Home/CurrentData/' + params.locationId + '/' + params.productTypeId },
            controller: 'CurrentDataController'
        });
}
configFunction.$inject = ['$routeProvider', '$locationProvider'];

ShopsDataApp.config(configFunction);