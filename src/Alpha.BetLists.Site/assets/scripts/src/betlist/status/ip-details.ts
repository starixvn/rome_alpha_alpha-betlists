namespace BetList.Status {
    export class IpDetails {
        private static instance: IpDetails;

        public static get Instance(): IpDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new IpDetails() : this.instance;
            return this.instance;
        }

        public ShowIpInfo = (ip: string) => {
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.OldMainAppPath() + '_IPInfo/IpInfo.aspx?ip=' + ip,
                'IP Details',
                400,
                300);
        }
    }
}

let OpenIPInfo: Function

$(document).ready(() => {
    OpenIPInfo = BetList.Status.IpDetails.Instance.ShowIpInfo;
})