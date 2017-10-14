angular.module('app', ['ea.treeview'])
    .controller('logCategoryController', function ($scope, $http, eaTreeViewFactory) {
        $scope.showCounts = false;

        $http.get(window.location.href + "/api/category")
        .then(function (response) {

                $scope.model = {
                    categories: response.data
                };
                eaTreeViewFactory.setItems($scope.model.categories, $scope.$id);
            });

        $scope.show = function(item) {
            $scope.model.selectedCategory = item.display;
            $scope.model.selectedCategoryId = item.stateName;
        };

        $scope.getCounts = function (categoryId) {
            if (categoryId === undefined) {
                alert("Please select a category first.");
            } else {
                $http.get(window.location.href + "/api/category/getcounts",
                        {
                            params: { categoryId: categoryId }
                        })
                    .then(function(response) {
                        $scope.model.totalCount = response.data.totalCount;
                        $scope.model.totalNoKBA = response.data.totalWithNoKBA;
                        $scope.model.percentNoKBA = response.data.percentWithNoKBA;
                        $scope.model.totalNoSubscription = response.data.totalWithNoSubscription;
                        $scope.model.percentNoSubscription = response.data.percentWithNoSubscription;
                        $scope.showCounts = true;
                    });
            }
        };
    }).run(function ($rootScope, eaTreeViewFactory) {
    $rootScope.$on('$stateChangeSuccess', function (event, args) {
        eaTreeViewFactory.resetItemTemplateUrl();
    });
});