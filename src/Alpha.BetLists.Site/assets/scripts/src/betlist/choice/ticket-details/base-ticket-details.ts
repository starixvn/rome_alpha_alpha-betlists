namespace BetList.Choice.TicketDetails {
    export class BaseTicketDetails {
        public readonly ViewerId = () => $('#viewer-id').val();
        public readonly ViewerLevelId = () => $('#viewer-level-id').val();
    }
}