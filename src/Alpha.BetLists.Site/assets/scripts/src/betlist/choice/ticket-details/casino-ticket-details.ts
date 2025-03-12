namespace BetList.Choice.TicketDetails {
    export class CasinoTicketDetails extends BaseTicketDetails {
        private static instance: CasinoTicketDetails;

        public static get Instance(): CasinoTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new CasinoTicketDetails() : this.instance;

            return this.instance;
        }

        public ShowRnGCasinoDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType);
            this.ShowDetailsPopup(params);
        }

        public ShowVGamingDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Vgaming");
            this.ShowDetailsPopup(params);
        }

        public ShowSabaClubDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Saba.games");
            this.ShowDetailsPopup(params);
        }

        public ShowMacauGamesDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Macau Games");
            this.ShowDetailsPopup(params);
        }
        public showVideoPokerGameDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Arcadia Gaming");
            this.ShowDetailsPopup(params);
        }
        public ShowFishingWorldDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Fishing World");
            this.ShowDetailsPopup(params);
        }
        public ShowJokerDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Joker");
            this.ShowDetailsPopup(params);
        }
        public ShowPPDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "PP");
            this.ShowDetailsPopup(params);
        }

        public showSGDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Spadegaming");
            this.ShowDetailsPopup(params);
        }

        public showPGSoftDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "PG Soft");
            this.ShowDetailsPopup(params);
        }

        public ShowMaxGameDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "MaxGame");
            this.ShowDetailsPopup(params);
        }

        public ShowHabaneroDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Habanero");
            this.ShowDetailsPopup(params);
        }

        public ShowCGDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "CG");
            this.ShowDetailsPopup(params);
        }

        public ShowAdvantPlayBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "AdvantPlay");
            this.ShowDetailsPopup(params);
        }

        public ShowAdvantPlayMiniBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "AdvantPlay Mini");
            this.ShowDetailsPopup(params);
        }


        private CreatePopupDetailsParam = (refNo: string, winlossDate: Date, custId: string, transType: string, sportName: string = "Saba Casino"): string => {
            return "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + sportName
                + "&productId=6";
        }

        private ShowDetailsPopup = (params: string) => Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params, "", 1500);
    }

    export class AllbetTicketDetails extends BaseTicketDetails {
        private static instance: AllbetTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new AllbetTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowRunningBetsDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=Allbet&productId=21';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'Allbet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class JackpotTicketDetails extends BaseTicketDetails {
        private static instance: JackpotTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new JackpotTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowJackpotWonDetails = (jackpotTransId: string, custId: string, betType: string) => {
            const params = '?transid=' + jackpotTransId
                + '&custid=' + custId
                + '&bettype=' + betType
                + '&productId=' + 6;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1260);
        }
    }

    export class PPJackpotTicketDetails extends BaseTicketDetails {
        private static instance: PPJackpotTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PPJackpotTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowJackpotWonDetails = (jackpotTransId: string, custId: string, betType: string) => {
            const params = '?transid=' + jackpotTransId
                + '&custid=' + custId
                + '&bettype=' + betType
                + '&productId=' + 32;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1260);
        }
    }

    export class SexyGamingTicketDetails extends BaseTicketDetails {
        private static instance: SexyGamingTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SexyGamingTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowSexyGamingBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=AE Sexy&productId=45';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'AE Sexy - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class SAGamingTicketDetails extends BaseTicketDetails {
        private static instance: SAGamingTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SAGamingTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowSAGamingBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=SA Gaming&productId=39';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'SA Gaming - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class GTCasinoTicketDetails extends BaseTicketDetails {
        private static instance: GTCasinoTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GTCasinoTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowGTCasinoBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=IBCBetLiveCasino&productId=46';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'IBCBet Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class IONTicketDetails extends BaseTicketDetails {
        private static instance: IONTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new IONTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowIONBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=ION&productId=52';

            let width = 1510;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'ION - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class BBinTicketDetails extends BaseTicketDetails {
        private static instance: BBinTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BBinTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowBBinBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=BBin&productId=47';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/BBinIndex' + params,
                'BBIN - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class RNGWarTicketDetails extends BaseTicketDetails {
        private static instance: RNGWarTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new RNGWarTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowRNGWarBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=Funky Games&productId=50';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'Funky Games - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class WMTicketDetails extends BaseTicketDetails {
        private static instance: WMTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new WMTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowWMBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=WM&productId=51';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'WM - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class BGTicketDetails extends BaseTicketDetails {
        private static instance: BGTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BGTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowBGBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=BG&productId=57';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'BG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class PPLiveCasinoTicketDetails extends BaseTicketDetails {
        private static instance: PPLiveCasinoTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PPLiveCasinoTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowPragmaticLiveCasinoBetsDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=PP Live Casino&productId=65';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'PP Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class FGGTicketDetails extends BaseTicketDetails {
        private static instance: FGGTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new FGGTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowFGGBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=FGG&productId=70';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'FGG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class SealEntertainmentTicketDetails extends BaseTicketDetails {
        private static instance: SealEntertainmentTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SealEntertainmentTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowSealEntertainmentBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=SEAL Entertainment&productId=72';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'SEAL Entertainment - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }
    
    export class MGRNGTicketDetails extends BaseTicketDetails {
        private static instance: MGRNGTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new MGRNGTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowMGRNGCasinoBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=MG-RNG&productId=73';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'MG-RNG - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class JiliTicketDetails extends BaseTicketDetails {
        private static instance: JiliTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new JiliTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowJiliBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=Jili&productId=75';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'Jili - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class YeeBetTicketDetails extends BaseTicketDetails {
        private static instance: YeeBetTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new YeeBetTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowYeeBetTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=YeeBet&productId=76';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'YeeBet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class Live22TicketDetails extends BaseTicketDetails {
        private static instance: Live22TicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new Live22TicketDetails() : this.instance;
            return this.instance;
        }

        public ShowLive22TicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=Live22&productId=79';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'Live22 - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }
    
    export class WETicketDetails extends BaseTicketDetails {
        private static instance: WETicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new WETicketDetails() : this.instance;
            return this.instance;
        }

        public ShoWELiveCasinoBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=WE Live Casino&productId=80';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'WE Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class UUSlotsTicketDetails extends BaseTicketDetails {
        private static instance: UUSlotsTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new UUSlotsTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowUUSlotsTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=UUSLOTS&productId=78';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'UU SLOTS - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class SabaxDTicketDetails extends BaseTicketDetails {
        private static instance: SabaxDTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SabaxDTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowSabaxDTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string, sportName: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=' + sportName
                + '&productId=86';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                sportName + ' - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class FastSpinTicketDetails extends BaseTicketDetails {
        private static instance: FastSpinTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new FastSpinTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowFastSpinTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=FastSpin&productId=87';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'FastSpin - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }
        
    export class FACHAITicketDetails extends BaseTicketDetails {
        private static instance: FACHAITicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new FACHAITicketDetails() : this.instance;
            return this.instance;
        }

        public ShowFACHAITicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=FACHAI&productId=89';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'FA CHAI - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }
    
    export class NextspinTicketDetails extends BaseTicketDetails {
        private static instance: NextspinTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new NextspinTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowNextspinTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=Nextspin&productId=85';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'Nextspin - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class ONCasinoTicketDetails extends BaseTicketDetails {
        private static instance: ONCasinoTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ONCasinoTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowONCasinoTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=ON Casino&productId=96';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'ON Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class PlaytechTicketDetails extends BaseTicketDetails {
        private static instance: PlaytechTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PlaytechTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowPlaytechTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=Playtech&productId=100';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'Playtech - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }
    
    export class PlayStarTicketDetails extends BaseTicketDetails {
        private static instance: PlayStarTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PlayStarTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowPlayStarBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=PLAYSTAR&productId=91';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'PLAYSTAR - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class SabaKenoTicketDetails extends BaseTicketDetails {
        private static instance: SabaKenoTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SabaKenoTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowSabaKenoDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=SABA Keno&productId=102';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'SABA Keno - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class GPIRNGTicketDetails extends BaseTicketDetails {
        private static instance: GPIRNGTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GPIRNGTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowGPIBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=GPI&productId=48';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'GPI - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class GPILiveCasinoTicketDetails extends BaseTicketDetails {
        private static instance: GPILiveCasinoTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GPILiveCasinoTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowGPILCTicketDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=GPI Live Casino&productId=90';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/LiveCasino' + params,
                'GPI Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class ASKMESLOTTicketDetails extends BaseTicketDetails {
        private static instance: ASKMESLOTTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ASKMESLOTTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowASKMESLOTBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = '?refno=' + refNo
                + '&custid=' + custId
                + '&transType=' + transType
                + '&sportName=ASKMESLOT&productId=103';

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'ASKMESLOT - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }

    export class FunkyGamesTicketDetails extends BaseTicketDetails {
        private static instance: FunkyGamesTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance)
                ? new FunkyGamesTicketDetails()
                : this.instance;
            return this.instance;
        }

        public ShowFunkyGamesBetDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = `?refno=${refNo}&custid=${custId}&transType=${transType}&sportName=Funky Games&productId=104`;

            let width = 1500;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/RNG' + params,
                'Funky Games - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        };
    }
}

let showRnGCasinoDetails: Function;
let showVoidBridgeDetails: Function;
let showGlobalGaming: Function;
let showPPDetails: Function;
let showTodayRunningBetsDetails: Function; // Allbet details
let showSexyGamingBetDetails: Function;
let showSAGamingDetails: Function;
let showGTCasinoBetsDetails: Function;
let showProgressiveJackpotWonDetails: Function;
let showPPJackpotWonDetails: Function;
let showSGJackpotWonDetails: Function;
let showVideoPokerGameDetails: Function;
let showSGDetails: Function;
let showPGSoftDetails: Function;
let showSabaClubDetails: Function;
let showVGamingDetails: Function;
let showBBINBetDetails: Function;
let showRNGWARBetDetails: Function;
let showIONCasinoBetDetails: Function;
let showJokerBetDetails: Function;
let showWMBetDetails: Function;
let showBGBetDetails: Function;
let showMaxGameBetDetails: Function;
let showPragmaticLiveCasinoBetsDetails: Function;
let showHabaneroBetDetails: Function;
let showCGBetDetails: Function;
let showAdvantPlayBetDetails: Function;
let showAdvantPlayMiniBetDetails: Function;
let showFGGBetDetails: Function;
let showSealEntertainmentBetDetails: Function;
let showMGRNGCasinoBetDetails: Function;
let showJiliBetDetails: Function;
let showYeeBetBetDetails: Function;
let showLive22BetDetails: Function;
let showWELiveCasinoBetDetails: Function;
let showUUSlotsBetDetails: Function;
let showSabaxDDetails: Function;
let showFastSpinBetDetails: Function;
let showFACHAIBetDetails: Function;
let showNextspinBetDetails: Function;
let showONLiveCasinoBetDetails: Function;
let showPlaytechBetDetails: Function;
let showGPIBetDetails: Function;
let showGPILCBetDetails: Function;
let showPlayStarBetDetails: Function;
let showASKMESLOTBetDetails: Function;
let showSabaKenoDetails: Function;
let showFunkyGamesBetDetails: Function;

$(document).ready(() => {
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
    showLive22BetDetails = BetList.Choice.TicketDetails.Live22TicketDetails.Instance.ShowLive22TicketDetails;
    showWELiveCasinoBetDetails = BetList.Choice.TicketDetails.WETicketDetails.Instance.ShoWELiveCasinoBetDetails;
    showUUSlotsBetDetails = BetList.Choice.TicketDetails.UUSlotsTicketDetails.Instance.ShowUUSlotsTicketDetails;
    showSabaxDDetails = BetList.Choice.TicketDetails.SabaxDTicketDetails.Instance.ShowSabaxDTicketDetails;
    showFastSpinBetDetails = BetList.Choice.TicketDetails.FastSpinTicketDetails.Instance.ShowFastSpinTicketDetails;
    showFACHAIBetDetails = BetList.Choice.TicketDetails.FACHAITicketDetails.Instance.ShowFACHAITicketDetails;
    showNextspinBetDetails = BetList.Choice.TicketDetails.NextspinTicketDetails.Instance.ShowNextspinTicketDetails;
    showONLiveCasinoBetDetails = BetList.Choice.TicketDetails.ONCasinoTicketDetails.Instance.ShowONCasinoTicketDetails;
    showPlaytechBetDetails = BetList.Choice.TicketDetails.PlaytechTicketDetails.Instance.ShowPlaytechTicketDetails;
    showGPIBetDetails = BetList.Choice.TicketDetails.GPIRNGTicketDetails.Instance.ShowGPIBetDetails;
    showPlayStarBetDetails = BetList.Choice.TicketDetails.PlayStarTicketDetails.Instance.ShowPlayStarBetDetails;
    showASKMESLOTBetDetails = BetList.Choice.TicketDetails.ASKMESLOTTicketDetails.Instance.ShowASKMESLOTBetDetails;
    showSabaKenoDetails = BetList.Choice.TicketDetails.SabaKenoTicketDetails.Instance.ShowSabaKenoDetails;
    showFunkyGamesBetDetails = BetList.Choice.TicketDetails.FunkyGamesTicketDetails.Instance.ShowFunkyGamesBetDetails;
})