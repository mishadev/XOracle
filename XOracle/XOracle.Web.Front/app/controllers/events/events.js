(function () {
    'use strict';
    var controllerId = 'events';
    angular.module('app').controller(controllerId, ['common', 'datacontext', events]);

    function events(common, datacontext) {
        var vm = this,
            log = common.logger.getLogFn(controllerId),
            logError = common.logger.getLogFn(controllerId, 'error'),
            defaultEvent = {
                title: '',
                closeDate: '',
                endDate: '',
                expectedEventCondition: '',
                currencyType: 'Reputation',
                eventRelationType: '*..*',
                startRate: '0',
                endRate: '100',
                locusRage: '1',
                algorithmType: 'Exponential',
                judgingAccounts: ''
            };

        vm.events = [];
        vm.hasEvents = false;
        vm.title = 'Events';
        vm.ready = false;
        vm.event = defaultEvent;
        vm.eventRelationTypes = [];
        vm.currencyTypes = [];
        vm.algorithmTypes = [];

        activate();

        /*private*/

        function activate() {
            var promises = [getEvents(), getEventRelationTypes(), getCurrencyType(), getAlgorithmType()];
            common.activateController(promises, controllerId)
                .then(function () {
                    vm.ready = true;
                }, logErrors);
        }

        function getEvents() {
            return datacontext.GetEvents()
                .then(function (data) {
                    if (data && data.length > 0) {
                        vm.events = data;
                        vm.hasEvents = true;
                    }
                }, logErrors);
        }

        function getEventRelationTypes() {
            return datacontext.GetEventRelationTypes()
                .then(function (data) {
                    if (data && data.length > 0)
                        vm.eventRelationTypes = data;
                }, logErrors);
        }

        function getCurrencyType() {
            return datacontext.GetCurrencyType()
                .then(function (data) {
                    if (data && data.length > 0)
                        vm.currencyTypes = data;
                }, logErrors);
        }

        function getAlgorithmType() {
            return datacontext.GetAlgorithmType()
               .then(function (data) {
                   if (data && data.length > 0)
                       vm.algorithmTypes = data;
               }, logErrors);
        }

        function logErrors(data) {
            logError(data.ExceptionMessage || data.Message || data);
        }

        /* public */

        vm.create = function () {
            return datacontext.CreateEvent(vm.event)
               .then(function (data) {
                   log('Event created!')
               }, logErrors);
        }
    }
})();