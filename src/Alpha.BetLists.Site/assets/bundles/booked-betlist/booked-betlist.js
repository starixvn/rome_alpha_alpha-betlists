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
    function DateRangePickerJQueryOption(onClosed, language, selectorId, showShortcuts, autoClose) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (language === void 0) { language = "default"; }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (showShortcuts === void 0) { showShortcuts = true; }
        if (autoClose === void 0) { autoClose = true; }
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
        this.showShortcuts = showShortcuts;
        this.autoClose = autoClose;
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
        this.showShortcuts = showShortcuts;
        this.autoClose = autoClose;
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
            showShortcuts: options.showShortcuts,
            shortcuts: null,
            customShortcuts: this.getPredefinedRanges(dateRangePicker),
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true,
            maxDays: 92,
            autoClose: options.autoClose
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
var StatusDropdownOption = /** @class */ (function () {
    function StatusDropdownOption(listData, onDropdownHidden, selectorStatusId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorStatusId === void 0) { selectorStatusId = "dropdown-status"; }
        this.listData = listData;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorStatusId = selectorStatusId;
        this.listData = listData;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorStatusId = selectorStatusId;
    }
    return StatusDropdownOption;
}());
var StatusDropdownComponent = /** @class */ (function () {
    function StatusDropdownComponent() {
        this.onlyShowHistoricStatusIds = [];
    }
    StatusDropdownComponent.prototype.render = function (options) {
        this.renderDropdown(options);
        $("#" + options.selectorStatusId).closest(".form-group").removeClass("hide");
    };
    StatusDropdownComponent.prototype.renderDropdown = function (options) {
        var self = this;
        var list = options.listData.map(function (item) {
            return self.buildCheckboxItem(item, {
                productId: item.Value
            });
        });
        var dropdown = $("#" + options.selectorStatusId);
        this.dropdownStatus = dropdown.fanexMultiSelect({
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
            listData: list,
            onLeave: function () {
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
    };
    StatusDropdownComponent.prototype.buildCheckboxItem = function (item, attributes) {
        if (attributes === void 0) { attributes = undefined; }
        return {
            value: item.Value,
            text: item.Text,
            checked: item.Selected,
            attributes: attributes
        };
    };
    StatusDropdownComponent.prototype.getCheckedValues = function () {
        var ids = {
            StatusIds: this.dropdownStatus.fanexMultiSelect("getCheckedValues")
        };
        return ids;
    };
    StatusDropdownComponent.prototype.getUncheckedValues = function () {
        return {
            unselectedStatusIds: this.dropdownStatus.fanexMultiSelect("getUncheckedValues")
        };
    };
    StatusDropdownComponent.prototype.showHistoricProducts = function () {
        this.onlyShowHistoricStatusIds.length;
    };
    StatusDropdownComponent.prototype.hideHistoricProducts = function () {
        this.onlyShowHistoricStatusIds.length;
    };
    return StatusDropdownComponent;
}());
//# sourceMappingURL=status-dropdown.js.map
"use strict";
var MonitoringBetsCopyHtml = /** @class */ (function () {
    function MonitoringBetsCopyHtml() {
    }
    MonitoringBetsCopyHtml.prototype.init = function () {
        var _this = this;
        $("#icon-copy").tooltip();
        var clipboard = new Clipboard("#icon-copy", {
            text: function () { return _this._copyTableHtml($("#tbl-betlist")); }
        });
        if (window.document.queryCommandSupported && window.document.queryCommandSupported("copy")) {
            clipboard.on("success", function (e) {
                var $this = $(e.trigger);
                $this.attr("data-original-title", $("#hidden-CopiedText").val());
                $this.tooltip("show");
                setTimeout(function () {
                    $this.tooltip("hide");
                    $this.attr("data-original-title", $("#hidden-CopyText").val());
                }, 3000);
            }).on("error", function (e) {
                $(e.trigger).remove();
                console.error("Action:", e.action);
                console.error("Trigger:", e.trigger);
            });
        }
    };
    MonitoringBetsCopyHtml.prototype._copyTableHtml = function (tableReport) {
        var html = ['<table>'];
        this._appendTableHeader(tableReport, html);
        this._appendTableBody(tableReport, html);
        html.push('</table>');
        return html.join("");
    };
    MonitoringBetsCopyHtml.prototype._appendTableHeader = function (tableReport, html) {
        html.push('<thead><tr>');
        tableReport.find("thead tr:first th").each(function () {
            var $this = $(this);
            html.push('<th>', this.innerText, '</th>');
        });
        html.push('</tr></thead>');
    };
    MonitoringBetsCopyHtml.prototype._appendTableBody = function (tableReport, html) {
        html.push('<tbody>');
        tableReport.find("tbody tr").each(function () {
            html.push('<tr>');
            $(this).find("td").each(function (index) {
                html.push('<td>', index === 1 ? "'" : "", this.innerText, '</td>');
            });
            html.push('</tr>');
        });
        html.push('</tbody>');
    };
    return MonitoringBetsCopyHtml;
}());
//# sourceMappingURL=monitoring-bets-copyhtml.js.map
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