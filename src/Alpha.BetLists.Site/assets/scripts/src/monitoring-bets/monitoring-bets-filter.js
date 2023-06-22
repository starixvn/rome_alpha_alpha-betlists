"use strict";
var MonitoringBetsFilterComponent = /** @class */ (function () {
    function MonitoringBetsFilterComponent() {
    }
    MonitoringBetsFilterComponent.prototype.render = function () {
        this.dropdownFilter = $("#monitoring-bets-filter");
        this.dropdownFilter.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
    };
    MonitoringBetsFilterComponent.prototype.getSelected = function () {
        return this.dropdownFilter.find("option:selected:first").val();
    };
    return MonitoringBetsFilterComponent;
}());
//# sourceMappingURL=monitoring-bets-filter.js.map