class CasinoResult extends SportsbookResult {
    private readonly productIds: string = $('#product-ids').val();
    private readonly oldMainPath: string = $('#old-main-path').val();
    private readonly betListType: string = $('#betlist-type').val();
    private readonly fromDate: string = $('#from-date').val();
    private readonly toDate: string = $('#to-date').val();
    private readonly custId: string = $('#cust-id').val();
    private readonly isHistoricReport: boolean = $('#is-historic-report').val();

    public ViewCasinoResultDetails = (refNo: string, userName: string) => {
        var params = '?typegame=casinogame&ibcrefno=' + refNo
            + '&bettype=Casino'
            + '&uname=' + userName
            + '&type=' + this.betListType
            + '&custId=' + this.custId
            + '&productIds=' + this.productIds
            + '&fdate=' + this.fromDate
            + '&tdate=' + this.toDate
            + '&isHistoryReport=' + this.isHistoricReport ? 1 : 0;

        window.location.href = this.oldMainPath + 'RnGCasino/HandHistory' + params;
    }

    public ViewP2PResultDetails = (refNo: string, betTypeId: number, userName: string) => {
        var params = '?typegame=p2pgame&ibcrefno=' + refNo
            + '&bettype=' + betTypeId
            + '&uname=' + userName
            + '&custid=' + this.custId
            + '&fdate=' + this.fromDate
            + '&tdate-' + this.toDate;

        window.location.href = this.oldMainPath + '_BetList/P2P/TableSession.aspx' + params;
    }
}