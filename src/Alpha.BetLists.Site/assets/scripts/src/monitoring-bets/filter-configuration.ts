"use strict";

class FilterConfigurationView {
    private inlineMessage: InlineMessageComponent;
    private betTypeDropdown: DropdownListComponent;
    private stakeInput: JQuery;
    private matchLiveIndicatorDropdown: DropdownListComponent;
    private groupOfSportsDropdown: DropdownListComponent;
    private sportLeagueMatchTreeview: SportLeagueMatchTreeViewComponent;
    private monitoredWatchlistMemberTreeview: SportLeagueMatchTreeViewComponent;
    private memberTagsInput: UsernameTagsInputComponent;

    private inlineMessagePopup: InlineMessageComponent;
    private filterNameInput: JQuery;
    private isDefaultCheckbox: JQuery;

    private addFilterFormData: any;

    init() {
        const welcomePopup = $("#welcome-popup");
        welcomePopup.length && welcomePopup.modal("show");

        this.initComponents();
        this.bindEvents();
    }

    private initComponents() {
        this.inlineMessage = new InlineMessageComponent();

        this.betTypeDropdown = new DropdownListComponent();
        this.betTypeDropdown.render("ddl-bettypes");

        this.matchLiveIndicatorDropdown = new DropdownListComponent();
        this.matchLiveIndicatorDropdown.render("ddl-matchLiveIndicator", {
            onChange: (select) => this.getSports(select.val(), this.groupOfSportsDropdown.getSelected()[0])
        });

        this.groupOfSportsDropdown = new DropdownListComponent();
        this.groupOfSportsDropdown.render("ddl-groupOfSport", {
            onChange: (select) => this.getSports(this.matchLiveIndicatorDropdown.getSelected()[0], select.val())
        });

        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption(
            "sportleaguematch-treeview",
            "Search Sport/ League",
            "No Sport/ League found",
            "Select All",
            "No information is available",
            false,
            400
        ));

        this.monitoredWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.monitoredWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption(
            "monitored-watchlist-members",
            "Search member username",
            "No member username found",
            "All (Include newly add ...)",
            "No information is available",
            false,
            400
        ));

        this.memberTagsInput = new UsernameTagsInputComponent();
        this.memberTagsInput.render(new UsernameTagsInputOption(
            "username",
            100,
            "Max: # username(s)",
            "Duplicated username"
        ));

        this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
    }

    private bindEvents() {
        this.stakeInput = $("#txt-stake");
        this.stakeInput.on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });

        const boxFilterBySports = $("#box-filterBySports");
        $("#rdo-allSports").change(function () {
            this.checked && boxFilterBySports.addClass("hide");
        });
        const rdoSpecificSports = $("#rdo-specificSports").change((e) => {
            if ($(e.target).is(":checked")) {
                boxFilterBySports.removeClass("hide");
                this.getSports(
                    this.matchLiveIndicatorDropdown.getSelected()[0],
                    this.groupOfSportsDropdown.getSelected()[0]
                );
            }
        });

        const boxFilterByMembers = $("#box-filterByMembers");
        $("#rdo-allMembers").change(function () {
            this.checked && boxFilterByMembers.addClass("hide");
        });
        const rdoSpecificMembers = $("#rdo-specificMembers").change(() => {
            if (rdoSpecificMembers.is(":checked")) {
                boxFilterByMembers.removeClass("hide");
                this.monitoredWatchlistMemberTreeview.addIconLoading();
                $.get("GetMonitoredWatchlistMembers", (data) =>
                    this.monitoredWatchlistMemberTreeview.renderTreeNodeList(data.map(function (d) {
                        return {
                            value: d.CustId,
                            text: d.Username
                        };
                    }))
                );
            }
        });

        const addFilterPopup = $("#add-filter-popup");
        addFilterPopup.on("hidden.bs.modal", function () {
            $("#txt-filterName").val("");
            $("#chk-isDefault").prop("checked", false);
        });
        addFilterPopup.find(".btn-submit").click(() => {
            const formDataPopup = this.getFormDataPopup();
            if (formDataPopup) {
                this.addFilterFormData = $.extend(this.addFilterFormData, formDataPopup);
                AJAXHelper.postJSON("AddFilter", this.addFilterFormData, (result) => {
                    this.inlineMessagePopup.show(result);
                    if (!result.ErrCode) {
                        this.addFilterFormData = null;
                        this.reloadPageAfterTimeout();
                    }
                });
            }
        });

        $(".button-reset").click(() => {
            this.addFilterFormData = this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            this.addFilterFormData && addFilterPopup.modal("show");

            console.dir(this.addFilterFormData);
        });
    }

    private getSports(isLive: string, groupSport: string) {
        this.sportLeagueMatchTreeview.addIconLoading();
        $.get(`FilterBySports?isLive=${isLive}&groupSport=${groupSport}`, (data) => {
            const listData = data.map(function (sport) {
                return {
                    value: sport.Id,
                    text: sport.Name,
                    items: sport.Leagues.map(function (league) {
                        return {
                            value: league.Id,
                            text: league.Name
                        };
                    })
                };
            });

            return this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
        });
    }

    private getFormData(rdoSpecificSports: JQuery, rdoSpecificMembers: JQuery) {
        let betType: string;
        if (this.betTypeDropdown.isSelectAll()) {
            betType = "0";
        } else {
            betType = this.betTypeDropdown.getSelected().join();
            if (!betType.length) {
                this.inlineMessage.showError($("#hidden-BetTypesRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }

        const stake = $.trim(this.stakeInput.val().replace(/,/g, ""));
        if (!stake.length) {
            this.inlineMessage.showError($("#hidden-StakeRequiredError").val());
            this.stakeInput.focus();
            return;
        }

        let sports = [{ sport: 0 }];
        let isLive = false;
        if (rdoSpecificSports.is(":checked")) {
            isLive = this.matchLiveIndicatorDropdown.getSelected()[0] === "1";
            if (!this.sportLeagueMatchTreeview.isSelectAll()) {
                sports = this.sportLeagueMatchTreeview.getSportsLeagues();
                if (!sports.length) {
                    this.inlineMessage.showError($("#hidden-SportsRequiredError").val());
                    this.scrollTopInlineMessage();
                    return;
                }
            }
        }

        let isWatchList = 2;
        let inputList = "0";
        let watchList = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.monitoredWatchlistMemberTreeview.isSelectAll()) {
                isWatchList = 1;
            } else {
                isWatchList = 0;
                watchList = this.monitoredWatchlistMemberTreeview.getSelected().join();
            }

            inputList = this.memberTagsInput.getCustIds().join();
            if (!watchList.length && !inputList.length) {
                this.inlineMessage.showError($("#hidden-FilterByMembersRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }

        this.inlineMessage.hide();
        return {
            betType,
            stake,
            isLive,
            sports,
            isWatchList,
            inputList,
            watchList
        };
    }

    private getFormDataPopup() {
        const filterNameElement = $("#txt-filterName");
        const filterName = $.trim(filterNameElement.val());
        if (!filterName.length) {
            this.inlineMessagePopup.showError($("#hidden-FilterNameRequiredError").val());
            return;
        }

        const isDefault = $("#chk-isDefault").is(":checked");
        this.inlineMessagePopup.hide();

        return {
            filterName,
            isDefault
        };
    }

    private scrollTopInlineMessage() {
        $("html, body").scrollTop(this.inlineMessage.getInlineMessage().offset().top);
    }

    private reloadPageAfterTimeout(timeout = 3000) {
        setTimeout(function () {
            window.location.reload();
        }, timeout);
    }
}

$(function () {
    new FilterConfigurationView().init();
});