namespace Common {
    export class PopupDetails {
        private static instance: PopupDetails;
        private topWindow: any;

        public static Instance(topWindow: any): PopupDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;
            this.instance.topWindow = topWindow;

            return this.instance;
        }

        public Show = (url: string, title?: string, width?: number, height?: number, options?: any) => {
            this.topWindow.popupManager.openMultiple(url, title, width, height, undefined, undefined, undefined, undefined, undefined, undefined, options);
        }
    }
}