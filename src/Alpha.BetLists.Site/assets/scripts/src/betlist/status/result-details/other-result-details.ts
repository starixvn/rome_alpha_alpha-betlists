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

    export class LeapGamingResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&sportId=' + sportTypeId +
                '&transDesc=' + transDesc +
                '&productId=49';
            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, "", 835, 675);
        }
    }
}