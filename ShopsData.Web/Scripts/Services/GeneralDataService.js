var GeneralDataService = function ($http) {
    return {
        getDataSources: function () {
            return $http({
                method: 'GET',
                url: 'api/dataSource'
            });
        },
        getProductTypes: function () {
            return $http({
                method: 'GET',
                url: 'api/productType'
            });
        },
        getLocations: function () {
            return $http({
                method: 'GET',
                url: 'api/location'
            });
        }
    };
}

GeneralDataService.$inject = ['$http'];