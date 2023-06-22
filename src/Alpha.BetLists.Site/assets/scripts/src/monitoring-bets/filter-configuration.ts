"use strict";

class FilterConfigurationView {
    private inlineMessage: InlineMessageComponent;
    private bettypeDropdown: DropdownListComponent;
    private matchLiveIndicatorDropdown: DropdownListComponent;
    private groupOfSportsDropdown: DropdownListComponent;
    private sportLeagueMatchTreeview: SportLeagueMatchTreeViewComponent;
    private monitoredWatchlistMemberTreeview: SportLeagueMatchTreeViewComponent;
    private memberTagsInput: UsernameTagsInputComponent;
    private stake: JQuery;
    private addFilterPopup: JQuery;

    init() {
        const welcomePopup = $("#welcome-popup");
        welcomePopup.length && welcomePopup.modal("show");

        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        this.inlineMessage = new InlineMessageComponent();

        this.bettypeDropdown = new DropdownListComponent();
        this.bettypeDropdown.render("ddl-bettypes");

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
            "sportleaguematch-treeview"
        ));

        this.monitoredWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.monitoredWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption(
            "monitored-watchlist-members",
            "Search member username",
            "No member username found",
            "All (Include newly add ...)"
        ));

        this.memberTagsInput = new UsernameTagsInputComponent();
        this.memberTagsInput.render(new UsernameTagsInputOption(
            "username",
            100,
            "Max: # username(s)",
            "Duplicated username"
        ));

        this.stake = $("#txt-stake");
    }

    private bindEvents() {
        this.stake.on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });

        const boxFilterBySports = $("#box-filterBySports");
        $("#rdo-allSports").change(function () {
            this.checked && boxFilterBySports.addClass("hide");
        });
        $("#rdo-specificSports").change((e) => {
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

        $(".btn-reset").click(() => {
            this.inlineMessage.hide();
            const formData = this.getFormData(rdoSpecificMembers);
            console.dir(formData);

            if (formData) {
                this.addFilterPopup = $("#add-filter-popup");
                this.addFilterPopup.modal("show")
            }
        });
    }

    private getSports(isLive: string, groupSport: string) {
        this.sportLeagueMatchTreeview.addIconLoading();
        $.get(`FilterBySports?isLive=${isLive}&groupSport=${groupSport}`, (data) => {
            const listData = [];
            data.forEach(function (sport) {
                const leagueList = []
                sport.Leagues.forEach(function (league) {
                    leagueList.push({
                        value: league.Id,
                        text: league.Name,
                        attributes: {
                            "data-leagueid": league.Id
                        },
                        items: league.Matches.map(function (m) {
                            return {
                                value: m.MatchId,
                                text: m.MatchName,
                                subText: m.KickOffTimeStr,
                                attributes: {
                                    "data-homeid": m.HomeId,
                                    "data-awayid": m.AwayId,
                                    "data-isneutral": m.IsNeutral
                                }
                            };
                        })
                    });
                });

                listData.push({
                    value: sport.Id,
                    text: sport.Name,
                    attributes: {
                        "data-sportid": sport.Id
                    },
                    items: leagueList
                });
            });

            return this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
        });
    }

    private getFormData(rdoSpecificMembers: JQuery) {
        const betType = this.bettypeDropdown.isSelectAll() ? "0" : this.bettypeDropdown.getSelected().join();
        const stake = +this.stake.val();
        const isLive = this.matchLiveIndicatorDropdown.getSelected()[0] === "1";
        let sportTypeList = "0";
        if (!this.sportLeagueMatchTreeview.isSelectAll()) {

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
        }

        return {
            betType,
            stake,
            isLive,
            sportTypeList,
            isWatchList,
            inputList,
            watchList
        };
    }
}

$(function () {
    new FilterConfigurationView().init();
});