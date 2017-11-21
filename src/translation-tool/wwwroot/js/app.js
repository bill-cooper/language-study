angular.module('app', ['ui.bootstrap'])
.filter('html', function ($sce) {
    return function (input) {
        return $sce.trustAsHtml(input);
    }
})
.controller('formCtrl', function ($scope, $http, $sce, $uibModal, $document) {


    var $ctrl = this;
    $ctrl.modalbody = "<div>test</div>";

    $ctrl.animationsEnabled = true;

    $ctrl.open = function (size, parentSelector) {
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
                body: function () {
                    return $ctrl.modalbody;
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

    $scope.playAudio = function (audio) {

        var url = "https://audio00.forvo.com/audios/mp3/"
        if ($(element).hasClass('active-audio')) {
            $("#jquery_jplayer_1").jPlayer("stop");
            $('.audio-overlay').removeClass('active-audio');
        } else {

            var url = $(element).find('a').attr('href');
            $('.audio-overlay').removeClass('active-audio');
            $(element).addClass('active-audio');
            $("#jquery_jplayer_1").jPlayer("setMedia", {
                mp3: url
            }).jPlayer("play");
        }
        return false;

    }


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
.controller('ModalInstanceCtrl', function ($uibModalInstance, body) {
    var $ctrl = this;
    $ctrl.modalbody = body;


    $ctrl.ok = function () {
        $uibModalInstance.close($ctrl.selected.item);
    };

    $ctrl.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})

 .run(function ($rootScope) {
    $rootScope.$on('$stateChangeSuccess', function (event, args) {
    });
 });



