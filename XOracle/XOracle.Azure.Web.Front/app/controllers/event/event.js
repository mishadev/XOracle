(function () {
    'use strict';
    var controllerId = 'event';
    angular.module('app').controller(controllerId, ['common', 'datacontext', event]);

    function event(common, datacontext) {
        var vm = this,
            log = common.logger.getLogFn(controllerId),
            logError = common.logger.getLogFn(controllerId, 'error');

        vm.title = 'Event';
        vm.ready = false;
        vm.eventExists = false;
        vm.event = null;

        activate();

        /*private*/

        function activate() {
            var promises = [getEvent()];
            common.activateController(promises, controllerId)
                .then(function () {
                    vm.ready = true;
                }, logErrors);
        }

        function getEvent() {
            return datacontext.GetEvent(common.$routeParams.EventId)
                .then(function (data) {
                    vm.eventExists = !!data.EventId;
                    if (vm.eventExists) {
                        vm.event = data;
                    }
                }, logErrors);
        }

        function logErrors(data) {
            logError(data.ExceptionMessage || data.Message || data);
        }

        /* public */
    }
})();