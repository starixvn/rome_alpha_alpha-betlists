"use strict";

class CancelledBetListView {
    $: JQueryStatic;
    $constants: CancelledBetListConstants;
    $betlistForm: JQuery;
    $pageTitle: JQuery;
    membersComponent: SingleSelectionDropdownComponent;
    agentsComponent: SingleSelectionDropdownComponent;
    mastersComponent: SingleSelectionDropdownComponent;
    constructor($: JQueryStatic, reportTypeComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.$constants = new CancelledBetListConstants();
        this.membersComponent = reportTypeComponent;
        this.agentsComponent = reportTypeComponent;
        this.mastersComponent = reportTypeComponent;
    }

    public init = () => {
        this.initComponents();
        this.cacheElements();
        this.registerEvents();
    }

    public initComponents = () => {
        var membersOption = new SingleSelectionDropdownOption("dropdown-members", "ReportType", this.onChangeSelectedOfUsersDropdown);
        this.membersComponent.render(membersOption);
        var agentsOption = new SingleSelectionDropdownOption("dropdown-agents", "ReportType", this.onChangeSelectedOfUsersDropdown);
        this.agentsComponent.render(agentsOption);
        var mastersOption = new SingleSelectionDropdownOption("dropdown-masters", "ReportType", this.onChangeSelectedOfUsersDropdown);
        this.mastersComponent.render(mastersOption);
    }

    public registerEvents() {
        this.$(this.$constants.betListFilterId).on("click", this.onClickIconFilter);
    }

    onChangeSelectedOfUsersDropdown = () => {
        this.$betlistForm.submit();
    }

    onClickIconFilter = () => {
        let filter = this.$betlistForm;
        let isHidden = filter.hasClass("hide");

        if (isHidden) {
            this.$pageTitle.removeClass("hide-filter");
            this.$betlistForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$betlistForm.addClass("hide");
        }
    }

    public cacheElements() {
        this.$betlistForm = this.$("#betlist-form");
        this.$pageTitle = this.$(".bl_title");
    };
}

$(function () {
    const cancelledBet = new CancelledBetListView($, new SingleSelectionDropdownComponent($));

    cancelledBet.init();
})