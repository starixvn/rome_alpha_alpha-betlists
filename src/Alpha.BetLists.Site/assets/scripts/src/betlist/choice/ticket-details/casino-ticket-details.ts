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

        public ShowMacauGamesDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Macau Games");
            this.ShowDetailsPopup(params);
        }

        public ShowFishingWorldDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = this.CreatePopupDetailsParam(refNo, winlossDate, custId, transType, "Fishing World");
            this.ShowDetailsPopup(params);
        }

        private CreatePopupDetailsParam = (refNo: string, winlossDate: Date, custId: string, transType: string, sportName: string = "RNG Casino"): string => {
            return "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + sportName;
        }

        private ShowDetailsPopup = (params: string) => Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'BetDetail/RNGCasino' + params, "", 1120);
    }

    export class AllbetTicketDetails extends BaseTicketDetails {
        private static instance: AllbetTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new AllbetTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowRunningBetsDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = 'refno=' + refNo + '&custid=' + custId + '&transType=' + transType;

            let width = 1047;
            let roleId = Number(this.ViewerLevelId);

            if (roleId <= 3) {
                width -= 105;
            }

            if (roleId <= 2) {
                width -= 105;
            }

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.BetListAppPath() + 'BetDetail/Allbet?' + params,
                'Allbet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0], width, 800);
        }
    }
}

let showRnGCasinoDetails: Function;
let showVoidBridgeDetails: Function;
let showGlobalGaming: Function;
let showTodayRunningBetsDetails: Function // Allbet details

$(document).ready(() => {
    showRnGCasinoDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails;
    showVoidBridgeDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails;
    showGlobalGaming = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails;
    showTodayRunningBetsDetails = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;
})
