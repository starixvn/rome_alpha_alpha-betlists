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
                + "&sportName=" + (sportName ? sportName : CasinoTicketDetails.currentSportName)

            Common.PopupDetails.Instance.Show(Common.AppPath.OldMainAppPath + 'LiveCasinoBetList/BetDetail/RNGBetDetail' + params);
        }
    }

    export class AllbetTicketDetails extends BaseTicketDetails {
        private static instance: AllbetTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new AllbetTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowRunningBetsDetails = (refNo: string, winlossDate: Date, custId: string, transType: string) => {
            var params = 'refno=' + refNo + '&custid=' + custId + '&transType=' + transType;

            Common.PopupDetails.Instance.Show(
                Common.AppPath.OldMainAppPath + 'LiveCasinoBetList/BetDetail/Index?' + params,
                'Allbet - Detailed Bet list - ' + winlossDate.toString().split(' ')[0]);
        }
    }

    export class P2PTicketDetails extends BaseTicketDetails {
        private static instance: P2PTicketDetails;

        public static get Instance(): P2PTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new P2PTicketDetails() : this.instance;

            return this.instance;
        }

        public ShowHandHistory = (custId: string, ibcRefno: string, currency: string, tableId: string,
            tableName: string, tournamentId: string, tournamentName: string,
            gameType: string, betType: string, date: string) => {
            const url = '../P2P/HandHistory.aspx?custid=' + custId + '&ibcrefno=' + ibcRefno
                + '&currency=' + currency + '&tableid=' + tableId
                + '&tablename=' + tableName + '&tournamentid=' + tournamentId
                + '&tournamentname=' + tournamentName + '&gametype=' + gameType
                + '&bettype=' + betType + '&Date=' + date;

            window.location.href = url;
        }

        public ShowHandHistoryDetail = (custId: string, handNo: string, handType: string, currency: string) => {
            const url = '../P2P/HandHistoryDetail.aspx?custid=' + custId + '&handno=' + handNo
                + '&handtype=' + handType + '&currency=' + currency;

            window.location.href = url;
        }

        public ShowCommissionDetail = (ibcRefno: string, currency: string, tableId: string,
            tableName: string, tournamentId: string, tournamentName: string,
            gameType: string, betType: string) => {
            const url = '../P2P/CommissionDetail.aspx?ibcrefno=' + ibcRefno + '&currency=' + currency
                + '&tableid=' + tableId + '&tablename=' + tableName
                + '&tournamentid=' + tournamentId + '&tournamentname=' + tournamentName
                + '&gametype=' + gameType + '&bettype=' + betType;

            window.location.href = url;
        }
    }
}

let showRnGCasinoDetails: Function;
let showVoidBridgeDetails: Function;
let showGlobalGaming: Function;
let showTodayRunningBetsDetails: Function // Allbet details
let ViewHandHistory: Function;
let ViewHandHistoryDetail: Function;
let ViewCommissionDetail: Function;

// Todo
let showNG2Details: Function;

$(document).ready(() => {
    showRnGCasinoDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("RNG Casino").ShowDetails;
    showVoidBridgeDetails = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Macau Games").ShowDetails;
    showGlobalGaming = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Fishing World").ShowDetails;
    showTodayRunningBetsDetails = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;
    ViewHandHistory = BetList.Choice.TicketDetails.P2PTicketDetails.Instance.ShowHandHistory;
    ViewHandHistoryDetail = BetList.Choice.TicketDetails.P2PTicketDetails.Instance.ShowHandHistoryDetail;
    ViewCommissionDetail = BetList.Choice.TicketDetails.P2PTicketDetails.Instance.ShowCommissionDetail;
})