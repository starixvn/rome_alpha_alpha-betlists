"use strict";
var AJAXHelper = /** @class */ (function () {
    function AJAXHelper() {
    }
    AJAXHelper.getJSONWithoutBlockUI = function (url, onSuccess) {
        var _this = this;
        $.getJSON(url, onSuccess).fail(function (jqXHR) { return _this.handleOnFail(jqXHR); });
    };
    AJAXHelper.post = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "POST",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); },
            complete: onComplete
        });
    };
    AJAXHelper.handleOnFail = function (jqXHR) {
        $.unblockUI();
        if (jqXHR) {
            if (jqXHR.statusText === "abort") {
                return;
            }
            if (jqXHR.status === 401) {
                window.top["age"].SignOut();
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = window.top["alpha"].urls.mainRootPath + "PartialUM/Index?umfrom=" + jsonData.UMFrom + "&umto=" + jsonData.UMTo;
                }
            }
        }
    };
    AJAXHelper.get = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "GET",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); },
            complete: onComplete
        });
    };
    return AJAXHelper;
}());
//# sourceMappingURL=ajax-helper.js.map
"use strict";
var InlineMessageComponent = /** @class */ (function () {
    function InlineMessageComponent(selectorId) {
        if (selectorId === void 0) { selectorId = "inline-message"; }
        this.inlineMessage = $("#" + selectorId);
    }
    InlineMessageComponent.prototype.getInlineMessage = function () {
        return this.inlineMessage;
    };
    InlineMessageComponent.prototype.show = function (result) {
        this.inlineMessage
            .html(result.ErrMsg)
            .removeClass("msg-error msg-success hide")
            .addClass(result.ErrCode ? "msg-error" : "msg-success");
    };
    InlineMessageComponent.prototype.showError = function (message) {
        this.show({ ErrCode: 1, ErrMsg: message });
    };
    InlineMessageComponent.prototype.showSuccess = function (message) {
        this.show({ ErrCode: 0, ErrMsg: message });
    };
    InlineMessageComponent.prototype.hide = function () {
        this.inlineMessage.addClass("hide").html("");
    };
    return InlineMessageComponent;
}());
//# sourceMappingURL=inline-message.js.map
"use strict";
var SaveFilterView = /** @class */ (function () {
    function SaveFilterView() {
    }
    SaveFilterView.prototype.init = function (element, getSelectedFilterParamsFunc) {
        var _this = this;
        if (getSelectedFilterParamsFunc === void 0) { getSelectedFilterParamsFunc = undefined; }
        if (element.length) {
            this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
            var saveFilterPopup_1 = $("#save-filter-popup");
            this.filterNameInput = saveFilterPopup_1.find("#txt-filterName");
            this.isDefaultCheckbox = saveFilterPopup_1.find("#chk-isDefault");
            saveFilterPopup_1
                .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
                .on("hide.bs.modal", function () {
                _this.filterNameInput.val("");
                TipsyHelper.clear(_this.filterNameInput);
                _this.isDefaultCheckbox.prop("checked", false);
                _this.inlineMessagePopup.hide();
            });
            saveFilterPopup_1.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(getSelectedFilterParamsFunc); });
            element.click(function () {
                saveFilterPopup_1.modal("show");
            });
        }
    };
    SaveFilterView.prototype.handleSaveFilterPopup = function (getSelectedFilterParamsFunc) {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post($("#hidden-SaveFilterUrl").val(), formDataPopup, function (result) {
                _this.inlineMessagePopup.show(result);
                if (!result.ErrCode) {
                    var url = undefined;
                    if (getSelectedFilterParamsFunc) {
                        var filterParam = getSelectedFilterParamsFunc();
                        filterParam.filterName = formDataPopup.filterName;
                        url = window.location.href.split("?")[0] + "?" + $.param(filterParam);
                    }
                    Common.Utils.ReloadPageAfterTimeout(url);
                }
            });
        }
    };
    SaveFilterView.prototype.getFormDataPopup = function () {
        var filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            TipsyHelper.showError(this.filterNameInput, $("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = this.isDefaultCheckbox.is(":checked");
        return {
            filterName: filterName,
            isDefault: isDefault
        };
    };
    return SaveFilterView;
}());
//# sourceMappingURL=save-filter.js.map
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