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
            var params = '?gameId=' + refNo + '&roleId=' + this.ViewerLevelId + '&productId=6';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
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
                '&gameId=' + refNo +
                '&productId=22';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
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
                '&gameId=' + betCheck +
                '&productId=24';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class PPResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&gameId=' + betCheck +
                '&productId=32';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class SpadegamingResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&gameId=' + betCheck +
                '&productId=36';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

}
