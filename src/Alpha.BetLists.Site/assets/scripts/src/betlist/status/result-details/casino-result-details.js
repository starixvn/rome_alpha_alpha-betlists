var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
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