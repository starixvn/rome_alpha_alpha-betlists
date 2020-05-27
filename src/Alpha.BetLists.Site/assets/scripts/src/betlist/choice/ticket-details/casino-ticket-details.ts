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

        public ShowSabaClubDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Sabaclub");
            this.ShowDetailsPopup(params);
        }

        public ShowMacauGamesDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Macau Games");
            this.ShowDetailsPopup(params);
        }
        public showVideoPokerGameDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Card Club");
            this.ShowDetailsPopup(params);
        }
        public ShowFishingWorldDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Fishing World");
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

        private CreatePopupDetailsParam = (refNo: string, winlossDate: Date, custId: string, transType: string, sportName: string = "RNG Casino"): string => {
            return "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + sportName
                + "&productId=6";
        }

        private ShowDetailsPopup = (params: string) => Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params, "", 1120);
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

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params,
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
                + '&bettype=' + betType;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'JackpotDetails/Index' + params, "", 1120);
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
                + '&sportName=SexyGaming&productId=45';

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params,
                'Sexy Gaming - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
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
                + '&sportName=SexyGaming&productId=45';

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params,
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

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params,
                'IBCBet Live Casino - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
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

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params,
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
                + '&sportName=ION&productId=50';

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'CasinoDetails/Index' + params,
                'RNG War - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
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
let showVideoPokerGameDetails: Function;
let showSGDetails: Function;
let showSabaClubDetails: Function;
let showBBINBetDetails: Function;
let showRNGWARBetDetails: Function;

$(document).ready(() => {
    showRnGCasinoDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails;
    showVoidBridgeDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails;
    showVideoPokerGameDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.showVideoPokerGameDetails;
    showGlobalGaming = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails;
    showPPDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowPPDetails;
    showTodayRunningBetsDetails = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;
    showProgressiveJackpotWonDetails = BetList.Choice.TicketDetails.JackpotTicketDetails.Instance.ShowJackpotWonDetails;
    showSGDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.showSGDetails;
    showSexyGamingBetDetails = BetList.Choice.TicketDetails.SexyGamingTicketDetails.Instance.ShowSexyGamingBetDetails;
    showSabaClubDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowSabaClubDetails;
    showSAGamingDetails = BetList.Choice.TicketDetails.SAGamingTicketDetails.Instance.ShowSAGamingBetDetails;
    showGTCasinoBetsDetails = BetList.Choice.TicketDetails.GTCasinoTicketDetails.Instance.ShowGTCasinoBetDetails;
    showBBINBetDetails = BetList.Choice.TicketDetails.BBinTicketDetails.Instance.ShowBBinBetDetails;
    showRNGWARBetDetails = BetList.Choice.TicketDetails.RNGWarTicketDetails.Instance.ShowRNGWarBetDetails;
})