"use strict";
var WinLossBetListView = /** @class */ (function () {
    function WinLossBetListView(dateRangePickerComponent, productAndProductDetailDropdownComponent, weekSelectionComponent, breadcrumbLevelComponent, productList, productDetailList) {
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.weekSelectionComponent = weekSelectionComponent;
        this.productList = productList;
        this.productDetailList = productDetailList;
    }
    WinLossBetListView.prototype.init = function () {
        this.cacheDOMElements();
        this.bindShowHideFilterEvent();
        this.bindExportExcelEvent();
        this.renderComponents();
        // make fadeOut effect shorter 
        $.blockUI.defaults.fadeOut = 0;
    };
    WinLossBetListView.prototype.cacheDOMElements = function () {
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val(),
            productIds: $("#product-ids").val(),
            resourceIds: $("#resource-ids").val(),
            custId: $("#cust-id").val(),
            custName: $("#cust-name").val()
        };
        var betlistPath = $("#betlist-path").val();
        this.betListWeekSelectionUrl = betlistPath + "winloss/betlistweekselection?";
        this.betListWeekSelection = $("#betlist-week-selection");
        this.betListUrl = betlistPath + "winloss/betlist?";
        this.tableBetList = this.betListWeekSelection.find("#tbl-betlist");
    };
    WinLossBetListView.prototype.bindShowHideFilterEvent = function () {
        var hideFilterText = $("#hidden-hide-filter-text").val();
        var showFilterText = $("#hidden-show-filter-text").val();
        var formFilter = $("#form-filter");
        var iconFilter = $("#icon-filter").click(function () {
            var isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("title", showFilterText);
                formFilter.addClass("hide");
                $("#week-selection").addClass("hide");
            }
            else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", hideFilterText);
                formFilter.removeClass("hide");
                $("#week-selection").removeClass("hide");
            }
        });
    };
    WinLossBetListView.prototype.bindExportExcelEvent = function () {
        var _this = this;
        $("#icon-excel").off().click(function (e) {
            if (_this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }
            var $this = $(e.target);
            var excelParams = $.extend({}, _this.globalFormData);
            $this.attr("href", $this.data("url") + "?" + $.param(excelParams));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });
    };
    WinLossBetListView.prototype.renderComponents = function () {
        var _this = this;
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function (dateRange) {
            $.extend(_this.globalFormData, dateRange, _this.productAndProductDetailDropdownComponent.getCheckedValues());
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListWeekSelection();
        }, this.getDateRangePickerLocale($("#language").data("currentlanguage"))));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function (productsAndResources) {
            $.extend(_this.globalFormData, _this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListWeekSelection();
        }));
        var dateRange = this.dateRangePickerComponent.getDateRangeValues();
        this.weekSelectionComponent.init(new WeekSelectionOption(function (dateRange) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            $.extend(_this.globalFormData, dateRange);
            _this.getBetListData();
        }));
        var activeWeek = this.weekSelectionComponent.getActiveWeek();
        // update fromDate/toDate of globalFormData if activeWeek is NOT empty
        if (Object.keys(activeWeek).length) {
            this.globalFormData.fromDate = activeWeek.weekFromDate;
            this.globalFormData.toDate = activeWeek.weekToDate;
        }
        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption(function (e) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.tableBetList.block();
            var $this = $(e.target);
            var custLevelId = +$this.data("custlevelid");
            var breadcrumbLevels = _this.breadcrumbLevelComponent.getBreadcrumbLevels(custLevelId);
            var reportParams = _this.weekSelectionComponent.getActiveWeek();
            if (breadcrumbLevels.length > 1) {
                reportParams.breadcrumbLevelJson = JSON.stringify(breadcrumbLevels);
            }
            reportParams.showColumns = $("#hidden-show-columns").val();
            $.extend(reportParams, _this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: custLevelId
            }, _this.dateRangePickerComponent.getDateRangeValues());
            $this.attr("href", _this.buildWinLossReportUrl(reportParams));
        }));
    };
    WinLossBetListView.prototype.getDateRangePickerLocale = function (language) {
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "cn",
            "zh-TW": "tc"
        };
        return locale[language] || "default";
    };
    WinLossBetListView.prototype.buildWinLossReportUrl = function (reportFormData) {
        var action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    };
    WinLossBetListView.prototype.getBetListWeekSelection = function () {
        var _this = this;
        this.betListWeekSelection.block();
        this.addUnselectedProductAndProductDetailForStatistic();
        var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbLevels()) });
        $.get(this.betListWeekSelectionUrl + $.param(formData), function (html) {
            _this.betListWeekSelection.html(html);
            _this.tableBetList = _this.betListWeekSelection.find("#tbl-betlist");
            _this.weekSelectionComponent.bindEvents();
            _this.breadcrumbLevelComponent.bindEvents();
            _this.bindExportExcelEvent();
            _this.updateCurrentUrl();
            var activeWeek = _this.weekSelectionComponent.getActiveWeek();
            // update fromDate/toDate of globalFormData to activeWeek
            if (Object.keys(activeWeek).length) {
                _this.globalFormData.fromDate = activeWeek.weekFromDate;
                _this.globalFormData.toDate = activeWeek.weekToDate;
            }
            _this.betListWeekSelection.unblock();
        }).fail(function (jqXHR) { return _this.handleAjaxError(jqXHR, _this.betListWeekSelection); });
    };
    WinLossBetListView.prototype.getBetListData = function () {
        var _this = this;
        this.tableBetList.block();
        this.addUnselectedProductAndProductDetailForStatistic();
        this.addIsWeekSelectionForStatistic();
        $.get(this.betListUrl + $.param(this.globalFormData), function (html) {
            _this.tableBetList.find("tbody").html(html);
            _this.tableBetList.unblock();
        }).fail(function (jqXHR) { return _this.handleAjaxError(jqXHR, _this.tableBetList); })
            .always(function () { return _this.removeIsWeekSelectionForStatistic(); });
    };
    WinLossBetListView.prototype.isInvalidGlobalFormData = function () {
        return this.globalFormData.productIds.length === 0;
    };
    WinLossBetListView.prototype.handleAjaxError = function (jqXHR, element) {
        if (jqXHR.status === 429) {
            window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
        }
        else if (jqXHR.status === 500 && jqXHR.responseText) {
            var responseObject = JSON.parse(jqXHR.responseText);
            if (responseObject.UMFrom && responseObject.UMTo) {
                var cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                var siteMainUrl = cookielessSiteMainUrl.length
                    ? cookielessSiteMainUrl.val()
                    : window.top["alpha"].urls.mainRootPath;
                window.location.href = siteMainUrl + "partialum/index?" + $.param(responseObject);
            }
        }
        else {
            element.unblock();
        }
    };
    WinLossBetListView.prototype.updateCurrentUrl = function () {
        if (this.globalFormData.productIds) {
            var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbLevels()) });
            try {
                window.top.indexManager.updateCurrentUrl($.param(formData));
            }
            catch (e) {
            }
        }
    };
    WinLossBetListView.prototype.addUnselectedProductAndProductDetailForStatistic = function () {
        var moreParams = {};
        var productAndProductDetail = this.productAndProductDetailDropdownComponent.getUncheckedValues();
        if (productAndProductDetail.unselectedProductIds) {
            moreParams.unselectedProductIds = productAndProductDetail.unselectedProductIds;
        }
        else {
            delete this.globalFormData.unselectedProductIds;
        }
        if (productAndProductDetail.unselectedResourceIds) {
            moreParams.unselectedResourceIds = productAndProductDetail.unselectedResourceIds;
        }
        else {
            delete this.globalFormData.unselectedResourceIds;
        }
        $.extend(this.globalFormData, moreParams);
    };
    WinLossBetListView.prototype.addIsWeekSelectionForStatistic = function () {
        this.globalFormData.isWeekSelection = 1;
    };
    WinLossBetListView.prototype.removeIsWeekSelectionForStatistic = function () {
        delete this.globalFormData.isWeekSelection;
    };
    return WinLossBetListView;
}());
$(function () {
    new WinLossBetListView(new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new WeekSelectionComponent(), new BreadcrumbLevelComponent(), window.productList, window.productDetailList).init();
});
//# sourceMappingURL=winloss-betlist.js.map