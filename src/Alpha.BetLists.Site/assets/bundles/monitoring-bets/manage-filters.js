"use strict";
var AJAXHelper = /** @class */ (function () {
    function AJAXHelper() {
    }
    AJAXHelper.getJSONWithoutBlockUI = function (url, onSuccess) {
        var _this = this;
        $.getJSON(url, onSuccess).fail(function (jqXHR) { return _this.handleOnFail(jqXHR); });
    };
    AJAXHelper.postJSON = function (url, formData, onSuccess) {
        var _this = this;
        $.blockUI();
        $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            dataType: "json",
            type: "POST",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); }
        });
    };
    AJAXHelper.handleOnFail = function (jqXHR) {
        $.unblockUI();
        if (jqXHR) {
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
            .addClass(result.ErrCode ? "msg-error" : "msg-success")
            .removeClass("hide");
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
var ManageFiltersView = /** @class */ (function () {
    function ManageFiltersView(inlineMessageComponent) {
        this.inlineMessageComponent = inlineMessageComponent;
    }
    ManageFiltersView.prototype.init = function () {
        var _this = this;
        $("#btn-delete-all-filters").click(function () { return _this.handleOnClickButtonDeleteAllFilters(); });
        var table = $("#table-manage-filters");
        var radioDefaultFilter = table.find("input[name=default]:checked");
        var iconCloseList = table.find(".ic-close");
        table.find(".collapse-expand,.filter-name").click(this.handleToggleFilterDetails);
        table.find(".ic-edit").click(function (e) { return _this.handleOnClickIconEdit(e, iconCloseList, radioDefaultFilter); });
        table.find(".ic-bin").click(function (e) { return _this.handleOnClickIconBin(e); });
        table.find(".ic-check").click(function (e) { return _this.handleOnClickIconCheck(e); });
        iconCloseList.click(function (e) { return _this.handleOnClickIconClose($(e.target), radioDefaultFilter); });
        table.find(".treeview-sport").click(this.handleToggleSports);
        table.find(".tree-node-name").click(this.handleTreeviewCollapseExpand);
    };
    ManageFiltersView.prototype.handleOnClickButtonDeleteAllFilters = function () {
        var _this = this;
        this.inlineMessageComponent.hide();
        window.top.bootboxConfirm($("#hidden-confirmMessageRemoveAllFilters").val(), undefined, function () {
            _this.handleDeleteFilters($("#hidden-allFilterIds").val());
        });
    };
    ManageFiltersView.prototype.handleToggleFilterDetails = function (table) {
        var $this = $(this);
        var iconArrow = $this.hasClass("arrow") ? $this : $this.prev();
        var isCollapse = iconArrow.hasClass("right");
        var createdTimeColumn = iconArrow.parent().prev();
        var parentRow = iconArrow.closest("tr");
        var filterId = parentRow.data("filterid");
        var siblingRows = parentRow.siblings("[data-filterid=" + filterId + "]");
        if (isCollapse) {
            iconArrow.removeClass("right").addClass("down");
            createdTimeColumn.attr("rowspan", siblingRows.length + 1);
            $("#arrow-sport-" + filterId).removeClass("right").addClass("down");
            siblingRows.removeClass("hide");
        }
        else {
            iconArrow.removeClass("down").addClass("right");
            createdTimeColumn.removeAttr("rowspan");
            siblingRows.addClass("hide");
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
    ManageFiltersView.prototype.handleOnClickIconBin = function (e) {
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
        AJAXHelper.postJSON("DeleteFilters", { ids: ids }, function (results) {
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
        var filterName = $.trim($("#input-filter-name-" + filterId).val());
        var formData = {
            filterId: filterId,
            filterName: filterName,
            isDefault: $("#radio-default-" + filterId).is(":checked")
        };
        if (this.originalData.filterId === formData.filterId
            && this.originalData.filterName === formData.filterName
            && this.originalData.isDefault === formData.isDefault) {
            this.inlineMessageComponent.showError("There is no change to update");
        }
        else {
            AJAXHelper.postJSON("UpdateFilter", formData, function (result) {
                _this.inlineMessageComponent.show(result);
                result.ErrCode || Common.Utils.ReloadPageAfterTimeout();
            });
        }
    };
    ManageFiltersView.prototype.handleOnClickIconClose = function (iconClose, radioDefaultFilter) {
        this.inlineMessageComponent.hide();
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
        var createdTimeColumn = $("#created-time-" + parentRow.data("filterid"));
        var rowspan = +createdTimeColumn.attr("rowspan");
        if (iconArrow.hasClass("right")) {
            iconArrow.removeClass("right").addClass("down");
            parentRow.next().removeClass("hide");
            createdTimeColumn.attr("rowspan", ++rowspan);
        }
        else {
            iconArrow.removeClass("down").addClass("right");
            parentRow.next().addClass("hide");
            createdTimeColumn.attr("rowspan", --rowspan);
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
    new ManageFiltersView(new InlineMessageComponent()).init();
});
//# sourceMappingURL=manage-filters.js.map