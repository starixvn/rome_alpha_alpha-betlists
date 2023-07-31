"use strict";
declare var TipsyHelper;

class SaveFilterView {
    private inlineMessagePopup: InlineMessageComponent;
    private filterNameInput: JQuery;
    private isDefaultCheckbox: JQuery;

    init(element: JQuery) {
        if (element.length) {
            this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");

            const saveFilterPopup = $("#save-filter-popup");
            this.filterNameInput = saveFilterPopup.find("#txt-filterName");
            this.isDefaultCheckbox = saveFilterPopup.find("#chk-isDefault");
            saveFilterPopup
                .on("shown.bs.modal", () => this.filterNameInput.focus())
                .on("hide.bs.modal", () => {
                    this.filterNameInput.val("");
                    TipsyHelper.clear(this.filterNameInput);
                    this.isDefaultCheckbox.prop("checked", false);
                    this.inlineMessagePopup.hide();
                });
            saveFilterPopup.find("#btn-save-filter-popup").click(() => this.handleSaveFilterPopup());

            element.click(function () {
                saveFilterPopup.modal("show");
            });
        }
    }

    private handleSaveFilterPopup() {
        const formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post($("#hidden-SaveFilterUrl").val(), formDataPopup, (result) => {
                this.inlineMessagePopup.show(result);
                !result.ErrCode && Common.Utils.ReloadPageAfterTimeout();
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
        return {
            filterName,
            isDefault
        };
    }
}