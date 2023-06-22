"use strict";
var FilterConfigurationView = /** @class */ (function () {
    function FilterConfigurationView() {
    }
    FilterConfigurationView.prototype.init = function () {
        var welcomePopup = $("#welcome-popup");
        welcomePopup.length && welcomePopup.modal("show");
        this.cacheDOMElements();
        this.bindEvents();
    };
    FilterConfigurationView.prototype.cacheDOMElements = function () {
        var _this = this;
        this.inlineMessage = new InlineMessageComponent();
        this.bettypeDropdown = new DropdownListComponent();
        this.bettypeDropdown.render("ddl-bettypes");
        this.matchLiveIndicatorDropdown = new DropdownListComponent();
        this.matchLiveIndicatorDropdown.render("ddl-matchLiveIndicator", {
            onChange: function (select) { return _this.getSports(select.val(), _this.groupOfSportsDropdown.getSelected()[0]); }
        });
        this.groupOfSportsDropdown = new DropdownListComponent();
        this.groupOfSportsDropdown.render("ddl-groupOfSport", {
            onChange: function (select) { return _this.getSports(_this.matchLiveIndicatorDropdown.getSelected()[0], select.val()); }
        });
        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption("sportleaguematch-treeview"));
        this.monitoredWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.monitoredWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption("monitored-watchlist-members", "Search member username", "No member username found", "All (Include newly add ...)"));
        this.memberTagsInput = new UsernameTagsInputComponent();
        this.memberTagsInput.render(new UsernameTagsInputOption("username", 100, "Max: # username(s)", "Duplicated username"));
        this.stake = $("#txt-stake");
    };
    FilterConfigurationView.prototype.bindEvents = function () {
        var _this = this;
        this.stake.on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });
        var boxFilterBySports = $("#box-filterBySports");
        $("#rdo-allSports").change(function () {
            this.checked && boxFilterBySports.addClass("hide");
        });
        $("#rdo-specificSports").change(function (e) {
            if ($(e.target).is(":checked")) {
                boxFilterBySports.removeClass("hide");
                _this.getSports(_this.matchLiveIndicatorDropdown.getSelected()[0], _this.groupOfSportsDropdown.getSelected()[0]);
            }
        });
        var boxFilterByMembers = $("#box-filterByMembers");
        $("#rdo-allMembers").change(function () {
            this.checked && boxFilterByMembers.addClass("hide");
        });
        var rdoSpecificMembers = $("#rdo-specificMembers").change(function () {
            if (rdoSpecificMembers.is(":checked")) {
                boxFilterByMembers.removeClass("hide");
                _this.monitoredWatchlistMemberTreeview.addIconLoading();
                $.get("GetMonitoredWatchlistMembers", function (data) {
                    return _this.monitoredWatchlistMemberTreeview.renderTreeNodeList(data.map(function (d) {
                        return {
                            value: d.CustId,
                            text: d.Username
                        };
                    }));
                });
            }
        });
        $(".btn-reset").click(function () {
            _this.inlineMessage.hide();
            var formData = _this.getFormData(rdoSpecificMembers);
            console.dir(formData);
            if (formData) {
                _this.addFilterPopup = $("#add-filter-popup");
                _this.addFilterPopup.modal("show");
            }
        });
    };
    FilterConfigurationView.prototype.getSports = function (isLive, groupSport) {
        var _this = this;
        this.sportLeagueMatchTreeview.addIconLoading();
        $.get("FilterBySports?isLive=" + isLive + "&groupSport=" + groupSport, function (data) {
            var listData = [];
            data.forEach(function (sport) {
                var leagueList = [];
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
            return _this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
        });
    };
    FilterConfigurationView.prototype.getFormData = function (rdoSpecificMembers) {
        var betType = this.bettypeDropdown.isSelectAll() ? "0" : this.bettypeDropdown.getSelected().join();
        var stake = +this.stake.val();
        var isLive = this.matchLiveIndicatorDropdown.getSelected()[0] === "1";
        var sportTypeList = "0";
        if (!this.sportLeagueMatchTreeview.isSelectAll()) {
        }
        var isWatchList = 2;
        var inputList = "0";
        var watchList = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.monitoredWatchlistMemberTreeview.isSelectAll()) {
                isWatchList = 1;
            }
            else {
                isWatchList = 0;
                watchList = this.monitoredWatchlistMemberTreeview.getSelected().join();
            }
            inputList = this.memberTagsInput.getCustIds().join();
        }
        return {
            betType: betType,
            stake: stake,
            isLive: isLive,
            sportTypeList: sportTypeList,
            isWatchList: isWatchList,
            inputList: inputList,
            watchList: watchList
        };
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map