"use strict";
class BreadcrumbLevelOption {
    constructor(
        public breadcrumbs = [],
        public onClick = Function(),
        public selectorId = "breadcrumb") {
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
}

class BreadcrumbLevelComponent {
    render(options: BreadcrumbLevelOption) {
        const breadcrumbLength = options.breadcrumbs.length;
        if (breadcrumbLength > 0) {
            const html = options.breadcrumbs.map(function (item, index) {
                return index + 1 === breadcrumbLength
                    ? `<li>${item.custName}</li>`
                    : `<li><a class="upline-link" href="javascript:;" data-custid="${item.custId}" data-custname="${item.custName}" data-custlevelid="${item.custLevelId}">${item.custName}</a></li>`;
            });

            const breadcrumbSelector = $("#" + options.selectorId);
            breadcrumbSelector
                .html(html.join(""))
                .removeClass("hide")
                .find(".upline-link").off().click(options.onClick);
        }
    }
}