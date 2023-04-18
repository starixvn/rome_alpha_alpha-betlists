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
                .removeClass("hide")
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
        this.isHistoricReport = false;
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        this.renderDateRangePicker($("#" + options.selectorId), options);
    };
    DateRangePickerJQueryComponent.prototype.renderDateRangePicker = function (dateRangePicker, options, raiseOnClosedEvent) {
        var _this = this;
        if (raiseOnClosedEvent === void 0) { raiseOnClosedEvent = false; }
        var olderThan3Months = dateRangePicker.data("label-olderthan3months");
        var backToCurrentReport = dateRangePicker.data("label-backtocurrentreport");
        dateRangePicker
            .dateRangePicker(this.getDateRangeOptions(dateRangePicker, options))
            .bind("datepicker-change", function (_, obj) {
            if (obj.value === olderThan3Months || obj.value === backToCurrentReport) {
                _this.isHistoricReport = obj.value === olderThan3Months;
                dateRangePicker.off();
                dateRangePicker.data("dateRangePicker").destroy();
                _this.renderDateRangePicker(dateRangePicker, options, true);
            }
            else {
                _this.fromDate = moment(obj.date1).format(alphaReports.format.shortDateWithSlash_DatePicker);
                _this.toDate = moment(obj.date2).format(alphaReports.format.shortDateWithSlash_DatePicker);
            }
        }).bind("datepicker-closed", function () {
            options.onClosed();
        }).data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
        raiseOnClosedEvent && options.onClosed();
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeOptions = function (dateRangePicker, options) {
        var dateRangeOptions;
        if (this.isHistoricReport) {
            var today = moment(dateRangePicker.data("currentdate"));
            this.fromDate = moment(today).startOf("M").format(alphaReports.format.shortDateWithSlash_DatePicker);
            this.toDate = today.format(alphaReports.format.shortDateWithSlash_DatePicker);
            dateRangeOptions = {
                startDate: dateRangePicker.data("historicmindate"),
                endDate: moment(today).endOf("M").format(alphaReports.format.shortDateWithSlash_DatePicker),
                customShortcuts: this.getHistoricPredefinedRanges(dateRangePicker),
                maxDays: 31,
                monthSelect: true,
                yearSelect: true
            };
        }
        else {
            this.fromDate = dateRangePicker.data("fromdate");
            this.toDate = dateRangePicker.data("todate");
            dateRangeOptions = {
                startDate: dateRangePicker.data("mindate"),
                endDate: dateRangePicker.data("maxdate"),
                customShortcuts: this.getPredefinedRanges(dateRangePicker)
            };
        }
        return $.extend({
            format: alphaReports.format.shortDateWithSlash_DatePicker,
            separator: " - ",
            language: "en",
            showShortcuts: true,
            shortcuts: null,
            duration: 0,
            singleMonth: true
        }, dateRangeOptions);
    };
    DateRangePickerJQueryComponent.prototype.getPredefinedRanges = function (dateRangePicker) {
        var minDate = moment(dateRangePicker.data("mindate"));
        var today = moment(dateRangePicker.data("currentdate"));
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var ranges = [
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
        if (dateRangePicker.data("isshowhistoricreport") === 1) {
            ranges.push({
                name: dateRangePicker.data("label-olderthan3months"),
                dates: function () {
                    return [undefined, undefined];
                }
            });
        }
        return ranges;
    };
    DateRangePickerJQueryComponent.prototype.getHistoricPredefinedRanges = function (dateRangePicker) {
        return [{
                name: dateRangePicker.data("label-backtocurrentreport"),
                dates: function () {
                    return [undefined, undefined];
                }
            }];
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate, isHistoricReport: this.isHistoricReport };
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
        this.renderProductDropdown(options);
        this.renderProductDetailDropdown(options);
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDropdown = function (options) {
        var self = this;
        var productIdWithSubProductIds = {};
        var productList = options.products.map(function (item) {
            var checkboxItem = self.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds_1.push(item.Id);
                    return self.buildCheckboxItem(item);
                });
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
                self.dropdownProductDetail.fanexMultiSelect("setSelectAll", this.checked);
            },
            onLeave: options.onDropdownHidden
        });
    };
    ProductAndProductDetailDropdownComponent.prototype.handleOnCheckProduct = function (productId, checked) {
        if (checked) {
            this.dropdownProductDetail.fanexMultiSelect("selectByCustomAttributes", { productId: productId });
        }
        else {
            this.dropdownProductDetail.fanexMultiSelect("deselectByCustomAttributes", { productId: productId });
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetailDropdown = function (options) {
        var self = this;
        var productIdsHasDetail = {};
        var productDetailList = options.productDetails.map(function (item) {
            productIdsHasDetail[item.ProductId] = true;
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
            onCheck: function () {
                var checked = this.checked;
                var productId = $(this).data("productid");
                if (checked) {
                    self.dropdownProduct.fanexMultiSelect("select", productId);
                }
                else {
                    if (!self.dropdownProductDetail.fanexMultiSelect("getSelectedItemsByCustomAttributes", { productId: productId }).length) {
                        self.dropdownProduct.fanexMultiSelect("deselect", productId);
                    }
                }
            },
            onCheckAll: function () {
                if (this.checked) {
                    self.dropdownProduct.fanexMultiSelect("select", Object.keys(productIdsHasDetail));
                }
                else {
                    self.dropdownProduct.fanexMultiSelect("deselect", Object.keys(productIdsHasDetail));
                }
            },
            onLeave: options.onDropdownHidden
        });
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
    ProductAndProductDetailDropdownComponent.prototype.getSelectedItems = function () {
        return {
            productIds: this.dropdownProduct.fanexMultiSelect("getSelectedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getSelectedValues")
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
        if (selectorId === void 0) { selectorId = "labelWeekRange"; }
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
        this.labelWeekRange = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (formData) {
        var _this = this;
        moment.locale(this.weekRangeOption.language);
        this.labelWeekRange.html(this.buildWeekRangeHtml(formData.fromDate, formData.toDate));
        var labelWeeks = this.labelWeekRange.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick({
                fromDate: $this.data("fromdate"),
                toDate: $this.data("todate")
            });
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (fromDate, toDate) {
        var weekRange = [];
        var startDate = moment(fromDate);
        var endDate = moment(toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : startDateOrEndDateStartOfWeek.format(alphaReports.format.day) + " - " + endDate.format(alphaReports.format.dayMonthName);
            weekRange.push("<a href=\"javascript:;\" class=\"label-week\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
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
            var $this = $(e.target);
            $this.attr("href", $this.data("url") + "?" + $.param(_this.globalFormData));
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
        this.breadcrumbs = JSON.parse($("#breadcrumb-level-json").val());
    };
    WinLossBetListView.prototype.renderComponents = function () {
        var _this = this;
        this.breadcrumbLevelComponent.render(new BreadcrumbLevelOption(this.breadcrumbs, function (e) {
            var $this = $(e.target);
            var custLevelId = +$this.data("custlevelid");
            var reportFormData = $.extend({}, _this.globalFormData, {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: custLevelId
            });
            var breadcrumbLevelJson = _this.breadcrumbs.filter(function (item) { return item.custLevelId >= custLevelId; });
            if (breadcrumbLevelJson.length > 1) {
                reportFormData.breadcrumbLevelJson = JSON.stringify(breadcrumbLevelJson);
            }
            $this.attr("href", _this.buildWinLossReportUrl(reportFormData));
        }));
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function () { return _this.getBetListAndRenderWeekRange(); }));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function () { return _this.getBetListAndRenderWeekRange(); }));
        this.weekRangeComponent.init(new WeekRangeOption(function (dateRange) {
            _this.getBetList($.extend(_this.globalFormData, dateRange));
        }, this.getMomentLocale()));
        this.weekRangeComponent.render(this.globalFormData);
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
        var dateRange = this.dateRangePickerComponent.getDateRangeValues();
        var productsAndResources = this.productAndProductDetailDropdownComponent.getSelectedItems();
        $.extend(this.globalFormData, dateRange, productsAndResources);
        this.getBetList(this.globalFormData, function () { return _this.weekRangeComponent.render(_this.globalFormData); });
        this.updateCurrentUrl();
    };
    WinLossBetListView.prototype.getBetList = function (formData, callback) {
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.tableBetList.block();
        $.get(this.winlossBetListUrl + $.param(formData), function (html) {
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
        });
    };
    WinLossBetListView.prototype.updateCurrentUrl = function () {
        var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbs) });
        window.top.indexManager.updateCurrentUrl($.param(formData));
    };
    return WinLossBetListView;
}());
$(function () {
    new WinLossBetListView(new BreadcrumbLevelComponent(), new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new WeekRangeComponent(), window.productList, window.productDetailList).init();
});
//# sourceMappingURL=winloss-betlist.js.map