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
            let width = 900;
            let roleId = Number(this.ViewerLevelId);
            if (roleId >= 3) {
                width += 93;
            }
            if (roleId >= 4) {
                width += 93;
            }

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'Lucky3/ViewDetail' + params, "Lucky 3 - Detailed Bet List", width, 800);
        }
    }
}

let showLucky3Details: Function;

$(document).ready(() => {
    showLucky3Details = BetList.Choice.TicketDetails.Lucky3TicketDetails.CreateInstance().ShowLucky3Details;
})