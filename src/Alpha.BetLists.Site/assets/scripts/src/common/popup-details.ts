namespace Common {
    export class PopupDetails {
        private static instance: PopupDetails;
        private topWindow: any;
        private mainDomain: string;
        private betlistUrl: string;

        public static Instance(topWindow: any): PopupDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;
            this.instance.topWindow = topWindow;

            this.instance.mainDomain = $("#MainDomain").val();
            this.instance.betlistUrl = $("#BetListUrl").val();

            return this.instance;
        }

        public Show = (url: string, title?: string, width?: number, height?: number, options?: any) => {
            if (!this.mainDomain) {
                this.topWindow.popupManager.openMultiple(url, title, width, height, undefined, undefined, undefined, undefined, undefined, undefined, options);
            }
            else {
                this.mainDomain = this.mainDomain.indexOf('http') === -1 ?
                    location.origin.split(':')[0] + ':' + this.mainDomain + '/site-main' : this.mainDomain;
                this.topWindow.postMessage({
                    event_id: "betlist-result",
                    title: title,
                    url: (this.IsCallingOtherSites(url) ? $("#MainDomain").val() : this.betlistUrl) + url
                }, this.mainDomain);
            }
        }

        public IsCallingOtherSites = (url: string) => {
            return url.indexOf('/site-results/') > -1 || url.indexOf('/ex-main/') > -1 || url.indexOf('/ex-totalbetsforecast/') > -1;
        }
    }
}