namespace BetList.Choice.TicketDetails {
    export class NG2TicketDetails extends BaseTicketDetails {
        private static instance: NG2TicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new NG2TicketDetails() : this.instance;
            return this.instance;
        }

        public ShowNG2BetDetails = (transId: string, refNo: string, custId: string) => {
            const params = "?refNo=" + refNo;

            Common.PopupDetails.Instance.Show(Common.AppPath.ResultsAppPath + 'ng2/ParlayDetail' + params);
        }
    }
}

let showNG2Details: Function;

$(document).ready(() => {
    showNG2Details = BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails;
})