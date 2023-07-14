"use strict";
var FilterConfigurationView = /** @class */ (function () {
    function FilterConfigurationView() {
    }
    FilterConfigurationView.prototype.init = function () {
        var welcomePopup = $("#welcome-popup");
        welcomePopup.length && welcomePopup.modal("show");
        this.initComponents();
        this.bindEvents();
    };
    FilterConfigurationView.prototype.initComponents = function () {
        var _this = this;
        var noInfoText = $("#hidden-NoInformationIsAvailable").val();
        this.inlineMessage = new InlineMessageComponent();
        this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
        this.betTypeDropdown = new DropdownListComponent();
        this.betTypeDropdown.render("ddl-bettype");
        this.liveStatusDropdown = new DropdownListComponent();
        this.liveStatusDropdown.render("ddl-live-status");
        this.sportGroupDropdown = new DropdownListComponent();
        this.sportGroupDropdown.render("ddl-sport-group", {
            onChange: function (select) { return _this.getSports(select.val()); }
        });
        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption("sportleaguematch-treeview", noInfoText, $("#hidden-SearchSportLeague").val(), $("#hidden-SportLeagueNotFound").val(), $("#hidden-SelectAll").val(), true, $("#hidden-ExpandAll").val(), $("#hidden-SelectedOnly").val()));
        this.myWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.myWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption("my-watchlist-members", noInfoText, $("#hidden-SearchMemberUsername").val(), $("#hidden-MemberUsernameNotFound").val(), $("#hidden-AllIncludedNewlyAddedMembers").val()));
        this.usernameTagsInput = new UsernameTagsInputComponent();
        this.usernameTagsInput.render(new UsernameTagsInputOption("username", 100, $("#hidden-MaxNoUsernames").val(), $("#hidden-UsernameAlreadyExists").val()));
    };
    FilterConfigurationView.prototype.bindEvents = function () {
        var _this = this;
        this.stakeInput = $("#txt-stake").on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });
        var sportsContainer = $("#sports-container");
        $("#rdo-all-sports").change(function () {
            this.checked && sportsContainer.addClass("hide");
        });
        var rdoSpecificSports = $("#rdo-specific-sports").change(function (e) {
            if (rdoSpecificSports.is(":checked")) {
                sportsContainer.removeClass("hide");
                _this.getSports(_this.sportGroupDropdown.getSelected()[0]);
            }
        });
        var membersContainer = $("#members-container");
        $("#rdo-all-members").change(function () {
            this.checked && membersContainer.addClass("hide");
        });
        var rdoSpecificMembers = $("#rdo-specific-members").change(function () {
            if (rdoSpecificMembers.is(":checked")) {
                membersContainer.removeClass("hide");
                _this.getMyWatchlistMembers();
            }
        });
        var addFilterPopup = $("#add-filter-popup");
        this.filterNameInput = addFilterPopup.find("#txt-filterName");
        this.isDefaultCheckbox = addFilterPopup.find("#chk-isDefault");
        addFilterPopup
            .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
            .on("hidden.bs.modal", function () {
            _this.filterNameInput.val("");
            _this.isDefaultCheckbox.prop("checked", false);
            _this.inlineMessagePopup.hide();
        });
        addFilterPopup.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(); });
        $(".button-apply").click(function (e) {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && _this.handleApplyFilter();
        });
        $(".button-save").click(function (e) {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && addFilterPopup.modal("show");
        });
    };
    FilterConfigurationView.prototype.getSports = function (groupSport) {
        var _this = this;
        this.sportGroupDropdown.disable();
        this.sportLeagueMatchTreeview.addIconLoading();
        AJAXHelper.getJSONWithoutBlockUI("FilterBySports?groupSport=" + groupSport, function (data) {
            var listData = data.map(function (sport) {
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
            _this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
            _this.sportGroupDropdown.enable();
        });
    };
    FilterConfigurationView.prototype.getMyWatchlistMembers = function () {
        var _this = this;
        this.myWatchlistMemberTreeview.addIconLoading();
        AJAXHelper.getJSONWithoutBlockUI("GetMyWatchlistMembers", function (data) {
            return _this.myWatchlistMemberTreeview.renderTreeNodeList(data.map(function (d) {
                return {
                    value: d.CustId,
                    text: d.Username
                };
            }));
        });
    };
    FilterConfigurationView.prototype.getFormData = function (rdoSpecificSports, rdoSpecificMembers) {
        var betType;
        if (this.betTypeDropdown.isSelectAll()) {
            betType = "0";
        }
        else {
            betType = this.betTypeDropdown.getSelected().join();
            if (!betType.length) {
                this.inlineMessage.showError($("#hidden-BetTypesRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }
        var stake = $.trim(this.stakeInput.val().replace(/,/g, ""));
        if (!stake.length) {
            this.inlineMessage.showError($("#hidden-StakeRequiredError").val());
            this.stakeInput.focus();
            return;
        }
        var liveStatus = this.liveStatusDropdown.getSelected()[0];
        var sports = [{ sport: "0", league: "0" }];
        if (rdoSpecificSports.is(":checked")) {
            var sportGroup = this.sportGroupDropdown.getSelected()[0];
            if (sportGroup === "0" && !this.sportLeagueMatchTreeview.isSelectAll() || sportGroup !== "0") {
                sports = this.sportLeagueMatchTreeview.getSportsLeagues();
                if (!sports.length) {
                    this.inlineMessage.showError($("#hidden-SportsRequiredError").val());
                    this.scrollTopInlineMessage();
                    return;
                }
            }
        }
        var filterByMembers = 2; // all members
        var inputList = "0";
        var myWatchlist = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.myWatchlistMemberTreeview.isSelectAll()) {
                filterByMembers = 1; // all my wachlist members
            }
            else {
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
            betType: betType,
            stake: stake,
            liveStatus: liveStatus,
            sports: sports,
            filterByMembers: filterByMembers,
            inputList: inputList,
            myWatchlist: myWatchlist
        };
    };
    FilterConfigurationView.prototype.getFormDataPopup = function () {
        var filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            this.inlineMessagePopup.showError($("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = this.isDefaultCheckbox.is(":checked");
        this.inlineMessagePopup.hide();
        return {
            filterName: filterName,
            isDefault: isDefault
        };
    };
    FilterConfigurationView.prototype.handleApplyFilter = function () {
        var _this = this;
        this.addFilterFormData.filterName = "Default";
        AJAXHelper.postJSON("ApplyFilter", this.addFilterFormData, function (result) {
            if (result.ErrCode) {
                _this.inlineMessage.showError(result.ErrMsg);
            }
            else {
                _this.addFilterFormData = null;
                var url = window.location.href.split("?")[0];
                window.location.href = url.replace(/FilterConfiguration/i, "");
            }
        });
    };
    FilterConfigurationView.prototype.handleSaveFilterPopup = function () {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            this.addFilterFormData = $.extend(this.addFilterFormData, formDataPopup);
            AJAXHelper.postJSON("AddFilter", this.addFilterFormData, function (result) {
                _this.inlineMessagePopup.show(result);
                if (!result.ErrCode) {
                    _this.addFilterFormData = null;
                    Common.Utils.ReloadPageAfterTimeout(window.location.href.split("?")[0]);
                }
            });
        }
    };
    FilterConfigurationView.prototype.scrollTopInlineMessage = function () {
        $("html, body").scrollTop(this.inlineMessage.getInlineMessage().offset().top);
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map