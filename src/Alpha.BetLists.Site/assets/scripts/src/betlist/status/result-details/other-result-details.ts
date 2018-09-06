namespace BetList.Status.ResultDetails {
    export class NG2ResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number) => {
            const params = '?gambleId=' + matchId;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ng2/TicketDetail' + params, "", 500, 400);
        }
    }

    export class PokerGamesResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number,
            betCheck?: string,
            custId?: number,
            transDesc?: string) => {
            const params = '?custId=' + custId + '&gameTypeId=' + betTypeId + '&transId3rd=' + betCheck;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'pokergames/Index' + params, "", 999, 500);
        }
    }
}