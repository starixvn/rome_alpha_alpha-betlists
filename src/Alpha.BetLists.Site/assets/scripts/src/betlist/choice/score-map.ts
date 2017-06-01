namespace BetList.Choice {
    export class ScoreMap {
        private static instance: ScoreMap;

        public static get Instance(): ScoreMap {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ScoreMap() : this.instance;
            return this.instance;
        }

        public ShowScoreMap = (matchIds: string, betType: string, liveIndicator: number) => {
            const params = liveIndicator == 1 ?
                "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=" + betType + "&bettypes_dead=-1" :
                "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=-1&bettypes_dead=" + betType;

            Common.PopupDetails.Instance.Show(Common.AppPath.OldMainAppPath() + "ForecastScoreMap/Index?" + params, "", 1008);
        }
    }
}

let OpenScoreMap: Function

$(document).ready(() => {
    OpenScoreMap = BetList.Choice.ScoreMap.Instance.ShowScoreMap;
})