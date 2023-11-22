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
                    _this.ShowDetailsPopup = function (params) { return Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, "", 1500); };
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
                        var width = 1500;
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
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1260);
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
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1260);
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1510;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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
                        var width = 1500;
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