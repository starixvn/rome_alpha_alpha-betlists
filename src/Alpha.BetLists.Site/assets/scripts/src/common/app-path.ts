namespace Common {
    export class AppPath {
        public static OldMainAppPath = () => $('#old-main-path').val();
        public static readonly BetListAppPath = () => $('#betlist-path').val();
        public static readonly ResultsAppPath = () => $("#results-path").val();
    }
}