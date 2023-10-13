namespace BetList.Choice.TicketDetails {
    export class SportsbookTicketDetails extends BaseTicketDetails {
        private static instance: SportsbookTicketDetails;

        public static get Instance(): SportsbookTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SportsbookTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowSingleMatchParlayDetail = (transId: string, custId: string, status: string, transIndex: string) => {
            const params = "&TransactionId=" + transId
                + "&CustomerId=" + custId
                + "&Status=" + status;

            const url = Common.AppPath.BetListAppPath() + 'BetBuilder/ViewDetail?' + params;
            this.showSingleMatchParlyTickets(transId, url, transIndex);
        }

        private showSingleMatchParlyTickets = (transId: string, url: string, transIndex: string) => {
            const ticketsContent: JQuery = $('#divEvent_' + transId + transIndex);

            if (ticketsContent.html() === '') {
                const getSingleMatchParlaySetting: JQueryAjaxSettings = {
                    method: 'GET',
                    url: url,
                    beforeSend: () => {
                        const blockIconContent = '<span class="block-icon-small"></span>';
                        ticketsContent.html(blockIconContent);
                        ticketsContent.show();
                    },
                    success: (responseHtml) => {
                        ticketsContent.html(responseHtml);                        
                    },
                }

                $.ajax(getSingleMatchParlaySetting);
            }
            else {
                if (ticketsContent.is(':visible')) {
                    ticketsContent.hide();
                }
                else {
                    ticketsContent.show();                    
                }
            }
        }
    }

    export class ParlayTicketDetails extends BaseTicketDetails {
        private static instance: ParlayTicketDetails;

        public static get Instance(): ParlayTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowComboParlayDetails = (refNo: string, winlostDate: Date, custId: string, betTeam: string) => {
            const params = '?refno=' + refNo
                + '&winlostdate=' + winlostDate               
                + '&custid=' + custId
                + '&betteam=' + betTeam;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'Parlay/ComboParlayDetail' + params, "", 800, 438);
        }

        public ShowMixParlayTickets = (transId: string, refNo: string, custId: number) => {
            const params = '?transid=' + transId + '&custId=' + custId;
            const url = Common.AppPath.BetListAppPath() + 'Parlay/MixParlay' + params;
            this.showComboMixParlayTickets(transId, '', url);
        }

        public ShowComboParlayOldTickets = (transId: string, transIndex: string, refNo: string, custId: string) => {
            const params = '?transid=' + transId + '&custId=' + custId;
            const url = Common.AppPath.BetListAppPath() + 'Parlay/ComboParlay' + params;
            this.showComboMixParlayTickets(transId, transIndex, url);
        }

        public ShowComboParlayNewTickets = (
            transId: string,
            transIndex: string,
            refNo: string,
            handicap1: string,
            handicap2: string,
            betCheck: string,
            betTeam: string,
            custId: string) => {
            const params = '?transid=' + transId
                + '&handicap1=' + handicap1
                + '&handicap2=' + handicap2
                + '&betcheck=' + betCheck
                + '&betteam=' + betTeam
                + '&custId=' + custId;

            const url = Common.AppPath.BetListAppPath() + 'Parlay/ComboParlay' + params;
            this.showComboMixParlayTickets(transId, transIndex, url);
        }

        private showComboMixParlayTickets = (transId: string, transIndex: string, url: string) => {
            const ticketsContent: JQuery = $('#divEvent_' + transId + transIndex);
            const comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            var maxPayoutDiv = $('#maxPayout_' + transId + transIndex);

            if (ticketsContent.html() === '') {
                const getComboMixParlaySetting: JQueryAjaxSettings = {
                    method: 'GET',
                    url: url,
                    beforeSend: () => {
                        const blockIconContent = '<span class="block-icon-small"></span>';
                        ticketsContent.html(blockIconContent);
                        ticketsContent.show();
                    },
                    success: (responseHtml) => {
                        ticketsContent.html(responseHtml);
                        if (ticketsContent.find(".offer-name-list").length) {
                            comboOfferName.hide();
                            if (maxPayoutDiv.length) {
                                maxPayoutDiv.hide();
                                ticketsContent.find(".max-payout").each(function () {
                                    $(this).show();
                                });
                            }
                        }
                    },
                }

                $.ajax(getComboMixParlaySetting);
            }
            else {
                if (ticketsContent.is(':visible')) {
                    ticketsContent.hide();
                    comboOfferName.show();
                    maxPayoutDiv.show();
                }
                else {
                    ticketsContent.show();
                    if (ticketsContent.find(".offer-name-list").length) {
                        comboOfferName.hide();
                        maxPayoutDiv.hide();
                    }
                }
            }
        }        
    }

    export class CashOutHistoryDetails extends BaseTicketDetails {
        private static instance: CashOutHistoryDetails;

        public static get Instance(): CashOutHistoryDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new CashOutHistoryDetails() : this.instance;
            return this.instance;
        }

        public ViewNewCashOutHistory = (transId: string, custId: string, winlostDate: Date) => {
            const params = '?transid=' + transId
                + '&winlostdate=' + winlostDate
                + '&custid=' + custId;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CashOutHistory/Index' + params, "", 600, 438);
        }
    }
}

let showMP: Function;
let showCombMPDetail: Function;
let showCombMP: Function;
let showNewCombMP: Function;
let showSingleMatchParlayDetail: Function
let ViewNewCashOutHistory: Function
$(document).ready(() => {
    showMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets;
    showCombMPDetail = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayDetails;
    showCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets;
    showNewCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayNewTickets;
    showSingleMatchParlayDetail = BetList.Choice.TicketDetails.SportsbookTicketDetails.Instance.ShowSingleMatchParlayDetail;
    ViewNewCashOutHistory = BetList.Choice.TicketDetails.CashOutHistoryDetails.Instance.ViewNewCashOutHistory;
})
