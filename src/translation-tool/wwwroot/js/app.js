angular.module('app', ['ui.bootstrap'])
.controller('formCtrl', function ($scope, $http, $sce) {

    $scope.dynamicPopover = {
        content: '',
        audios: [],
        templateUrl: 'popup.html'
    };

    $scope.originText = "";
    $scope.words = [];

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


    $scope.processWords = function (word) {

        $http({
            url: "/api/words/translation",
            method: "POST",
            data: { 'input': $scope.originText }
        }).then(function (response) {
                $scope.words = response.data;
        });

    };
})
.controller('wordPopupCtrl', function ($scope, $http, $sce) {



    $scope.wordPopover = {
        content: 'Hello World!',
        word: 'The Word'
    };

 
})
    .controller('popupCtrl', function ($scope, $http, $sce) {


        $scope.dynamicPopover = {
            content: 'Hello, World!',
            templateUrl: 'myPopoverTemplate.html',
            title: 'Title'
        };


    })

 .run(function ($rootScope) {
    $rootScope.$on('$stateChangeSuccess', function (event, args) {
    });
 });



