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

    export class VirtualRacingResultDetails extends BaseResultDetails implements ResultDetails {
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
            transdesc?: string) => {
            const params = '?matchid=' + matchId
                + '&bettypeid=' + betTypeId
                + '&race=' + race
                + '&endtime=' + winLostDate
                + '&sportId=' + sportTypeId
                + '&leagueId=' + league
                + '&isOutright=' + isOutright
                + '&refnum=' + refNo
                + '&betid=' + betId
                + '&transdesc=' + transdesc
                + '&isViewResultFromBetList=true';
            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, "", 1000, 300);
        };
    }
}