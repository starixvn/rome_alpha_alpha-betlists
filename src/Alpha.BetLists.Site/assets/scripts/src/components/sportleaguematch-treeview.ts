"use strict";

class SportLeagueMatchTreeViewOption {
    constructor(
        public selectorId = "sportleaguematch-treeview",
        public filterPlaceholder = "Search Sport/ League/ Match",
        public filterNotFoundText = "No Sport/ League/ Match found",
        public selectAllText = "Select All",
        public noDataText = "No information is available",
        public expandGroupsByDefault = true,
        public maxHeight = 400) {
        this.selectorId = selectorId;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.noDataText = noDataText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
    }
}

class SportLeagueMatchTreeViewComponent {
    private treeviewOption: SportLeagueMatchTreeViewOption;
    private filterInput: JQuery;
    private notFoundText: JQuery;
    private selectAllContainer: JQuery;
    private selectAll: JQuery;
    private checkboxListContainer: JQuery;
    private checkboxList;

    render(option: SportLeagueMatchTreeViewOption) {
        this.treeviewOption = option;

        const selector = $(`#${this.treeviewOption.selectorId}`)
            .addClass("treeview")
            .html(this.renderTreeviewNoData());
        this.filterInput = selector.find(`#filter-input-${this.treeviewOption.selectorId}`);
        this.notFoundText = selector.find(`#not-found-${this.treeviewOption.selectorId}`);
        this.selectAllContainer = selector.find(".selectall-box");
        this.selectAll = selector.find(`#select-all-${this.treeviewOption.selectorId}`);
        this.checkboxListContainer = selector.find(".checkbox-list");
        this.checkboxList = this.checkboxListContainer.find(":checkbox");

        this.bindPersistentEvents(selector);
        this.disableFilerAndSelectAll(true);
    }

    private renderTreeviewNoData() {
        const html = [];
        this.appendFilter(html);
        this.appendSelectAll(html);
        html.push(
            '<ul class="checkbox-list" style="max-height:',
            this.treeviewOption.maxHeight,
            'px">',
            this.renderNoData(),
            '</ul>'
        );

        return html.join("");
    }

    private appendFilter(html) {
        const filterId = `filter-input-${this.treeviewOption.selectorId}`;
        const iconDeleteId = `icon-delete-${this.treeviewOption.selectorId}`;
        const notFoundTextId = `not-found-${this.treeviewOption.selectorId}`;

        html.push(
            '<div class="filter-box">',
            '<input type="text" id="',
            filterId,
            '" placeholder="',
            this.treeviewOption.filterPlaceholder,
            '" autocomplete="off"/><span class="icon-close" id="',
            iconDeleteId,
            '"></span><div id="',
            notFoundTextId,
            '" class="not-found">',
            this.treeviewOption.filterNotFoundText,
            '</div></div>'
        );
    }

    private appendSelectAll(html) {
        const selectAllId = `select-all-${this.treeviewOption.selectorId}`;
        const collapseExpandAllId = `icon-dropdown-all-${this.treeviewOption.selectorId}`;
        html.push('<div class="selectall-box">');

        if (this.enableCollapseExpandAll()) {
            html.push(
                '<span id="',
                collapseExpandAllId,
                '" class="icon-dropdown-all',
                this.treeviewOption.expandGroupsByDefault ? " expand" : "",
                '"></span>'
            );
        }

        html.push(
            '<label class="node-label"><input type="checkbox" id="',
            selectAllId,
            '"/>',
            this.treeviewOption.selectAllText,
            '</label></div>'
        );
    }

    private enableCollapseExpandAll() {
        //const listData = this.treeviewOption.listData;

        //for (let i = 0, max = listData.length; i < max; i++) {
        //    if (listData[i].items && listData[i].items.length > 0) {
        //        return true;
        //    }
        //}

        return false;
    }

    private renderNoData() {
        return '<li><label class="node-label">' + this.treeviewOption.noDataText + '</label></li>';
    }

    private bindPersistentEvents(selector: JQuery) {
        this.filterInput.keyup((e) => {
            e.preventDefault();
            e.which === 13 && this.handleFiltering(this.filterInput.val().toLowerCase());
        });

        selector.find(`#icon-delete-${this.treeviewOption.selectorId}`).click(() => {
            if (this.filterInput.val().length) {
                this.filterInput.val("");
                this.handleFiltering();
            }
        });

        this.selectAll.click(() => {
            this.checkboxList.prop("checked", this.selectAll.is(":checked"));
        });
    }

    private handleFiltering(filterValue = "") {
        let notFound = true;

        if (filterValue.length) {
            this.checkboxList.each((_, item) => {
                const checkbox = $(item);
                const treeNode = checkbox.closest(".tree-node");
                const text = checkbox.data("text");
                if (text.indexOf(filterValue) !== -1) {
                    this.showParentTreeNode(checkbox);
                    treeNode.removeClass("filter-hidden");
                    notFound = false;
                } else {
                    treeNode.addClass("filter-hidden");
                }
            });
        } else {
            this.checkboxList.each(function (_, item) {
                $(item).closest(".tree-node").removeClass("filter-hidden");
            });
            notFound = false;
        }

        this.notFoundText.toggleClass("show", notFound)
        this.selectAllContainer.toggleClass("filter-hidden", notFound);
    }

