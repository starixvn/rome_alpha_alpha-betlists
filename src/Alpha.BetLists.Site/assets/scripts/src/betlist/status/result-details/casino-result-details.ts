namespace BetList.Status.ResultDetails {
    export class CasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = '?typegame=casinogame&ibcrefno=' + refNo
                + '&bettype=Casino'
                + '&uname=' + userName
                + '&type=' + this.BetListType
                + '&custId=' + this.CustId
                + '&productIds=' + this.ProductIds
                + '&fdate=' + this.FromDate
                + '&tdate=' + this.ToDate
                + '&isHistoryReport=' + (this.IsHistoricReport ? 1 : 0);

            window.location.replace(Common.AppPath.OldMainAppPath() + 'RnGCasino/HandHistory' + params);
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
            const params = '?typegame=p2pgame&ibcrefno=' + refNo
                + '&bettype=' + betTypeId
                + '&uname=' + userName
                + '&custid=' + this.CustId
                + '&fdate=' + this.FromDate
                + '&tdate-' + this.ToDate;

            window.location.replace(Common.AppPath.OldMainAppPath() + '_BetList/P2P/TableSession.aspx' + params);
        }
    }
}
