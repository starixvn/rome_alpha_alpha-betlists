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

                if (!title) {
                    var queryStrings = url.indexOf('?') !== -1 ?
                        url.split('&') : [''];

                    for (var i in queryStrings) {
                        if (queryStrings[i].toLowerCase().indexOf('parlay') !== -1
                            && (url.toLowerCase().indexOf('betteam=3') !== -1 || url.toLowerCase().indexOf('betteam=7') !== -1)) {
                            title = "System Parlay";
                            break;
                        }

                        if (queryStrings[i].toLowerCase().indexOf('parlay') !== -1
                            && url.toLowerCase().indexOf('betteam=1') !== -1) {
                            title = "Mix Parlay";
                            break;
                        }

                        if (queryStrings[i].toLowerCase().indexOf('cashout') !== -1) {
                            var transId = "";
                            for (var j in queryStrings) {
                                if (queryStrings[j].toLowerCase().indexOf("transid") !== -1) {
                                    transId = queryStrings[j].split('=')[1];
                                    break;
                                }
                            }

                            title = "Cash Out History - " + transId;
                            break;
                        }

                        if (queryStrings[i].toLowerCase().indexOf('sportname') !== -1) {
                            title = queryStrings[i].split('=')[1] + ' Details';
                            break;
                        }
                    }
                }

                this.topWindow.postMessage({
                    event_id: "betlist-result",
                    title: title,
                    width: width,
                    height: height,
                    options: options,
                    url: (this.IsCallingOtherSites(url) ? $("#MainDomain").val() : this.betlistUrl) + url
                }, this.mainDomain);
            }
        }

        public IsCallingOtherSites = (url: string) => {
            return url.indexOf('/site-results/') > -1 || url.indexOf('/ex-main/') > -1 || url.indexOf('/ex-totalbetsforecast/') > -1;
        }
    }
}