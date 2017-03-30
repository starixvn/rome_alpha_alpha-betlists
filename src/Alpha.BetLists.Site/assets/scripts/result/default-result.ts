class DefaultResult {
    private readonly sportsbookResult = new SportsbookResult();
    private readonly casinoResult = new CasinoResult();

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
        if (this.isCasino(betTypeId)) {
            this.casinoResult.ViewCasinoResultDetails(refNo, userName);
            return;
        }

        if (this.isP2P(betTypeId)) {
            this.casinoResult.ViewP2PResultDetails(refNo, betTypeId, userName);
            return;
        }

        if (this.isNG2(betTypeId)) {
            // TODO: NG2Result
            return;
        }

        if (this.isMixParlay(betTypeId)) {
            this.sportsbookResult.ViewMixParlayResultDetails(betTypeId, winLostDate, refNoMixParlay);
            return;
        }

        this.sportsbookResult.ViewSportsbookResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId)
    }

    private isCasino = (betTypeId: number): boolean => betTypeId == 71 || betTypeId == 72;
    private isMixParlay = (betTypeId: number): boolean => betTypeId == 9 || betTypeId == 29;
    private isP2P = (betTypeId: number): boolean => betTypeId == 1000 || betTypeId == 1001 || betTypeId == 1002;
    private isMetricGaming = (betTypeId: number): boolean => betTypeId >= 801 && betTypeId <= 899;
    private isNG2 = (betTypeId: number): boolean => betTypeId >= 2501 && betTypeId <= 2699;
}

let ViewResult: Function;

$(document).ready(() => {
    ViewResult = new DefaultResult().ViewResultDetails
})