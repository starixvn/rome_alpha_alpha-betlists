"use strict";
const defaultValue = 60;

class AutoRefreshOption {
    constructor(
        public onText = "ON",
        public offText = "OFF",
        public callback = Function()) {
        this.onText = onText;
        this.offText = offText;
        this.callback = callback;
    }
}

class AutoRefreshComponent {
    public autoRefreshOption: AutoRefreshOption;
    private timeout;
    private autoRefresh: JQuery;
    private labelStatusText: JQuery;
    private labelCountDown: JQuery;

    init(option: AutoRefreshOption) {
        this.autoRefreshOption = option;
        this.autoRefresh = $("#auto-refresh");
        this.labelStatusText = this.autoRefresh.find("#lbl-status-text")
            .data("running", "0")
            .text(this.autoRefreshOption.onText);
        this.labelCountDown = this.autoRefresh.find("#lbl-count-down");
        this.labelCountDown.text(defaultValue).data("original", defaultValue);

        this.autoRefresh.find("#btn-status").click(() => this.labelStatusText.data("running") === "1" ? this.stop() : this.start());

        this.autoRefresh.find(".auto-refresh-menu-item").click((e) => {
            const second = $(e.target).text();
            this.labelCountDown.text(second).data("original", second);
        });
    }

    setIsLoading() {
        this.labelCountDown.text("").addClass("icon-loading");
    }

    isOn() {
        return !this.autoRefresh.hasClass("off");
    }

    start() {
        clearTimeout(this.timeout);
        this.autoRefresh.removeClass("off");
        this.labelStatusText
            .data("running", "1")
            .text(this.autoRefreshOption.onText);
        this.labelCountDown
            .removeClass("icon-loading")
            .text(this.labelCountDown.data("original"));
        this.timeout = setTimeout(() => this.decrement(), 1000);
    }

    stop() {
        clearTimeout(this.timeout);
        this.autoRefresh.addClass("off");
        this.labelStatusText
            .data("running", "0")
            .text(this.autoRefreshOption.offText);
        this.labelCountDown
            .removeClass("icon-loading")
            .text(this.labelCountDown.data("original"));
    }

    private decrement() {
        let value = +this.labelCountDown.text();
        if (value === 1) {
            clearTimeout(this.timeout);
            this.labelCountDown
                .text("")
                .addClass("icon-loading");
            this.autoRefreshOption.callback();
        } else {
            this.labelCountDown.text(--value);
            this.timeout = setTimeout(() => this.decrement(), 1000);
        }
    }
}