"use strict";
var BookedBetListView = /** @class */ (function () {
    function BookedBetListView(dateRangePickerComponent, statusDropdownComponent, statusList, monitoringBetsCopyHtml) {
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.statusDropdownComponent = statusDropdownComponent;
        this.statusList = statusList;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
    }
    BookedBetListView.prototype.init = function () {
        this.cacheDOMElements();
        this.bindShowHideFilterEvent();
        this.bindExportExcelEvent();
        this.renderComponents();
        this.monitoringBetsCopyHtml.init();
        // make fadeOut effect shorter 
        $.blockUI.defaults.fadeOut = 0;
    };
    BookedBetListView.prototype.cacheDOMElements = function () {
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val()
        };
        var betlistPath = $("#betlist-path").val();
        this.betListUrl = betlistPath + "BookedBet/GetData?";
        this.tableBetList = $("#betlist-data");
    };
    BookedBetListView.prototype.bindShowHideFilterEvent = function () {
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
            }
            else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", hideFilterText);
                formFilter.removeClass("hide");
            }
        });
    };
    BookedBetListView.prototype.bindExportExcelEvent = function () {
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
    BookedBetListView.prototype.renderComponents = function () {
        var _this = this;
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function (dateRange) {
            $.extend(_this.globalFormData, dateRange, _this.statusDropdownComponent.getCheckedValues());
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListData();
        }, this.getDateRangePickerLocale($("#language").data("currentlanguage")), "daterange-picker", false, false));
        this.statusDropdownComponent.render(new StatusDropdownOption(this.statusList, function (ids) {
            $.extend(_this.globalFormData, _this.dateRangePickerComponent.getDateRangeValues(), ids);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListData();
        }));
        var dateRange = this.dateRangePickerComponent.getDateRangeValues();
    };
    BookedBetListView.prototype.getDateRangePickerLocale = function (language) {
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "cn",
            "zh-TW": "tc"
        };
        return locale[language] || "default";
    };
    BookedBetListView.prototype.getBetListData = function () {
        var _this = this;
        this.tableBetList.block();
        $.get(this.betListUrl + $.param(this.globalFormData), function (html) {
            _this.tableBetList.html(html);
            _this.tableBetList.unblock();
        }).fail(function (jqXHR) { return _this.handleAjaxError(jqXHR, _this.tableBetList); });
    };
    BookedBetListView.prototype.isInvalidGlobalFormData = function () {
        return this.globalFormData.StatusIds.length === 0;
    };
    BookedBetListView.prototype.handleAjaxError = function (jqXHR, element) {
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
    BookedBetListView.prototype.updateCurrentUrl = function () {
        if (this.globalFormData.productIds) {
            var formData = $.extend({}, this.globalFormData);
            try {
                window.top.indexManager.updateCurrentUrl($.param(formData));
            }
            catch (e) {
            }
        }
    };
    return BookedBetListView;
}());
$(function () {
    new BookedBetListView(new DateRangePickerJQueryComponent(), new StatusDropdownComponent(), window.statusList, new MonitoringBetsCopyHtml()).init();
});
//# sourceMappingURL=index.js.map