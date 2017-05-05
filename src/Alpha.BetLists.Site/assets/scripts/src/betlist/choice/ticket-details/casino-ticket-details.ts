namespace BetList.Choice.TicketDetails {
    export class CasinoTicketDetails extends BaseTicketDetails {
        private static currentSportName: string;
        private static instance: CasinoTicketDetails;

        public static CreateInstance(sportName?: string): CasinoTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new CasinoTicketDetails() : this.instance;
            this.currentSportName = sportName ? sportName : "RNG Casino";

            return this.instance;
        }

        public ShowDetails = (refNo: string, winlossDate: Date, custId: string, transType: string, sportName: string) => {
            const params = "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + (sportName ? sportName : CasinoTicketDetails.currentSportName);

            Common.PopupDetails.Instance.Show(Common.AppPath.OldMainAppPath() + 'LiveCasinoBetList/BetDetail/RNGBetDetail' + params);
        }
    }

    export class AllbetTicketDetails extends BaseTicketDetails {
        private static instance: AllbetTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new AllbetTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowRunningBetsDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            const params = 'refno=' + refNo + '&custid=' + custId + '&transType=' + transType;

            Common.PopupDetails.Instance.Show(
                Common.AppPath.OldMainAppPath() + 'LiveCasinoBetList/BetDetail/Index?' + params,
                'Allbet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0]);
        }
    }
}

let showRnGCasinoDetails: Function;
let showVoidBridgeDetails: Function;
let showGlobalGaming: Function;
let showTodayRunningBetsDetails: Function // Allbet details

$(document).ready(() => {
    showRnGCasinoDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("RNG Casino").ShowDetails;
    showVoidBridgeDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Macau Games").ShowDetails;
    showGlobalGaming = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Fishing World").ShowDetails;
    showTodayRunningBetsDetails = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;
})