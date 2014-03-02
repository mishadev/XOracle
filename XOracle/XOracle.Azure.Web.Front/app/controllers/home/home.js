﻿(function () {
    'use strict';
    var controllerId = 'home';
    angular.module('app').controller(controllerId, ['common', 'datacontext', home]);

    function home(common, datacontext) {
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
                arbiterAccounts: ''
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

        function createBet(data) {
            return datacontext.CreateBet(data)
               .then(function (output) {
                   log(data.OutcomesType + ' Bet created!');
               }, logErrors);
        }

        /* public */

        vm.create = function () {
            return datacontext.CreateEvent(vm.event)
               .then(function (data) {
                   log('Event created!');
               }, logErrors);
        }

        vm.createHappenBet = function (event) {
            return createBet({ EventId: event.EventId, OutcomesType: "Happen", BetAmount: 1 });
        }

        vm.createNotHappenBet = function (event) {
            return createBet({ EventId: event.EventId, OutcomesType: "NotHappen", BetAmount: 1 });
        }
    }
})();