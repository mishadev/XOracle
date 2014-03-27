﻿(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());

    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {
        routes.forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/controllers/events/events.html',
                    title: 'events',
                    settings: {
                        nav: 1,
                        content: '<i class="icon-login"></i> Events'
                    }
                }
            },
            {
                url: '/SignUp',
                config: {
                    templateUrl: 'app/controllers/login/login.html',
                    title: 'login'
                }
            }
        ];
    }
})();