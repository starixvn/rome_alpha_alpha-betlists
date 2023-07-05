"use strict";
var BreadcrumbLevelOption = /** @class */ (function () {
    function BreadcrumbLevelOption(onClick, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "breadcrumb-level"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return BreadcrumbLevelOption;
}());
var BreadcrumbLevelComponent = /** @class */ (function () {
    function BreadcrumbLevelComponent() {
    }
    BreadcrumbLevelComponent.prototype.init = function (options) {
        this.breadcrumbLevelOption = options;
        this._buildBreadcrumbLevels();
        this.bindEvents();
    };
    BreadcrumbLevelComponent.prototype._buildBreadcrumbLevels = function () {
        var _this = this;
        this.breadcrumbLevels = [];
        $("#" + this.breadcrumbLevelOption.selectorId).find("li").each(function (_, item) {
            var uplineLinkList = $(item).find(".upline-link");
            if (uplineLinkList.length) {
                var uplineLink = uplineLinkList.eq(0);
                _this.breadcrumbLevels.push({
                    custId: uplineLink.data("custid"),
                    custName: uplineLink.data("custname"),
                    custLevelId: +uplineLink.data("custlevelid")
                });
            }
            else {
                _this.breadcrumbLevels.push({
                    custName: item.innerText
                });
            }
        });
    };
    BreadcrumbLevelComponent.prototype.bindEvents = function () {
        $("#" + this.breadcrumbLevelOption.selectorId)
            .find(".upline-link")
            .off()
            .click(this.breadcrumbLevelOption.onClick);
    };
    BreadcrumbLevelComponent.prototype.getBreadcrumbLevels = function (custLevelId) {
        if (custLevelId === void 0) { custLevelId = 0; }
        return custLevelId
            ? this.breadcrumbLevels.filter(function (item) { return item.custLevelId >= custLevelId; })
            : this.breadcrumbLevels;
    };
    return BreadcrumbLevelComponent;
}());
//# sourceMappingURL=breadcrumb-level.js.map