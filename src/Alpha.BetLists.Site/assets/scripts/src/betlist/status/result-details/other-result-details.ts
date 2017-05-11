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

            Common.PopupDetails.Instance.Show(Common.AppPath.ResultsAppPath() + 'ng2/TicketDetail' + params);
        }
    }
}