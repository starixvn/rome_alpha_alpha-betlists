"use strict";

class FilterConfigurationView {
    private inlineMessage: InlineMessageComponent;
    private betTypeDropdown: DropdownListComponent;
    private stakeInput: JQuery;
    private liveStatusDropdown: DropdownListComponent;
    private sportGroupDropdown: DropdownListComponent;
    private sportLeagueMatchTreeview: SportLeagueMatchTreeViewComponent;
    private myWatchlistMemberTreeview: SportLeagueMatchTreeViewComponent;
    private usernameTagsInput: UsernameTagsInputComponent;

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
        const noInfoText = $("#hidden-NoInformationIsAvailable").val();
        this.inlineMessage = new InlineMessageComponent();
        this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");

        this.betTypeDropdown = new DropdownListComponent();
        this.betTypeDropdown.render("ddl-bettype");

        this.liveStatusDropdown = new DropdownListComponent();
        this.liveStatusDropdown.render("ddl-live-status");

        this.sportGroupDropdown = new DropdownListComponent();
        this.sportGroupDropdown.render("ddl-sport-group", {
            onChange: (select) => this.getSports(select.val())
        });

        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption(
            "sportleaguematch-treeview",
            noInfoText,
            $("#hidden-SearchSportLeague").val(),
            $("#hidden-SportLeagueNotFound").val(),
            $("#hidden-SelectAll").val(),
            true,
            $("#hidden-ExpandAll").val(),
            $("#hidden-SelectedOnly").val()
        ));

        this.myWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.myWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption(
            "my-watchlist-members",
            noInfoText,
            $("#hidden-SearchMemberUsername").val(),
            $("#hidden-MemberUsernameNotFound").val(),
            $("#hidden-AllIncludedNewlyAddedMembers").val()
        ));

        this.usernameTagsInput = new UsernameTagsInputComponent();
        this.usernameTagsInput.render(new UsernameTagsInputOption(
            "username", 100, $("#hidden-MaxNoUsernames").val(), $("#hidden-UsernameAlreadyExists").val()
        ));
    }

    private bindEvents() {
        this.stakeInput = $("#txt-stake").on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });

        const sportsContainer = $("#sports-container");
        $("#rdo-all-sports").change(function () {
            this.checked && sportsContainer.addClass("hide");
        });
        const rdoSpecificSports = $("#rdo-specific-sports").change((e) => {
            if (rdoSpecificSports.is(":checked")) {
                sportsContainer.removeClass("hide");
                this.getSports(this.sportGroupDropdown.getSelected()[0]);
            }
        });

        const membersContainer = $("#members-container");
        $("#rdo-all-members").change(function () {
            this.checked && membersContainer.addClass("hide");
        });
        const rdoSpecificMembers = $("#rdo-specific-members").change(() => {
            if (rdoSpecificMembers.is(":checked")) {
                membersContainer.removeClass("hide");
                this.getMyWatchlistMembers();
            }
        });

        const addFilterPopup = $("#add-filter-popup");
        this.filterNameInput = addFilterPopup.find("#txt-filterName");
        this.isDefaultCheckbox = addFilterPopup.find("#chk-isDefault");
        addFilterPopup
            .on("shown.bs.modal", () => this.filterNameInput.focus())
            .on("hidden.bs.modal", () => {
                this.filterNameInput.val("");
                this.isDefaultCheckbox.prop("checked", false);
                this.inlineMessagePopup.hide();
            });
        addFilterPopup.find("#btn-save-filter-popup").click(() => this.handleSaveFilterPopup());

        $(".button-apply").click((e) => {
            this.addFilterFormData = this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            this.addFilterFormData && this.handleApplyFilter();
        });

        $(".button-save").click((e) => {
            this.addFilterFormData = this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            this.addFilterFormData && addFilterPopup.modal("show");
        });
    }

    private getSports(groupSport: string) {
        this.sportGroupDropdown.disable();
        this.sportLeagueMatchTreeview.addIconLoading();
        AJAXHelper.getJSONWithoutBlockUI(`FilterBySports?groupSport=${groupSport}`, (data) => {
            const listData = data.map(function (sport) {
                return {
                    value: sport.Id,
                    text: sport.Name,
                    items: sport.Leagues.map(function (league) {
                        return {
                            value: league.LeagueId,
                            text: league.LeagueName
                        };
                    })
                };
            });

            this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
            this.sportGroupDropdown.enable();
        });
    }

    private getMyWatchlistMembers() {
        this.myWatchlistMemberTreeview.addIconLoading();
        AJAXHelper.getJSONWithoutBlockUI("GetMyWatchlistMembers", (data) =>
            this.myWatchlistMemberTreeview.renderTreeNodeList(data.map(function (d) {
                return {
                    value: d.CustId,
                    text: d.Username
                };
            }))
        );
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

        const liveStatus = this.liveStatusDropdown.getSelected()[0];

        let sports = [{ sport: "0", league: "0" }];
        if (rdoSpecificSports.is(":checked")) {
            const sportGroup = this.sportGroupDropdown.getSelected()[0];
            if (sportGroup === "0" && !this.sportLeagueMatchTreeview.isSelectAll() || sportGroup !== "0") {
                sports = this.sportLeagueMatchTreeview.getSportsLeagues();
                if (!sports.length) {
                    this.inlineMessage.showError($("#hidden-SportsRequiredError").val());
                    this.scrollTopInlineMessage();
                    return;
                }
            }
        }

        let filterByMembers = 2; // all members
        let inputList = "0";
        let myWatchlist = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.myWatchlistMemberTreeview.isSelectAll()) {
                filterByMembers = 1; // all my wachlist members
            } else {
                filterByMembers = 0;
                myWatchlist = this.myWatchlistMemberTreeview.getSelected().join();
            }

            inputList = this.usernameTagsInput.getCustIds().join();
            if (!myWatchlist.length && !inputList.length) {
                this.inlineMessage.showError($("#hidden-FilterByMembersRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }

        this.inlineMessage.hide();
        return {
            betType,
            stake,
            liveStatus,
            sports,
            filterByMembers,
            inputList,
            myWatchlist
        };
    }

    private getFormDataPopup() {
        const filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            this.inlineMessagePopup.showError($("#hidden-FilterNameRequiredError").val());
            return;
        }

        const isDefault = this.isDefaultCheckbox.is(":checked");
        this.inlineMessagePopup.hide();

        return {
            filterName,
            isDefault
        };
    }

    private handleApplyFilter() {
        this.addFilterFormData.filterName = "Default";
        AJAXHelper.postJSON("ApplyFilter", this.addFilterFormData, (result) => {
            if (result.ErrCode) {
                this.inlineMessage.showError(result.ErrMsg);
            } else {
                this.addFilterFormData = null;
                let url = window.location.href.split("?")[0];
                window.location.href = url.replace(/FilterConfiguration/i, "");
            }
        });
    }

    private handleSaveFilterPopup() {
        const formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            this.addFilterFormData = $.extend(this.addFilterFormData, formDataPopup);
            AJAXHelper.postJSON("AddFilter", this.addFilterFormData, (result) => {
                this.inlineMessagePopup.show(result);
                if (!result.ErrCode) {
                    this.addFilterFormData = null;
                    Common.Utils.ReloadPageAfterTimeout(window.location.href.split("?")[0]);
                }
            });
        }
    }

    private scrollTopInlineMessage() {
        $("html, body").scrollTop(this.inlineMessage.getInlineMessage().offset().top);
    }
}

$(function () {
    new FilterConfigurationView().init();
});