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
            this.isNotifyCheckbox = saveFilterPopup_1.find("#chk-isNotify");
            saveFilterPopup_1
                .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
                .on("hide.bs.modal", function () {
                _this.filterNameInput.val("");
                TipsyHelper.clear(_this.filterNameInput);
                _this.isDefaultCheckbox.prop("checked", false);
                _this.isNotifyCheckbox.prop("checked", false);
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
                if (result.ErrCode) {
                    $(".lnk-chatbot").click(function () { location.href = $("#chatbot-path").val(); });
                }
                else {
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
        var isNotify = this.isNotifyCheckbox.is(":checked");
        return {
            filterName: filterName,
            isDefault: isDefault,
            isNotify: isNotify
        };
    };
    return SaveFilterView;
}());
//# sourceMappingURL=save-filter.js.map