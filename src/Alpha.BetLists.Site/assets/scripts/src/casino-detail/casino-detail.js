/// <reference path="../components/pager.ts" />
"use strict";
var CasionDetailView = /** @class */ (function () {
    function CasionDetailView($, pagerComponent) {
        var _this = this;
        this.pageList = [100, 200, 300, 500, 1000];
        this.init = function () {
            if (_this.isiOS()) {
                var maxHeight = window.top.window.innerHeight - 260;
                _this.$tblContainer = _this.$("#table-container");
                _this.$tblContainer.css({ 'max-height': maxHeight, overflow: 'auto' });
            }
            _this.pagerComponent.pageList = _this.pageList;
            _this.pagerComponent.render();
        };
        this.isiOS = function () {
            var isiOS = /iPhone|iPad|iPod/i.test(navigator.userAgent);
            return isiOS;
        };
        this.$ = $;
        this.pagerComponent = pagerComponent;
    }
    return CasionDetailView;
}());
;
$(function () {
    var rngCasinoView = new CasionDetailView($, new PagerComponent($));
    rngCasinoView.init();
});
//# sourceMappingURL=casino-detail.js.map