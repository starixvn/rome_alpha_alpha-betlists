var Common;
(function (Common) {
    var PopupDetails = /** @class */ (function () {
        function PopupDetails() {
            var _this = this;
            this.Show = function (url, title, width, height, options) {
                if (!_this.mainDomain) {
                    _this.topWindow.popupManager.openMultiple(url, title, width, height, undefined, undefined, undefined, undefined, undefined, undefined, options);
                }
                else {
                    _this.mainDomain = _this.mainDomain.indexOf('http') === -1 ?
                        location.origin.split(':')[0] + ':' + _this.mainDomain + '/site-main' : _this.mainDomain;
                    if (!title) {
                        var queryStrings = url.indexOf('?') !== -1 ?
                            url.split('&') : [''];
                        for (var i in queryStrings) {
                            if (queryStrings[i].toLowerCase().indexOf('parlay') !== -1
                                && (url.toLowerCase().indexOf('betteam=3') !== -1 || url.toLowerCase().indexOf('betteam=7') !== -1)) {
                                title = "System Parlay";
                                break;
                            }
                            if (queryStrings[i].toLowerCase().indexOf('parlay') !== -1
                                && url.toLowerCase().indexOf('betteam=1') !== -1) {
                                title = "Mix Parlay";
                                break;
                            }
                            if (queryStrings[i].toLowerCase().indexOf('cashout') !== -1) {
                                var transId = "";
                                for (var j in queryStrings) {
                                    if (queryStrings[j].toLowerCase().indexOf("transid") !== -1) {
                                        transId = queryStrings[j].split('=')[1];
                                        break;
                                    }
                                }
                                title = "Cash Out History - " + transId;
                                break;
                            }
                            if (queryStrings[i].toLowerCase().indexOf('sportname') !== -1) {
                                title = queryStrings[i].split('=')[1] + ' Details';
                                break;
                            }
                        }
                    }
                    _this.topWindow.postMessage({
                        event_id: "betlist-result",
                        title: title,
                        width: width,
                        height: height,
                        options: options,
                        url: (_this.IsCallingOtherSites(url) ? $("#MainDomain").val() : _this.betlistUrl) + url
                    }, _this.mainDomain);
                }
            };
            this.IsCallingOtherSites = function (url) {
                return url.indexOf('/site-results/') > -1 || url.indexOf('/ex-main/') > -1 || url.indexOf('/ex-totalbetsforecast/') > -1;
            };
        }
        PopupDetails.Instance = function (topWindow) {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;
            this.instance.topWindow = topWindow;
            this.instance.mainDomain = $("#MainDomain").val();
            this.instance.betlistUrl = $("#BetListUrl").val();
            return this.instance;
        };
        return PopupDetails;
    }());
    Common.PopupDetails = PopupDetails;
})(Common || (Common = {}));
//# sourceMappingURL=popup-details.js.map
var Common;
(function (Common) {
    var Utils = /** @class */ (function () {
        function Utils() {
        }
        Utils.IsNullOrUndefinedOrEmpty = function (value) { return value === null || value === undefined || value === ''; };
        Utils.ParseBoolean = function (value) {
            if (!Utils.IsNullOrUndefinedOrEmpty(value)) {
                return value.toLowerCase() === "true";
            }
            return false;
        };
        return Utils;
    }());
    Common.Utils = Utils;
})(Common || (Common = {}));
//# sourceMappingURL=utils.js.map
var Common;
(function (Common) {
    var AppPath = /** @class */ (function () {
        function AppPath() {
        }
        AppPath.OldMainAppPath = function () { return $('#old-main-path').val(); };
        AppPath.BetListAppPath = function () { return $('#betlist-path').val(); };
        AppPath.ResultsAppPath = function () { return $("#results-path").val(); };
        AppPath.TotalBetsForecastAppPath = function () { return $("#totalbets-forecast-path").val(); };
        AppPath.MainAppPath = function () { return $("#site-main-path").val(); };
        return AppPath;
    }());
    Common.AppPath = AppPath;
})(Common || (Common = {}));
//# sourceMappingURL=app-path.js.map
"use strict";
var AlphaCore = /** @class */ (function () {
    function AlphaCore($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy",
            dayMonthName: "DD MMM",
            day: "DD"
        };
        this.sort = {
            none: 0,
            ascending: 1,
            descending: -1
        };
        this.keyCode = {
            enter: 13
        };
        this.numberDisplayedProductsOnFilter = 1;
        this.maxTimesToShowTooltip = 5;
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.splitCell = function (firstCell, cellClass, lastCellStyle) {
            if (cellClass === void 0) { cellClass = ""; }
            if (lastCellStyle === void 0) { lastCellStyle = "bold"; }
            var this$ = _this.$;
            var combinedCells = _this.$("." + firstCell + "-combined");
            combinedCells.each(function (index) {
                var oldValues = this$(this).find("li");
                var cellsLength = oldValues.length;
                var oldCells = "";
                for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                    oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                        (i === lastCellIndex ? " " + lastCellStyle : "") + "\">" +
                        this$(oldValues[i]).html() +
                        "</td>";
                }
                this$(combinedCells[index]).before(oldCells);
            });
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : _this.$.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : _this.$.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.combineConsecutiveCells = function (firstCells, numberOfCombinedCells, combinedCellClass, combinedCellColSpan) {
            if (combinedCellColSpan === void 0) { combinedCellColSpan = 1; }
            if (numberOfCombinedCells <= 1) {
                return;
            }
            var this$ = _this.$;
            firstCells.each(function (index) {
                var newCell = "<td class=\"visible-stack-mode combinedcell " +
                    combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";
                var currentCell = this$(firstCells[index]);
                var isLastCell = false;
                for (var i = 0; i < numberOfCombinedCells; i++) {
                    newCell += "<li>" + currentCell.html() + "</li>";
                    if (currentCell.next().length) {
                        currentCell = currentCell.next();
                    }
                    else {
                        isLastCell = true;
                    }
                }
                newCell += "</ul>\
                        </td>";
                if (isLastCell) {
                    currentCell.after(newCell);
                }
                else {
                    currentCell.prev().after(newCell);
                }
            });
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };
        this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
            if (typeof queryString === "undefined"
                || queryString === null) {
                return "";
            }
            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");
            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;
            return newQueryString;
        };
        this.correctEvenRowClass = function ($tableReport) {
            var this$ = _this.$;
            var isEvenRow = false;
            $tableReport.find("tbody > tr").each(function () {
                var $row = this$(this);
                if ($row.hasClass("even-row") && !isEvenRow) {
                    $row.removeClass("even-row");
                }
                else if (!$row.hasClass("even-row") && isEvenRow) {
                    $row.addClass("even-row");
                }
                isEvenRow = !isEvenRow;
            });
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.formatNumber = function (value) { return value.toLocaleString("en-US"); };
        this.formatCurrency = function (value) { return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); };
        this.$ = $;
        this.store = store;
    }
    return AlphaCore;
}());
var alphaReports = new AlphaCore($, store);
//# sourceMappingURL=core.js.map
var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var ResultDetails;
        (function (ResultDetails) {
            var ResultDetailsFactory = /** @class */ (function () {
                function ResultDetailsFactory() {
                }
                ResultDetailsFactory.CreateInstance = function (betTypeId) {
                    if (betTypeId === 9 || betTypeId === 29 || betTypeId === 2799) {
                        this.mixParlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mixParlayResult) ? new ResultDetails.MixParlayResultDetails() : this.mixParlayResult;
                        return this.mixParlayResult;
                    }
                    if (betTypeId >= 1011 && betTypeId <= 1020) {
                        this.fishingWorldResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fishingWorldResult) ? new ResultDetails.FishingWorldResultDetails() : this.fishingWorldResult;
                        return this.fishingWorldResult;
                    }
                    if (betTypeId >= 1611 && betTypeId <= 1699) {
                        this.SabaClubResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.SabaClubResult) ? new ResultDetails.SabaClubResultDetails() : this.SabaClubResult;
                        return this.SabaClubResult;
                    }
                    if ((betTypeId >= 2101 && betTypeId <= 2399) || (betTypeId >= 2901 && betTypeId <= 2999)) {
                        this.rngCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.rngCasinoResult) ? new ResultDetails.RnGCasinoResultDetails : this.rngCasinoResult;
                        return this.rngCasinoResult;
                    }
                    if (betTypeId >= 4001 && betTypeId <= 4100 || betTypeId >= 5800 && betTypeId <= 5999) {
                        this.macauGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.macauGamesResult) ? new ResultDetails.MacauGamesResultDetails() : this.macauGamesResult;
                        return this.macauGamesResult;
                    }
                    if ((betTypeId >= 2401 && betTypeId <= 2499) ||
                        (betTypeId >= 3001 && betTypeId <= 3099) ||
                        (betTypeId >= 3101 && betTypeId <= 3149) ||
                        (betTypeId >= 3151 && betTypeId <= 3200) ||
                        (betTypeId >= 3201 && betTypeId <= 3299) ||
                        (betTypeId >= 3401 && betTypeId <= 3499)) {
                        this.allbetResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.allbetResult) ? new ResultDetails.AllbetResultDetails() : this.allbetResult;
                        return this.allbetResult;
                    }
                    if (betTypeId == 3300) {
                        this.bbinResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.bbinResult) ? new ResultDetails.BBINResultDetails() : this.bbinResult;
                        return this.bbinResult;
                    }
                    if (221 <= betTypeId && betTypeId <= 227) {
                        this.fastMarketsResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fastMarketsResult) ? new ResultDetails.FastMarketsResultDetails() : this.fastMarketsResult;
                        return this.fastMarketsResult;
                    }
                    if ((betTypeId >= 4101 && betTypeId <= 4300) ||
                        betTypeId >= 6701 && betTypeId <= 7000) {
                        this.ppResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.ppResult) ? new ResultDetails.PPResultDetails() : this.ppResult;
                        return this.ppResult;
                    }
                    if (betTypeId >= 4301 && betTypeId <= 4600) {
                        this.spadegamingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.spadegamingResult) ? new ResultDetails.SpadegamingResultDetails() : this.spadegamingResult;
                        return this.spadegamingResult;
                    }
                    if (betTypeId >= 1901 && betTypeId <= 1999) {
                        this.pokerGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.pokerGamesResult) ? new ResultDetails.PokerGamesResultDetails() : this.pokerGamesResult;
                        return this.pokerGamesResult;
                    }
                    if (betTypeId >= 4601 && betTypeId <= 4900) {
                        this.virtualGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.virtualGamesResult) ? new ResultDetails.VirtualGamesResultDetails() : this.virtualGamesResult;
                        return this.virtualGamesResult;
                    }
                    if (betTypeId >= 5301 && betTypeId <= 5399) {
                        this.LeapGamingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.LeapGamingResult) ? new ResultDetails.LeapGamingResultDetails() : this.LeapGamingResult;
                        return this.LeapGamingResult;
                    }
                    if (betTypeId >= 5500 && betTypeId <= 5599) {
                        this.RNGWarResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.RNGWarResult) ? new ResultDetails.RNGWarCasinoResultDetails() : this.RNGWarResult;
                        return this.RNGWarResult;
                    }
                    if (betTypeId >= 5601 && betTypeId <= 5699) {
                        this.IONCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.IONCasinoResult) ? new ResultDetails.IONCasinoResultDetails() : this.IONCasinoResult;
                        return this.IONCasinoResult;
                    }
                    if (betTypeId >= 3501 && betTypeId <= 3599) {
                        this.BGResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.BGResult) ? new ResultDetails.BGResultDetails() : this.BGResult;
                        return this.BGResult;
                    }
                    if (betTypeId >= 5701 && betTypeId <= 5799) {
                        this.GDGResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.GDGResult) ? new ResultDetails.GDGResultDetails() : this.GDGResult;
                        return this.GDGResult;
                    }
                    if (betTypeId >= 6001 && betTypeId <= 6300) {
                        this.PGSoftCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.PGSoftCasinoResult) ? new ResultDetails.PGSoftCasinoResultDetails() : this.PGSoftCasinoResult;
                        return this.PGSoftCasinoResult;
                    }
                    if (betTypeId >= 6401 && betTypeId <= 6700) {
                        this.JokerResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.JokerResult) ? new ResultDetails.JokerResultDetails() : this.JokerResult;
                        return this.JokerResult;
                    }
                    if (betTypeId >= 3601 && betTypeId <= 3610) {
                        this.linkedGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.linkedGamesResult) ? new ResultDetails.Happy5LinkedGamesResultDetails() : this.linkedGamesResult;
                        return this.linkedGamesResult;
                    }
                    if (betTypeId >= 3611 && betTypeId <= 3620) {
                        this.sportsLotteryResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sportsLotteryResult) ? new ResultDetails.SportsLotteryResultDetails() : this.sportsLotteryResult;
                        return this.sportsLotteryResult;
                    }
                    if (betTypeId >= 5401 && betTypeId <= 5499) {
                        this.maxGameResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.maxGameResult) ? new ResultDetails.MaxGameResultDetails() : this.maxGameResult;
                        return this.maxGameResult;
                    }
                    if (betTypeId >= 7301 && betTypeId <= 7600) {
                        this.cgResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.cgResult) ? new ResultDetails.CGResultDetails() : this.cgResult;
                        return this.cgResult;
                    }
                    if (betTypeId >= 3701 && betTypeId <= 3799) {
                        this.ppLiveCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.ppLiveCasinoResult) ? new ResultDetails.PPLiveCasinoResultDetails() : this.ppLiveCasinoResult;
                        return this.ppLiveCasinoResult;
                    }
                    if (betTypeId >= 7001 && betTypeId <= 7300) {
                        this.habaneroResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.habaneroResult) ? new ResultDetails.HabaneroResultDetails() : this.habaneroResult;
                        return this.habaneroResult;
                    }
                    if (betTypeId >= 3802 && betTypeId <= 3899) {
                        this.VGamingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.VGamingResult) ? new ResultDetails.VGamingResultDetails() : this.VGamingResult;
                        return this.VGamingResult;
                    }
                    if (betTypeId >= 7701 && betTypeId <= 7799) {
                        this.advantPlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.advantPlayResult) ? new ResultDetails.AdvantPlayResultDetails() : this.advantPlayResult;
                        return this.advantPlayResult;
                    }
                    if (betTypeId >= 7801 && betTypeId <= 7899) {
                        this.advantPlayMiniResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.advantPlayMiniResult) ? new ResultDetails.AdvantPlayMiniResultDetails() : this.advantPlayResult;
                        return this.advantPlayMiniResult;
                    }
                    if (betTypeId >= 8201 && betTypeId <= 8299) {
                        this.fggResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fggResult) ? new ResultDetails.FGGResultDetails() : this.fggResult;
                        return this.fggResult;
                    }
                    if (betTypeId >= 8900 && betTypeId <= 8999) {
                        this.sealResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sealResult) ? new ResultDetails.SEALResultDetails() : this.sealResult;
                        return this.sealResult;
                    }
                    if (betTypeId >= 8400 && betTypeId <= 8699) {
                        this.mgResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mgResult) ? new ResultDetails.MGResultDetails() : this.mgResult;
                        return this.mgResult;
                    }
                    if (betTypeId >= 5100 && betTypeId <= 5299) {
                        this.jiliResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.jiliResult) ? new ResultDetails.JiliResultDetails() : this.jiliResult;
                        return this.jiliResult;
                    }
                    this.sportsbookResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sportsbookResult) ? new ResultDetails.SportsbookResultDetails : this.sportsbookResult;
                    return this.sportsbookResult;
                };
                return ResultDetailsFactory;
            }());
            ResultDetails.ResultDetailsFactory = ResultDetailsFactory;
            var BaseResultDetails = /** @class */ (function () {
                function BaseResultDetails() {
                    this.ResultDetailsPath = Common.AppPath.ResultsAppPath() + "Normal/ResultDetails";
                    this.ProductIds = $('#product-ids').val();
                    this.BetListType = $('#betlist-type').val();
                    this.FromDate = $('#from-date').val();
                    this.ToDate = $('#to-date').val();
                    this.CustId = $('#cust-id').val();
                    this.ViewerLevelId = $('#viewer-level-id').val();
                    this.IsHistoricReport = Common.Utils.ParseBoolean($('#is-historic-report').val());
                    this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc, siteId) {
                        ResultDetailsFactory.CreateInstance(betTypeId).ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc, siteId);
                    };
                }
                Object.defineProperty(BaseResultDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BaseResultDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return BaseResultDetails;
            }());
            ResultDetails.BaseResultDetails = BaseResultDetails;
        })(ResultDetails = Status.ResultDetails || (Status.ResultDetails = {}));
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
var ViewResult;
$(document).ready(function () {
    ViewResult = BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails;
});
//# sourceMappingURL=base-result-details.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var ResultDetails;
        (function (ResultDetails) {
            var SportsbookResultDetails = /** @class */ (function (_super) {
                __extends(SportsbookResultDetails, _super);
                function SportsbookResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, transdesc) {
                        var params = '?matchid=' + matchId
                            + '&bettypeid=' + betTypeId
                            + '&race=' + race
                            + '&sportId=' + sportTypeId
                            + '&leagueId=' + league
                            + '&isOutright=' + isOutright
                            + '&refnum=' + refNo
                            + '&betid=' + betId
                            + '&transdesc=' + transdesc
                            + '&isViewResultFromBetList=true';
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 730, 250);
                    };
                    return _this;
                }
                return SportsbookResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.SportsbookResultDetails = SportsbookResultDetails;
            var MixParlayResultDetails = /** @class */ (function (_super) {
                __extends(MixParlayResultDetails, _super);
                function MixParlayResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId) {
                        var params = '?refnum=' + refNoMixParlay
                            + '&wldate=' + winLostDate
                            + '&bettypeid=' + betTypeId
                            + '&matchid=1&isViewResultFromBetList=true';
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 800, 500);
                    };
                    return _this;
                }
                return MixParlayResultDetails;
            }(SportsbookResultDetails));
            ResultDetails.MixParlayResultDetails = MixParlayResultDetails;
            var FastMarketsResultDetails = /** @class */ (function (_super) {
                __extends(FastMarketsResultDetails, _super);
                function FastMarketsResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, hdp2) {
                        var params = '?matchid=' + matchId
                            + '&bettypeid=' + betTypeId
                            + '&race=' + race
                            + '&sportId=' + sportTypeId
                            + '&leagueId=' + league
                            + '&isOutright=' + isOutright
                            + '&refnum=' + refNo
                            + '&betid=' + betId
                            + '&isViewResultFromBetList=true'
                            + '&hdp2=' + hdp2;
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 730, 250);
                    };
                    return _this;
                }
                return FastMarketsResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.FastMarketsResultDetails = FastMarketsResultDetails;
        })(ResultDetails = Status.ResultDetails || (Status.ResultDetails = {}));
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=sportsbook-result-details.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var ResultDetails;
        (function (ResultDetails) {
            var AllbetResultDetails = /** @class */ (function (_super) {
                __extends(AllbetResultDetails, _super);
                function AllbetResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc) {
                        var params = '?transDesc=' + transDesc + '&betCheck=' + betCheck + '&bettypeId=' + betTypeId + '&sportId=' + sportTypeId;
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 835, 500);
                    };
                    return _this;
                }
                return AllbetResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.AllbetResultDetails = AllbetResultDetails;
            var RnGCasinoResultDetails = /** @class */ (function (_super) {
                __extends(RnGCasinoResultDetails, _super);
                function RnGCasinoResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=6';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/RngCasinoResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return RnGCasinoResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.RnGCasinoResultDetails = RnGCasinoResultDetails;
            var MacauGamesResultDetails = /** @class */ (function (_super) {
                __extends(MacauGamesResultDetails, _super);
                function MacauGamesResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=22';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MacauGamesResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return MacauGamesResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.MacauGamesResultDetails = MacauGamesResultDetails;
            var FishingWorldResultDetails = /** @class */ (function (_super) {
                __extends(FishingWorldResultDetails, _super);
                function FishingWorldResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck) {
                        var params = '?custId=' + _this.CustId +
                            '&roleId=' + _this.ViewerLevelId +
                            '&gameId=' + betCheck +
                            '&productId=24';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
                    };
                    return _this;
                }
                return FishingWorldResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.FishingWorldResultDetails = FishingWorldResultDetails;
            var PPResultDetails = /** @class */ (function (_super) {
                __extends(PPResultDetails, _super);
                function PPResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=32';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/PPResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return PPResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.PPResultDetails = PPResultDetails;
            var SpadegamingResultDetails = /** @class */ (function (_super) {
                __extends(SpadegamingResultDetails, _super);
                function SpadegamingResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=36';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/SpadegamingResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return SpadegamingResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.SpadegamingResultDetails = SpadegamingResultDetails;
            var VirtualGamesResultDetails = /** @class */ (function (_super) {
                __extends(VirtualGamesResultDetails, _super);
                function VirtualGamesResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=38';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/VirtualGamesResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return VirtualGamesResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.VirtualGamesResultDetails = VirtualGamesResultDetails;
            var RNGWarCasinoResultDetails = /** @class */ (function (_super) {
                __extends(RNGWarCasinoResultDetails, _super);
                function RNGWarCasinoResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=50';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/RNGWarResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return RNGWarCasinoResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.RNGWarCasinoResultDetails = RNGWarCasinoResultDetails;
            var IONCasinoResultDetails = /** @class */ (function (_super) {
                __extends(IONCasinoResultDetails, _super);
                function IONCasinoResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=52';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/IONCasinoResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return IONCasinoResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.IONCasinoResultDetails = IONCasinoResultDetails;
            var SabaClubResultDetails = /** @class */ (function (_super) {
                __extends(SabaClubResultDetails, _super);
                function SabaClubResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, matchCode, transDesc, betcheck) {
                        var params = '?custId=' + _this.CustId +
                            '&roleId=' + _this.ViewerLevelId +
                            '&gameRoundId=' + betcheck +
                            '&gameId=' + betTypeId +
                            '&sportId=' + sportTypeId +
                            '&siteId=' + matchCode +
                            '&productId=42';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'SabaClub/Index' + params, "", 835, 675);
                    };
                    return _this;
                }
                return SabaClubResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.SabaClubResultDetails = SabaClubResultDetails;
            var PGSoftCasinoResultDetails = /** @class */ (function (_super) {
                __extends(PGSoftCasinoResultDetails, _super);
                function PGSoftCasinoResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=55';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/PGSoftResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return PGSoftCasinoResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.PGSoftCasinoResultDetails = PGSoftCasinoResultDetails;
            var JokerResultDetails = /** @class */ (function (_super) {
                __extends(JokerResultDetails, _super);
                function JokerResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=56';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/JokerResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return JokerResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.JokerResultDetails = JokerResultDetails;
            var GDGResultDetails = /** @class */ (function (_super) {
                __extends(GDGResultDetails, _super);
                function GDGResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=59';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/GDGResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return GDGResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.GDGResultDetails = GDGResultDetails;
            var Happy5LinkedGamesResultDetails = /** @class */ (function (_super) {
                __extends(Happy5LinkedGamesResultDetails, _super);
                function Happy5LinkedGamesResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + refNo +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=3' +
                            '&BetTypeId=' + betTypeId +
                            '&SportTypeId=' + sportTypeId;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/LinkedGamesResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return Happy5LinkedGamesResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.Happy5LinkedGamesResultDetails = Happy5LinkedGamesResultDetails;
            var SportsLotteryResultDetails = /** @class */ (function (_super) {
                __extends(SportsLotteryResultDetails, _super);
                function SportsLotteryResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=63';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/SportsLotteryDetailResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return SportsLotteryResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.SportsLotteryResultDetails = SportsLotteryResultDetails;
            var BGResultDetails = /** @class */ (function (_super) {
                __extends(BGResultDetails, _super);
                function BGResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=57';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/BGResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return BGResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.BGResultDetails = BGResultDetails;
            var MaxGameResultDetails = /** @class */ (function (_super) {
                __extends(MaxGameResultDetails, _super);
                function MaxGameResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&BetType=' + betTypeId +
                            '&endtime=' + winLostDate +
                            '&productId=60';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MaxGameResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return MaxGameResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.MaxGameResultDetails = MaxGameResultDetails;
            var PPLiveCasinoResultDetails = /** @class */ (function (_super) {
                __extends(PPLiveCasinoResultDetails, _super);
                function PPLiveCasinoResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&BetType=' + betTypeId +
                            '&productId=65';
                        var mainDomain = location.origin.split(':')[0] + ':' + $("#MainDomain").val() + '/site-main';
                        window.top.postMessage({
                            event_id: "pplivecasino-result",
                            url: 'ThirdParty/PPLiveCasinoResult' + params
                        }, mainDomain);
                    };
                    return _this;
                }
                return PPLiveCasinoResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.PPLiveCasinoResultDetails = PPLiveCasinoResultDetails;
            var HabaneroResultDetails = /** @class */ (function (_super) {
                __extends(HabaneroResultDetails, _super);
                function HabaneroResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=61';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/HabaneroResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return HabaneroResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.HabaneroResultDetails = HabaneroResultDetails;
            var CGResultDetails = /** @class */ (function (_super) {
                __extends(CGResultDetails, _super);
                function CGResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=62';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/CGResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return CGResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.CGResultDetails = CGResultDetails;
            var VGamingResultDetails = /** @class */ (function (_super) {
                __extends(VGamingResultDetails, _super);
                function VGamingResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, matchCode, transDesc, betcheck) {
                        var params = '?custId=' + _this.CustId +
                            '&roleId=' + _this.ViewerLevelId +
                            '&gameRoundId=' + betcheck +
                            '&gameId=' + betTypeId +
                            '&sportId=' + sportTypeId +
                            '&siteId=' + matchCode +
                            '&currencyName=' + userName +
                            '&productId=66';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/VGamingResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return VGamingResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.VGamingResultDetails = VGamingResultDetails;
            var AdvantPlayResultDetails = /** @class */ (function (_super) {
                __extends(AdvantPlayResultDetails, _super);
                function AdvantPlayResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=67';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/AdvantPlayResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return AdvantPlayResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.AdvantPlayResultDetails = AdvantPlayResultDetails;
            var AdvantPlayMiniResultDetails = /** @class */ (function (_super) {
                __extends(AdvantPlayMiniResultDetails, _super);
                function AdvantPlayMiniResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=68';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/AdvantPlayMiniResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return AdvantPlayMiniResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.AdvantPlayMiniResultDetails = AdvantPlayMiniResultDetails;
            var BBINResultDetails = /** @class */ (function (_super) {
                __extends(BBINResultDetails, _super);
                function BBINResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&productId=47';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/BBINResult' + params, "", 835, 900);
                    };
                    return _this;
                }
                return BBINResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.BBINResultDetails = BBINResultDetails;
            var PokerGamesResultDetails = /** @class */ (function (_super) {
                __extends(PokerGamesResultDetails, _super);
                function PokerGamesResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&BetType=' + betTypeId +
                            '&productId=34';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/CardClubResult' + params, "", 1099, 500, { resizable: true });
                    };
                    return _this;
                }
                return PokerGamesResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.PokerGamesResultDetails = PokerGamesResultDetails;
            var FGGResultDetails = /** @class */ (function (_super) {
                __extends(FGGResultDetails, _super);
                function FGGResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&endtime=' + winLostDate +
                            '&productId=70';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/FGGResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return FGGResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.FGGResultDetails = FGGResultDetails;
            var SEALResultDetails = /** @class */ (function (_super) {
                __extends(SEALResultDetails, _super);
                function SEALResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&BetType=' + betTypeId +
                            '&productId=72';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/SEALResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return SEALResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.SEALResultDetails = SEALResultDetails;
            var MGResultDetails = /** @class */ (function (_super) {
                __extends(MGResultDetails, _super);
                function MGResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&BetType=' + betTypeId +
                            '&productId=73';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MGResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return MGResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.MGResultDetails = MGResultDetails;
            var JiliResultDetails = /** @class */ (function (_super) {
                __extends(JiliResultDetails, _super);
                function JiliResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, currencyId, gameId3rd, custId, transId3rd, siteId) {
                        var params = '?custId=' + custId +
                            '&transId3rd=' + transId3rd +
                            '&gameId3rd=' + gameId3rd +
                            '&currencyId=' + currencyId +
                            '&siteId=' + siteId +
                            '&BetType=' + betTypeId +
                            '&productId=75';
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/JiliResult' + params, "", 835, 675);
                    };
                    return _this;
                }
                return JiliResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.JiliResultDetails = JiliResultDetails;
        })(ResultDetails = Status.ResultDetails || (Status.ResultDetails = {}));
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=casino-result-details.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var ResultDetails;
        (function (ResultDetails) {
            var LeapGamingResultDetails = /** @class */ (function (_super) {
                __extends(LeapGamingResultDetails, _super);
                function LeapGamingResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc) {
                        var params = '?custId=' + _this.CustId +
                            '&roleId=' + _this.ViewerLevelId +
                            '&sportId=' + sportTypeId +
                            '&transDesc=' + transDesc +
                            '&productId=49';
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 835, 675);
                    };
                    return _this;
                }
                return LeapGamingResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.LeapGamingResultDetails = LeapGamingResultDetails;
        })(ResultDetails = Status.ResultDetails || (Status.ResultDetails = {}));
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=other-result-details.js.map
var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var IpDetails = /** @class */ (function () {
            function IpDetails() {
                this.ShowIpInfo = function (ip) {
                    Common.PopupDetails.Instance(window.top).Show(Common.AppPath.OldMainAppPath() + '_IPInfo/IpInfo.aspx?ip=' + ip, 'IP Details', 400, 400);
                };
            }
            Object.defineProperty(IpDetails, "Instance", {
                get: function () {
                    this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new IpDetails() : this.instance;
                    return this.instance;
                },
                enumerable: true,
                configurable: true
            });
            return IpDetails;
        }());
        Status.IpDetails = IpDetails;
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
var OpenIPInfo;
$(document).ready(function () {
    OpenIPInfo = BetList.Status.IpDetails.Instance.ShowIpInfo;
});
//# sourceMappingURL=ip-details.js.map
var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var ScoreMap = /** @class */ (function () {
            function ScoreMap() {
                this.ShowScoreMap = function (matchIds, betType, liveIndicator) {
                    var params = liveIndicator == 1 ?
                        "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=" + betType + "&bettypes_dead=-1" :
                        "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=-1&bettypes_dead=" + betType;
                    Common.PopupDetails.Instance(window.top).Show(Common.AppPath.TotalBetsForecastAppPath() + "ForecastScoreMap/Index?" + params, "", 1008);
                };
            }
            Object.defineProperty(ScoreMap, "Instance", {
                get: function () {
                    this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ScoreMap() : this.instance;
                    return this.instance;
                },
                enumerable: true,
                configurable: true
            });
            return ScoreMap;
        }());
        Choice.ScoreMap = ScoreMap;
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var OpenScoreMap;
$(document).ready(function () {
    OpenScoreMap = BetList.Choice.ScoreMap.Instance.ShowScoreMap;
});
//# sourceMappingURL=score-map.js.map
var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var TicketDetails;
        (function (TicketDetails) {
            var BaseTicketDetails = /** @class */ (function () {
                function BaseTicketDetails() {
                    this.ViewerId = function () { return $('#viewer-id').val(); };
                    this.ViewerLevelId = function () { return $('#viewer-level-id').val(); };
                }
                return BaseTicketDetails;
            }());
            TicketDetails.BaseTicketDetails = BaseTicketDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=base-ticket-details.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var TicketDetails;
        (function (TicketDetails) {
            var SportsbookTicketDetails = /** @class */ (function (_super) {
                __extends(SportsbookTicketDetails, _super);
                function SportsbookTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowSingleMatchParlayDetail = function (transId, custId, status, transIndex) {
                        var params = "&TransactionId=" + transId
                            + "&CustomerId=" + custId
                            + "&Status=" + status;
                        var url = Common.AppPath.BetListAppPath() + 'BetBuilder/ViewDetail?' + params;
                        _this.showSingleMatchParlyTickets(transId, url, transIndex);
                    };
                    _this.showSingleMatchParlyTickets = function (transId, url, transIndex) {
                        var ticketsContent = $('#divEvent_' + transId + transIndex);
                        if (ticketsContent.html() === '') {
                            var getSingleMatchParlaySetting = {
                                method: 'GET',
                                url: url,
                                beforeSend: function () {
                                    var blockIconContent = '<span class="block-icon-small"></span>';
                                    ticketsContent.html(blockIconContent);
                                    ticketsContent.show();
                                },
                                success: function (responseHtml) {
                                    ticketsContent.html(responseHtml);
                                },
                            };
                            $.ajax(getSingleMatchParlaySetting);
                        }
                        else {
                            if (ticketsContent.is(':visible')) {
                                ticketsContent.hide();
                            }
                            else {
                                ticketsContent.show();
                            }
                        }
                    };
                    return _this;
                }
                Object.defineProperty(SportsbookTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SportsbookTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return SportsbookTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.SportsbookTicketDetails = SportsbookTicketDetails;
            var ParlayTicketDetails = /** @class */ (function (_super) {
                __extends(ParlayTicketDetails, _super);
                function ParlayTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowComboParlayDetails = function (refNo, winlostDate, transId, betId, custId, betTeam) {
                        var params = '?refno=' + refNo
                            + '&winlostdate=' + winlostDate
                            + '&transid=' + transId
                            + '&betid=' + betId
                            + '&custid=' + custId
                            + '&betteam=' + betTeam;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'Parlay/ComboParlayDetail' + params, "", 800, 438);
                    };
                    _this.ShowMixParlayTickets = function (transId, refNo, custId) {
                        var params = '?transid=' + transId + '&custId=' + custId;
                        var url = Common.AppPath.BetListAppPath() + 'Parlay/MixParlay' + params;
                        _this.showComboMixParlayTickets(transId, '', url);
                    };
                    _this.ShowComboParlayOldTickets = function (transId, transIndex, refNo, custId) {
                        var params = '?transid=' + transId + '&custId=' + custId;
                        var url = Common.AppPath.BetListAppPath() + 'Parlay/ComboParlay' + params;
                        _this.showComboMixParlayTickets(transId, transIndex, url);
                    };
                    _this.ShowComboParlayNewTickets = function (transId, transIndex, refNo, handicap1, handicap2, betCheck, betTeam, custId) {
                        var params = '?transid=' + transId
                            + '&handicap1=' + handicap1
                            + '&handicap2=' + handicap2
                            + '&betcheck=' + betCheck
                            + '&betteam=' + betTeam
                            + '&custId=' + custId;
                        var url = Common.AppPath.BetListAppPath() + 'Parlay/ComboParlay' + params;
                        _this.showComboMixParlayTickets(transId, transIndex, url);
                    };
                    _this.showComboMixParlayTickets = function (transId, transIndex, url) {
                        var ticketsContent = $('#divEvent_' + transId + transIndex);
                        var comboOfferName = $('#combinationOfferName_' + transId + transIndex);
                        if (ticketsContent.html() === '') {
                            var getComboMixParlaySetting = {
                                method: 'GET',
                                url: url,
                                beforeSend: function () {
                                    var blockIconContent = '<span class="block-icon-small"></span>';
                                    ticketsContent.html(blockIconContent);
                                    ticketsContent.show();
                                },
                                success: function (responseHtml) {
                                    ticketsContent.html(responseHtml);
                                    if (ticketsContent.find(".offer-name-list").length) {
                                        comboOfferName.hide();
                                    }
                                },
                            };
                            $.ajax(getComboMixParlaySetting);
                        }
                        else {
                            if (ticketsContent.is(':visible')) {
                                ticketsContent.hide();
                                comboOfferName.show();
                            }
                            else {
                                ticketsContent.show();
                                if (ticketsContent.find(".offer-name-list").length) {
                                    comboOfferName.hide();
                                }
                            }
                        }
                    };
                    return _this;
                }
                Object.defineProperty(ParlayTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return ParlayTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.ParlayTicketDetails = ParlayTicketDetails;
            var CashOutHistoryDetails = /** @class */ (function (_super) {
                __extends(CashOutHistoryDetails, _super);
                function CashOutHistoryDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewNewCashOutHistory = function (transId, custId, winlostDate) {
                        var params = '?transid=' + transId
                            + '&winlostdate=' + winlostDate
                            + '&custid=' + custId;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CashOutHistory/Index' + params, "", 600, 438);
                    };
                    return _this;
                }
                Object.defineProperty(CashOutHistoryDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new CashOutHistoryDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return CashOutHistoryDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.CashOutHistoryDetails = CashOutHistoryDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var showMP;
var showCombMPDetail;
var showCombMP;
var showNewCombMP;
var showSingleMatchParlayDetail;
var ViewNewCashOutHistory;
$(document).ready(function () {
    showMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets;
    showCombMPDetail = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayDetails;
    showCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets;
    showNewCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayNewTickets;
    showSingleMatchParlayDetail = BetList.Choice.TicketDetails.SportsbookTicketDetails.Instance.ShowSingleMatchParlayDetail;
    ViewNewCashOutHistory = BetList.Choice.TicketDetails.CashOutHistoryDetails.Instance.ViewNewCashOutHistory;
});
//# sourceMappingURL=sportsbook-ticket-details.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var TicketDetails;
        (function (TicketDetails) {
            var CasinoTicketDetails = /** @class */ (function (_super) {
                __extends(CasinoTicketDetails, _super);
                function CasinoTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowRnGCasinoDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType);
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowVGamingDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Vgaming");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowSabaClubDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Saba.games");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowMacauGamesDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Macau Games");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.showVideoPokerGameDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Arcadia Gaming");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowFishingWorldDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Fishing World");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowJokerDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Joker");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowPPDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "PP");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.showSGDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Spadegaming");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.showPGSoftDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "PG Soft");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowMaxGameDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "MaxGame");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowHabaneroDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Habanero");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowCGDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "CG");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowAdvantPlayBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "AdvantPlay");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.ShowAdvantPlayMiniBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = _this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "AdvantPlay Mini");
                        _this.ShowDetailsPopup(params);
                    };
                    _this.CreatePopupDetailsParam = function (refNo, winlossDate, custId, transType, sportName) {
                        if (sportName === void 0) { sportName = "Saba Casino"; }
                        return "?refNo=" + refNo
                            + "&transType=" + transType
                            + "&custId=" + custId
                            + "&sportName=" + sportName
                            + "&productId=6";
                    };
                    _this.ShowDetailsPopup = function (params) { return Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, "", 1120); };
                    return _this;
                }
                Object.defineProperty(CasinoTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new CasinoTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return CasinoTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.CasinoTicketDetails = CasinoTicketDetails;
            var AllbetTicketDetails = /** @class */ (function (_super) {
                __extends(AllbetTicketDetails, _super);
                function AllbetTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowRunningBetsDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=Allbet&productId=21';
                        var width = 1047;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'Allbet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(AllbetTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new AllbetTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return AllbetTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.AllbetTicketDetails = AllbetTicketDetails;
            var JackpotTicketDetails = /** @class */ (function (_super) {
                __extends(JackpotTicketDetails, _super);
                function JackpotTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowJackpotWonDetails = function (jackpotTransId, custId, betType) {
                        var params = '?transid=' + jackpotTransId
                            + '&custid=' + custId
                            + '&bettype=' + betType
                            + '&productId=' + 6;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1120);
                    };
                    return _this;
                }
                Object.defineProperty(JackpotTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new JackpotTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return JackpotTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.JackpotTicketDetails = JackpotTicketDetails;
            var PPJackpotTicketDetails = /** @class */ (function (_super) {
                __extends(PPJackpotTicketDetails, _super);
                function PPJackpotTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowJackpotWonDetails = function (jackpotTransId, custId, betType) {
                        var params = '?transid=' + jackpotTransId
                            + '&custid=' + custId
                            + '&bettype=' + betType
                            + '&productId=' + 32;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1120);
                    };
                    return _this;
                }
                Object.defineProperty(PPJackpotTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PPJackpotTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return PPJackpotTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.PPJackpotTicketDetails = PPJackpotTicketDetails;
            var SexyGamingTicketDetails = /** @class */ (function (_super) {
                __extends(SexyGamingTicketDetails, _super);
                function SexyGamingTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowSexyGamingBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=AE Sexy&productId=45';
                        var width = 1047;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'AE Sexy - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(SexyGamingTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SexyGamingTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return SexyGamingTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.SexyGamingTicketDetails = SexyGamingTicketDetails;
            var SAGamingTicketDetails = /** @class */ (function (_super) {
                __extends(SAGamingTicketDetails, _super);
                function SAGamingTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowSAGamingBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=SA Gaming&productId=39';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'SA Gaming - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(SAGamingTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SAGamingTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return SAGamingTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.SAGamingTicketDetails = SAGamingTicketDetails;
            var GTCasinoTicketDetails = /** @class */ (function (_super) {
                __extends(GTCasinoTicketDetails, _super);
                function GTCasinoTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowGTCasinoBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=IBCBetLiveCasino&productId=46';
                        var width = 1047;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'IBCBet Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(GTCasinoTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GTCasinoTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return GTCasinoTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.GTCasinoTicketDetails = GTCasinoTicketDetails;
            var IONTicketDetails = /** @class */ (function (_super) {
                __extends(IONTicketDetails, _super);
                function IONTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowIONBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=ION&productId=52';
                        var width = 1047;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'ION - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(IONTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new IONTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return IONTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.IONTicketDetails = IONTicketDetails;
            var BBinTicketDetails = /** @class */ (function (_super) {
                __extends(BBinTicketDetails, _super);
                function BBinTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowBBinBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=BBin&productId=47';
                        var width = 1047;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'BBIN - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(BBinTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BBinTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return BBinTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.BBinTicketDetails = BBinTicketDetails;
            var RNGWarTicketDetails = /** @class */ (function (_super) {
                __extends(RNGWarTicketDetails, _super);
                function RNGWarTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowRNGWarBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=Funky Games&productId=50';
                        var width = 1047;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'Funky Games - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(RNGWarTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new RNGWarTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return RNGWarTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.RNGWarTicketDetails = RNGWarTicketDetails;
            var WMTicketDetails = /** @class */ (function (_super) {
                __extends(WMTicketDetails, _super);
                function WMTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowWMBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=WM&productId=51';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'WM - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(WMTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new WMTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return WMTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.WMTicketDetails = WMTicketDetails;
            var GDGTicketDetails = /** @class */ (function (_super) {
                __extends(GDGTicketDetails, _super);
                function GDGTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowGDGBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=GDG&productId=59';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'GDG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(GDGTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GDGTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return GDGTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.GDGTicketDetails = GDGTicketDetails;
            var BGTicketDetails = /** @class */ (function (_super) {
                __extends(BGTicketDetails, _super);
                function BGTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowBGBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=BG&productId=57';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'BG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(BGTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BGTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return BGTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.BGTicketDetails = BGTicketDetails;
            var PPLiveCasinoTicketDetails = /** @class */ (function (_super) {
                __extends(PPLiveCasinoTicketDetails, _super);
                function PPLiveCasinoTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowPragmaticLiveCasinoBetsDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=PP Live Casino&productId=65';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'PP Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(PPLiveCasinoTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PPLiveCasinoTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return PPLiveCasinoTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.PPLiveCasinoTicketDetails = PPLiveCasinoTicketDetails;
            var FGGTicketDetails = /** @class */ (function (_super) {
                __extends(FGGTicketDetails, _super);
                function FGGTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowFGGBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=FGG&productId=70';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'FGG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(FGGTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new FGGTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return FGGTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.FGGTicketDetails = FGGTicketDetails;
            var SealEntertainmentTicketDetails = /** @class */ (function (_super) {
                __extends(SealEntertainmentTicketDetails, _super);
                function SealEntertainmentTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowSealEntertainmentBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=SEAL Entertainment&productId=72';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'SEAL Entertainment - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(SealEntertainmentTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SealEntertainmentTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return SealEntertainmentTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.SealEntertainmentTicketDetails = SealEntertainmentTicketDetails;
            var MGRNGTicketDetails = /** @class */ (function (_super) {
                __extends(MGRNGTicketDetails, _super);
                function MGRNGTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowMGRNGCasinoBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=MG-RNG&productId=73';
                        var width = 1207;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'MG-RNG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(MGRNGTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new MGRNGTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return MGRNGTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.MGRNGTicketDetails = MGRNGTicketDetails;
            var JiliTicketDetails = /** @class */ (function (_super) {
                __extends(JiliTicketDetails, _super);
                function JiliTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowJiliBetDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=Jili&productId=75';
                        var width = 1207;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'Jili - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(JiliTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new JiliTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return JiliTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.JiliTicketDetails = JiliTicketDetails;
            var YeeBetTicketDetails = /** @class */ (function (_super) {
                __extends(YeeBetTicketDetails, _super);
                function YeeBetTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowYeeBetTicketDetails = function (refNo, winlossDate, custId, transType) {
                        var params = '?refno=' + refNo
                            + '&custid=' + custId
                            + '&transType=' + transType
                            + '&sportName=YeeBet&productId=76';
                        var width = 1147;
                        var roleId = Number(_this.ViewerLevelId);
                        if (roleId <= 3) {
                            width -= 105;
                        }
                        if (roleId <= 2) {
                            width -= 105;
                        }
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, 'YeeBet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
                    };
                    return _this;
                }
                Object.defineProperty(YeeBetTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new YeeBetTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return YeeBetTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.YeeBetTicketDetails = YeeBetTicketDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var showRnGCasinoDetails;
var showVoidBridgeDetails;
var showGlobalGaming;
var showPPDetails;
var showTodayRunningBetsDetails; // Allbet details
var showSexyGamingBetDetails;
var showSAGamingDetails;
var showGTCasinoBetsDetails;
var showProgressiveJackpotWonDetails;
var showPPJackpotWonDetails;
var showSGJackpotWonDetails;
var showVideoPokerGameDetails;
var showSGDetails;
var showPGSoftDetails;
var showSabaClubDetails;
var showVGamingDetails;
var showBBINBetDetails;
var showRNGWARBetDetails;
var showIONCasinoBetDetails;
var showJokerBetDetails;
var showWMBetDetails;
var showGDGBetDetails;
var showBGBetDetails;
var showMaxGameBetDetails;
var showPragmaticLiveCasinoBetsDetails;
var showHabaneroBetDetails;
var showCGBetDetails;
var showAdvantPlayBetDetails;
var showAdvantPlayMiniBetDetails;
var showFGGBetDetails;
var showSealEntertainmentBetDetails;
var showMGRNGCasinoBetDetails;
var showJiliBetDetails;
var showYeeBetBetDetails;
$(document).ready(function () {
    showRnGCasinoDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails;
    showVoidBridgeDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails;
    showVideoPokerGameDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.showVideoPokerGameDetails;
    showGlobalGaming = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails;
    showPPDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowPPDetails;
    showTodayRunningBetsDetails = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;
    showProgressiveJackpotWonDetails = BetList.Choice.TicketDetails.JackpotTicketDetails.Instance.ShowJackpotWonDetails;
    showPPJackpotWonDetails = BetList.Choice.TicketDetails.PPJackpotTicketDetails.Instance.ShowJackpotWonDetails;
    showSGJackpotWonDetails = BetList.Choice.TicketDetails.PPJackpotTicketDetails.Instance.ShowJackpotWonDetails;
    showSGDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.showSGDetails;
    showPGSoftDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.showPGSoftDetails;
    showSexyGamingBetDetails = BetList.Choice.TicketDetails.SexyGamingTicketDetails.Instance.ShowSexyGamingBetDetails;
    showSabaClubDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowSabaClubDetails;
    showVGamingDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowVGamingDetails;
    showSAGamingDetails = BetList.Choice.TicketDetails.SAGamingTicketDetails.Instance.ShowSAGamingBetDetails;
    showGTCasinoBetsDetails = BetList.Choice.TicketDetails.GTCasinoTicketDetails.Instance.ShowGTCasinoBetDetails;
    showIONCasinoBetDetails = BetList.Choice.TicketDetails.IONTicketDetails.Instance.ShowIONBetDetails;
    showBBINBetDetails = BetList.Choice.TicketDetails.BBinTicketDetails.Instance.ShowBBinBetDetails;
    showRNGWARBetDetails = BetList.Choice.TicketDetails.RNGWarTicketDetails.Instance.ShowRNGWarBetDetails;
    showJokerBetDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowJokerDetails;
    showWMBetDetails = BetList.Choice.TicketDetails.WMTicketDetails.Instance.ShowWMBetDetails;
    showGDGBetDetails = BetList.Choice.TicketDetails.GDGTicketDetails.Instance.ShowGDGBetDetails;
    showBGBetDetails = BetList.Choice.TicketDetails.BGTicketDetails.Instance.ShowBGBetDetails;
    showMaxGameBetDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMaxGameDetails;
    showPragmaticLiveCasinoBetsDetails = BetList.Choice.TicketDetails.PPLiveCasinoTicketDetails.Instance.ShowPragmaticLiveCasinoBetsDetails;
    showHabaneroBetDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowHabaneroDetails;
    showCGBetDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowCGDetails;
    showAdvantPlayBetDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowAdvantPlayBetDetails;
    showAdvantPlayMiniBetDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowAdvantPlayMiniBetDetails;
    showFGGBetDetails = BetList.Choice.TicketDetails.FGGTicketDetails.Instance.ShowFGGBetDetails;
    showSealEntertainmentBetDetails = BetList.Choice.TicketDetails.SealEntertainmentTicketDetails.Instance.ShowSealEntertainmentBetDetails;
    showMGRNGCasinoBetDetails = BetList.Choice.TicketDetails.MGRNGTicketDetails.Instance.ShowMGRNGCasinoBetDetails;
    showJiliBetDetails = BetList.Choice.TicketDetails.JiliTicketDetails.Instance.ShowJiliBetDetails;
    showYeeBetBetDetails = BetList.Choice.TicketDetails.YeeBetTicketDetails.Instance.ShowYeeBetTicketDetails;
});
//# sourceMappingURL=casino-ticket-details.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var TicketDetails;
        (function (TicketDetails) {
            var GoldDeluxeTicketDetails = /** @class */ (function (_super) {
                __extends(GoldDeluxeTicketDetails, _super);
                function GoldDeluxeTicketDetails() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                Object.defineProperty(GoldDeluxeTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GoldDeluxeTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                GoldDeluxeTicketDetails.prototype.ShowDetails = function (id) {
                    var element = document.getElementById(id);
                    if (element.style.display === "block") {
                        element.style.display = "none";
                    }
                    else {
                        element.style.display = "block";
                    }
                };
                return GoldDeluxeTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.GoldDeluxeTicketDetails = GoldDeluxeTicketDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var showDetail;
$(document).ready(function () {
    showDetail = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
});
//# sourceMappingURL=other-ticket-details.js.map