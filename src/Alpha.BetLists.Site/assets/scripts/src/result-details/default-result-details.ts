namespace ResultDetails {
    export class ResultDetailsFactory {
        private static sportsbookResult: ResultDetails;
        private static mixParlayResult: ResultDetails;
        private static casinoResult: ResultDetails;
        private static p2pResult: ResultDetails;

        public static CreateInstance(betTypeId: number): ResultDetails {
            if (betTypeId === 71 || betTypeId === 72) {
                this.casinoResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.casinoResult) ? new CasinoResultDetails() : this.casinoResult;
                return this.casinoResult;
            }

            if (betTypeId === 1000 || betTypeId === 1001 || betTypeId === 1002) {
                this.p2pResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.p2pResult) ? new P2PResultDetails() : this.p2pResult;
                return this.p2pResult;
            }

            if (betTypeId >= 2501 && betTypeId <= 2699) {
                // TODO: NG2Result
                return;
            }

            if (betTypeId === 9 || betTypeId === 29) {
                this.mixParlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mixParlayResult) ? new MixParlayResultDetails() : this.mixParlayResult;
                return this.mixParlayResult;
            }

            this.sportsbookResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.sportsbookResult) ? new SportsbookResultDetails : this.sportsbookResult;
            return this.sportsbookResult;
        }
    }

    export class DefaultResultDetails {
        public static get Instance() {
            return new DefaultResultDetails();
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
            betId: number) => {
            ResultDetailsFactory.CreateInstance(betTypeId).ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId)
        }
    }
}

let ViewResult: Function;

$(document).ready(() => {
    ViewResult = ResultDetails.DefaultResultDetails.Instance.ViewResultDetails;
})