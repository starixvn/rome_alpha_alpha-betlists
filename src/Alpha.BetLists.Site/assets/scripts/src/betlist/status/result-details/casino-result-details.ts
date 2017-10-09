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
            if ((betTypeId >= 2101 && betTypeId <= 2399) || (betTypeId >= 2901 && betTypeId <= 2999)) {
                this.ViewCasinoResultDetails(refNo);
            } else if (betTypeId >= 4001 && betTypeId <= 7999) {
                this.ViewVoidBridgeResultDetails(refNo);
            }
            else if (betTypeId >= 1011 && betTypeId <= 1020) {
                this.ViewFishingWorldResultDetails(betCheck);
            }
        }

        private ViewCasinoResultDetails = (refNo: string) => {
            var params = '?refno=' + refNo + '&roleId=' + this.ViewerLevelId;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.OldMainAppPath() + 'RNGCasinoBetList/GetHandHistory' + params, "", 835, 675);
        }

        private ViewVoidBridgeResultDetails = (refNo: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameplayid=' + refNo;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.OldMainAppPath() + 'RNGCasinoBetList/GetVoidbridgeResult' + params, "", 835, 675);
        }

        private ViewFishingWorldResultDetails = (refNo: string) => {
            var params = '?custId=' + this.CustId +
                '&roleId=' + this.ViewerLevelId +
                '&gameplayid=' + refNo;
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.OldMainAppPath() + 'RNGCasinoBetList/GetFishingWorldResult' + params, "", 835, 675);
        }
    }
}
