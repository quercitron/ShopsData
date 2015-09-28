var CurrentDataService = function ($http) {
    return {
        getDataSummary: function (locationId, productTypeId) {
            return $http({
                method: 'GET',
                url: 'api/currentData/' + locationId + '/' + productTypeId
            });
        },
        getDataSummaryGrouped: function (locationId, productTypeId) {
            return $http({
                method: 'GET',
                url: 'api/currentDataGrouped/' + locationId + '/' + productTypeId
            });
        }
    };
}

CurrentDataService.$inject = ['$http'];