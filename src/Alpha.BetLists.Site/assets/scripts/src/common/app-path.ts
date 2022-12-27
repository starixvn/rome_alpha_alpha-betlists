namespace Common {
    export class AppPath {
        public static OldMainAppPath = () => $('#old-main-path').val();
        public static readonly BetListAppPath = () => $('#betlist-path').val();
        public static readonly ResultsAppPath = () => $("#results-path").val();
        public static readonly TotalBetsForecastAppPath = () => $("#totalbets-forecast-path").val();
        public static MainAppPath = () => $("#site-main-path").val();
    }
}