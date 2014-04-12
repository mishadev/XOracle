(function () {
    'use strict';

    // Define the common module 
    // Contains services:
    //  - common
    //  - logger
    //  - spinner
    var commonModule = angular.module('common', []);

    // Must configure the common service and set its 
    // events via the commonConfigProvider
    commonModule.provider('commonConfig', function () {
        this.config = {
            // These are the properties we need to set
            //controllerActivateSuccessEvent: '',
            //spinnerToggleEvent: ''
        };

        this.$get = function () {
            return {
                config: this.config
            };
        };
    });

    commonModule.factory('common',
        ['$q', '$rootScope', '$timeout', '$http', '$location', '$routeParams', 'commonConfig', 'logger', common]);

    function common($q, $rootScope, $timeout, $http, $location, $routeParams, commonConfig, logger) {
        var throttles = {},
            allowAnonymous = ['login', 'shell', 'sidebar'],

            service = {
                // common angular dependencies
                $broadcast: $broadcast,
                $q: $q,
                $timeout: $timeout,
                $http: $http,
                $location: $location,
                $routeParams: $routeParams,
                // generic
                activateController: activateController,
                createSearchThrottle: createSearchThrottle,
                debouncedThrottle: debouncedThrottle,
                isNumber: isNumber,
                logger: logger, // for accessibility
                textContains: textContains,
                archiveSessionStorageToLocalStorage: archiveSessionStorageToLocalStorage,
                restoreSessionStorageFromLocalStorage: restoreSessionStorageFromLocalStorage,
                getPlural: getPlural,
                /*Auth*/
                //SetAccessToken: setAccessToken,
                ClearAccessToken: clearAccessToken,
                GetSecurityHeaders: getSecurityHeaders,
                IsLogedIn: isLogedIn
                //ParseQueryString: parseQueryString,
                //EnsuteUserAuthenticated: ensuteUserAuthenticated
            };

        $rootScope.$on('$routeChangeStart',
            function (event, next, current) {
                ensuteUserAuthenticated(next.$$route);
            }
        );

        return service;

        function activateController(promises, controllerId) {
            return $q.all(promises).then(function (eventArgs) {
                var data = { controllerId: controllerId };
                $broadcast(commonConfig.config.controllerActivateSuccessEvent, data);
            });
            return $q.defer().promise;
        }

        function $broadcast() {
            return $rootScope.$broadcast.apply($rootScope, arguments);
        }

        function createSearchThrottle(viewmodel, list, filteredList, filter, delay) {
            // custom delay or use default
            delay = +delay || 300;
            // if only vm and list parameters were passed, set others by naming convention 
            if (!filteredList) {
                // assuming list is named sessions,
                // filteredList is filteredSessions
                filteredList = 'filtered' + list[0].toUpperCase() + list.substr(1).toLowerCase(); // string
                // filter function is named sessionFilter
                filter = list + 'Filter'; // function in string form
            }

            // create the filtering function we will call from here
            var filterFn = function () {
                // translates to ...
                // vm.filteredSessions 
                //      = vm.sessions.filter(function(item( { returns vm.sessionFilter (item) } );
                viewmodel[filteredList] = viewmodel[list].filter(function(item) {
                    return viewmodel[filter](item);
                });
            };

            return (function () {
                // Wrapped in outer IFFE so we can use closure 
                // over filterInputTimeout which references the timeout
                var filterInputTimeout;

                // return what becomes the 'applyFilter' function in the controller
                return function(searchNow) {
                    if (filterInputTimeout) {
                        $timeout.cancel(filterInputTimeout);
                        filterInputTimeout = null;
                    }
                    if (searchNow || !delay) {
                        filterFn();
                    } else {
                        filterInputTimeout = $timeout(filterFn, delay);
                    }
                };
            })();
        }

        function debouncedThrottle(key, callback, delay, immediate) {
            var defaultDelay = 1000;
            delay = delay || defaultDelay;
            if (throttles[key]) {
                $timeout.cancel(throttles[key]);
                throttles[key] = undefined;
            }
            if (immediate) {
                callback();
            } else {
                throttles[key] = $timeout(callback, delay);
            }
        }

        function isNumber(val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        }

        function textContains(text, searchText) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }

        // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage temporarily
        // to localStorage to work around this problem.
        function archiveSessionStorageToLocalStorage() {
            var backup = {};

            for (var i = 0; i < sessionStorage.length; i++) {
                backup[sessionStorage.key(i)] = sessionStorage[sessionStorage.key(i)];
            }

            localStorage["sessionStorageBackup"] = JSON.stringify(backup);
            sessionStorage.clear();
        };

        // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage temporarily
        // to localStorage to work around this problem.
        function restoreSessionStorageFromLocalStorage() {
            var backupText = localStorage["sessionStorageBackup"],
                backup;

            if (backupText) {
                backup = JSON.parse(backupText);

                for (var key in backup) {
                    sessionStorage[key] = backup[key];
                }

                localStorage.removeItem("sessionStorageBackup");
            }
        };

        // Other private operations
        function getSecurityHeaders() {
            var accessToken = getSecurityAccessToken()

            if (accessToken) {
                return { "Authorization": "Bearer " + accessToken };
            }

            return {};
        }

        function getSecurityAccessToken()
        {
            return sessionStorage["accessToken"] || localStorage["accessToken"];
        }

        // Operations
        function clearAccessToken() {
            localStorage.removeItem("accessToken");
            sessionStorage.removeItem("accessToken");
        }

        function setAccessToken(accessToken, persistent) {
            if (persistent) {
                localStorage["accessToken"] = accessToken;
            } else {
                sessionStorage["accessToken"] = accessToken;
            }
        }

        function ensuteUserAuthenticated(route) {
            if (!isLogedIn()) {
                if (route && route.title === 'auth') {
                    var token = parseQueryString($location.path().substr(1));
                    validateToken(token);
                    verifyStateMatch(token);

                    if (typeof (token.error) !== "undefined") {
                        logger.getLogFn('app', 'error')(token.error);
                    } else if (typeof (token.access_token) !== "undefined") {
                        setAccessToken(token.access_token);

                        $location.path('/');
                    }
                } else if (route && route.title !== 'login') {
                    $location.path('SignUp').replace();
                }
            }
        }

        function isLogedIn() {
            return !!getSecurityAccessToken();
        }

        function validateToken(token) {
            if (typeof (token.access_token) === "undefined" || typeof (token.token_type) === "undefined")
                token.error = "invalid_token";
        }

        function verifyStateMatch(token) {
            if (typeof (token.access_token) !== "undefined") {
                var state = sessionStorage["state"];
                sessionStorage.removeItem("state");

                if (state === null || token.state !== state)
                    token.error = "invalid_state";
            }
        }

        function parseQueryString(queryString) {
            var data = {},
                pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

            if (queryString === null) {
                return data;
            }

            pairs = queryString.split("&");

            for (var i = 0; i < pairs.length; i++) {
                pair = pairs[i];
                separatorIndex = pair.indexOf("=");

                if (separatorIndex === -1) {
                    escapedKey = pair;
                    escapedValue = null;
                } else {
                    escapedKey = pair.substr(0, separatorIndex);
                    escapedValue = pair.substr(separatorIndex + 1);
                }

                key = decodeURIComponent(escapedKey);
                value = decodeURIComponent(escapedValue);

                data[key] = value;
            }

            return data;
        }

        function getPlural(count, singular, plural) {
            if (count) return singular;
            if (plural === 'undefined') plural = singular + "s";

            return count > 1 ? plural : singular;
        }
    }
})();