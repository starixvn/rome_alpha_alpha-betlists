"use strict";
var MonitoringBetsView = /** @class */ (function () {
    function MonitoringBetsView(autoRefreshComponent, monitoringBetsCopyHtml) {
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
    }
    MonitoringBetsView.prototype.init = function () {
        this.dropdownFilter = $("#monitoring-bets-filter");
        this.dropdownFilter.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
        //this.dropdownFilter.find("option:selected:first").val();
        this.dropdownTop = $("#monitoring-bets-top");
        this.dropdownTop.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
        //this.dropdownTop.find("option:selected:first").val();
        var self = this;
        this.autoRefreshComponent.init(new AutoRefreshOption("ON", "OFF", function () {
            setTimeout(function () {
                console.log('get data from server');
                self.autoRefreshComponent.start();
            }, 3000);
        }));
        //this.monitoringBetsCopyHtml.init();
    };
    return MonitoringBetsView;
}());
$(function () {
    new MonitoringBetsView(new AutoRefreshComponent(), new MonitoringBetsCopyHtml(Clipboard)).init();
});
//# sourceMappingURL=monitoring-bets.js.map