(function () {
    'use strict';
    var controllerId = 'event';
    var app = angular.module('app');
    app.controller(controllerId, ['common', 'datacontext', event]);

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
                .then(Drow, logErrors)
                .then(function () {
                    vm.ready = true;
                });
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

        function Drow() {
            var canvas = document.getElementById('retChart');
            if (canvas.getContext) {
                var ctx = canvas.getContext('2d');

                DrowChart(ctx, canvas.height, canvas.width);
                DrowHorizontalLine(ctx, canvas.height, canvas.width, vm.event.TimeLeftPercentage, '#ff0000');
                DrowHorizontalLine(ctx, canvas.height, canvas.width, vm.event.NowPercentage, '#000000');
            }
        }

        function DrowChart(ctx, height, width) {
            var offset = width * .5;
            var length = vm.event.BetRateData.length;

            ctx.beginPath();
            ctx.moveTo(offset, 0);
            for (var idx in vm.event.BetRateData) {
                var x = offset - (vm.event.BetRateData[idx] / 255) * offset;
                var y = idx / length * height;

                ctx.lineTo(x, y);
                idx++;
            }

            ctx.moveTo(offset, 0);

            ctx.stroke();
        }

        function DrowHorizontalLine(ctx, height, width, percentage, color) {
            var h = height * (1 - percentage);

            ctx.beginPath();
            ctx.moveTo(0, h);
            ctx.lineTo(width, h);
            ctx.strokeStyle = color;
            ctx.stroke();
        }

        /* public */
    }
})();