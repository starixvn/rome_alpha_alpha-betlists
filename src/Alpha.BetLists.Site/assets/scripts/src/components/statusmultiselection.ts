"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class StatusMultiSelectionOption {
    constructor(
        public dropdownId: string = "dropdown-status",
        public validateBeforeSubmit: boolean = true,
        public statusInputId: string = "status-ids",
        public onChange: Function = null) {
    }
}

class StatusMultiSelectionComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdownStatus: JQuery;
    $multiSelectButton: JQuery;

    options = new StatusMultiSelectionOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: StatusMultiSelectionOption = new StatusMultiSelectionOption()) => {
        this.options = <StatusMultiSelectionOption>this.$.extend({}, this.options, options);

        this.$dropdownStatus = this.$("#" + this.options.dropdownId);
        let hasAnyProduct = this.convertAttributeToBool(this.$dropdownStatus.data("has-any-status"));
        this.$hiddenInput = this.$("#" + this.options.statusInputId);
        let numberDisplayedStatusOnFilter = 1;

        this.$dropdownStatus.multiselect({
            maxHeight: 300,
            numberDisplayed: numberDisplayedStatusOnFilter,
            nonSelectedText: hasAnyProduct
                ? this.$dropdownStatus.data("label-select-status")
                : this.$dropdownStatus.data("label-there-is-no-status"),
            nSelectedText: ` ${this.$dropdownStatus.data("label-selected")}`,
            allSelectedText: this.$dropdownStatus.data("label-all-status"),
            includeSelectAllOption: hasAnyProduct,
            onChange: this.onChange,
            onSelectAll: this.onChange,
            language: this.$dropdownStatus.data("language")
        });

        this.$multiSelectButton = this.$dropdownStatus.parent().find(".multiselect.btn");

        return this.$dropdownStatus;
    };

    onChange = (option: any, checked: boolean) => {
        this.validate();
        this.$hiddenInput.val(this.getSelectedItems().join(","));

        if (this.options.onChange) {
            this.options.onChange();
        }
    };

    validate = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.$dropdownStatus.find("option:selected").length
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
        }
    };

    getSelectedItems = () => {
        let selected = [];
        let selectedOptions = this.$dropdownStatus.find("option:selected");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected;
    };

    convertAttributeToBool = (attrValue: string) => {
        return !!attrValue && attrValue.toString().toLowerCase() === "true";
    };
}