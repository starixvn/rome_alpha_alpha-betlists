namespace BetList.Status.ResultDetails {
    export class ResultDetailsFactory {
        private static sportsbookResult: ResultDetails;
        private static mixParlayResult: ResultDetails;
        private static casinoResult: ResultDetails;
        private static ng2Result: ResultDetails;
        private static allbetResult: ResultDetails;
        private static rngCasinoResult: ResultDetails;
        private static fishingWorldResult: ResultDetails;
        private static macauGamesResult: ResultDetails;
        private static fastMarketsResult: ResultDetails;
        private static ppResult: ResultDetails;
        private static pokerGamesResult: ResultDetails;

        public static CreateInstance(betTypeId: number): ResultDetails {
            if (betTypeId >= 2501 && betTypeId <= 2699) {
                this.ng2Result = Common.Utils.IsNullOrUndefinedOrEmpty(this.ng2Result) ? new NG2ResultDetails() : this.ng2Result;
                return this.ng2Result;
            }

            if (betTypeId === 9 || betTypeId === 29 || betTypeId === 2799) {
                this.mixParlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mixParlayResult) ? new MixParlayResultDetails() : this.mixParlayResult;
                return this.mixParlayResult;
            }

            if (betTypeId >= 1011 && betTypeId <= 1020) {
                this.fishingWorldResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fishingWorldResult) ? new FishingWorldResultDetails() : this.fishingWorldResult;
                return this.fishingWorldResult;
            }

            if ((betTypeId >= 2101 && betTypeId <= 2399) || (betTypeId >= 2901 && betTypeId <= 2999)) {
                this.rngCasinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.rngCasinoResult) ? new RnGCasinoResultDetails : this.rngCasinoResult;
                return this.rngCasinoResult;
            }

            if (betTypeId >= 4001 && betTypeId <= 4100) {
                this.macauGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.macauGamesResult) ? new MacauGamesResultDetails() : this.macauGamesResult;
                return this.macauGamesResult;
            }

            if (betTypeId >= 2401 && betTypeId <= 2499) {
                this.allbetResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.allbetResult) ? new AllbetResultDetails() : this.allbetResult;
                return this.allbetResult;
            }

            if (221 <= betTypeId && betTypeId <= 227) {
                this.fastMarketsResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.fastMarketsResult) ? new FastMarketsResultDetails() : this.fastMarketsResult;
                return this.fastMarketsResult;
            }

            if (betTypeId >= 4101 && betTypeId <= 4300) {
                this.ppResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.ppResult) ? new PPResultDetails() : this.ppResult;
                return this.ppResult;
            }

            if (betTypeId >= 1901 && betTypeId <= 1999) {
                this.pokerGamesResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.pokerGamesResult) ? new PokerGamesResultDetails() : this.pokerGamesResult;
                return this.pokerGamesResult;
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
            transDesc?: string) => {
            ResultDetailsFactory.CreateInstance(betTypeId).ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc)
        }
    }
}

let ViewResult: Function;

$(document).ready(() => {
    ViewResult = BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails;
})
