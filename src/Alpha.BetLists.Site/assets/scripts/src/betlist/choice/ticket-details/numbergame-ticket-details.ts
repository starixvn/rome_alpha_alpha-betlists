namespace BetList.Choice.TicketDetails {
    export class Lucky3TicketDetails extends BaseTicketDetails {
        private static instance: Lucky3TicketDetails;

        public static CreateInstance(): Lucky3TicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new Lucky3TicketDetails() : this.instance;

            return this.instance;
        }

        public ShowLucky3Details = (transId: string, custId: string) => {
            const params = "?TransactionId=" + transId
                + "&custId=" + custId;

            Common.PopupDetails.Instance.Show(Common.AppPath.BetListAppPath() + 'Lucky3/ViewDetail' + params);
        }
    }    
}

let showLucky3Details: Function;

$(document).ready(() => {
    showLucky3Details = BetList.Choice.TicketDetails.Lucky3TicketDetails.CreateInstance().ShowLucky3Details;
})