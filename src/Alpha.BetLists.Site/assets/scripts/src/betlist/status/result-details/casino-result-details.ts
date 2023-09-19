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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=6';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/RngCasinoResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=22';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MacauGamesResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=32';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/PPResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=36';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/SpadegamingResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=38';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/VirtualGamesResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=50';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/RNGWarResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=52';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/IONCasinoResult' + params, "", 835, 675);
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
            var params = '?custId=' + (transDesc || this.CustId) +
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=55'
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/PGSoftResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=56';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/JokerResult' + params, "", 835, 675);
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
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=59';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/GDGResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + refNo +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=3' +
                '&BetTypeId=' + betTypeId +
                '&SportTypeId=' + sportTypeId;

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/LinkedGamesResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=63';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/SportsLotteryDetailResult' + params, "", 835, 675);
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
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=57';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/BGResult' + params, "", 835, 675);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&endtime=' + winLostDate +
                '&productId=60';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MaxGameResult' + params, "", 835, 675);
        }
    }

    export class PPLiveCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=65';

            var mainDomain = location.origin.split(':')[0] + ':' + $("#MainDomain").val() + '/site-main';
            window.top.postMessage({
                event_id: "pplivecasino-result",
                url: 'ThirdParty/PPLiveCasinoResult' + params
            }, mainDomain);
        }
    }

    export class HabaneroResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=61';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/HabaneroResult' + params, "", 835, 675);
        }
    }

    export class CGResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=62';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/CGResult' + params, "", 835, 675);
        }
    }

    export class VGamingResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = '?custId=' + (transDesc || this.CustId) +
                '&roleId=' + this.ViewerLevelId +
                '&gameRoundId=' + betcheck +
                '&gameId=' + betTypeId +
                '&sportId=' + sportTypeId +
                '&siteId=' + matchCode +
                '&currencyName=' + userName +
                '&productId=66';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/VGamingResult' + params, "", 835, 675);
        }
    }


    export class AdvantPlayResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=67';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/AdvantPlayResult' + params, "", 835, 675);
        }
    }

    export class AdvantPlayMiniResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=68';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/AdvantPlayMiniResult' + params, "", 835, 675);
        }
    }

    export class BBINResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=47';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/BBINResult' + params, "", 835, 900);
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=34';

            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/CardClubResult' + params, "", 1099, 500, { resizable: true });
        }
    }

    export class FGGResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&endtime=' + winLostDate +
                '&productId=70';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/FGGResult' + params, "", 835, 675);
        }
    }

    export class SEALResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=72';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/SEALResult' + params, "", 835, 675);
        }
    }

    export class MGResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=73';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/MGResult' + params, "", 835, 675);
        }
    }


    export class JiliResultDetails extends BaseResultDetails implements ResultDetails {
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
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&gameId3rd=' + gameId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=75';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ThirdParty/JiliResult' + params, "", 835, 675);
        }
    }
}
