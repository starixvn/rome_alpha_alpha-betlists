namespace BetList.Status.ResultDetails {
    export class AllbetResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = '?transDesc=' + transDesc + '&betCheck=' + betCheck + '&bettypeId=' + betTypeId + '&sportId=' + sportTypeId;

            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, "", 835, 800);
        }
    }

    export class RnGCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?refno=' + refNo + '&roleId=' + this.ViewerLevelId;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'HandHistory/RNGCasino' + params, "", 835, 675);
        }
    }

    export class MacauGamesResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&refNo=' + refNo;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'HandHistory/MacauGames' + params, "", 835, 675);
        }
    }

    export class FishingWorldResultDetails extends BaseResultDetails implements ResultDetails {
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
            betCheck?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&refNo=' + refNo;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'HandHistory/FishingWorld' + params, "", 835, 675);
        }
    }
}
