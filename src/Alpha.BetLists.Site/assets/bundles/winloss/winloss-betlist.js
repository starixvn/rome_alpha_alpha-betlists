"use strict";
var BreadcrumbLevelOption = /** @class */ (function () {
    function BreadcrumbLevelOption(breadcrumbs, onClick, selectorId) {
        if (breadcrumbs === void 0) { breadcrumbs = []; }
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "breadcrumb"; }
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return BreadcrumbLevelOption;
}());
var BreadcrumbLevelComponent = /** @class */ (function () {
    function BreadcrumbLevelComponent() {
    }
    BreadcrumbLevelComponent.prototype.render = function (options) {
        var breadcrumbLength = options.breadcrumbs.length;
        if (breadcrumbLength > 0) {
            var html = options.breadcrumbs.map(function (item, index) {
                return index + 1 === breadcrumbLength
                    ? "<li>" + item.custName + "</li>"
                    : "<li><a class=\"upline-link\" href=\"javascript:;\" data-custid=\"" + item.custId + "\" data-custname=\"" + item.custName + "\" data-custlevelid=\"" + item.custLevelId + "\">" + item.custName + "</a></li>";
            });
            var breadcrumbSelector = $("#" + options.selectorId);
            breadcrumbSelector
                .html(html.join(""))
                .find(".upline-link").off().click(options.onClick);
        }
    };
    return BreadcrumbLevelComponent;
}());
//# sourceMappingURL=breadcrumb-level.js.map
"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onClosed, selectorId) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.onClosed = onClosed;
        this.selectorId = selectorId;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
        this.historyMaxDateRange = 30;
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#" + options.selectorId);
        var historicPivotDateText = dateRangePicker.data("historicpivotdate");
        var historicPivotDate = moment(historicPivotDateText);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        dateRangePicker
            .dateRangePicker({
            format: alphaReports.format.shortDateWithSlash_DatePicker,
            separator: " - ",
            language: "en",
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: true,
            shortcuts: null,
            customShortcuts: this.getPredefinedRanges(dateRangePicker),
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true
        }).bind("datepicker-change", function (_, obj) {
            var startDate = moment(obj.date1);
            var endDate = moment(obj.date2);
            if (endDate.isBefore(historicPivotDate) && endDate.diff(startDate, "d") > _this.historyMaxDateRange) {
                window.top.bootboxAlert("Data before " + historicPivotDateText + " can be retrieved " + (_this.historyMaxDateRange + 1) + " days in max, please review your date range.");
                dateRangePicker.data("dateRangePicker").setDateRange(_this.fromDate, _this.toDate, false);
                return;
            }
            if (startDate.isBefore(historicPivotDate) && endDate.isSameOrAfter(historicPivotDate) && endDate.diff(startDate, "d") > _this.historyMaxDateRange) {
                window.top.bootboxAlert("Data before " + historicPivotDateText + " can be retrieved " + (_this.historyMaxDateRange + 1) + " days in max, please review your date range.");
                dateRangePicker.data("dateRangePicker").setDateRange(_this.fromDate, _this.toDate, false);
                return;
            }
            _this.fromDate = startDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            _this.toDate = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
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
        return [
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
                    return [thisWeekStartOfDay.toDate(), todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-lastweek"),
                dates: function () {
                    return [moment(thisWeekStartOfDay).subtract(1, "w").toDate(), moment(thisWeekStartOfDay).subtract(1, "d").toDate()];
                }
            },
            {
                name: dateRangePicker.data("label-thismonth"),
                dates: function () {
                    return [moment(today).startOf("M").toDate(), todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-lastmonth"),
                dates: function () {
                    return [
                        lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
                        moment(lastMonthStartOfDay).endOf("M").toDate()
                    ];
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
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function (options) {
        this.renderProductDetailDropdown(options);
        this.renderProductDropdown(options);
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
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
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
        var self = this;
        var productIdWithSubProductIds = {};
        var uncheckedProductIds = [];
        var productList = options.products.map(function (item) {
            var checkboxItem = self.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds_1.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return self.buildCheckboxItem(item);
                });
            }
            else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }
            return checkboxItem;
        });
        var dropdown = $("#" + options.selectorProductId);
        this.dropdownProduct = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            expandGroupsByDefault: false,
            listData: productList,
            onCheck: function () {
                var checked = this.checked;
                var productId = this.value;
                var subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    subProductIds.forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
                else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                var checked = this.checked;
                self.dropdownProductDetail.fanexMultiSelect("showAllItems", checked);
                self.dropdownProductDetail.fanexMultiSelect("disable", !checked);
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
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map
"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, language, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (language === void 0) { language = "en"; }
        if (selectorId === void 0) { selectorId = "week-range-container"; }
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
    }
    return WeekRangeOption;
}());
var WeekRangeComponent = /** @class */ (function () {
    function WeekRangeComponent() {
    }
    WeekRangeComponent.prototype.init = function (options) {
        this.weekRangeOption = options;
        this.weekRangeContainer = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (dateRange, activeWeekRange) {
        var _this = this;
        moment.locale(this.weekRangeOption.language);
        this.weekRangeContainer.html(this.buildWeekRangeHtml(dateRange, activeWeekRange));
        var labelWeeks = this.weekRangeContainer.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (dateRange, activeWeekRange) {
        var weekRange = [];
        var startDate = moment(dateRange.fromDate);
        var endDate = moment(dateRange.toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : startDateOrEndDateStartOfWeek.format(alphaReports.format.day) + " - " + endDate.format(alphaReports.format.dayMonthName);
            var activeClassName = activeWeekRange
                && activeWeekRange.weekFromDate === fromDateValue
                && activeWeekRange.weekToDate === toDateValue ? " active" : "";
            weekRange.push("<a href=\"javascript:;\" class=\"label-week" + activeClassName + "\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
    };
    WeekRangeComponent.prototype.getActiveWeekRange = function () {
        var activeWeekRange = this.weekRangeContainer.find(".label-week.active");
        if (activeWeekRange.length) {
            var firstActiveWeekRange = activeWeekRange.eq(0);
            return {
                weekFromDate: firstActiveWeekRange.data("fromdate"),
                weekToDate: firstActiveWeekRange.data("todate"),
            };
        }
        return {};
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map
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
            if (_this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }
            var $this = $(e.target);
            var excelParams = $.extend({}, _this.globalFormData, { custName: $this.data("custname") });
            $this.attr("href", $this.data("url") + "?" + $.param(excelParams));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });
        this.renderComponents();
    };
    WinLossBetListView.prototype.cacheDOMElements = function () {
        this.globalFormData = {
            fromDate: $("#from-date").val(),
            toDate: $("#to-date").val(),
            productIds: $("#product-ids").val(),
            resourceIds: $("#resource-ids").val(),
            custId: $("#cust-id").val()
        };
        this.winlossBetListUrl = $("#betlist-path").val() + "winloss/getdata?";
        this.tableBetList = $("#tbl-betlist");
        var errorExceedRow = $("#error-exceed-row");
        errorExceedRow.length
            && this.tableBetList.find("tbody tr:first td").addClass("error-exceed-row").html(errorExceedRow.val());
        var breadcrumbLevelJson = $("#breadcrumb-level-json").val();
        this.breadcrumbs = breadcrumbLevelJson.length ? JSON.parse(breadcrumbLevelJson) : [];
    };
    WinLossBetListView.prototype.renderComponents = function () {
        var _this = this;
        this.breadcrumbLevelComponent.render(new BreadcrumbLevelOption(this.breadcrumbs, function (e) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.tableBetList.block();
            var $this = $(e.target);
            var custLevelId = +$this.data("custlevelid");
            var breadcrumbLevel = _this.breadcrumbs.filter(function (item) { return item.custLevelId >= custLevelId; });
            var reportParams = _this.weekRangeComponent.getActiveWeekRange();
            if (breadcrumbLevel.length > 1) {
                reportParams.breadcrumbLevelJson = JSON.stringify(breadcrumbLevel);
            }
            $.extend(reportParams, _this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: custLevelId
            }, _this.dateRangePickerComponent.getDateRangeValues());
            $this.attr("href", _this.buildWinLossReportUrl(reportParams));
        }));
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function (dateRange) {
            $.extend(_this.globalFormData, dateRange);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListAndRenderWeekRange();
        }));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function (productsAndResources) {
            $.extend(_this.globalFormData, _this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getBetListAndRenderWeekRange();
        }));
        this.weekRangeComponent.init(new WeekRangeOption(function (dateRange) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.addIsWeekSelectionForStatistic();
            $.extend(_this.globalFormData, dateRange);
            _this.getBetList();
        }, this.getMomentLocale()));
        var weekFromDate = $("#week-from-date");
        var weekToDate = $("#week-to-date");
        var activeWeekRange = weekFromDate.length > 0 && weekToDate.length > 0
            ? { weekFromDate: weekFromDate.val(), weekToDate: weekToDate.val() }
            : undefined;
        this.weekRangeComponent.render(this.globalFormData, activeWeekRange);
        // update fromDate/toDate of globalFormData if activeWeekRange is NOT undefined
        if (activeWeekRange) {
            this.globalFormData.fromDate = activeWeekRange.weekFromDate;
            this.globalFormData.toDate = activeWeekRange.weekToDate;
        }
    };
    WinLossBetListView.prototype.getMomentLocale = function () {
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };
        return locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    };
    WinLossBetListView.prototype.buildWinLossReportUrl = function (reportFormData) {
        var action = reportFormData.custLevelId === 4 ? "master" : (reportFormData.custLevelId === 3 ? "agent" : "member");
        return $("#winloss-detail-report-path").val() + action + "new?" + $.param(reportFormData);
    };
    WinLossBetListView.prototype.getBetListAndRenderWeekRange = function () {
        var _this = this;
        this.getBetList(function () {
            _this.weekRangeComponent.render(_this.globalFormData, undefined);
            _this.updateCurrentUrl();
        });
    };
    WinLossBetListView.prototype.getBetList = function (callback) {
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.addUnselectedProductAndProductDetailForStatistic();
        this.tableBetList.block();
        $.get(this.winlossBetListUrl + $.param(this.globalFormData), function (html) {
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
        }).always(function () { return _this.removeIsWeekSelectionForStatistic(); });
    };
    WinLossBetListView.prototype.isInvalidGlobalFormData = function () {
        return this.globalFormData.productIds.length === 0;
    };
    WinLossBetListView.prototype.updateCurrentUrl = function () {
        if (this.globalFormData.productIds) {
            var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbs) });
            window.top.indexManager.updateCurrentUrl($.param(formData));
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
    new WinLossBetListView(new BreadcrumbLevelComponent(), new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new WeekRangeComponent(), window.productList, window.productDetailList).init();
});
//# sourceMappingURL=winloss-betlist.js.map