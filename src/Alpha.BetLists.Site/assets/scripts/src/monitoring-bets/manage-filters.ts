"use strict";

interface Window {
    bootboxConfirm: any;
}

class ManageFiltersView {
    private inlineMessageComponent: InlineMessageComponent;
    private originalData: any;
    private saveFilter: SaveFilterView;
    private topWindow: any;

    constructor(saveFilter: SaveFilterView, topWindow: any) {
        this.saveFilter = saveFilter;
        this.topWindow = topWindow;
    }

    init() {
        this.inlineMessageComponent = new InlineMessageComponent();
        $("#btn-delete-all-filters").click(() => this.handleOnClickButtonDeleteAllFilters());

        const table = $("#table-manage-filters");
        const radioDefaultFilter = table.find("input[name=default]:checked");
        const iconCloseList = table.find(".ic-close");

        const self = this;
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

        table.find(".div-notify").click(e => this.handleOnClickDivNotify(e, radioDefaultFilter));
        table.find(".ic-edit").click(e => this.handleOnClickIconEdit(e, iconCloseList, radioDefaultFilter));

        table.find(".ic-delete").click(e => this.handleOnClickIconDelete(e));

        table.find(".ic-check").click(e => this.handleOnClickIconCheck(e));

        iconCloseList.click(e => this.handleOnClickIconClose($(e.target), radioDefaultFilter));

        table.find(".treeview-sport").click(this.handleToggleSports);

        table.find(".tree-node-name").click(this.handleTreeviewCollapseExpand);

        this.saveFilter.init($("#icon-save-filter"));
    }

    private handleOnClickButtonDeleteAllFilters() {
        this.inlineMessageComponent.hide();
        try {
            window.top.bootboxConfirm($("#hidden-confirmMessageRemoveAllFilters").val(), undefined, () => {
                this.handleDeleteFilters($("#hidden-allFilterIds").val());
            });
        } catch (e) { // internal cannot access window.top (error: cross-origin frame)
            if (confirm($("#hidden-confirmMessageRemoveAllFilters").val())) {
                this.handleDeleteFilters($("#hidden-allFilterIds").val());
            }
        }
    }

    private handleToggleCollapseExpand($target, getSiblingRowsWithCustomData = true) {
        const className = $target.attr("class");
        const iconArrow = $target.find(".arrow");
        const parentRow = iconArrow.closest("tr");
        const customData = getSiblingRowsWithCustomData ? `[data-${className.replace(/-/g, "")}=1]` : "";
        const siblingRows = parentRow.siblings(`[data-filterid=${parentRow.data("filterid")}]${customData}`);

        if (iconArrow.hasClass("right")) {
            iconArrow.removeClass("right").addClass("down")
            siblingRows.removeClass(`${className}-hidden`);
        } else {
            iconArrow.removeClass("down").addClass("right");
            siblingRows.addClass(`${className}-hidden`);
        }
    }

    private handleOnClickIconEdit(e, iconCloseList, radioDefaultFilter) {
        this.inlineMessageComponent.hide();

        // ensure inline-edit only one row
        iconCloseList.each((_, item) => {
            const iconClose = $(item);
            iconClose.parent().hasClass("hide") || this.handleOnClickIconClose(iconClose, radioDefaultFilter);
        });

        const iconEdit = $(e.target);
        const filterId = iconEdit.closest("tr").data("filterid");
        iconEdit.parent().addClass("hide").next().removeClass("hide");
        $(`#span-filter-name-${filterId}`).addClass("hide");
        const filterName = $(`#input-filter-name-${filterId}`).removeClass("hide").val();
        const isDefault = $(`#radio-default-${filterId}`).prop("disabled", false).is(":checked");
        const isNotify = $(`#div-notify-${filterId}`).hasClass("ic-notification");
        $(`#radio-default-${filterId}`).prop("disabled", false);
        this.originalData = { filterId, filterName, isDefault, isNotify };
    }

    private handleOnClickDivNotify(e, radioDefaultFilter) {
        const divNotify = $(e.target);
        const filterId = divNotify.closest("tr").data("filterid");
        const isNotify = $(`#div-notify-${filterId}`).hasClass("ic-notification");
        const message = isNotify ? $("#confirmTurnOffNotification").val() : $("#confirmTurnOnNotification").val();

        this.topWindow.bootboxConfirm(
            message,
            undefined,
            () => {
                const filterNameElement = $(`#input-filter-name-${filterId}`);
                const filterName = $.trim(filterNameElement.val());
                if (!filterName) {
                    TipsyHelper.showError(filterNameElement, $("#hidden-filterNameRequiredError").val());
                    return;
                }

                const isDefault = $(`#radio-default-${filterId}`).is(":checked");
                this.originalData = { filterId, filterName, isDefault, isNotify: !isNotify };
                this.handleOnClickIconClose(divNotify.closest("tr").find(".ic-close"), radioDefaultFilter);

                const formData = {
                    filterId,
                    filterName,
                    isDefault: isDefault,
                    isNotify: !isNotify
                };

                AJAXHelper.post("UpdateFilter", formData, (result) => {
                    this.inlineMessageComponent.show(result);
                    this.originalData.isNotify = isNotify;
                    if (result.ErrCode) {
                        divNotify.removeClass("ic-notification").removeClass("ic-notification-off");
                        divNotify.addClass(this.originalData.isNotify ? "ic-notification" : "ic-notification-off");
                        $(".lnk-chatbot").click(() => { location.href = $("#chatbot-path").val(); });
                    } else {
                        Common.Utils.ReloadPageAfterTimeout();
                    }
                });
            });
    }

