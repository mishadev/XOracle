(function () {
    'use strict';

    var serviceId = 'datacontext',
        /*Account*/
        logoutUrl = '/api/Account/Logout',
        loginUrl = '/Token',
        registerUrl = '/api/Account/Register',
        registerExternalUrl = '/api/Account/RegisterExternal',
        externalLoginsUrl = '/api/Account/ExternalLogins',
        manageInfoUrl = '/api/Account/ManageInfo',
        userInfoUrl = '/api/Account/UserInfo',
        /*Event*/
        getEventsUrl = '/api/event/GetEvents',
        createEventUrl = '/api/event/CreateEvent',
        getEventRelationTypesUrl = '/api/event/EventRelationTypes',
        /*Commmon*/
        getCurrencyTypeUrl = '/api/common/CurrencyTypes',
        getAlgorithmTypeUrl = '/api/common/AlgorithmTypes';

    angular.module('app').factory(serviceId, ['common', datacontext]);

    function datacontext(common) {
        var $q = common.$q;
        var $http = common.$http;

        var service = {
            /*Account*/
            Logout: logout,
            GetExternalLogins: getExternalLogins,
            GetUserInfo: getUserInfo,
            /*Event*/
            GetEvents: getEvents,
            CreateEvent: createEvent,
            GetEventRelationTypes: getEventRelationTypes,
            /*Commmon*/
            GetCurrencyType: getCurrencyType,
            GetAlgorithmType: getAlgorithmType
        };

        return service;

        /*Account*/
        function logout()
        {
            var deferred = $q.defer();

            deferred.notify('Log Out');

            $http({
                method: 'POST',
                url: logoutUrl,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                common.ClearAccessToken();
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }      

        function getExternalLogins(returnUrl, generateState)
        {
            var deferred = $q.defer();

            deferred.notify('External Logins');

            $http({
                method: 'GET',
                url: externalLoginsUrl,
                params: { returnUrl: returnUrl, generateState: generateState ? "true" : "false" }
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }

        function getUserInfo()
        {
            var deferred = $q.defer();

            deferred.notify('Get User Info');

            $http({
                method: 'GET',
                url: userInfoUrl,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }

        /*Event*/
        function getEvents()
        {
            var deferred = $q.defer();

            deferred.notify('Get Events Url');

            $http({
                method: 'GET',
                url: getEventsUrl,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }

        function createEvent(data)
        {
            var deferred = $q.defer();

            deferred.notify('Create Event');

            $http({
                method: 'POST',
                url: createEventUrl,
                data: data,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }

        function getEventRelationTypes()
        {
            var deferred = $q.defer();

            deferred.notify('Get Event Relation Types');

            $http({
                method: 'GET',
                url: getEventRelationTypesUrl,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }

        /*Common*/
        function getCurrencyType() {
            var deferred = $q.defer();

            deferred.notify('Get Currency Type');

            $http({
                method: 'GET',
                url: getCurrencyTypeUrl,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }

        function getAlgorithmType() {
            var deferred = $q.defer();

            deferred.notify('Get Algorithm Type');

            $http({
                method: 'GET',
                url: getAlgorithmTypeUrl,
                headers: common.GetSecurityHeaders()
            })
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            return deferred.promise;
        }
    }
})();