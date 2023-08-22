namespace BetList.Status.ResultDetails {
    export class ResultDetailsFactory {
        private static sportsbookResult: ResultDetails;
        private static mixParlayResult: ResultDetails;
        private static allbetResult: ResultDetails;
        private static bbinResult: ResultDetails;
        private static rngCasinoResult: ResultDetails;
        private static fishingWorldResult: ResultDetails;
        private static macauGamesResult: ResultDetails;
        private static fastMarketsResult: ResultDetails;
        private static ppResult: ResultDetails;
        private static spadegamingResult: ResultDetails;
        private static pokerGamesResult: ResultDetails;
        private static virtualGamesResult: ResultDetails;
        private static RNGWarResult: ResultDetails;
        private static IONCasinoResult: ResultDetails;
        private static SabaClubResult: ResultDetails;
        private static PGSoftCasinoResult: ResultDetails;
        private static JokerResult: ResultDetails;
        private static LeapGamingResult: ResultDetails;
        private static GDGResult: ResultDetails;
        private static BGResult: ResultDetails;
        private static maxGameResult: ResultDetails;
        private static linkedGamesResult: ResultDetails;
        private static sportsLotteryResult: ResultDetails;
        private static ppLiveCasinoResult: ResultDetails;
        private static habaneroResult: ResultDetails;
        private static cgResult: ResultDetails;
        private static VGamingResult: ResultDetails;
        private static advantPlayResult: ResultDetails;
        private static advantPlayMiniResult: ResultDetails;
        private static fggResult: ResultDetails;
        private static sealResult: ResultDetails;
        private static mgResult: ResultDetails;
        private static jiliResult: ResultDetails;

        public static CreateInstance(betTypeId: number): ResultDetails {
            if (betTypeId === 9 || betTypeId === 29 || betTypeId === 2799) {
                this.mixParlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mixParlayResult) ? new MixParlayResultDetails() : this.mixParlayResult;
                return this.mixParlayResult;
            }

            if (betTypeId >= 1011 && betTypeId <= 1020) {
                this.fishingWorldResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fishingWorldResult) ? new FishingWorldResultDetails() : this.fishingWorldResult;
                return this.fishingWorldResult;
            }

            if (betTypeId >= 1611 && betTypeId <= 1699) {
                this.SabaClubResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.SabaClubResult) ? new SabaClubResultDetails() : this.SabaClubResult;
                return this.SabaClubResult;
            }

            if ((betTypeId >= 2101 && betTypeId <= 2399) || (betTypeId >= 2901 && betTypeId <= 2999)) {
                this.rngCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.rngCasinoResult) ? new RnGCasinoResultDetails : this.rngCasinoResult;
                return this.rngCasinoResult;
            }

            if (betTypeId >= 4001 && betTypeId <= 4100 || betTypeId >= 5800 && betTypeId <= 5999) {
                this.macauGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.macauGamesResult) ? new MacauGamesResultDetails() : this.macauGamesResult;
                return this.macauGamesResult;
            }

            if ((betTypeId >= 2401 && betTypeId <= 2499) ||
                (betTypeId >= 3001 && betTypeId <= 3099) ||
                (betTypeId >= 3101 && betTypeId <= 3149) ||
                (betTypeId >= 3151 && betTypeId <= 3200) ||
                (betTypeId >= 3201 && betTypeId <= 3299) ||
                (betTypeId >= 3401 && betTypeId <= 3499)) {
                this.allbetResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.allbetResult) ? new AllbetResultDetails() : this.allbetResult;
                return this.allbetResult;
            }

            if (betTypeId == 3300) {
                this.bbinResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.bbinResult) ? new BBINResultDetails() : this.bbinResult;
                return this.bbinResult;
            }

            if (221 <= betTypeId && betTypeId <= 227) {
                this.fastMarketsResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fastMarketsResult) ? new FastMarketsResultDetails() : this.fastMarketsResult;
                return this.fastMarketsResult;
            }

            if ((betTypeId >= 4101 && betTypeId <= 4300) ||
                betTypeId >= 6701 && betTypeId <= 7000) {
                this.ppResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.ppResult) ? new PPResultDetails() : this.ppResult;
                return this.ppResult;
            }

            if (betTypeId >= 4301 && betTypeId <= 4600) {
                this.spadegamingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.spadegamingResult) ? new SpadegamingResultDetails() : this.spadegamingResult;
                return this.spadegamingResult;
            }

            if (betTypeId >= 1901 && betTypeId <= 1999) {
                this.pokerGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.pokerGamesResult) ? new PokerGamesResultDetails() : this.pokerGamesResult;
                return this.pokerGamesResult;
            }

            if (betTypeId >= 4601 && betTypeId <= 4900) {
                this.virtualGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.virtualGamesResult) ? new VirtualGamesResultDetails() : this.virtualGamesResult;
                return this.virtualGamesResult;
            }

            if (betTypeId == 5301) {
                this.LeapGamingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.LeapGamingResult) ? new LeapGamingResultDetails() : this.LeapGamingResult;
                return this.LeapGamingResult;
            }

            if (betTypeId >= 5500 && betTypeId <= 5599) {
                this.RNGWarResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.RNGWarResult) ? new RNGWarCasinoResultDetails() : this.RNGWarResult;
                return this.RNGWarResult;
            }
            if (betTypeId >= 5601 && betTypeId <= 5699) {
                this.IONCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.IONCasinoResult) ? new IONCasinoResultDetails() : this.IONCasinoResult;
                return this.IONCasinoResult;
            }

            if (betTypeId >= 3501 && betTypeId <= 3599) {
                this.BGResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.BGResult) ? new BGResultDetails() : this.BGResult;
                return this.BGResult;
            }
            if (betTypeId >= 5701 && betTypeId <= 5799) {
                this.GDGResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.GDGResult) ? new GDGResultDetails() : this.GDGResult;
                return this.GDGResult;
            }
            if (betTypeId >= 6001 && betTypeId <= 6300) {
                this.PGSoftCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.PGSoftCasinoResult) ? new PGSoftCasinoResultDetails() : this.PGSoftCasinoResult;
                return this.PGSoftCasinoResult;
            }

            if (betTypeId >= 6401 && betTypeId <= 6700) {
                this.JokerResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.JokerResult) ? new JokerResultDetails() : this.JokerResult;
                return this.JokerResult;
            }

            if (betTypeId >= 3601 && betTypeId <= 3610) {
                this.linkedGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.linkedGamesResult) ? new Happy5LinkedGamesResultDetails() : this.linkedGamesResult;
                return this.linkedGamesResult;
            }

            if (betTypeId >= 3611 && betTypeId <= 3620) {
                this.sportsLotteryResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sportsLotteryResult) ? new SportsLotteryResultDetails() : this.sportsLotteryResult;
                return this.sportsLotteryResult;
            }

            if (betTypeId >= 5401 && betTypeId <= 5499) {
                this.maxGameResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.maxGameResult) ? new MaxGameResultDetails() : this.maxGameResult;
                return this.maxGameResult;
            }

            if (betTypeId >= 7301 && betTypeId <= 7600) {
                this.cgResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.cgResult) ? new CGResultDetails() : this.cgResult;
                return this.cgResult;
            }

            if (betTypeId >= 3701 && betTypeId <= 3799) {
                this.ppLiveCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.ppLiveCasinoResult) ? new PPLiveCasinoResultDetails() : this.ppLiveCasinoResult;
                return this.ppLiveCasinoResult;
            }
            
            if (betTypeId >= 7001 && betTypeId <= 7300) {
                this.habaneroResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.habaneroResult) ? new HabaneroResultDetails() : this.habaneroResult;
                return this.habaneroResult;
            }

            if (betTypeId >= 3802 && betTypeId <= 3899) {
                this.VGamingResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.VGamingResult) ? new VGamingResultDetails() : this.VGamingResult;
                return this.VGamingResult;
            }

            if (betTypeId >= 7701 && betTypeId <= 7799) {
                this.advantPlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.advantPlayResult) ? new AdvantPlayResultDetails() : this.advantPlayResult;
                return this.advantPlayResult;
            }

            if (betTypeId >= 7801 && betTypeId <= 7899) {
                this.advantPlayMiniResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.advantPlayMiniResult) ? new AdvantPlayMiniResultDetails() : this.advantPlayResult;
                return this.advantPlayMiniResult;
            }

            if (betTypeId >= 8201 && betTypeId <= 8299) {
                this.fggResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fggResult) ? new FGGResultDetails() : this.fggResult;
                return this.fggResult;
            }

            if (betTypeId >= 8900 && betTypeId <= 8999) {
                this.sealResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sealResult) ? new SEALResultDetails() : this.sealResult;
                return this.sealResult;
            }
            
            if (betTypeId >= 8400 && betTypeId <= 8699) {
                this.mgResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mgResult) ? new MGResultDetails() : this.mgResult;
                return this.mgResult;
            }

            if (betTypeId >= 5100 && betTypeId <= 5299) {
                this.jiliResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.jiliResult) ? new JiliResultDetails() : this.jiliResult;
                return this.jiliResult;
            }

            this.sportsbookResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sportsbookResult) ? new SportsbookResultDetails : this.sportsbookResult;
            return this.sportsbookResult;
        }
    }

    export class BaseResultDetails {
        protected readonly ResultDetailsPath: string = Common.AppPath.ResultsAppPath() + "Normal/ResultDetails";
        protected readonly ProductIds: string = $('#product-ids').val();
        protected readonly BetListType: string = $('#betlist-type').val();
        protected readonly FromDate: string = $('#from-date').val();
        protected readonly ToDate: string = $('#to-date').val();
        protected readonly CustId: string = $('#cust-id').val();
        protected readonly ViewerLevelId: string = $('#viewer-level-id').val();
        protected readonly IsHistoricReport: boolean = Common.Utils.ParseBoolean($('#is-historic-report').val());
        private static instance: BaseResultDetails;

        public static get Instance() {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new BaseResultDetails() : this.instance;
            return this.instance;
        }

        public ViewResultDetails = (
            matchId: number,
            race: number,
            betTypeId: number,
            sportTypeId: number,
            refNo: string,
            userName: string,
            winLostDate: Date,
            refNoMixParlay: number,
            league: number,
            isOutright: boolean,
            betId: number,
            betCheck?: string,
            custId?: number,
            transDesc?: string,
            siteId?: string) => {
            ResultDetailsFactory.CreateInstance(betTypeId).ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc, siteId)
        }
    }
}

let ViewResult: Function;

$(document).ready(() => {
    ViewResult = BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails;
})