    private showParentTreeNode(checkbox: JQuery) {
        checkbox.parents(".subtree").each(function (_, item) {
            const subtree = $(item);
            //subtree.prev().find(":checkbox").removeClass("filter-hidden");
            subtree.parent().removeClass("filter-hidden");
        });
    }

    private appendTreeNode(html, data) {
        const value = data.value || "";

        if (value) {
            const isHasChild = data.items && data.items.length > 0;
            html.push('<li class="tree-node">');

            if (isHasChild) {
                html.push(
                    '<span class="icon-dropdown',
                    this.treeviewOption.expandGroupsByDefault ? " expand" : "",
                    '"></span>',
                    this.renderCheckbox(data, isHasChild),
                    '<ul class="subtree',
                    this.treeviewOption.expandGroupsByDefault ? " expand" : "",
                    '">'
                );

                data.items.forEach((item) => html.push(this.appendTreeNode(html, item)));

                html.push("</ul>");
            } else {
                html.push(this.renderCheckbox(data, isHasChild));
            }

            html.push("</li>");
        }
    }

    private renderCheckbox(data, isHasChild: boolean) {
        const title = data.text || "";
        const attributes = data.attributes || {};
        const html = [
            '<label class="node-label"><input type="checkbox" data-text="',
            title.toLowerCase(),
            '" value="',
            data.value,
            '"',
            isHasChild ? ' class="checkbox-subtree"' : "",
            data.checked ? " checked" : ""
        ];
        Object.keys(attributes).forEach(function (key) {
            html.push(' ', key, '="', attributes[key], '"');
        });
        html.push('/><span>', title, '</span>');
        data.subText &&html.push('<span>', data.subText, '</span>');
        html.push('</label>');

        return html.join("");
    }

    private bindEphemeralEvents() {
        this.checkboxList.off().click((e) => {
            this.toggleCheckbox($(e.target));
            this.updateSelectAll();
        });

        const iconDropdownAll = this.selectAllContainer.find(`#icon-dropdown-all-${this.treeviewOption.selectorId}`);
        const iconDropdownList = this.checkboxListContainer.find(".icon-dropdown");

        iconDropdownAll.off().click((e) => {
            const icon = $(e.target);
            const isExpand = icon.hasClass("expand");
            iconDropdownList.each((_, item) => this.collapseExpand($(item), !isExpand));
            icon.toggleClass("expand", !isExpand);
        });

        iconDropdownList.off().click((e) => {
            const icon = $(e.target);
            const isExpand = icon.hasClass("expand");
            this.collapseExpand(icon, !isExpand);
            //if (isExpand) {
            //    if (!iconDropdownList.filter(":not(.expand)").length) {
            //        iconDropdownAll.addClass("expand");
            //    }
            //} else {
            //    if (!iconDropdownList.filter(".expand").length) {
            //        iconDropdownAll.removeClass("expand");
            //    }
            //}
        });
    }

    private collapseExpand(icon: JQuery, isExpand: boolean) {
        icon.next().next().toggleClass("expand", isExpand)
        icon.toggleClass("expand", isExpand)
    }

    private toggleCheckbox(checkbox: JQuery) {
        const checked = checkbox.is(":checked");

        // check/uncheck children
        checkbox.hasClass("checkbox-subtree") &&
            checkbox.parent().next(".subtree").find(":checkbox").prop("checked", checked);

        // check/uncheck parents
        checkbox.parents(".subtree").each((_, item) => {
            const subtree = $(item);
            const subtreeCheckbox = subtree.prev().find(":checkbox");
            const subtreeCheckboxes = subtree.find(":checkbox");

            this.updateCheckboxStatus(subtreeCheckbox, subtreeCheckboxes);
        });
    }

    private updateSelectAll() {
        const allCheckboxes = this.checkboxList.filter(":not(.filter-hidden,.checkbox-subtree)");
        this.updateCheckboxStatus(this.selectAll, allCheckboxes);
    }

    private updateCheckboxStatus(checkbox: JQuery, allCheckboxes) {
        const allCheckboxesLength = allCheckboxes.length;
        const checkedCheckboxesLength = allCheckboxes.filter(":checked").length;
        const isSelectAll = checkedCheckboxesLength > 0 && checkedCheckboxesLength === allCheckboxesLength;
        checkbox.prop({
            indeterminate: !isSelectAll && checkedCheckboxesLength > 0,
            checked: isSelectAll
        });
    }

    private disableFilerAndSelectAll(disable) {
        this.filterInput.prop("disabled", disable);
        this.selectAll.prop("disabled", disable);
        const selectAllLabel = this.selectAll.parent();
        if (disable) {
            selectAllLabel.attr("disabled", "disabled");
        } else {
            selectAllLabel.removeAttr("disabled");
        }
    }

    public addIconLoading() {
        this.checkboxListContainer.html('<li><span class="icon-loading"></span></li>');
    }

    public renderTreeNodeList(listData) {
        const html = []
        const hasListData = listData.length > 0;
        if (hasListData) {
            listData.forEach((item) => this.appendTreeNode(html, item));
        } else {
            html.push(this.renderNoData());
        }

        this.checkboxListContainer.html(html.join(""));
        this.checkboxList = this.checkboxListContainer.find(":checkbox");

        if (hasListData) {
            this.bindEphemeralEvents();
            this.updateSelectAll();
        }

        this.disableFilerAndSelectAll(!hasListData);
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