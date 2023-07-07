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
        this.inlineMessage = new InlineMessageComponent();
        this.betTypeDropdown = new DropdownListComponent();
        this.betTypeDropdown.render("ddl-bettypes");
        this.matchLiveIndicatorDropdown = new DropdownListComponent();
        this.matchLiveIndicatorDropdown.render("ddl-matchLiveIndicator", {
            onChange: function (select) { return _this.getSports(select.val(), _this.groupOfSportsDropdown.getSelected()[0]); }
        });
        this.groupOfSportsDropdown = new DropdownListComponent();
        this.groupOfSportsDropdown.render("ddl-groupOfSport", {
            onChange: function (select) { return _this.getSports(_this.matchLiveIndicatorDropdown.getSelected()[0], select.val()); }
        });
        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption("sportleaguematch-treeview", "Search Sport/ League", "No Sport/ League found", "Select All", "No information is available", false, 400));
        this.monitoredWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.monitoredWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption("monitored-watchlist-members", "Search member username", "No member username found", "All (Include newly add ...)", "No information is available", false, 400));
        this.memberTagsInput = new UsernameTagsInputComponent();
        this.memberTagsInput.render(new UsernameTagsInputOption("username", 100, "Max: # username(s)", "Duplicated username"));
        this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
    };
    FilterConfigurationView.prototype.bindEvents = function () {
        var _this = this;
        this.stakeInput = $("#txt-stake");
        this.stakeInput.on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });
        var boxFilterBySports = $("#box-filterBySports");
        $("#rdo-allSports").change(function () {
            this.checked && boxFilterBySports.addClass("hide");
        });
        var rdoSpecificSports = $("#rdo-specificSports").change(function (e) {
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
        var addFilterPopup = $("#add-filter-popup");
        addFilterPopup.on("hidden.bs.modal", function () {
            $("#txt-filterName").val("");
            $("#chk-isDefault").prop("checked", false);
        });
        addFilterPopup.find(".btn-submit").click(function () {
            var formDataPopup = _this.getFormDataPopup();
            if (formDataPopup) {
                _this.addFilterFormData = $.extend(_this.addFilterFormData, formDataPopup);
                AJAXHelper.postJSON("AddFilter", _this.addFilterFormData, function (result) {
                    _this.inlineMessagePopup.show(result);
                    if (!result.ErrCode) {
                        _this.addFilterFormData = null;
                        _this.reloadPageAfterTimeout();
                    }
                });
            }
        });
        $(".button-reset").click(function () {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && addFilterPopup.modal("show");
            console.dir(_this.addFilterFormData);
        });
    };
    FilterConfigurationView.prototype.getSports = function (isLive, groupSport) {
        var _this = this;
        this.sportLeagueMatchTreeview.addIconLoading();
        $.get("FilterBySports?isLive=" + isLive + "&groupSport=" + groupSport, function (data) {
            var listData = data.map(function (sport) {
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
            return _this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
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
        var sports = [{ sport: 0 }];
        var isLive = false;
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
            if (!watchList.length && !inputList.length) {
                this.inlineMessage.showError($("#hidden-FilterByMembersRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }
        this.inlineMessage.hide();
        return {
            betType: betType,
            stake: stake,
            isLive: isLive,
            sports: sports,
            isWatchList: isWatchList,
            inputList: inputList,
            watchList: watchList
        };
    };
    FilterConfigurationView.prototype.getFormDataPopup = function () {
        var filterNameElement = $("#txt-filterName");
        var filterName = $.trim(filterNameElement.val());
        if (!filterName.length) {
            this.inlineMessagePopup.showError($("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = $("#chk-isDefault").is(":checked");
        this.inlineMessagePopup.hide();
        return {
            filterName: filterName,
            isDefault: isDefault
        };
    };
    FilterConfigurationView.prototype.scrollTopInlineMessage = function () {
        $("html, body").scrollTop(this.inlineMessage.getInlineMessage().offset().top);
    };
    FilterConfigurationView.prototype.reloadPageAfterTimeout = function (timeout) {
        if (timeout === void 0) { timeout = 3000; }
        setTimeout(function () {
            window.location.reload();
        }, timeout);
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map