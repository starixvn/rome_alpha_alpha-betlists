"use strict";

class FindTicketView {
    $: JQueryStatic;

    $refNo: JQuery;
    $submitButton: JQuery;

    constructor(
        $: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.$refNo = this.$("#ref-no");
        this.$submitButton = this.$("#submit-betList-form");
    };


    registerEvents() {
        this.$submitButton.prop("disabled", true);
        this.onChangeRefNoTextBox();
    }

    onChangeRefNoTextBox = () => {
        this.$refNo.on('change keyup paste',() => {
            const content = this.$refNo.val().trim();
            if (content) {
                this.$("#RefNo").val(content);
                this.$submitButton.prop("disabled", false);
            }
            else {
                this.$submitButton.prop("disabled", true);
            }
        });

    }
}

$(function () {
    const findTicketView = new FindTicketView(
        $);

    findTicketView.init();
})