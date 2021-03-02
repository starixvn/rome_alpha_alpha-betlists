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

            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, "", 835, 500);
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

    export class VirtualGamesResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = '?gameId=' + refNo;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'VirtualGames/Detail' + params, "", 835, 675);
        }
    }

    export class RNGWarCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = '?gameId=' + betCheck + '&roleId=' + this.ViewerLevelId + '&productId=50';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }
    export class IONCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = '?gameId=' + betCheck + '&roleId=' + this.ViewerLevelId + '&productId=52';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class SabaClubResultDetails extends BaseResultDetails implements ResultDetails {
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
            matchCode?: string,
            transDesc?: number,
            betcheck?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameRoundId=' + betcheck +
                '&gameId=' + betTypeId +
                '&sportId=' + sportTypeId +
                '&siteId=' + matchCode +
                '&productId=42';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'SabaClub/Index' + params, "", 835, 675);
        }
    }

    export class PGSoftCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&gameId=' + transDesc +
                '&gameCode=' + betCheck +
                '&productId=55'
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class JokerResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&gameId=' + transDesc +
                '&gameCode=' + betCheck +
                '&productId=56';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class GDGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            siteId?: number,
            isOutright?: boolean,
            betId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameId=' + transId3rd +
                '&gameCode=' + gameId3rd +
                '&siteId=' + siteId +
                '&productId=59';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class Happy5LinkedGamesResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = '?custId=' + betId +
                '&TransId3rd=' + refNo +
                '&BetTypeId=' + betTypeId +
                '&SportTypeId=' + sportTypeId;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'LinkedGames/Index' + params, "", 835, 675);
        }
    }

    export class SportsLotteryResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&gameId=' + transDesc +
                '&productId=63';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }
   export class BGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            currencyId?: number,
            isOutright?: boolean,
            betId?: number,
            siteId?: string,
            custId?: number,
            transId3rd?: string) => {
            var params = '?custId=' + this.CustId +
                '&gameId=' + transId3rd +
                '&gameCode=' + currencyId +
                '&siteId=' + siteId +
                '&productId=57';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/Detail' + params, "", 835, 675);
        }
    }

    export class MaxGameResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&TransId3rd=' + transDesc +
                '&BetType=' + betTypeId +
                '&EndTime=' + betCheck +
                '&SiteId=' + betId +
                '&productId=60';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MaxGameResult' + params, "", 835, 675);
        }
    }    
}
