"use strict";
class BreadcrumbLevelOption {
    constructor(
        public onClick = Function(),
        public selectorId = "breadcrumb-level") {
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
}

class BreadcrumbLevelComponent {
    private breadcrumbLevelOption: BreadcrumbLevelOption;
    private breadcrumbLevels;

    init(options: BreadcrumbLevelOption) {
        this.breadcrumbLevelOption = options;
        this._buildBreadcrumbLevels();
        this.bindEvents();
    }

    _buildBreadcrumbLevels() {
        this.breadcrumbLevels = [];
        $("#" + this.breadcrumbLevelOption.selectorId).find("li").each((_, item: any) => {
            const uplineLinkList = $(item).find(".upline-link");

            if (uplineLinkList.length) {
                const uplineLink = uplineLinkList.eq(0);
                this.breadcrumbLevels.push({
                    custId: uplineLink.data("custid"),
                    custName: uplineLink.data("custname"),
                    custLevelId: +uplineLink.data("custlevelid")
                });
            } else {
                this.breadcrumbLevels.push({
                    custName: item.innerText
                });
            }
        });
    }

    bindEvents() {
        $("#" + this.breadcrumbLevelOption.selectorId)
            .find(".upline-link")
            .off()
            .click(this.breadcrumbLevelOption.onClick);
    }

    getBreadcrumbLevels(custLevelId = 0) {
        return custLevelId
            ? this.breadcrumbLevels.filter((item) => item.custLevelId >= custLevelId)
            : this.breadcrumbLevels;
    }
}