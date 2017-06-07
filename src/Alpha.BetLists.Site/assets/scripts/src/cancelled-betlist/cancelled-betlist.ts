"use strict";

class CancelledBetListView {
    $: JQueryStatic;
    $constants: CancelledBetListConstants;
    $betlistForm: JQuery;
    $pageTitle: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
        this.$constants = new CancelledBetListConstants();
    }

    init = () => {
        this.cacheElements();
        this.registerEvents();
    }

    registerEvents() {
        this.$(this.$constants.betListFilterId).on("click", this.onClickIconFilter);
        this.$(this.$constants.dropdownUsersClass).on("change", this.onChangeSelectedOfUsersDropdown);
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

    cacheElements() {
        this.$betlistForm = this.$(this.$constants.betListFormId);
        this.$pageTitle = this.$(this.$constants.pageTitleClass); 
    };
}

$(function () {
    const cancelledBet = new CancelledBetListView($);

    cancelledBet.init();
})