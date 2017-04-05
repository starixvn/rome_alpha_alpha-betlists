namespace BetList.Choice.TicketDetails {
    export class SportsbookTicketDetails extends BaseTicketDetails {
        private static instance: SportsbookTicketDetails;

        public static get Instance(): SportsbookTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SportsbookTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowColossusBetsDetails = (refNo: string, winlossDate: Date, transId: string, custId: string, eventId: string) => {
            const params = "Refno=" + refNo
                + "&WinLossDate=" + winlossDate
                + "&TransId=" + transId
                + "&CustId=" + custId
                + "&EventId=" + eventId
                + "&RoleId=" + this.ViewerLevelId
                + "&AccId=" + this.ViewerId;

            Common.PopupDetails.Instance.Show(Common.AppPath.OldMainAppPath + 'ColossusBets/BetDetails?' + params);
        }
    }

    export class ParlayTicketDetails extends BaseTicketDetails {
        private static instance: ParlayTicketDetails;

        public static get Instance(): ParlayTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowMixParlayTickets = (transId: string) => {
            const params = '?transId=' + transId;
            const url = Common.AppPath.OldMainAppPath + '_BetList/MixParlay/MixParlay.aspx' + params;
            this.showComboMixParlayTickets(transId, '', url);
        }

        public ShowComboParlayDetails = (refNo: string, winlostDate: Date, transId: string, betId: string, custId: string) => {
            const params = '?refno=' + refNo
                + '&wldate=' + winlostDate
                + '&transid=' + transId
                + '&betid=' + betId
                + '&custid=' + custId;

            Common.PopupDetails.Instance.Show(Common.AppPath.OldMainAppPath + '_BetList/MixParlay/CombinationMixParlayDetail.aspx' + params);
        }

        public ShowComboParlayOldTickets = (transId: string, transIndex: string) => {
            const params = '?transid=' + transId;
            const url = Common.AppPath.OldMainAppPath + '_BetList/MixParlay/CombinationMixParlay.aspx' + params;
            this.showComboMixParlayTickets(transId, transIndex, params);
        }

        public ShowComboParlayNewTickets = (
            transId: string,
            transIndex: string,
            refNo: string,
            handicap1: string,
            handicap2: string,
            betCheck: string,
            betTeam: string) => {
            const params = '?transid=' + transId
                + '&hdp1=' + handicap1
                + '&hdp2=' + handicap2
                + '&betcheck=' + betCheck
                + '&betteam=' + betTeam;

            const url = Common.AppPath.OldMainAppPath + '_BetList/MixParlay/CombinationMixParlay.aspx' + params;
            this.showComboMixParlayTickets(transId, transIndex, url);
        }

        private showComboMixParlayTickets = (transId: string, transIndex: string, url: string) => {
            const subbetsContent: JQuery = $('#divEvent_' + transId + transIndex);
            const comboOfferName = $('#combinationOfferName_' + transId + transIndex);

            if (subbetsContent.html() === '') {
                const getComboMixParlaySetting: JQueryAjaxSettings = {
                    method: 'GET',
                    url: url,
                    beforeSend: () => {
                        const blockIconContent = '<span class="block-icon-small "></span>';
                        subbetsContent.html(blockIconContent);
                        subbetsContent.show();
                    },
                    success: (responseHtml) => {
                        subbetsContent.html(responseHtml);
                        comboOfferName.hide();
                    },
                }

                $.ajax(getComboMixParlaySetting);
            }
            else {
                if (subbetsContent.is(':visible')) {
                    subbetsContent.hide();
                    comboOfferName.show();
                }
                else {
                    subbetsContent.show();
                    comboOfferName.hide();
                }
            }
        }
    }
}

let showMP: Function;
let showCombMPDetail: Function;
let showCombMP: Function;
let showNewCombMP: Function;
let showColossusBetsDetails: Function

$(document).ready(() => {
    showMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets;
    showCombMPDetail = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayDetails;
    showCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets;
    showNewCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayNewTickets;
    showColossusBetsDetails = BetList.Choice.TicketDetails.SportsbookTicketDetails.Instance.ShowColossusBetsDetails;
})