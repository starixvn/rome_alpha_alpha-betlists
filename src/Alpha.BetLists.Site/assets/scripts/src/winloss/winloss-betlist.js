"use strict";
var WinLossBetListView = /** @class */ (function () {
    function WinLossBetListView(breadcrumbLevelComponent, dateRangePickerComponent, productAndProductDetailDropdownComponent, weekRangeComponent, productList, productDetailList) {
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.weekRangeComponent = weekRangeComponent;
        this.productList = productList;
        this.productDetailList = productDetailList;
    }
    WinLossBetListView.prototype.init = function () {
        var _this = this;
        this.cacheDOMElements();
        $("#icon-excel").click(function (e) {
            if (_this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }
            var $this = $(e.target);
            var excelParams = $.extend({}, _this.globalFormData, { custName: $this.data("custname") });
            $this.attr("href", $this.data("url") + "?" + $.param(excelParams));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });
        this.renderComponents();
    };
    WinLossBetListView.prototype.cacheDOMElements = function () {
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val(),
            productIds: $("#product-ids").val(),
            resourceIds: $("#resource-ids").val(),
            custId: $("#cust-id").val()
        };
        this.winlossBetListUrl = $("#betlist-path").val() + "winloss/getdata?";
        this.tableBetList = $("#tbl-betlist");
        var errorExceedRow = $("#error-exceed-row");
        errorExceedRow.length
            && this.tableBetList.find("tbody tr:first td").addClass("error-exceed-row").html(errorExceedRow.val());
        var breadcrumbLevelJson = $("#breadcrumb-level-json").val();
        this.breadcrumbs = breadcrumbLevelJson.length ? JSON.parse(breadcrumbLevelJson) : [];
    };
    WinLossBetListView.prototype.renderComponents = function () {
        var _this = this;
        this.breadcrumbLevelComponent.render(new BreadcrumbLevelOption(this.breadcrumbs, function (e) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.tableBetList.block();
            var $this = $(e.target);
            var custLevelId = +$this.data("custlevelid");
            var reportFormData = $.extend({}, _this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: custLevelId
            }, _this.dateRangePickerComponent.getDateRangeValues());
            var breadcrumbLevel = _this.breadcrumbs.filter(function (item) { return item.custLevelId >= custLevelId; });
            if (breadcrumbLevel.length > 1) {
                reportFormData.breadcrumbLevelJson = JSON.stringify(breadcrumbLevel);
            }
            $this.attr("href", _this.buildWinLossReportUrl(reportFormData));
        }));
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function (dateRange) {
            $.extend(_this.globalFormData, dateRange);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListAndRenderWeekRange();
        }));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function (productsAndResources) {
            $.extend(_this.globalFormData, _this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListAndRenderWeekRange();
        }));
        this.weekRangeComponent.init(new WeekRangeOption(function (dateRange) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            $.extend(_this.globalFormData, dateRange);
            _this.getBetList();
        }, this.getMomentLocale()));
        this.weekRangeComponent.render(this.globalFormData);
    };
    WinLossBetListView.prototype.getMomentLocale = function () {
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };
        return locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    };
    WinLossBetListView.prototype.buildWinLossReportUrl = function (reportFormData) {
        var action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    };
    WinLossBetListView.prototype.getBetListAndRenderWeekRange = function () {
        var _this = this;
        this.getBetList(function () {
            _this.weekRangeComponent.render(_this.globalFormData);
            _this.updateCurrentUrl();
        });
    };
    WinLossBetListView.prototype.getBetList = function (callback) {
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.tableBetList.block();
        $.get(this.winlossBetListUrl + $.param(this.globalFormData), function (html) {
            callback();
            _this.tableBetList.find("tbody").html(html);
            _this.tableBetList.unblock();
        }).fail(function (jqXHR) {
            if (jqXHR.status === 429) {
                window.location.href = window.top["alpha"].urls.mainRootPath + "error/blocked";
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    window.location.href = window.top["alpha"].urls.mainRootPath + "partialum/index?" + $.param(responseObject);
                }
            }
            else {
                _this.tableBetList.unblock();
            }
        });
    };
    WinLossBetListView.prototype.isInvalidGlobalFormData = function () {
        return this.globalFormData.productIds.length === 0;
    };
    WinLossBetListView.prototype.updateCurrentUrl = function () {
        if (this.globalFormData.productIds) {
            var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbs) });
            window.top.indexManager.updateCurrentUrl($.param(formData));
        }
    };
    return WinLossBetListView;
}());
$(function () {
    new WinLossBetListView(new BreadcrumbLevelComponent(), new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new WeekRangeComponent(), window.productList, window.productDetailList).init();
});
//# sourceMappingURL=winloss-betlist.js.map