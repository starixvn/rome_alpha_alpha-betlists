"use strict";

class FindTicketView {
    $: JQueryStatic;

    $refNo: JQuery;
    $submitButton: JQuery;
    $betListTableContainer: JQuery;

    getBetsUrl = "";

    constructor(
        $: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();

        setTimeout(() => {
            let mainDomain = this.$("#MainDomain").val();
            if (mainDomain.length) {
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-feedback",
                    feedback_id: 2
                }, mainDomain);
            } else {
                window.top.indexManager.initFeedbackFunction(2);
            }
        }, 1000);
    };

    cacheDOMElements = () => {
        this.$refNo = this.$("#ref-no");
        this.$submitButton = this.$("#submit-betList-form");
        this.$betListTableContainer = this.$("#tbl-betlist");
        this.getBetsUrl = this.$("#get-bets-url").val();
    };

    private getTickets = (e) => {
        e.preventDefault();
        this.$.blockUI();
        AJAXHelper.get(this.getBetsUrl, { RefNo: this.$("#RefNo").val() }, this.successGetLastBets);
    };

    private successGetLastBets = (responseHtml: any) => {
        if ((responseHtml.ErrorCode && responseHtml.ErrorCode !== "0") ||
            (responseHtml.ErrCode && responseHtml.ErrCode !== "0")) {
            return;
        }

        this.$betListTableContainer.children('tbody').html(responseHtml);

    };

    registerEvents() {
        this.$submitButton.prop("disabled", true);
        this.$submitButton.on("click", this.getTickets);
        this.onChangeRefNoTextBox();
    }

    onChangeRefNoTextBox = () => {
        this.$refNo.on('change keyup paste', () => {
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