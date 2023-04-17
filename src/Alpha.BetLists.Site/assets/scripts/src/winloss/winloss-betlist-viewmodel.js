"use strict";
var WinLossBetListViewModel = /** @class */ (function () {
    function WinLossBetListViewModel() {
    }
    WinLossBetListViewModel.prototype.init = function () {
        this.BetListUrl = $("#betlist-path").val();
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };
        this.MomentLocale = locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    };
    return WinLossBetListViewModel;
}());
//# sourceMappingURL=winloss-betlist-viewmodel.js.map