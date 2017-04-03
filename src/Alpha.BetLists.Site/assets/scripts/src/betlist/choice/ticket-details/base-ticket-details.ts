namespace BetList.Choice.TicketDetails {
    export class BaseTicketDetails {
        protected readonly ViewerId: string = $('#viewer-id').val();
        protected readonly ViewerLevelId: string = $('#viewer-level-id').val();
    }
}