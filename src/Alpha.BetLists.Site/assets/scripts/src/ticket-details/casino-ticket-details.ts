namespace TicketDetails {
    export class RnGCasinoTicketDetails extends BaseTicketDetails {
        private static instance: RnGCasinoTicketDetails;

        public static get Instance(): RnGCasinoTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new RnGCasinoTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowDetails = (refNo: string, winlossDate: Date, custId: string, transType: string, sportName: string) => {
            const params = "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + (sportName ? sportName : "RNG Casino")

            Common.PopupDetails.Instance.Show(this.OldMainPath + 'LiveCasinoBetList/BetDetail/RNGBetDetail' + params);
        }
    }
}

let showRnGCasinoDetails: Function;

$(document).ready(() => {
    showRnGCasinoDetails = TicketDetails.RnGCasinoTicketDetails.Instance.ShowDetails;
})