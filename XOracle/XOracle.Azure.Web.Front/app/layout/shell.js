(function () {
    'use strict';

    var controllerId = 'shell';
    angular.module('app').controller(controllerId,
        ['$rootScope', '$route', 'common', 'config', shell]);

    function shell($rootScope, $route, common, config) {
        var vm = this,
            logSuccess = common.logger.getLogFn(controllerId, 'success'),
            events = config.events;

        vm.isBusy = true;

        (function activate() {
            logSuccess('xOracle loaded!', null, true);
            common.activateController([], controllerId);
        })();

        //private

        $rootScope.$on('$routeChangeStart',
            function (event, next, current) { toggleSpinner(true); }
        );

        $rootScope.$on(events.controllerActivateSuccess,
            function (data) { toggleSpinner(false); }
        );

        $rootScope.$on(events.spinnerToggle,
            function (data) { toggleSpinner(data.show); }
        );

        function toggleSpinner(on) { vm.isBusy = on; }
    };
})();