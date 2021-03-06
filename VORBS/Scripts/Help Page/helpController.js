﻿(function () {
    angular.module('vorbs.supportPage')
        .controller('HelpController', HelpController);


    HelpController.$inject = ['$scope', '$http', '$resource'];

    function HelpController($scope, $http, $resource) {
        CreateHelpServices($resource);

        $scope.locations = Locations.query({ status: true, extraInfo: true },
            function (success) {
                $scope.currentLocation = $scope.locations[0];
            });

    }

    function CreateHelpServices($resource) {
        Locations = $resource('/api/locations/:status/:extraInfo', { status: 'active', extraInfo: 'extraInfo' }, {
            query: { method: 'GET', isArray: true }
        });

        Locations.prototype =
            {
                GetContactDetail: function (name) {
                    for (var i = 0; i < this.locationCredentials.length; i++) {
                        if (this.locationCredentials[i].department == name) {
                            return this.locationCredentials[i];
                        }
                    }
                }
            };
    }

})();