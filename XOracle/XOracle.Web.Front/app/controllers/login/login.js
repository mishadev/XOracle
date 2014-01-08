(function () {
    'use strict';
    var controllerId = 'login';
    angular.module('app').controller(controllerId, ['common', 'datacontext', login]);

    function login(common, datacontext) {
        var vm = this;
        var log = common.logger.getLogFn(controllerId);
        var logError = common.logger.getLogFn(controllerId, 'error');

        vm.loginProviders = [];
        vm.title = 'Login';
        vm.ready = false;

        activate();

        /*private*/

        function activate() {
            var promises = [externalLoginProviders()];
            common.activateController(promises, controllerId)
                .then(function () {

                    vm.ready = true;
                });
        }

        function externalLoginProviders() {
            return datacontext.ExternalLoginsUrl(window.location.href)
                .then(function (data) {
                    vm.loginProviders = data;
                }, logErrors);
        }

        function login() {
            return datacontext.Login().then(function (data) {
                debugger
                return vm.user = data;
            }, logErrors);
        }

        function logErrors(data)
        {
            logError(data.ExceptionMessage || data.Message || data);
        }

        /*public*/

        vm.externalLogin = function (provider) {
            sessionStorage["state"] = provider.State;
            sessionStorage["loginUrl"] = provider.Url;
            // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage temporarily
            // to localStorage to work around this problem.
            common.archiveSessionStorageToLocalStorage();
            window.location = provider.Url;
        }
    }
})();