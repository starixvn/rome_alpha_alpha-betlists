"use strict";

interface Window {
    bootboxConfirm: any;
}

class ManageFiltersView {
    private inlineMessageComponent: InlineMessageComponent;
    private originalData: any;

    constructor(inlineMessageComponent: InlineMessageComponent) {
        this.inlineMessageComponent = inlineMessageComponent;
    }

    init() {
        $("#btn-delete-all-filters").click(() => this.handleOnClickButtonDeleteAllFilters());

        const table = $("#table-manage-filters");
        const radioDefaultFilter = table.find("input[name=default]:checked");
        const iconCloseList = table.find(".ic-close");

        table.find(".collapse-expand,.filter-name").click(this.handleToggleFilterDetails);

        table.find(".ic-edit").click(e => this.handleOnClickIconEdit(e, iconCloseList, radioDefaultFilter));

        table.find(".ic-bin").click(e => this.handleOnClickIconBin(e));

        table.find(".ic-check").click(e => this.handleOnClickIconCheck(e));

        iconCloseList.click(e => this.handleOnClickIconClose($(e.target), radioDefaultFilter));

        table.find(".treeview-sport").click(this.handleToggleSports);

        table.find(".tree-node-name").click(this.handleTreeviewCollapseExpand);
    }

    private handleOnClickButtonDeleteAllFilters() {
        this.inlineMessageComponent.hide();
        window.top.bootboxConfirm($("#hidden-confirmMessageRemoveAllFilters").val(), undefined, () => {
            this.handleDeleteFilters($("#hidden-allFilterIds").val());
        });
    }

    private handleToggleFilterDetails(table) {
        const $this = $(this);
        const iconArrow = $this.hasClass("arrow") ? $this : $this.prev();
        const isCollapse = iconArrow.hasClass("right");
        const createdTimeColumn = iconArrow.parent().prev();
        const parentRow = iconArrow.closest("tr");
        const filterId = parentRow.data("filterid");
        const siblingRows = parentRow.siblings(`[data-filterid=${filterId}]`);

        if (isCollapse) {
            iconArrow.removeClass("right").addClass("down")
            createdTimeColumn.attr("rowspan", siblingRows.length + 1);
            $(`#arrow-sport-${filterId}`).removeClass("right").addClass("down");
            siblingRows.removeClass("hide");
        } else {
            iconArrow.removeClass("down").addClass("right");
            createdTimeColumn.removeAttr("rowspan");
            siblingRows.addClass("hide");
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
        this.originalData = { filterId, filterName, isDefault };
    }

    private handleOnClickIconBin(e) {
        this.inlineMessageComponent.hide();
        const iconBin = $(e.target);
        window.top.bootboxConfirm($("#hidden-confirmMessageRemoveFilter").val(), undefined, () => {
            const filterId = iconBin.closest("tr").data("filterid");
            this.handleDeleteFilters(filterId);
        });
    }

    private handleDeleteFilters(ids) {
        AJAXHelper.postJSON("DeleteFilters", { ids }, (results) => {
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
        const filterName = $.trim($(`#input-filter-name-${filterId}`).val());
        const formData = {
            filterId,
            filterName,
            isDefault: $(`#radio-default-${filterId}`).is(":checked")
        };

        if (this.originalData.filterId === formData.filterId
            && this.originalData.filterName === formData.filterName
            && this.originalData.isDefault === formData.isDefault) {
            this.inlineMessageComponent.showError("There is no change to update");
        } else {
            AJAXHelper.postJSON("UpdateFilter", formData, (result) => {
                this.inlineMessageComponent.show(result);
                result.ErrCode || Common.Utils.ReloadPageAfterTimeout();
            });
        }
    }

    private handleOnClickIconClose(iconClose, radioDefaultFilter) {
        this.inlineMessageComponent.hide();

        const filterId = iconClose.closest("tr").data("filterid");
        iconClose.parent().addClass("hide").prev().removeClass("hide");
        const filterName = $(`#span-filter-name-${filterId}`).removeClass("hide").text();
        $(`#input-filter-name-${filterId}`).addClass("hide").val(filterName);
        $(`#radio-default-${filterId}`).prop("disabled", true);

        // reset UI to original data
        radioDefaultFilter.prop("checked", true);
        this.originalData = {};
    }

    private handleToggleSports() {
        const $this = $(this);
        const iconArrow = $this.find(".arrow");
        const parentRow = $this.closest("tr");
        const createdTimeColumn = $(`#created-time-${parentRow.data("filterid")}`);
        let rowspan = +createdTimeColumn.attr("rowspan");

        if (iconArrow.hasClass("right")) {
            iconArrow.removeClass("right").addClass("down")
            parentRow.next().removeClass("hide")
            createdTimeColumn.attr("rowspan", ++rowspan);
        } else {
            iconArrow.removeClass("down").addClass("right");
            parentRow.next().addClass("hide");
            createdTimeColumn.attr("rowspan", --rowspan);
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
    new ManageFiltersView(new InlineMessageComponent()).init();
});
