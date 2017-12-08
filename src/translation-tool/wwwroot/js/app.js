angular.module('app', ['ui.bootstrap'])
.filter('html', function ($sce) {
    return function (input) {
        return $sce.trustAsHtml(input);
    }
})
.directive('elemReady', function ($parse) {
        return {
            restrict: 'A',
            link: function ($scope, elem, attrs) {
                elem.ready(function () {
                    $scope.$apply(function () {
                        var func = $parse(attrs.elemReady);
                        func($scope);
                    })
                })
            }
        }
})
.controller('formCtrl', function ($scope, $http, $sce, $uibModal, $document) {


    var $ctrl = this;

    $ctrl.animationsEnabled = true;



    $ctrl.openModal = function (word, derivate, size, parentSelector) {
        var parentElem = parentSelector ?
          angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
        var modalInstance = $uibModal.open({
            animation: $ctrl.animationsEnabled,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'modal.html',
            controller: 'ModalInstanceCtrl',
            controllerAs: '$ctrl',
            size: size,
            appendTo: parentElem,
            resolve: {
                word: function () {
                    return word;
                },
                derivate: function () {
                    return derivate;
                }
            }
        });
    }



    $scope.dynamicPopover = {
        content: '',
        audios: [],
        templateUrl: 'popup.html'
    };

    $scope.originText = "";
    $scope.translation = {};

    $scope.wordPopup = function (word) {

        $scope.dynamicPopover.word = word.value;
        $scope.dynamicPopover.audios = [];

        if (word.audios.length == 0) {

            $http({
                url: "/api/words/audios",
                method: "POST",
                data: { 'input': word.value }
            }).then(function (response) {
                word.audios = response.data;
                $scope.dynamicPopover.audios = word.audios;
            });

        } else {
            $scope.dynamicPopover.audios = word.audios;
        }

        if (word.info == null) {

            $http({
                url: "/api/words/info",
                method: "POST",
                data: { 'input': word.value }
            }).then(function (response) {
                word.info = response.data;
                $scope.dynamicPopover.accentedWord = word.info.accentedWord;
                $scope.dynamicPopover.derivate = word.info.derivate;
                $scope.dynamicPopover.translation = word.info.translation;
            });

        } else {
            $scope.dynamicPopover.accentedWord = word.info.accentedWord;
            $scope.dynamicPopover.derivate = word.info.derivate;
            $scope.dynamicPopover.translation = word.info.translation;
        }

        

    };


    $scope.processInput = function () {

        $http({
            url: "/api/words/translation",
            method: "POST",
            data: { 'input': $scope.originText }
        }).then(function (response) {
                $scope.translation = response.data;
        });

    };
})
.controller('ModalInstanceCtrl', function ($scope, $http, $uibModalInstance, word, derivate) {
    var $ctrl = this;

    $scope.modaltitle = derivate;
    $scope.modalbody = "";

    $http({
        url: "/api/words/detail",
        method: "POST",
        data: { 'input': derivate }
    }).then(function (response) {
        $scope.modalbody = response.data;
        setTimeout(function () {
            $('.word').find('table').addClass("table").addClass("table-bordered");
            $('.word').find('a').each(function()
            { 
                this.href = this.href.replace(/^\/ru\//,
                   "https://en.openrussian.org/ru/");
            });
        }, 100);
    });

    $ctrl.writeLog = function () {
        console.log('triggered');
    };


    $ctrl.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };






})

 .run(function ($rootScope) {
     $rootScope.$on('viewContentLoaded', function (event, args) {
    });
 });

angular.element(function () {
    console.log('page loading completed');
});



