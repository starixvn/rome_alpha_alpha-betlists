namespace BetList.Choice.TicketDetails {
    export class NG2TicketDetails extends BaseTicketDetails {
        private static instance: NG2TicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new NG2TicketDetails() : this.instance;
            return this.instance;
        }

        public ShowNG2BetDetails = (transId: string, refNo: string, custId: string) => {
            const params = "?refNo=" + refNo;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ng2/ParlayDetail' + params, "", 600, 350);
        }
    }

    export class GoldDeluxeTicketDetails extends BaseTicketDetails {
        private static instance: GoldDeluxeTicketDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GoldDeluxeTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowDetails(id: string) {
            const element = document.getElementById(id);

            if (element.style.display === "block") {
                element.style.display = "none"
            } else {
                element.style.display = "block";
            }
        }
    }
}

let showNG2Details: Function;
let showDetail: Function;

$(document).ready(() => {
    showNG2Details = BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails;
    showDetail = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
})