(function () {
    'use strict';

    var serviceId = 'datacontext',
        getUserInfoUrl = "/api/Account/GetUserInfo",
        logoutUrl = "/api/Account/Logout",
        loginUrl = "/Token",
        registerUrl = "/api/Account/Register",
        registerExternalUrl = "/api/Account/RegisterExternal",
        externalLoginsUrl = "/api/Account/ExternalLogins",
        removeLoginUrl = "/api/Account/RemoveLogin",
        setPasswordUrl = "/api/Account/SetPassword",
        manageInfoUrl = "/api/Account/ManageInfo",
        userInfoUrl = "/api/Account/UserInfo";

    angular.module('app').factory(serviceId, ['common', datacontext]);

    // Route operations
    function externalLoginsUrlParams(returnUrl, generateState) {
        return { returnUrl: returnUrl, generateState: generateState ? "true" : "false" };
    }

    function manageInfoUrlParams(returnUrl, generateState) {
        return { returnUrl: returnUrl, generateState: generateState ? "true" : "false" };
    }

    // Other private operations
    function getSecurityHeaders() {
        var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }

        return {};
    }

    // Operations
    function clearAccessToken() {
        localStorage.removeItem("accessToken");
        sessionStorage.removeItem("accessToken");
    };

    function setAccessToken(accessToken, persistent) {
        if (persistent) {
            localStorage["accessToken"] = accessToken;
        } else {
            sessionStorage["accessToken"] = accessToken;
        }
    };

    function datacontext(common) {
        var $q = common.$q;
        var $http = common.$http;

        var service = {
            GetUserInfo: getUserInfo,
            Logout: logout,
            Login: login,
            Register: register,
            RegisterExternal: registerExternal,
            RemoveLogin: removeLogin,
            SetPassword: setPassword,
            UserInfo: userInfo,
            SetAccessToken: setAccessToken,
            ClearAccessToken: clearAccessToken,
            ExternalLoginsUrl: externalLogins,
        };

        return service;

        function getUserInfo() {
            var deferred = $q.defer();

            deferred.notify('Get User Info');

            scope.$apply(function () {
                $http({
                    method: 'GET',
                    url: getUserInfoUrl,
                    headers: getSecurityHeaders()
                })
                .success(function (data, status, headers, config) {
                    deferred.resolve(data);
                })
                .error(function (data, status, headers, config) {
                    deferred.reject('Greeting ' + name + ' is not allowed.');
                });
            });

            return deferred.promise;
        }

        function logout()
        {
            var deferred = $q.defer();

            deferred.notify('Log Out');

            return deferred.promise;
        }

        function login(params)
        {
            debugger
            var deferred = $q.defer();

            deferred.notify('Log In');

            $http({
                method: 'GET',
                url: loginUrl,
                params: params
            })
            .success(function (data, status, headers, config) {
                debugger

                deferred.resolve('Hello, ' + name + '!');
            })
            .error(function (data, status, headers, config) {
                debugger

                deferred.reject('Greeting ' + name + ' is not allowed.');
            });

            return deferred.promise;
        }

        function register()
        {
            var deferred = $q.defer();

            deferred.notify('Register');

            return deferred.promise;
        }

        function registerExternal()
        {
            var deferred = $q.defer();

            deferred.notify('Register');

            return deferred.promise;
        }

        function removeLogin()
        {
            var deferred = $q.defer();

            deferred.notify('Remove Login');

            return deferred.promise;
        }

        function setPassword()
        {
            var deferred = $q.defer();

            deferred.notify('Set Password');

            return deferred.promise;
        }

        function userInfo()
        {
            var deferred = $q.defer();

            deferred.notify('User Info');

            return deferred.promise;
        }

        function externalLogins(returnUrl, generateState)
        {
            var deferred = $q.defer();

            deferred.notify('External Logins');

            $http({
                method: 'GET',
                url: externalLoginsUrl,
                params: externalLoginsUrlParams(returnUrl, generateState)
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