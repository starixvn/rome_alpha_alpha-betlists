"use strict";

class DropdownListComponent {
    private dropdownList: JQuery;

    render(selectorId: string, options = {}) {
        this.dropdownList = $("#" + selectorId);
        const multiple = this.dropdownList.attr("multiple");

        let defaults: any = { maxHeight: 300 };
        if (multiple) {
            defaults = $.extend(defaults, {
                numberDisplayed: 1,
                nonSelectedText: this.dropdownList.data("label-select"),
                nSelectedText: ` ${this.dropdownList.data("label-selected")}`,
                allSelectedText: this.dropdownList.data("label-all"),
                selectAllText: this.dropdownList.data("label-selectall"),
                includeSelectAllOption: true
            });
        }

        this.dropdownList.multiselect($.extend(defaults, options));
        return this.dropdownList;
    };

    getSelected() {
        const selected = [];
        this.dropdownList.find("option:selected").each(function (_, item: any) {
            selected.push(item.value);
        });

        return selected;
    }

    isSelectAll() {
        return this.dropdownList.next().find(".multiselect-container .multiselect-all :checkbox").is(":checked");
    }

    enable() {
        this.dropdownList.multiselect("enable");
    }

    disable() {
        this.dropdownList.multiselect("disable");
    }
}