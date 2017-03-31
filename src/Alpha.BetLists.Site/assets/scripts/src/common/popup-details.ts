namespace Common {
    export class PopupDetails {
        private static instance: PopupDetails;
        private readonly DetailsModal = $('#details-modal');
        private readonly DetailsTitle = $('#details-modal .modal-title');
        private readonly DetailsFrame = $('#details-modal iframe');
        private readonly DetailsLoadingImage = $('#details-modal #loading-image');

        constructor() {
            this.registerEvents();
        }

        public static get Instance(): PopupDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;

            return this.instance;
        }

        public Show = (url: string) => {
            this.DetailsLoadingImage.show();
            this.DetailsFrame.contents().find('body').html('');
            this.DetailsFrame.attr('src', url);
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
}