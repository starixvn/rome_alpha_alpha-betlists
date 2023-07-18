namespace BetList.Choice.TicketDetails {
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

let showDetail: Function;

$(document).ready(() => {
    showDetail = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
})