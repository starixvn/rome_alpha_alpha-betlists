namespace ResultDetails {
    export interface ResultDetails {
        ViewResultDetails(matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number): void;
    }

    export class SportsbookResultDetails implements ResultDetails {
        protected readonly ResultDetailsPath: string = $("#result-details-path").val();

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
            var params = '?matchid=' + matchId
                + '&bettypeid=' + betTypeId
                + '&race=' + race
                + '&sportId=' + sportTypeId
                + '&leagueId=' + league
                + '&isOutright=' + isOutright
                + '&refnum=' + refNo
                + '&betid=' + betId
                + '&isViewResultFromBetList=false';

            Common.PopupDetails.Instance.Show(this.ResultDetailsPath + params)
        };
    }

    export class MixParlayResultDetails extends SportsbookResultDetails implements ResultDetails {
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
            var params = '?refnum=' + refNoMixParlay
                + '&wldate=' + winLostDate
                + '&bettypeid=' + betTypeId
                + '&matchid=1&isViewResultFromBetList=true';

            Common.PopupDetails.Instance.Show(this.ResultDetailsPath + params);
        };
    }
}