(function () {
    'use strict';
    var controllerId = 'user';
    angular.module('app').controller(controllerId, ['common', 'datacontext', user]);

    function user(common, datacontext) {
        var vm = this,
            log = common.logger.getLogFn(controllerId),
            logError = common.logger.getLogFn(controllerId, 'error');

        vm.title = 'User Events';
        vm.events = [];
        vm.hasEvents = false;
        vm.ready = false;
        vm.userExists = false;
        vm.isMe = false;

        activate();

        /*private*/

        function activate() {
            var promises = [Init()];
            common.activateController(promises, controllerId)
                .then(function () { }, logErrors);
        }

        function Init() {
            return datacontext.GetUserInfo(common.$routeParams.AccountName)
                .then(function (data) {
                    vm.userExists = !!data.UserId;
                    if (vm.userExists) {
                        vm.isMe = data.IsCurrent;
                        getEvents(data.UserId);
                    }
                }, logErrors);
        }

        function getEvents(accountId) {
            return datacontext.GetEvents(accountId)
                .then(function (data) {
                    if (data && data.length > 0) {
                        vm.events = data;
                        vm.hasEvents = true;
                    }
                    vm.ready = true;
                }, logErrors);
        }

        function logErrors(data) {
            logError(data.ExceptionMessage || data.Message || data);
        }

        /* public */

        vm.follow = function () {
            log('followed!');
        }
    }
})();