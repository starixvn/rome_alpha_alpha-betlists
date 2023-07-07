"use strict";

class SportLeagueMatchTreeViewOption {
    constructor(
        public selectorId = "sportleaguematch-treeview",
        public filterPlaceholder = "Search Sport/ League",
        public filterNotFoundText = "No Sport/ League found",
        public selectAllText = "Select All",
        public noDataText = "No information is available",
        public expandGroupsByDefault = false,
        public maxHeight = 400,
        public expandAllText = "Expand All",
        public selectedOnlyText = "Selected Only") {
        this.selectorId = selectorId;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.noDataText = noDataText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
        this.expandAllText = expandAllText;
        this.selectedOnlyText = selectedOnlyText;
    }
}

class SportLeagueMatchTreeViewComponent {
    private treeviewOption: SportLeagueMatchTreeViewOption;
    private filterInput: JQuery;
    private notFoundText: JQuery;
    private selectAllContainer: JQuery;
    private selectAll: JQuery;
    private checkboxListContainer: JQuery;

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
        const selectedOnlyId = `selected-only-${this.treeviewOption.selectorId}`;
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
            '</label>',
            '<label class="node-label"><input type="checkbox" id="',
            collapseExpandAllId,
            '"/>',
            this.treeviewOption.expandAllText,
            '</label>',
            '<label class="node-label"><input type="checkbox" id="',
            selectedOnlyId,
            '"/>',
            this.treeviewOption.selectedOnlyText,
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
            const checked = this.selectAll.is(":checked");
            this.checkboxListContainer.find(":checkbox").prop({
                indeterminate: false,
                checked: checked
            });
        });
    }

    private handleFiltering(filterValue = "") {
        let notFound = true;

        if (filterValue.length) {
            this.checkboxListContainer.find(".tree-node").each((_, item) => {
                const treeNode = $(item);
                const text = treeNode.data("text");
                if (text.indexOf(filterValue) !== -1) {
                    this.showParentTreeNode(treeNode);
                    treeNode.removeClass("filter-hidden");
                    notFound = false;
                } else {
                    treeNode.addClass("filter-hidden");
                }
            });
        } else {
            this.checkboxListContainer.find(".tree-node").removeClass("filter-hidden");
            notFound = false;
        }

        this.notFoundText.toggleClass("show", notFound)
        this.selectAllContainer.toggleClass("filter-hidden", notFound);
    }

    private showParentTreeNode(treeNode: JQuery) {
        treeNode.parents(".subtree").each(function (_, item) {
            $(item).parent().removeClass("filter-hidden");
        });
    }

    private appendTreeNode(html, data, level: number) {
        const isHasChild = data.items && data.items.length > 0;
        html.push('<li class="tree-node" data-text="', data.text.toLowerCase(), '">');

        if (isHasChild) {
            html.push(
                '<span class="icon-dropdown',
                this.treeviewOption.expandGroupsByDefault ? " expand" : "",
                '"></span>',
                this.renderCheckbox(data, isHasChild, level),
                '<ul class="subtree',
                this.treeviewOption.expandGroupsByDefault ? " expand" : "",
                '">'
            );

            level++;
            data.items.forEach((item) => html.push(this.appendTreeNode(html, item, level)));

            html.push("</ul>");
        } else {
            html.push(this.renderCheckbox(data, isHasChild, level));
        }

        html.push("</li>");
    }

    private renderCheckbox(data, isHasChild: boolean, level: number) {
        return [
            '<label class="node-label"><input type="checkbox" value="',
            data.value,
            '"',
            isHasChild ? ' class="checkbox-subtree"' : "",
            ' data-level="',
            level,
            '"/>',
            data.text,
            '</label>'
        ].join("");
    }

    private bindEphemeralEvents() {
        this.checkboxListContainer.find(":checkbox").off().click((e) => {
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
        const allCheckboxes = this.checkboxListContainer.find(":checkbox:not(.checkbox-subtree)");
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
        this.disableFilerAndSelectAll(true);
    }

    public renderTreeNodeList(listData) {
        const html = []
        const hasListData = listData.length > 0;
        if (hasListData) {
            listData.forEach((item) => this.appendTreeNode(html, item, 0));
        } else {
            html.push(this.renderNoData());
        }

        this.checkboxListContainer.html(html.join(""));

        if (hasListData) {
            this.bindEphemeralEvents();
            this.updateSelectAll();
        }

        this.disableFilerAndSelectAll(!hasListData);
    }

    public getSelected() {
        const selected = [];
        this.checkboxListContainer.find(":checkbox:checked").each(function (_, item: any) {
            selected.push(item.value);
        });

        return selected;
    }

    public isSelectAll() {
        return this.selectAll.is(":checked");
    }

    public getSportsLeagues() {
        const sportsAllLeagues = [];
        const sports = [];

        this.checkboxListContainer.find(":checkbox[data-level=0]").each(function (_, sportElement: any) {
            if (sportElement.checked) {
                sportsAllLeagues.push(sportElement.value);
            } else if (sportElement.indeterminate) {
                $(sportElement).parent().next(".subtree").find(":checkbox:checked").each(function (_, leagueElement: any) {
                    sports.push({ sport: sportElement.value, league: leagueElement.value });
                });
            }
        });

        sportsAllLeagues.length && sports.push({ sport: sportsAllLeagues.join() });

        return sports;
    }
}