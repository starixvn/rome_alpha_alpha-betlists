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
            var $this = $(e.target);
            $this.attr("href", $this.data("url") + "?" + $.param(_this.globalFormData));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val(),
            productIds: $("#product-ids").val(),
            resourceIds: $("#resource-ids").val(),
            custId: $("#cust-id").val()
        };
        this.renderComponents();
    };
    WinLossBetListView.prototype.cacheDOMElements = function () {
        this.winlossBetListUrl = $("#betlist-path").val() + "winloss/getdata?";
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };
        this.momentLocale = locale[window.top.$("#language").data("currentlanguage")] || locale.default;
        this.tableBetList = $(".tbl-betlist");
    };
    WinLossBetListView.prototype.renderComponents = function () {
        var _this = this;
        var self = this;
        var breadcrumbLevelJson = this.getBreadcrumbLevelJson();
        this.breadcrumbLevelComponent.render(new BreadcrumbLevelOption(breadcrumbLevelJson, function () {
            var $this = $(this);
            var custLevelId = +$this.data("custlevelid");
            var reportFormData = $.extend({}, self.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: custLevelId
            });
            var breadcrumbs = breadcrumbLevelJson.filter(function (item) { return item.custLevelId >= custLevelId; });
            if (breadcrumbs.length > 1) {
                reportFormData.breadcrumbLevelJson = JSON.stringify(breadcrumbs);
            }
            $this.attr("href", self.buildWinLossReportUrl(reportFormData));
        }));
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function () { return _this.getBetListAndRenderWeekRange(); }));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function () { return _this.getBetListAndRenderWeekRange(); }));
        this.weekRangeComponent.init(new WeekRangeOption(function (dateRange) {
            _this.getBetList($.extend(_this.globalFormData, dateRange));
        }, this.momentLocale));
        this.weekRangeComponent.render(this.globalFormData);
    };
    WinLossBetListView.prototype.getBreadcrumbLevelJson = function () {
        var breadcrumbLevelJson = JSON.parse($("#breadcrumb-level-json").val());
        breadcrumbLevelJson.push({
            custId: this.globalFormData.custId,
            custName: $("#cust-name").val(),
            custLevelId: 1
        });
        return breadcrumbLevelJson;
    };
    WinLossBetListView.prototype.buildWinLossReportUrl = function (reportFormData) {
        var action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    };
    WinLossBetListView.prototype.getBetListAndRenderWeekRange = function () {
        var _this = this;
        var dateRange = this.dateRangePickerComponent.getDateRangeValues();
        var productsAndResources = this.productAndProductDetailDropdownComponent.getSelectedItems();
        $.extend(this.globalFormData, dateRange, productsAndResources);
        this.getBetList(this.globalFormData, function () { return _this.weekRangeComponent.render(_this.globalFormData); });
    };
    WinLossBetListView.prototype.getBetList = function (formData, callback) {
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.tableBetList.block();
        $.get(this.winlossBetListUrl + $.param(formData), function (html) {
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
    return WinLossBetListView;
}());
$(function () {
    new WinLossBetListView(new BreadcrumbLevelComponent(), new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new WeekRangeComponent(), window.productList, window.productDetailList).init();
});
//# sourceMappingURL=winloss-betlist.js.map