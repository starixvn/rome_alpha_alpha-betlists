"use strict";
interface Window {
    $: JQueryStatic,
    productList: any,
    productDetailList: any,
    indexManager: any,
    alpha: any
}

class WinLossBetListView {
    private dateRangePickerComponent: DateRangePickerJQueryComponent;
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent;
    private weekSelectionComponent: WeekSelectionComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private productList: any;
    private productDetailList: any;
    private globalFormData: any;

    private betListWeekSelectionUrl: string;
    private betListWeekSelection: JQuery;

    private betListUrl: string;
    private tableBetList: JQuery;

    constructor(
        dateRangePickerComponent: DateRangePickerJQueryComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent,
        weekSelectionComponent: WeekSelectionComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        productList: any,
        productDetailList: any) {
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.weekSelectionComponent = weekSelectionComponent;
        this.productList = productList;
        this.productDetailList = productDetailList;
    }

    init() {
        this.cacheDOMElements();
        this.bindShowHideFilterEvent();
        this.bindExportExcelEvent();
        this.renderComponents();

        // make fadeOut effect shorter 
        $.blockUI.defaults.fadeOut = 0; 
    }

    private cacheDOMElements() {
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val(),
            productIds: $("#product-ids").val(),
            resourceIds: $("#resource-ids").val(),
            custId: $("#cust-id").val(),
            custName: $("#cust-name").val()
        };
        const betlistPath = $("#betlist-path").val();
        this.betListWeekSelectionUrl = betlistPath + "winloss/betlistweekselection?";
        this.betListWeekSelection = $("#betlist-week-selection");
        this.betListUrl = betlistPath + "winloss/betlist?";
        this.tableBetList = this.betListWeekSelection.find("#tbl-betlist");
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
                $("#week-selection").addClass("hide");
            } else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", hideFilterText);
                formFilter.removeClass("hide");
                $("#week-selection").removeClass("hide");
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
                this.constraintDateRangeAndProductDropdownList(dateRange);
                $.extend(this.globalFormData, dateRange, this.productAndProductDetailDropdownComponent.getCheckedValues());
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.getBetListWeekSelection();
            }, this.getDateRangePickerLocale($("#language").data("currentlanguage"))));

        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(
            this.productList,
            this.productDetailList,
            (productsAndResources) => {
                $.extend(this.globalFormData, this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.getBetListWeekSelection();
            }));

        const dateRange = this.dateRangePickerComponent.getDateRangeValues();
        this.constraintDateRangeAndProductDropdownList(dateRange);

        this.weekSelectionComponent.init(
            new WeekSelectionOption((dateRange) => {
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                $.extend(this.globalFormData, dateRange);
                this.getBetListData();
            }));

        const activeWeek = this.weekSelectionComponent.getActiveWeek();
        // update fromDate/toDate of globalFormData if activeWeek is NOT empty
        if (Object.keys(activeWeek).length) {
            this.globalFormData.fromDate = activeWeek.weekFromDate;
            this.globalFormData.toDate = activeWeek.weekToDate;
        }

        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption((e) => {
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.tableBetList.block();
            const $this = $(e.target);
            const custLevelId = +$this.data("custlevelid");
            const breadcrumbLevels = this.breadcrumbLevelComponent.getBreadcrumbLevels(custLevelId);

            let reportParams: any = this.weekSelectionComponent.getActiveWeek();
            if (breadcrumbLevels.length > 1) {
                reportParams.breadcrumbLevelJson = JSON.stringify(breadcrumbLevels);
            }
            $.extend(reportParams, this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId
            }, this.dateRangePickerComponent.getDateRangeValues());

            $this.attr("href", this.buildWinLossReportUrl(reportParams));
        }));
    }

    private constraintDateRangeAndProductDropdownList(dateRange) {
        const historicPivotDate = moment(this.dateRangePickerComponent.getHistoricPivotDate());
        if (moment(dateRange.fromDate).isSameOrAfter(historicPivotDate)) {
            this.productAndProductDetailDropdownComponent.hideHistoricProducts();
        } else {
            this.productAndProductDetailDropdownComponent.showHistoricProducts();
        }
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

    private buildWinLossReportUrl(reportFormData) {
        const action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    }

    private getBetListWeekSelection() {
        this.betListWeekSelection.block();

        this.addUnselectedProductAndProductDetailForStatistic();

        const formData = $.extend(
            {},
            this.globalFormData,
            { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbLevels()) });
        $.get(this.betListWeekSelectionUrl + $.param(formData), (html) => {
            this.betListWeekSelection.html(html);

            this.tableBetList = this.betListWeekSelection.find("#tbl-betlist");
            this.weekSelectionComponent.bindEvents();
            this.breadcrumbLevelComponent.bindEvents();
            this.bindExportExcelEvent();
            this.updateCurrentUrl();

            this.betListWeekSelection.unblock();
        }).fail((jqXHR) => this.handleAjaxError(jqXHR, this.betListWeekSelection));
    }

    private getBetListData() {
        this.tableBetList.block();

        this.addUnselectedProductAndProductDetailForStatistic();
        this.addIsWeekSelectionForStatistic();

        $.get(this.betListUrl + $.param(this.globalFormData), (html) => {
            this.tableBetList.find("tbody").html(html);
            this.tableBetList.unblock();
        }).fail((jqXHR) => this.handleAjaxError(jqXHR, this.tableBetList))
          .always(() => this.removeIsWeekSelectionForStatistic());
    }

    private isInvalidGlobalFormData() {
        return this.globalFormData.productIds.length === 0;
    }

    private handleAjaxError(jqXHR, element: JQuery) {
        if (jqXHR.status === 429) {
            window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
        } else if (jqXHR.status === 500 && jqXHR.responseText) {
            const responseObject = JSON.parse(jqXHR.responseText);
            if (responseObject.UMFrom && responseObject.UMTo) {
                window.location.href = window.top.alpha.urls.mainRootPath + "partialum/index?" + $.param(responseObject);
            }
        } else {
            element.unblock();
        }
    }

    private updateCurrentUrl() {
        if (this.globalFormData.productIds) {
            const formData = $.extend(
                {},
                this.globalFormData,
                { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbLevels()) });
            try {
                window.top.indexManager.updateCurrentUrl($.param(formData));
            } catch (e) {
            }
        }
    }

    private addUnselectedProductAndProductDetailForStatistic() {
        let moreParams: any = {};
        const productAndProductDetail = this.productAndProductDetailDropdownComponent.getUncheckedValues();
        if (productAndProductDetail.unselectedProductIds) {
            moreParams.unselectedProductIds = productAndProductDetail.unselectedProductIds;
        } else {
            delete this.globalFormData.unselectedProductIds;
        }

        if (productAndProductDetail.unselectedResourceIds) {
            moreParams.unselectedResourceIds = productAndProductDetail.unselectedResourceIds;
        } else {
            delete this.globalFormData.unselectedResourceIds;
        }

        $.extend(this.globalFormData, moreParams);
    }

    private addIsWeekSelectionForStatistic() {
        this.globalFormData.isWeekSelection = 1;
    }

    private removeIsWeekSelectionForStatistic() {
        delete this.globalFormData.isWeekSelection;
    }
}

$(function () {
    new WinLossBetListView(
        new DateRangePickerJQueryComponent(),
        new ProductAndProductDetailDropdownComponent(),
        new WeekSelectionComponent(),
        new BreadcrumbLevelComponent(),
        window.productList,
        window.productDetailList).init();
});
