angular.module('PopupDemo').controller('PopupDemoCont', ['$scope', '$modal', function ($scope, $modal) {
    $scope.open = function () {
        console.log('opening pop up');
        var modalInstance = $modal.open({
            templateUrl: 'popup.html',
        });
    }
}]);