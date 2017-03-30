class SportsbookResult {
    public static readonly ResultDetailsPath: string = $("#result-details-path").val();
    private readonly DetailsModal = $('#result-details-modal');
    private readonly DetailsTitle = $('#result-details-modal .modal-title');
    private readonly DetailsFrame = $('#result-details-modal iframe');
    private readonly DetailsLoadingImage = $('#result-details-modal #loading-image');

    public ViewSportsbookResultDetails = (
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
        var params = '?matchid=' + matchId
            + '&bettypeid=' + betTypeId
            + '&race=' + race
            + '&sportId=' + sportTypeId
            + '&leagueId=' + league
            + '&isOutright=' + isOutright
            + '&refnum=' + refNo
            + '&betid=' + betId
            + '&isViewResultFromBetList=false';

        this.showResultDetailsPopup(params);
    }

    public ViewMixParlayResultDetails = (betTypeId: number, winLostDate: Date, refNoMixParlay: number) => {
        var params = '?refnum=' + refNoMixParlay
            + '&wldate=' + winLostDate
            + '&bettypeid=' + betTypeId
            + '&matchid=1&isViewResultFromBetList=true';

        this.showResultDetailsPopup(params);
    }

    constructor() {
        this.registerEvents();
    }

    private showResultDetailsPopup = (resultParams: string) => {
        this.DetailsFrame.attr('src', SportsbookResult.ResultDetailsPath + resultParams);
        this.DetailsModal.modal({ show: true });
    }

    private registerEvents = () => {
        this.DetailsFrame.on('load', () => {
            this.DetailsLoadingImage.hide();
            var detailTitle = this.DetailsFrame.contents().find('title').html();
            this.DetailsTitle.html(detailTitle);
            this.DetailsFrame.show();
        })
    }
}