"use strict";

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
    private autoRefreshOption: AutoRefreshOption;
    private timeout;
    private isRunning = false;
    private labelStatusText: JQuery;
    private labelCountDown: JQuery;

    init(option: AutoRefreshOption) {
        this.autoRefreshOption = option;
        const autoRefresh = $("#auto-refresh");
        this.labelStatusText = autoRefresh.find("#lbl-status-text");
        this.labelCountDown = autoRefresh.find("#lbl-count-down");

        autoRefresh.find("#btn-status").click(() => this.isRunning ? this.stop() : this.start());

        autoRefresh.find(".auto-refresh-menu-item").click((e) => {
            clearTimeout(this.timeout);
            const menuItem = $(e.target);
            const second = menuItem.text();
            this.labelCountDown.text(second).data("original", second);

            this.timeout = setTimeout(() => this.decrement(), 1000);
        });
    }

    start() {
        clearTimeout(this.timeout);
        this.isRunning = true;
        this.labelStatusText.text(this.autoRefreshOption.onText);
        this.labelCountDown.text(this.labelCountDown.data("original"));
        this.timeout = setTimeout(() => this.decrement(), 1000);
    }

    stop() {
        clearTimeout(this.timeout);
        this.isRunning = false;
        this.labelStatusText.text(this.autoRefreshOption.offText);
        this.labelCountDown.text(this.labelCountDown.data("original"));
    }



    private decrement() {
        let value = +this.labelCountDown.text();
        if (value === 1) {
            clearTimeout(this.timeout);
            this.labelCountDown.text("...");
            this.autoRefreshOption.callback();
        } else {
            this.labelCountDown.text(--value);
            this.timeout = setTimeout(() => this.decrement(), 1000);
        }
    }
}