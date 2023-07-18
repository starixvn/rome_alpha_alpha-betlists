namespace BetList.Status.ResultDetails {
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