"use strict";
var BreadcrumbLevelOption = /** @class */ (function () {
    function BreadcrumbLevelOption(onClick, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "breadcrumb-level"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return BreadcrumbLevelOption;
}());
var BreadcrumbLevelComponent = /** @class */ (function () {
    function BreadcrumbLevelComponent() {
    }
    BreadcrumbLevelComponent.prototype.init = function (options) {
        this.breadcrumbLevelOption = options;
        this._buildBreadcrumbLevels();
        this.bindEvents();
    };
    BreadcrumbLevelComponent.prototype._buildBreadcrumbLevels = function () {
        var _this = this;
        this.breadcrumbLevels = [];
        $("#" + this.breadcrumbLevelOption.selectorId).find("li").each(function (_, item) {
            var uplineLinkList = $(item).find(".upline-link");
            if (uplineLinkList.length) {
                var uplineLink = uplineLinkList.eq(0);
                _this.breadcrumbLevels.push({
                    custId: uplineLink.data("custid"),
                    custName: uplineLink.data("custname"),
                    custLevelId: +uplineLink.data("custlevelid")
                });
            }
            else {
                _this.breadcrumbLevels.push({
                    custName: item.innerText
                });
            }
        });
    };
    BreadcrumbLevelComponent.prototype.bindEvents = function () {
        $("#" + this.breadcrumbLevelOption.selectorId)
            .find(".upline-link")
            .off()
            .click(this.breadcrumbLevelOption.onClick);
    };
    BreadcrumbLevelComponent.prototype.getBreadcrumbLevels = function (custLevelId) {
        if (custLevelId === void 0) { custLevelId = 0; }
        return custLevelId
            ? this.breadcrumbLevels.filter(function (item) { return item.custLevelId >= custLevelId; })
            : this.breadcrumbLevels;
    };
    return BreadcrumbLevelComponent;
}());
//# sourceMappingURL=breadcrumb-level.js.map
"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onClosed, language, selectorId) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (language === void 0) { language = "default"; }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#" + options.selectorId);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        dateRangePicker
            .dateRangePicker({
            format: alphaReports.format.shortDateWithSlash_DatePicker,
            separator: " - ",
            language: options.language,
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: true,
            shortcuts: null,
            customShortcuts: this.getPredefinedRanges(dateRangePicker),
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true,
            maxDays: 92
        }).bind("datepicker-change", function (_, obj) {
            _this.fromDate = moment(obj.date1).format(alphaReports.format.shortDateWithSlash_DatePicker);
            _this.toDate = moment(obj.date2).format(alphaReports.format.shortDateWithSlash_DatePicker);
        })
            .bind("datepicker-closed", function () { return options.onClosed(_this.getDateRangeValues()); })
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    };
    DateRangePickerJQueryComponent.prototype.getPredefinedRanges = function (dateRangePicker) {
        var minDate = moment(dateRangePicker.data("mindate"));
        var today = moment(dateRangePicker.data("currentdate"));
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var lastMonthEndOfDay = moment(lastMonthStartOfDay).endOf("M").toDate();
        var last3MonthsStartOfDay = moment(today).subtract(3, "M").startOf("M");
        var realLastMonthStartOfDay = lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate();
        var realLast3MonthsStartOfDay = last3MonthsStartOfDay.diff(minDate) >= 0 ? last3MonthsStartOfDay.toDate() : minDate.toDate();
        var shortcuts = [
            {
                name: dateRangePicker.data("label-today"),
                dates: function () {
                    return [todayValue, todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-yesterday"),
                dates: function () {
                    var yesterdayValue = yesterday.toDate();
                    return [yesterdayValue, yesterdayValue];
                }
            },
            {
                name: dateRangePicker.data("label-thisweek"),
                dates: function () {
                    return [
                        thisWeekStartOfDay.diff(minDate) >= 0 ? thisWeekStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-lastweek"),
                dates: function () {
                    var lastWeekStartOfDay = moment(thisWeekStartOfDay).subtract(1, "w");
                    return [
                        lastWeekStartOfDay.diff(minDate) >= 0 ? lastWeekStartOfDay.toDate() : minDate.toDate(),
                        moment(thisWeekStartOfDay).subtract(1, "d").toDate()
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-thismonth"),
                dates: function () {
                    var thisMonthStartOfDay = moment(today).startOf("M");
                    return [
                        thisMonthStartOfDay.diff(minDate) >= 0 ? thisMonthStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-lastmonth"),
                dates: function () {
                    return [realLastMonthStartOfDay, lastMonthEndOfDay];
                }
            },
            {
                name: dateRangePicker.data("label-sincelastmonth"),
                dates: function () {
                    return [
                        lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            }
        ];
        if (realLastMonthStartOfDay !== realLast3MonthsStartOfDay) {
            shortcuts.push({
                name: dateRangePicker.data("label-last3months"),
                dates: function () {
                    return [realLast3MonthsStartOfDay, lastMonthEndOfDay];
                }
            });
        }
        return shortcuts;
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map
"use strict";
var ProductAndProductDetailDropdownOption = /** @class */ (function () {
    function ProductAndProductDetailDropdownOption(products, productDetails, onDropdownHidden, selectorProductId, selectorProductDetailId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorProductId === void 0) { selectorProductId = "dropdown-product"; }
        if (selectorProductDetailId === void 0) { selectorProductDetailId = "dropdown-product-detail"; }
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
    }
    return ProductAndProductDetailDropdownOption;
}());
var ProductAndProductDetailDropdownComponent = /** @class */ (function () {
    function ProductAndProductDetailDropdownComponent() {
        this.onlyShowHistoricProductIds = [];
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function (options) {
        this.renderProductDetailDropdown(options);
        this.renderProductDropdown(options);
        $("#" + options.selectorProductDetailId).closest(".form-group").removeClass("hide");
        $("#" + options.selectorProductId).closest(".form-group").removeClass("hide");
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetailDropdown = function (options) {
        var self = this;
        var productDetailList = options.productDetails.map(function (item) {
            return self.buildCheckboxItem(item, {
                productId: item.ProductId
            });
        });
        var dropdown = $("#" + options.selectorProductDetailId);
        this.dropdownProductDetail = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: productDetailList,
            onLeave: function () {
                var productIds = self.dropdownProductDetail.fanexMultiSelect("getUncheckedProductIds");
                if (productIds.length) {
                    self.dropdownProduct.fanexMultiSelect("uncheck", productIds);
                }
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDropdown = function (options) {
        var _this = this;
        var productIdWithSubProductIds = {};
        var uncheckedProductIds = [];
        var productList = options.products.map(function (item) {
            item.OnlyShowHistoric && _this.onlyShowHistoricProductIds.push(item.Id);
            var checkboxItem = _this.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    item.OnlyShowHistoric && _this.onlyShowHistoricProductIds.push(item.Id);
                    subProductIds_1.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return _this.buildCheckboxItem(item);
                });
            }
            else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }
            return checkboxItem;
        });
        var self = this;
        var dropdown = $("#" + options.selectorProductId);
        this.dropdownProduct = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: productList,
            onCheck: function () {
                var productId = this.value;
                var checked = this.checked;
                var subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    var productIdString = checked
                        ? self.dropdownProduct.fanexMultiSelect("getCheckedValues")
                        : self.dropdownProduct.fanexMultiSelect("getUncheckedValues");
                    productIdString.split(",").forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
                else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                var checked = this.checked;
                var checkedValues = self.dropdownProduct.fanexMultiSelect("getCheckedValues");
                var uncheckedValues = self.dropdownProduct.fanexMultiSelect("getUncheckedValues");
                if ((checked && !uncheckedValues.length) || (!checked && !checkedValues.length)) {
                    self.dropdownProductDetail.fanexMultiSelect("showAllItems", checked);
                    self.dropdownProductDetail.fanexMultiSelect("disable", !checked);
                }
                else {
                    var productIdString = checked
                        ? checkedValues
                        : uncheckedValues;
                    productIdString.split(",").forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
            },
            onLeave: function () {
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
        this.initState(uncheckedProductIds);
    };
    ProductAndProductDetailDropdownComponent.prototype.initState = function (uncheckedProductIds) {
        var _this = this;
        if (uncheckedProductIds.length) {
            uncheckedProductIds.forEach(function (item) { return _this.handleOnCheckProduct(item, false); });
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.handleOnCheckProduct = function (productId, checked) {
        var action = checked ? "showItemsByCustomAttributes" : "hideItemsByCustomAttributes";
        this.dropdownProductDetail.fanexMultiSelect(action, { productId: productId });
    };
    ProductAndProductDetailDropdownComponent.prototype.buildCheckboxItem = function (item, attributes) {
        if (attributes === void 0) { attributes = undefined; }
        return {
            value: item.Id,
            text: item.Name,
            checked: item.Checked,
            attributes: attributes
        };
    };
    ProductAndProductDetailDropdownComponent.prototype.getCheckedValues = function () {
        var productsAndResources = {
            productIds: this.dropdownProduct.fanexMultiSelect("getCheckedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getCheckedValues")
        };
        this.dropdownProduct.fanexMultiSelect("addError", productsAndResources.productIds.length === 0);
        return productsAndResources;
    };
    ProductAndProductDetailDropdownComponent.prototype.getUncheckedValues = function () {
        return {
            unselectedProductIds: this.dropdownProduct.fanexMultiSelect("getUncheckedValues"),
            unselectedResourceIds: this.dropdownProductDetail.fanexMultiSelect("getUncheckedValues")
        };
    };
    ProductAndProductDetailDropdownComponent.prototype.showHistoricProducts = function () {
        this.onlyShowHistoricProductIds.length
            && this.dropdownProduct.fanexMultiSelect("showItemsByValues", this.onlyShowHistoricProductIds);
    };
    ProductAndProductDetailDropdownComponent.prototype.hideHistoricProducts = function () {
        this.onlyShowHistoricProductIds.length
            && this.dropdownProduct.fanexMultiSelect("hideItemsByValues", this.onlyShowHistoricProductIds);
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map
"use strict";
var WeekSelectionOption = /** @class */ (function () {
    function WeekSelectionOption(onClick, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "week-selection"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return WeekSelectionOption;
}());
var WeekSelectionComponent = /** @class */ (function () {
    function WeekSelectionComponent() {
    }
    WeekSelectionComponent.prototype.init = function (options) {
        this.weekSelectionOption = options;
        this.bindEvents();
    };
    WeekSelectionComponent.prototype.reRender = function (html) {
        this.getWeekContainer().html(html);
        this.bindEvents();
    };
    WeekSelectionComponent.prototype.bindEvents = function () {
        var _this = this;
        var labelWeeks = this.getWeekContainer().find(".label-week:not(.disabled)");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekSelectionOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekSelectionComponent.prototype.getActiveWeek = function () {
        var activeWeek = this.getWeekContainer().find(".label-week.active");
        if (activeWeek.length) {
            var firstActiveWeek = activeWeek.eq(0);
            return {
                weekFromDate: firstActiveWeek.data("fromdate"),
                weekToDate: firstActiveWeek.data("todate"),
            };
        }
        return {};
    };
    WeekSelectionComponent.prototype.getWeekContainer = function () {
        return $("#" + this.weekSelectionOption.selectorId);
    };
    return WeekSelectionComponent;
}());
//# sourceMappingURL=week-selection.js.map
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
        this.betListWeeksUrl = betlistPath + "winloss/getweeks?";
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
            _this.getWeeks();
            _this.getBetListWeekSelection();
        }, this.getDateRangePickerLocale($("#language").data("currentlanguage"))));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function (productsAndResources) {
            $.extend(_this.globalFormData, _this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getWeeks();
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
    WinLossBetListView.prototype.getWeeks = function () {
        var _this = this;
        $.get("" + this.betListWeeksUrl + $.param(this.globalFormData), function (html) {
            return _this.weekSelectionComponent.reRender(html);
        }).fail(function (jqXHR) { return _this.handleAjaxError(jqXHR); });
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
        if (element === void 0) { element = undefined; }
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
            element && element.unblock();
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