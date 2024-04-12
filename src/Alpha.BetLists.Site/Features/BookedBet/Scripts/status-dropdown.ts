"use strict";

class StatusDropdownOption {
    constructor(
        public listData: any,
        public onDropdownHidden = Function(),
        public selectorStatusId = "dropdown-status") {
        this.listData = listData;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorStatusId = selectorStatusId;
    }
}

class StatusDropdownComponent {
    private dropdownStatus: JQuery;
    private onlyShowHistoricStatusIds = [];

    render(options: StatusDropdownOption) {
        this.renderDropdown(options);

        $("#" + options.selectorStatusId).closest(".form-group").removeClass("hide");
    }

    private renderDropdown(options: StatusDropdownOption) {
        const self = this;
        const list = options.listData.map(function (item) {
            return self.buildCheckboxItem(item, {
                productId: item.Value
            });
        });

        const dropdown = $("#" + options.selectorStatusId);
        this.dropdownStatus = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: list,
            onLeave: function () {
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
    }

    private buildCheckboxItem(item, attributes = undefined) {
        return {
            value: item.Value,
            text: item.Text,
            checked: item.Selected,
            attributes
        }
    }

    getCheckedValues() {
        const ids = {
            StatusIds: this.dropdownStatus.fanexMultiSelect("getCheckedValues")
        };

        return ids;
    }

    getUncheckedValues() {
        return {
            unselectedStatusIds: this.dropdownStatus.fanexMultiSelect("getUncheckedValues")
        };
    }

    showHistoricProducts() {
        this.onlyShowHistoricStatusIds.length;
    }

    hideHistoricProducts() {
        this.onlyShowHistoricStatusIds.length;
    }
}