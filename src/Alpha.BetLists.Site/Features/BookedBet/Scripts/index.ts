"use strict";
interface Window {
    $: JQueryStatic,
    statusList: any,
    indexManager: any,
    alpha: any
}
declare var Clipboard: any;
class BookedBetListView {
    private dateRangePickerComponent: DateRangePickerJQueryComponent;
    private statusDropdownComponent: StatusDropdownComponent;
    private monitoringBetsCopyHtml: MonitoringBetsCopyHtml;

    private statusList: any;
    private globalFormData: any;

    private betListUrl: string;
    private tableBetList: JQuery;

    constructor(
        dateRangePickerComponent: DateRangePickerJQueryComponent,
        statusDropdownComponent: StatusDropdownComponent,
        statusList: any,
        monitoringBetsCopyHtml: MonitoringBetsCopyHtml) {
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.statusDropdownComponent = statusDropdownComponent;
        this.statusList = statusList;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
    }

    init() {
        this.cacheDOMElements();
        this.bindShowHideFilterEvent();
        this.bindExportExcelEvent();
        this.renderComponents();

        this.monitoringBetsCopyHtml.init();

        // make fadeOut effect shorter 
        $.blockUI.defaults.fadeOut = 0;
    }

    private cacheDOMElements() {
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val()
        };
        const betlistPath = $("#betlist-path").val();
        this.betListUrl = betlistPath + "BookedBet/GetData?";
        this.tableBetList = $("#betlist-data");
    }

    private bindShowHideFilterEvent() {
        const hideFilterText = $("#hidden-hide-filter-text").val();
        const showFilterText = $("#hidden-show-filter-text").val();
        const formFilter = $("#form-filter");
        const iconFilter = $("#icon-filter").click(function () {
            const isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("title", showFilterText);
                formFilter.addClass("hide");
            } else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", hideFilterText);
                formFilter.removeClass("hide");
            }
        });
    }

    private bindExportExcelEvent() {
        $("#icon-excel").off().click((e) => {
            if (this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }

            const $this = $(e.target);
            const excelParams = $.extend({}, this.globalFormData)
            $this.attr("href", $this.data("url") + "?" + $.param(excelParams));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });
    }

    private renderComponents() {
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(
            (dateRange) => {
                $.extend(this.globalFormData, dateRange, this.statusDropdownComponent.getCheckedValues());
                if (this.isInvalidGlobalFormData()) {
                    return;
                }
                this.getBetListData();
            }, this.getDateRangePickerLocale($("#language").data("currentlanguage")),
            "daterange-picker",
            false,
            false));


        this.statusDropdownComponent.render(new StatusDropdownOption(
            this.statusList,
            (ids) => {
                $.extend(this.globalFormData, this.dateRangePickerComponent.getDateRangeValues(), ids);
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.getBetListData();
            }));

        const dateRange = this.dateRangePickerComponent.getDateRangeValues();
    }

    private getDateRangePickerLocale(language: string) {
        const locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "cn",
            "zh-TW": "tc"
        };

        return locale[language] || "default";
    }

    private getBetListData() {
        this.tableBetList.block();

        $.get(this.betListUrl + $.param(this.globalFormData), (html) => {
            this.tableBetList.html(html);
            this.tableBetList.unblock();
        }).fail((jqXHR) => this.handleAjaxError(jqXHR, this.tableBetList));
    }

    private isInvalidGlobalFormData() {
        return this.globalFormData.StatusIds.length === 0;
    }

    private handleAjaxError(jqXHR, element: JQuery) {
        if (jqXHR.status === 429) {
            window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
        } else if (jqXHR.status === 500 && jqXHR.responseText) {
            const responseObject = JSON.parse(jqXHR.responseText);
            if (responseObject.UMFrom && responseObject.UMTo) {
                const cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                const siteMainUrl = cookielessSiteMainUrl.length
                    ? cookielessSiteMainUrl.val()
                    : window.top["alpha"].urls.mainRootPath;
                window.location.href = `${siteMainUrl}partialum/index?${$.param(responseObject)}`;
            }
        } else {
            element.unblock();
        }
    }

    private updateCurrentUrl() {
        if (this.globalFormData.productIds) {
            const formData = $.extend(
                {},
                this.globalFormData);
            try {
                window.top.indexManager.updateCurrentUrl($.param(formData));
            } catch (e) {
            }
        }
    }
}

$(function () {
    new BookedBetListView(
        new DateRangePickerJQueryComponent(),
        new StatusDropdownComponent(),
        window.statusList,
        new MonitoringBetsCopyHtml()).init();
});
