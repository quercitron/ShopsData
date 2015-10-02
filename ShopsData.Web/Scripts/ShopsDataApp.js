var ShopsDataApp = angular.module('ShopsDataApp', ['ngRoute']);

ShopsDataApp.controller('LandingPageController', LandingPageController);
ShopsDataApp.controller('CurrentDataController', CurrentDataController);
ShopsDataApp.controller('ProductDetailsController', ProductDetailsController);
ShopsDataApp.factory('CurrentDataService', CurrentDataService);
ShopsDataApp.factory('GeneralDataService', GeneralDataService);
ShopsDataApp.factory('ProductDetailsService', ProductDetailsService);

var configFunction = function ($routeProvider, $locationProvider) {
    /* todo: hashPefix? */
    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/CurrentData/:locationId/:productTypeId', {
            templateUrl: function (params) { return 'Home/CurrentData/' + params.locationId + '/' + params.productTypeId },
            controller: 'CurrentDataController'
        }).
        when('/ProductDetails/:locationId/:productId', {
            templateUrl: function (params) { return 'Home/ProductDetails/' + params.locationId + '/' + params.productId },
            controller: 'ProductDetailsController'
        });
}
configFunction.$inject = ['$routeProvider', '$locationProvider'];

ShopsDataApp.config(configFunction);