"use strict";
var SaveFilterView = /** @class */ (function () {
    function SaveFilterView() {
    }
    SaveFilterView.prototype.init = function () {
        var _this = this;
        this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
        var saveFilterPopup = $("#save-filter-popup");
        this.filterNameInput = saveFilterPopup.find("#txt-filterName");
        this.isDefaultCheckbox = saveFilterPopup.find("#chk-isDefault");
        saveFilterPopup
            .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
            .on("hide.bs.modal", function () {
            _this.filterNameInput.val("");
            TipsyHelper.clear(_this.filterNameInput);
            _this.isDefaultCheckbox.prop("checked", false);
            _this.inlineMessagePopup.hide();
        });
        saveFilterPopup.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(function () { return Common.Utils.ReloadPageAfterTimeout(_this.getCurrentUrlWithoutQueryString()); }); });
        /*saveFilterPopup.find("#btn-save-filter-apply-popup").click(
            () => this.handleSaveFilterPopup(() => {
                const url = this.getCurrentUrlWithoutQueryString();
                const mainPage = this.getMainPageUrl(url);
                Common.Utils.ReloadPageAfterTimeout(`${mainPage}?filterName=${this.addFilterFormData.filterName}`);
            })
        );*/
    };
    SaveFilterView.prototype.handleSaveFilterPopup = function (reloadPageCallback) {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post("AddFilter", formDataPopup, function (result) {
                _this.inlineMessagePopup.show(result);
                !result.ErrCode && reloadPageCallback();
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
    SaveFilterView.prototype.getCurrentUrlWithoutQueryString = function () {
        return window.location.href.split("?")[0];
    };
    return SaveFilterView;
}());
//# sourceMappingURL=save-filter.js.map