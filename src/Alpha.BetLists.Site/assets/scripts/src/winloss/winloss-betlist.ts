"use strict";
interface Window {
    $: JQueryStatic,
    productList: any,
    productDetailList: any,
    indexManager: any
}

class WinLossBetListView {
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private dateRangePickerComponent: DateRangePickerJQueryComponent;
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent;
    private weekRangeComponent: WeekRangeComponent
    private productList: any;
    private productDetailList: any;
    private globalFormData: any;

    private winlossBetListUrl: string;
    private tableBetList: JQuery;
    private breadcrumbs: any;

    constructor(
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        dateRangePickerComponent: DateRangePickerJQueryComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent,
        weekRangeComponent: WeekRangeComponent,
        productList: any,
        productDetailList: any) {
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.weekRangeComponent = weekRangeComponent;
        this.productList = productList;
        this.productDetailList = productDetailList;
    }

    init() {
        this.cacheDOMElements();
        $("#icon-excel").click((e) => {
            if (this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }

            const $this = $(e.target);
            const excelParams = $.extend({}, this.globalFormData, { custName: $this.data("custname") })
            $this.attr("href", $this.data("url") + "?" + $.param(excelParams));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });

        this.renderComponents();
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
        this.winlossBetListUrl = $("#betlist-path").val() + "winloss/getdata?";

        this.tableBetList = $("#tbl-betlist");
        const errorExceedRow = $("#error-exceed-row");
        errorExceedRow.length
            && this.tableBetList.find("tbody tr:first td").addClass("error-exceed-row").html(errorExceedRow.val());

        const breadcrumbLevelJson = $("#breadcrumb-level-json").val();
        this.breadcrumbs = breadcrumbLevelJson.length ? JSON.parse(breadcrumbLevelJson) : [];
    }

    private renderComponents() {
        const currentLanguage = $("#language").data("currentlanguage");

        this.breadcrumbLevelComponent.render(new BreadcrumbLevelOption(this.breadcrumbs, (e) => {
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.tableBetList.block();
            const $this = $(e.target);
            const custLevelId = +$this.data("custlevelid");
            const breadcrumbLevel = this.breadcrumbs.filter((item) => item.custLevelId >= custLevelId);

            let reportParams: any = this.weekRangeComponent.getActiveWeekRange();
            if (breadcrumbLevel.length > 1) {
                reportParams.breadcrumbLevelJson = JSON.stringify(breadcrumbLevel);
            }
            $.extend(reportParams, this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId
            }, this.dateRangePickerComponent.getDateRangeValues());

            $this.attr("href", this.buildWinLossReportUrl(reportParams));
        }));

        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(
            (dateRange) => {
                this.constraintDateRangeAndProductDropdownList(dateRange);
                $.extend(this.globalFormData, dateRange, this.productAndProductDetailDropdownComponent.getCheckedValues());
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.getBetListAndRenderWeekRange();
            }, this.getDateRangePickerLocale(currentLanguage)));

        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(
            this.productList,
            this.productDetailList,
            (productsAndResources) => {
                $.extend(this.globalFormData, this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.getBetListAndRenderWeekRange();
            }));

        const dateRange = this.dateRangePickerComponent.getDateRangeValues();
        this.constraintDateRangeAndProductDropdownList(dateRange);

        this.weekRangeComponent.init(
            new WeekRangeOption((dateRange) => {
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.addIsWeekSelectionForStatistic();

                $.extend(this.globalFormData, dateRange);
                this.getBetList();
            }, this.getMomentLocale(currentLanguage)));

        const weekFromDate = $("#week-from-date");
        const weekToDate = $("#week-to-date");
        const activeWeekRange = weekFromDate.length > 0 && weekToDate.length > 0
            ? { weekFromDate: weekFromDate.val(), weekToDate: weekToDate.val() }
            : undefined;
        this.weekRangeComponent.render(this.globalFormData, activeWeekRange);
        // update fromDate/toDate of globalFormData if activeWeekRange is NOT undefined
        if (activeWeekRange) {
            this.globalFormData.fromDate = activeWeekRange.weekFromDate;
            this.globalFormData.toDate = activeWeekRange.weekToDate;
        }
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

    private getMomentLocale(language: string) {
        const locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };

        return locale[language] || locale.default;
    }

    private buildWinLossReportUrl(reportFormData) {
        const action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    }

    private getBetListAndRenderWeekRange() {
        this.getBetList(() => {
            this.weekRangeComponent.render(this.globalFormData, undefined);
            this.updateCurrentUrl();
        });
    }

    private getBetList(callback = Function()) {
        this.addUnselectedProductAndProductDetailForStatistic();

        this.tableBetList.block();
        $.get(this.winlossBetListUrl + $.param(this.globalFormData), (html) => {
            callback();
            this.tableBetList.find("tbody").html(html);
            this.tableBetList.unblock();
        }).fail((jqXHR) => {
            if (jqXHR.status === 429) {
                window.location.href = window.top["alpha"].urls.mainRootPath + "error/blocked";
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    window.location.href = window.top["alpha"].urls.mainRootPath + "partialum/index?" + $.param(responseObject);
                }
            } else {
                this.tableBetList.unblock();
            }
        }).always(() => this.removeIsWeekSelectionForStatistic());
    }

    private isInvalidGlobalFormData() {
        return this.globalFormData.productIds.length === 0;
    }

    private updateCurrentUrl() {
        if (this.globalFormData.productIds) {
            const formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbs) });
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
        new BreadcrumbLevelComponent(),
        new DateRangePickerJQueryComponent(),
        new ProductAndProductDetailDropdownComponent(),
        new WeekRangeComponent(),
        window.productList,
        window.productDetailList).init();
});
