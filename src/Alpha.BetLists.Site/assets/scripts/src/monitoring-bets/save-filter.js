"use strict";
var SaveFilterView = /** @class */ (function () {
    function SaveFilterView() {
    }
    SaveFilterView.prototype.init = function (element) {
        var _this = this;
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
            saveFilterPopup_1.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(); });
            element.click(function () {
                saveFilterPopup_1.modal("show");
            });
        }
    };
    SaveFilterView.prototype.handleSaveFilterPopup = function () {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post($("#hidden-SaveFilterUrl").val(), formDataPopup, function (result) {
                _this.inlineMessagePopup.show(result);
                !result.ErrCode && Common.Utils.ReloadPageAfterTimeout();
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