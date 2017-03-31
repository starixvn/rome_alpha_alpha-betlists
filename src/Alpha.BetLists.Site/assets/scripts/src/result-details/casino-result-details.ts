namespace ResultDetails {
    export class CasinoResultDetails implements ResultDetails {
        protected readonly ProductIds: string = $('#product-ids').val();
        protected readonly OldMainPath: string = $('#old-main-path').val();
        protected readonly BetListType: string = $('#betlist-type').val();
        protected readonly FromDate: string = $('#from-date').val();
        protected readonly ToDate: string = $('#to-date').val();
        protected readonly CustId: string = $('#cust-id').val();
        protected readonly IsHistoricReport: boolean = $('#is-historic-report').val();

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
            var params = '?typegame=casinogame&ibcrefno=' + refNo
                + '&bettype=Casino'
                + '&uname=' + userName
                + '&type=' + this.BetListType
                + '&custId=' + this.CustId
                + '&productIds=' + this.ProductIds
                + '&fdate=' + this.FromDate
                + '&tdate=' + this.ToDate
                + '&isHistoryReport=' + this.IsHistoricReport ? 1 : 0;

            window.location.href = this.OldMainPath + 'RnGCasino/HandHistory' + params;
        }
    }

    export class P2PResultDetails extends CasinoResultDetails implements ResultDetails {
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
            var params = '?typegame=p2pgame&ibcrefno=' + refNo
                + '&bettype=' + betTypeId
                + '&uname=' + userName
                + '&custid=' + this.CustId
                + '&fdate=' + this.FromDate
                + '&tdate-' + this.ToDate;

            window.location.href = this.OldMainPath + '_BetList/P2P/TableSession.aspx' + params;
        }
    }
}