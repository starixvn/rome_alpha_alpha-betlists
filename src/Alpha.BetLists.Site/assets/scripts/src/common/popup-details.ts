namespace Common {
    export class PopupDetails {
        private static instance: PopupDetails;
        private readonly DetailsModal = $('#details-modal');
        private readonly DetailsModalContent = $("#details-modal .modal-dialog");
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

        public Show = (url: string, title?: string, width: number = 900, height: number = 500) => {
            this.DetailsLoadingImage.show();
            this.DetailsTitle.html(title ? title : 'Details');
            this.DetailsFrame.contents().find('body').html('');
            this.DetailsFrame.attr('src', url);
            this.DetailsModalContent.width(width);
            this.DetailsModalContent.height(height);
            this.DetailsModal.modal({ show: true });
        }

        private registerEvents = () => {
            this.DetailsFrame.on('load', () => {
                this.DetailsLoadingImage.hide();
                const detailTitle = this.DetailsFrame.contents().find('title').html() ?
                    this.DetailsFrame.contents().find('title').html() :
                    this.DetailsTitle.html();
                this.DetailsTitle.html(detailTitle);
                this.DetailsFrame.show();
            });
        }
    }
}