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
        this.betTypeDropdown.render("ddl-bettype", {
            onChange: function () { return TipsyHelper.clear($("#ddl-bettype").next(".btn-group")); }
        });
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
        this.usernameTagsInput.render(new UsernameTagsInputOption("username", 100, $("#hidden-MaxNoUsernames").val(), $("#hidden-UsernameAlreadyExists").val(), $("#hidden-UsernameSuggestionUrl").val()));
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
        var saveFilterPopup = $("#save-filter-popup");
        this.filterNameInput = saveFilterPopup.find("#txt-filterName");
        this.isDefaultCheckbox = saveFilterPopup.find("#chk-isDefault");
        this.isNotifyCheckbox = saveFilterPopup.find("#chk-isNotify");
        saveFilterPopup
            .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
            .on("hide.bs.modal", function () {
            _this.filterNameInput.val("");
            TipsyHelper.clear(_this.filterNameInput);
            _this.isDefaultCheckbox.prop("checked", false);
            _this.isNotifyCheckbox.prop("checked", false);
            _this.inlineMessagePopup.hide();
        });
        saveFilterPopup.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(function () {
            var currentUrl = window.location.href;
            if (currentUrl.toLowerCase().indexOf("welcome=true")) {
                Common.Utils.ReloadPageAfterTimeout(currentUrl.split("?")[0]);
            }
            else {
                Common.Utils.ReloadPageAfterTimeout();
            }
        }); });
        saveFilterPopup.find("#btn-save-filter-apply-popup").click(function () { return _this.handleSaveFilterPopup(function () {
            return Common.Utils.ReloadPageAfterTimeout(_this.getMainPageUrl(_this.addFilterFormData.filterName));
        }); });
        $(".button-apply.apply").click(function (e) {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && _this.handleApplyFilter();
        });
        $(".button-save.save").click(function (e) {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && saveFilterPopup.modal("show");
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
                    text: d.Username,
                    subText: d.Username2 ? " (" + d.Username2 + ")" : ""
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
                var betTypeElement = $("#ddl-bettype").next(".btn-group");
                TipsyHelper.showError(betTypeElement, $("#hidden-BetTypesRequiredError").val());
                this.scrollTopInlineMessage(betTypeElement);
                return;
            }
        }
        var stake = $.trim(this.stakeInput.val().replace(/,/g, ""));
        if (!stake.length) {
            TipsyHelper.showError(this.stakeInput, $("#hidden-StakeRequiredError").val());
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
        var inputListMembers = "0";
        var myWatchlist = "0";
        var myWatchlistMembers = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.myWatchlistMemberTreeview.isSelectAll()) {
                filterByMembers = 1; // all my wachlist members
            }
            else {
                filterByMembers = 0;
                var memberMyWatchlist = this.myWatchlistMemberTreeview.getSelectedMembers();
                myWatchlist = memberMyWatchlist.custIds.join();
                myWatchlistMembers = memberMyWatchlist.usernames.join();
            }
            var memberTags = this.usernameTagsInput.getMemberTags();
            inputList = memberTags.custIds.join();
            inputListMembers = memberTags.usernames.join();
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
            inputListMembers: inputListMembers,
            myWatchlist: myWatchlist,
            myWatchlistMembers: myWatchlistMembers
        };
    };
    FilterConfigurationView.prototype.getFormDataPopup = function () {
        var filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            TipsyHelper.showError(this.filterNameInput, $("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = this.isDefaultCheckbox.is(":checked");
        var isNotify = this.isNotifyCheckbox.is(":checked");
        return {
            filterName: filterName,
            isDefault: isDefault,
            isNotify: isNotify
        };
    };
    FilterConfigurationView.prototype.handleApplyFilter = function () {
        var _this = this;
        this.addFilterFormData.filterName = $("#hidden-UnsavedFilter").val();
        AJAXHelper.post("ApplyFilter", this.addFilterFormData, function (result) {
            if (result.ErrCode) {
                _this.inlineMessage.showError(result.ErrMsg);
            }
            else {
                window.location.href = _this.getMainPageUrl(_this.addFilterFormData.filterName);
            }
        });
    };
    FilterConfigurationView.prototype.handleSaveFilterPopup = function (reloadPageCallback) {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            this.addFilterFormData = $.extend(this.addFilterFormData, formDataPopup);
            AJAXHelper.post("AddFilter", this.addFilterFormData, function (result) {
                _this.inlineMessagePopup.show(result);
                if (result.ErrCode) {
                    $(".lnk-chatbot").click(function () { location.href = $("#chatbot-path").val(); });
                }
                else {
                    reloadPageCallback();
                    _this.addFilterFormData = null;
                }
            });
        }
    };
    FilterConfigurationView.prototype.getMainPageUrl = function (filterName) {
        return window.location.href.split("?")[0].replace(/FilterConfiguration/i, "") + "?filterName=" + encodeURIComponent(filterName);
    };
    FilterConfigurationView.prototype.scrollTopInlineMessage = function ($element) {
        if ($element === void 0) { $element = undefined; }
        $("html, body").scrollTop(($element || this.inlineMessage.getInlineMessage()).offset().top);
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map