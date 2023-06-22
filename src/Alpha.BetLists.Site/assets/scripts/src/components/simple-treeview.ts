"use strict";

class SimpleTreeViewComponent {
    private noDataText: JQuery;
    private selectAllContainer: JQuery;
    private selectAll: JQuery;
    private checkboxList;

    render(selectorId = "simple-treeview") {
        const selector = $(`#${selectorId}`);
        this.noDataText = selector.find(`#not-found-${selectorId}`);
        this.selectAllContainer = selector.find(".selectall-box");
        this.selectAll = this.selectAllContainer.find(`#select-all-${selectorId}`);
        this.checkboxList = selector.find(".checkbox-list :checkbox");

        this.registerEvents(selector, selectorId);
    }

    private registerEvents(selector: JQuery, selectorId: string) {
        this.selectAll.click(() => this.checkboxList.prop("checked", this.selectAll.is(":checked")));

        this.checkboxList.click((e) => {
            const allCheckboxesLength = this.checkboxList.length;
            const checkedCheckboxesLength = this.checkboxList.filter(":checked").length;
            const isSelectAll = checkedCheckboxesLength > 0 && checkedCheckboxesLength === allCheckboxesLength;
            this.selectAll.prop({
                indeterminate: !isSelectAll && checkedCheckboxesLength > 0,
                checked: isSelectAll
            });
        });

        this.registerFilterEvent(selector, selectorId);
    }

    private registerFilterEvent(selector: JQuery, selectorId: string) {
        const filterContainer = selector.find(".filter-box");
        const filterInput = filterContainer.find(`#filter-input-${selectorId}`);
        const noDataText = filterContainer.find(`#not-found-${selectorId}`);
        const selectAllContainer = this.selectAll.closest(".select-all-box");
        const handleFilterNotFound = function (notFound) {
            if (notFound) {
                noDataText.addClass("show");
                selectAllContainer.addClass("filter-hidden");
            } else {
                noDataText.removeClass("show");
                selectAllContainer.removeClass("filter-hidden");
            }
        };

        filterInput.keyup((e) => {
            e.preventDefault();
            e.which === 13 && this.handleFilterHideItems(filterInput.val().toLowerCase());
        });

        filterContainer.find(`#icon-delete-${selectorId}`).click(() => {
            if (filterInput.val().length) {
                filterInput.val("");
                this.handleFilterHideItems();
            }
        });
    }

    private handleFilterHideItems(filterValue = "") {
        let notFound = true;

        if (filterValue.length) {
            this.checkboxList.each((_, item) => {
                const checkbox = $(item);
                const treeNode = checkbox.closest(".tree-node");
                const text = checkbox.data("text");
                if (text.indexOf(filterValue) !== -1) {
                    checkbox.removeClass("filter-hidden");
                    treeNode.removeClass("filter-hidden");

                    notFound = false;
                } else {
                    checkbox.addClass("filter-hidden");
                    treeNode.addClass("filter-hidden");
                }
            });
        } else {
            this.checkboxList.each(function (_, item) {
                const checkbox = $(item).removeClass("filter-hidden");
                const treeNode = checkbox.closest(".tree-node").removeClass("filter-hidden");
            });
            notFound = false;
        }

        this.noDataText.toggleClass("show", notFound)
        this.selectAllContainer.toggleClass("filter-hidden", notFound);
    }

    public getSelected() {
        const selected = [];
        this.checkboxList.filter(":checked").each(function (_, item: any) {
            selected.push(item.value);
        });

        return selected;
    }

    public isSelectAll() {
        return this.selectAll.is(":checked");
    }
}