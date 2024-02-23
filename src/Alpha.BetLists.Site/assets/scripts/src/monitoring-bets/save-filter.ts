"use strict";
declare var TipsyHelper;

class SaveFilterView {
    private inlineMessagePopup: InlineMessageComponent;
    private filterNameInput: JQuery;
    private isDefaultCheckbox: JQuery;
    private isNotifyCheckbox: JQuery;
    init(element: JQuery,
        getSelectedFilterParamsFunc = undefined) {
        if (element.length) {
            this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");

            const saveFilterPopup = $("#save-filter-popup");
            this.filterNameInput = saveFilterPopup.find("#txt-filterName");
            this.isDefaultCheckbox = saveFilterPopup.find("#chk-isDefault");
            this.isNotifyCheckbox = saveFilterPopup.find("#chk-isNotify");

            saveFilterPopup
                .on("shown.bs.modal", () => this.filterNameInput.focus())
                .on("hide.bs.modal", () => {
                    this.filterNameInput.val("");
                    TipsyHelper.clear(this.filterNameInput);
                    this.isDefaultCheckbox.prop("checked", false);
                    this.isNotifyCheckbox.prop("checked", false);
                    this.inlineMessagePopup.hide();
                });
            saveFilterPopup.find("#btn-save-filter-popup").click(() => this.handleSaveFilterPopup(getSelectedFilterParamsFunc));

            element.click(function () {
                saveFilterPopup.modal("show");
            });
        }
    }

    private handleSaveFilterPopup(getSelectedFilterParamsFunc) {
        const formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post($("#hidden-SaveFilterUrl").val(), formDataPopup, (result) => {
                this.inlineMessagePopup.show(result);
                if (result.ErrCode) {
                    $(".lnk-chatbot").click(() => { location.href = $("#chatbot-path").val(); });
                } else {
                    let url = undefined;
                    if (getSelectedFilterParamsFunc) {
                        const filterParam = getSelectedFilterParamsFunc();
                        filterParam.filterName = formDataPopup.filterName;
                        url = `${window.location.href.split("?")[0]}?${$.param(filterParam)}`;
                    }

                    Common.Utils.ReloadPageAfterTimeout(url);
                }
            });
        }
    }

    private getFormDataPopup() {
        const filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            TipsyHelper.showError(this.filterNameInput, $("#hidden-FilterNameRequiredError").val());
            return;
        }

        const isDefault = this.isDefaultCheckbox.is(":checked");
        const isNotify = this.isNotifyCheckbox.is(":checked");
        return {
            filterName,
            isDefault,
            isNotify
        };
    }
}