    private handleOnClickRadioDefault(e, radioDefaultFilter) {
        const radioDefault = $(e.target);

        const filterId = radioDefault.closest("tr").data("filterid");
        const filterNameElement = $(`#input-filter-name-${filterId}`);
        const filterName = $.trim(filterNameElement.val());
        if (!filterName) {
            TipsyHelper.showError(filterNameElement, $("#hidden-filterNameRequiredError").val());
            return;
        }

        const isDefault = $(`#radio-default-${filterId}`).is(":checked");
        const isNotify = $(`#div-notify-${filterId}`).hasClass("ic-notification");
        this.originalData = { filterId, filterName, isDefault, isNotify };
        this.handleOnClickIconClose(radioDefault.closest("tr").find(".ic-close"), radioDefault);

        const formData = {
            filterId,
            filterName,
            isDefault: $(`#radio-default-${filterId}`).is(":checked"),
            isNotify: isNotify
        };

        AJAXHelper.post("UpdateFilter", formData, (result) => {
            this.inlineMessageComponent.show(result);
            result.ErrCode || Common.Utils.ReloadPageAfterTimeout();
        });
    }

    private handleOnClickIconDelete(e) {
        this.inlineMessageComponent.hide();
        const iconBin = $(e.target);
        try {
            window.top.bootboxConfirm($("#hidden-confirmMessageRemoveFilter").val(), undefined, () => {
                const filterId = iconBin.closest("tr").data("filterid");
                this.handleDeleteFilters(filterId);
            });
        } catch (e) { // internal cannot access window.top (error: cross-origin frame)
            if (confirm($("#hidden-confirmMessageRemoveFilter").val())) {
                const filterId = iconBin.closest("tr").data("filterid");
                this.handleDeleteFilters(filterId);
            }
        }
    }

    private handleDeleteFilters(ids) {
        AJAXHelper.post("DeleteFilters", { ids }, (results) => {
            if ($.isArray(results)) {
                const errors = [];

                results.forEach(function (item) {
                    item.ErrCode && errors.push(item.ErrMsg);
                });

                if (errors.length) {
                    this.inlineMessageComponent.showError(errors.join("<br/>"));
                } else {
                    this.inlineMessageComponent.showSuccess($("#hidden-removedSuccessfullyText").val());
                    Common.Utils.ReloadPageAfterTimeout();
                }
            } else {
                this.inlineMessageComponent.showError(results.ErrMsg);
            }
        });
    }

    private handleOnClickIconCheck(e) {
        this.inlineMessageComponent.hide();

        const iconCheck = $(e.target);
        const filterId = iconCheck.closest("tr").data("filterid");
        const filterNameElement = $(`#input-filter-name-${filterId}`);
        const filterName = $.trim(filterNameElement.val());
        if (!filterName) {
            TipsyHelper.showError(filterNameElement, $("#hidden-filterNameRequiredError").val());
            return;
        }

        const formData = {
            filterId,
            filterName,
            isDefault: $(`#radio-default-${filterId}`).is(":checked"),
            isNotify: $(`#div-notify-${filterId}`).hasClass("ic-notification")
        };

        if (this.originalData.filterId === formData.filterId
            && this.originalData.filterName === formData.filterName
            && this.originalData.isDefault === formData.isDefault
            && this.originalData.isNotify === formData.isNotify) {
            this.inlineMessageComponent.showError($("#hidden-noChangeToUpdate").val());
        } else {
            AJAXHelper.post("UpdateFilter", formData, (result) => {
                this.inlineMessageComponent.show(result);
                result.ErrCode || Common.Utils.ReloadPageAfterTimeout();
            });
        }
    }

    private handleOnClickIconClose(iconClose, radioDefaultFilter) {
        this.inlineMessageComponent.hide();
        TipsyHelper.clearAll();

        const filterId = iconClose.closest("tr").data("filterid");
        iconClose.parent().addClass("hide").prev().removeClass("hide");
        const filterName = $(`#span-filter-name-${filterId}`).removeClass("hide").text();
        $(`#input-filter-name-${filterId}`).addClass("hide").val(filterName);
        $(`#radio-default-${filterId}`).prop("disabled", true);

        const divNotify = $(`#div-notify-${filterId}`);
        divNotify.removeClass("ic-notification").removeClass("ic-notification-off");
        divNotify.addClass(this.originalData.isNotify ? "ic-notification" : "ic-notification-off");

        // reset UI to original data
        radioDefaultFilter.prop("checked", true);
        this.originalData = {};
    }

    private handleToggleSports() {
        const $this = $(this);
        const iconArrow = $this.find(".arrow");
        const parentRow = $this.closest("tr");

        if (iconArrow.hasClass("right")) {
            iconArrow.removeClass("right").addClass("down")
            parentRow.next().removeClass("hide")
        } else {
            iconArrow.removeClass("down").addClass("right");
            parentRow.next().addClass("hide");
        }
    }

    private handleTreeviewCollapseExpand() {
        const $this = $(this);
        const iconArrow = $this.find(".arrow");
        const isCollapse = iconArrow.hasClass("right");

        if (isCollapse) {
            iconArrow.removeClass("right").addClass("down")
            $this.next().addClass("expand")
        } else {
            iconArrow.removeClass("down").addClass("right");
            $this.next().removeClass("expand");
        }
    }
}

$(function () {
    new ManageFiltersView(new SaveFilterView(), window.top).init();
});
