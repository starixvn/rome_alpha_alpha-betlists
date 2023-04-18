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
            const $this = $(e.target);
            $this.attr("href", $this.data("url") + "?" + $.param(this.globalFormData));
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
            custId: $("#cust-id").val()
        };
        this.winlossBetListUrl = $("#betlist-path").val() + "winloss/getdata?";
        
        this.tableBetList = $("#tbl-betlist");
        const errorExceedRow = $("#error-exceed-row");
        errorExceedRow.length
            && this.tableBetList.find("tbody tr:first td").addClass("error-exceed-row").html(errorExceedRow.val());

        this.breadcrumbs = JSON.parse($("#breadcrumb-level-json").val());
    }

    private renderComponents() {
        this.breadcrumbLevelComponent.render(new BreadcrumbLevelOption(this.breadcrumbs, (e) => {
            const $this = $(e.target);
            const custLevelId = +$this.data("custlevelid")
            const reportFormData = $.extend({}, this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId
            });

            const breadcrumbLevelJson = this.breadcrumbs.filter((item) => item.custLevelId >= custLevelId);
            if (breadcrumbLevelJson.length > 1) {
                reportFormData.breadcrumbLevelJson = JSON.stringify(breadcrumbLevelJson);
            }

            $this.attr("href", this.buildWinLossReportUrl(reportFormData));
        }));

        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(() => this.getBetListAndRenderWeekRange()));

        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(
            this.productList,
            this.productDetailList,
            () => this.getBetListAndRenderWeekRange()));

        this.weekRangeComponent.init(new WeekRangeOption((dateRange) => {
            this.getBetList($.extend(this.globalFormData, dateRange));
        }, this.getMomentLocale()));
        this.weekRangeComponent.render(this.globalFormData);
    }

    private getMomentLocale() {
        const locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };

        return locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    }

    private buildWinLossReportUrl(reportFormData) {
        const action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    }

    private getBetListAndRenderWeekRange() {
        const dateRange = this.dateRangePickerComponent.getDateRangeValues();
        const productsAndResources = this.productAndProductDetailDropdownComponent.getSelectedItems();
        $.extend(this.globalFormData, dateRange, productsAndResources);

        this.getBetList(this.globalFormData, () => this.weekRangeComponent.render(this.globalFormData));
        this.updateCurrentUrl();
    }

    private getBetList(formData: any, callback = Function()) {
        this.tableBetList.block();
        $.get(this.winlossBetListUrl + $.param(formData), (html) => {
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
        });
    }

    private updateCurrentUrl() {
        const formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbs) });
        window.top.indexManager.updateCurrentUrl($.param(formData));
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
