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
                    if (betTypeId === 9 || betTypeId === 29 || betTypeId === 2799 || betTypeId === 10449) {
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
                    if (betTypeId == 5301) {
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
                    if (betTypeId >= 7601 && betTypeId <= 7700) {
                        this.live22Result = Common.Utils.IsNullOrUndefinedOrEmpty(this.live22Result) ? new ResultDetails.Live22ResultDetails() : this.live22Result;
                        return this.live22Result;
                    }
                    if (betTypeId >= 10300 && betTypeId <= 10399) {
                        this.weResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.weResult) ? new ResultDetails.WEResultDetails() : this.weResult;
                        return this.weResult;
                    }
                    if (betTypeId >= 10000 && betTypeId <= 10299) {
                        this.uuSlotsResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.uuSlotsResult) ? new ResultDetails.UUSlotsResultDetails() : this.uuSlotsResult;
                        return this.uuSlotsResult;
                    }
                    if (betTypeId >= 10801 && betTypeId <= 11000) {
                        this.sabaxDResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sabaxDResult) ? new ResultDetails.SabaxDResultDetails() : this.sabaxDResult;
                        return this.sabaxDResult;
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