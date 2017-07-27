namespace BetList.Status.ResultDetails {
    export class ResultDetailsFactory {
        private static sportsbookResult: ResultDetails;
        private static mixParlayResult: ResultDetails;
        private static casinoResult: ResultDetails;
        private static p2pResult: ResultDetails;
        private static ng2Result: ResultDetails;

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
                this.ng2Result = Common.Utils.IsNullOrUndefinedOrEmpty(this.ng2Result) ? new NG2ResultDetails() : this.ng2Result;
                return this.ng2Result;
            }

            if (betTypeId === 9 || betTypeId === 29 || betTypeId === 2799) {
                this.mixParlayResult = Common.Utils.IsNullOrUndefinedOrEmpty(this.mixParlayResult) ? new MixParlayResultDetails() : this.mixParlayResult;
                return this.mixParlayResult;
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
            betId: number) => {
            ResultDetailsFactory.CreateInstance(betTypeId).ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId)
        }
    }
}

let ViewResult: Function;

$(document).ready(() => {
    ViewResult = BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails;
})