namespace TicketDetails {
    export class BaseTicketDetails {
        protected readonly OldMainPath: string = $('#old-main-path').val();
    }

    export class ParlayTicketDetails extends BaseTicketDetails {
        private static instance: ParlayTicketDetails;

        public static get Instance(): ParlayTicketDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
            return this.instance;
        }

        public ShowDetails = (refNo: string, winlostDate: Date, transId: string, betId: string, custId: string) => {
            var params = '?refno=' + refNo
                + '&wldate=' + winlostDate
                + '&transid=' + transId
                + '&betid=' + betId
                + '&custid=' + custId;

            Common.PopupDetails.Instance.Show(this.OldMainPath + '_BetList/MixParlay/CombinationMixParlayDetail.aspx' + params);
        }

        public ShowOldSubbets = (transId: string, transIndex: string) => {
            const queryParams = '?transid=' + transId;
            this.showComboMixParlaySubbets(transId, transIndex, queryParams);
        }

        public ShowNewSubbets = (
            transId: string,
            transIndex: string,
            refNo: string,
            handicap1: string,
            handicap2: string,
            betCheck: string,
            betTeam: string) => {
            const queryParams = '?transid=' + transId
                + '&hdp1=' + handicap1
                + '&hdp2=' + handicap2
                + '&betcheck=' + betCheck
                + '&betteam=' + betTeam;

            this.showComboMixParlaySubbets(transId, transIndex, queryParams);
        }

        private showComboMixParlaySubbets = (transId: string, transIndex: string, queryParams: string) => {
            const subbetsContent: JQuery = $('#divEvent_' + transId + transIndex);
            const comboOfferName = $('#combinationOfferName_' + transId + transIndex);

            if (subbetsContent.html() === '') {
                const getComboMixParlaySetting: JQueryAjaxSettings = {
                    method: 'GET',
                    url: this.OldMainPath + "_BetList/MixParlay/CombinationMixParlay.aspx" + queryParams,
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

let showCombMPDetail: Function;
let showCombMP: Function;
let showNewCombMP: Function;

$(document).ready(() => {
    showCombMPDetail = TicketDetails.ParlayTicketDetails.Instance.ShowDetails;
    showCombMP = TicketDetails.ParlayTicketDetails.Instance.ShowOldSubbets;
    showNewCombMP = TicketDetails.ParlayTicketDetails.Instance.ShowNewSubbets;
})