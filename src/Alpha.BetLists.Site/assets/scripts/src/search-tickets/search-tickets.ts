"use strict";

class SearchTicketView {
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
            let mainDomain = this.$("#MainDomainForFeedback").val();
            if (mainDomain.length) {
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-feedback",
                    feedback_id: 3
                }, mainDomain);
            } else {
                window.top.indexManager.initFeedbackFunction(3);
            }
        }, 1000);
    };

    private cacheDOMElements = () => {
        this.$refNo = this.$("#ref-no");
        this.$submitButton = this.$("#submit-betList-form");
        this.$betListTableContainer = this.$("#tbl-betlist");
        this.getBetsUrl = this.$("#get-bets-url").val();
    };

    private getTickets = (e) => {
        AJAXHelper.get("https://alpha-content.nexdev.net/api/articles", {}, (response) => {
            let content = this.$("#strapi-content");
            console.log(response);
            debugger;
        });
        e.preventDefault();
        this.$.blockUI();
        AJAXHelper.get(this.getBetsUrl, { RefNo: this.$refNo.val() }, this.successGetLastBets);
    };

    private successGetLastBets = (responseHtml: any) => {
        this.$betListTableContainer.children('tbody').html(responseHtml);

        this.expandParlay();
    };

    private expandParlay() {
        const parlayClickableId = `#r${this.$refNo.val()} a#hidden0`;
        const parlayItem = $(parlayClickableId);
        parlayItem && parlayItem[0] && parlayItem[0].click();
    }

    private registerEvents() {
        this.$submitButton.prop("disabled", true);
        this.$submitButton.on("click", this.getTickets);
        this.onChangeRefNoTextBox();
    }

    private onChangeRefNoTextBox = () => {
        this.$refNo.on('change keyup', () => {
            this.updateBtnStateOnTextChange();
        });

        this.$refNo.on('paste', () => {
            setTimeout(this.updateBtnStateOnTextChange, 100);
        });
    }

    private updateBtnStateOnTextChange() {
        const content = this.$("#ref-no").val().trim();
        if (content) {
            this.$("#ref-no").val(content);
            this.$("#submit-betList-form").prop("disabled", false);
        }
        else {
            this.$("#submit-betList-form").prop("disabled", true);
        }
    }
}

$(function () {
    new SearchTicketView($).init();
})