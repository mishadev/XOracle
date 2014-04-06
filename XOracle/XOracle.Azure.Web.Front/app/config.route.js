(function () {
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
                    templateUrl: 'app/controllers/home/home.html',
                    title: 'home',
                    settings: {
                        nav: 1,
                        content: '<i class="icon-login"></i> Home'
                    }
                }
            },
            {
                url: '/SignUp',
                config: {
                    templateUrl: 'app/controllers/login/login.html',
                    title: 'login'
                }
            },
            {
                url: '/access_token=:token&token_type=:type&expires_in=:expires&state=:state',
                config: {
                    templateUrl: 'app/controllers/home/home.html',
                    title: 'auth'
                }
            },
            {
                url: '/event/:EventId',
                config: {
                    templateUrl: 'app/controllers/event/event.html',
                    title: 'event'
                }
            },
            {
                url: '/:AccountName',
                config: {
                    templateUrl: 'app/controllers/user/user.html',
                    title: 'user'
                }
            }
        ];
    }
})();