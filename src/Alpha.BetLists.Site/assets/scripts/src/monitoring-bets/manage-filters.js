"use strict";
var ManageFiltersView = /** @class */ (function () {
    function ManageFiltersView(saveFilter) {
        this.saveFilter = saveFilter;
    }
    ManageFiltersView.prototype.init = function () {
        var _this = this;
        this.inlineMessageComponent = new InlineMessageComponent();
        $("#btn-delete-all-filters").click(function () { return _this.handleOnClickButtonDeleteAllFilters(); });
        var table = $("#table-manage-filters");
        var radioDefaultFilter = table.find("input[name=default]:checked");
        var iconCloseList = table.find(".ic-close");
        var self = this;
        table.find(".filter-name").click(function () {
            self.handleToggleCollapseExpand($(this), false);
        });
        table.find(".filter-by-tickets").click(function () {
            self.handleToggleCollapseExpand($(this));
        });
        table.find(".filter-by-sports").click(function () {
            self.handleToggleCollapseExpand($(this));
        });
        table.find(".filter-by-members").click(function () {
            self.handleToggleCollapseExpand($(this));
        });
        table.find(".ic-edit").click(function (e) { return _this.handleOnClickIconEdit(e, iconCloseList, radioDefaultFilter); });
        table.find(".ic-delete").click(function (e) { return _this.handleOnClickIconDelete(e); });
        table.find(".ic-check").click(function (e) { return _this.handleOnClickIconCheck(e); });
        iconCloseList.click(function (e) { return _this.handleOnClickIconClose($(e.target), radioDefaultFilter); });
        table.find(".treeview-sport").click(this.handleToggleSports);
        table.find(".tree-node-name").click(this.handleTreeviewCollapseExpand);
        this.saveFilter.init($("#icon-save-filter"));
    };
    ManageFiltersView.prototype.handleOnClickButtonDeleteAllFilters = function () {
        var _this = this;
        this.inlineMessageComponent.hide();
        window.top.bootboxConfirm($("#hidden-confirmMessageRemoveAllFilters").val(), undefined, function () {
            _this.handleDeleteFilters($("#hidden-allFilterIds").val());
        });
    };
    ManageFiltersView.prototype.handleToggleCollapseExpand = function ($target, getSiblingRowsWithCustomData) {
        if (getSiblingRowsWithCustomData === void 0) { getSiblingRowsWithCustomData = true; }
        var className = $target.attr("class");
        var iconArrow = $target.find(".arrow");
        var parentRow = iconArrow.closest("tr");
        var customData = getSiblingRowsWithCustomData ? "[data-" + className.replace(/-/g, "") + "=1]" : "";
        var siblingRows = parentRow.siblings("[data-filterid=" + parentRow.data("filterid") + "]" + customData);
        if (iconArrow.hasClass("right")) {
            iconArrow.removeClass("right").addClass("down");
            siblingRows.removeClass(className + "-hidden");
        }
        else {
            iconArrow.removeClass("down").addClass("right");
            siblingRows.addClass(className + "-hidden");
        }
    };
    ManageFiltersView.prototype.handleOnClickIconEdit = function (e, iconCloseList, radioDefaultFilter) {
        var _this = this;
        this.inlineMessageComponent.hide();
        // ensure inline-edit only one row
        iconCloseList.each(function (_, item) {
            var iconClose = $(item);
            iconClose.parent().hasClass("hide") || _this.handleOnClickIconClose(iconClose, radioDefaultFilter);
        });
        var iconEdit = $(e.target);
        var filterId = iconEdit.closest("tr").data("filterid");
        iconEdit.parent().addClass("hide").next().removeClass("hide");
        $("#span-filter-name-" + filterId).addClass("hide");
        var filterName = $("#input-filter-name-" + filterId).removeClass("hide").val();
        var isDefault = $("#radio-default-" + filterId).prop("disabled", false).is(":checked");
        this.originalData = { filterId: filterId, filterName: filterName, isDefault: isDefault };
    };
    ManageFiltersView.prototype.handleOnClickIconDelete = function (e) {
        var _this = this;
        this.inlineMessageComponent.hide();
        var iconBin = $(e.target);
        window.top.bootboxConfirm($("#hidden-confirmMessageRemoveFilter").val(), undefined, function () {
            var filterId = iconBin.closest("tr").data("filterid");
            _this.handleDeleteFilters(filterId);
        });
    };
    ManageFiltersView.prototype.handleDeleteFilters = function (ids) {
        var _this = this;
        AJAXHelper.post("DeleteFilters", { ids: ids }, function (results) {
            if ($.isArray(results)) {
                var errors_1 = [];
                results.forEach(function (item) {
                    item.ErrCode && errors_1.push(item.ErrMsg);
                });
                if (errors_1.length) {
                    _this.inlineMessageComponent.showError(errors_1.join("<br/>"));
                }
                else {
                    _this.inlineMessageComponent.showSuccess($("#hidden-removedSuccessfullyText").val());
                    Common.Utils.ReloadPageAfterTimeout();
                }
            }
            else {
                _this.inlineMessageComponent.showError(results.ErrMsg);
            }
        });
    };
    ManageFiltersView.prototype.handleOnClickIconCheck = function (e) {
        var _this = this;
        this.inlineMessageComponent.hide();
        var iconCheck = $(e.target);
        var filterId = iconCheck.closest("tr").data("filterid");
        var filterNameElement = $("#input-filter-name-" + filterId);
        var filterName = $.trim(filterNameElement.val());
        if (!filterName) {
            TipsyHelper.showError(filterNameElement, $("#hidden-filterNameRequiredError").val());
            return;
        }
        var formData = {
            filterId: filterId,
            filterName: filterName,
            isDefault: $("#radio-default-" + filterId).is(":checked")
        };
        if (this.originalData.filterId === formData.filterId
            && this.originalData.filterName === formData.filterName
            && this.originalData.isDefault === formData.isDefault) {
            this.inlineMessageComponent.showError($("#hidden-noChangeToUpdate").val());
        }
        else {
            AJAXHelper.post("UpdateFilter", formData, function (result) {
                _this.inlineMessageComponent.show(result);
                result.ErrCode || Common.Utils.ReloadPageAfterTimeout();
            });
        }
    };
    ManageFiltersView.prototype.handleOnClickIconClose = function (iconClose, radioDefaultFilter) {
        this.inlineMessageComponent.hide();
        TipsyHelper.clearAll();
        var filterId = iconClose.closest("tr").data("filterid");
        iconClose.parent().addClass("hide").prev().removeClass("hide");
        var filterName = $("#span-filter-name-" + filterId).removeClass("hide").text();
        $("#input-filter-name-" + filterId).addClass("hide").val(filterName);
        $("#radio-default-" + filterId).prop("disabled", true);
        // reset UI to original data
        radioDefaultFilter.prop("checked", true);
        this.originalData = {};
    };
    ManageFiltersView.prototype.handleToggleSports = function () {
        var $this = $(this);
        var iconArrow = $this.find(".arrow");
        var parentRow = $this.closest("tr");
        if (iconArrow.hasClass("right")) {
            iconArrow.removeClass("right").addClass("down");
            parentRow.next().removeClass("hide");
        }
        else {
            iconArrow.removeClass("down").addClass("right");
            parentRow.next().addClass("hide");
        }
    };
    ManageFiltersView.prototype.handleTreeviewCollapseExpand = function () {
        var $this = $(this);
        var iconArrow = $this.find(".arrow");
        var isCollapse = iconArrow.hasClass("right");
        if (isCollapse) {
            iconArrow.removeClass("right").addClass("down");
            $this.next().addClass("expand");
        }
        else {
            iconArrow.removeClass("down").addClass("right");
            $this.next().removeClass("expand");
        }
    };
    return ManageFiltersView;
}());
$(function () {
    new ManageFiltersView(new SaveFilterView()).init();
});
//# sourceMappingURL=manage-filters.js.map