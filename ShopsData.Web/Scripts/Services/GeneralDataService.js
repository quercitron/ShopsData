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
        }
    };
}

GeneralDataService.$inject = ['$http'];