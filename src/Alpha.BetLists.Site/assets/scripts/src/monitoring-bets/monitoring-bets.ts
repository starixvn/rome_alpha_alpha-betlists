"use strict";

class MonitoringBetsView {
    private dropdownFilter: JQuery;
    private dropdownTop: JQuery;
    private autoRefreshComponent: AutoRefreshComponent;
    private monitoringBetsCopyHtml: MonitoringBetsCopyHtml;

    constructor(
        autoRefreshComponent: AutoRefreshComponent,
        monitoringBetsCopyHtml: MonitoringBetsCopyHtml) {
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
    }

    init() {
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

        const self = this;
        this.autoRefreshComponent.init(new AutoRefreshOption(
            "ON",
            "OFF",
            function () {
                setTimeout(() => {
                    console.log('get data from server');
                    self.autoRefreshComponent.start();
                }, 3000);
            }));

        //this.monitoringBetsCopyHtml.init();
    }
}

$(function () {
    new MonitoringBetsView(
        new AutoRefreshComponent(),
        new MonitoringBetsCopyHtml(Clipboard)).init();
});