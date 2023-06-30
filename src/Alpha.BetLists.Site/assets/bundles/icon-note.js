var IconInformation = /** @class */ (function () {
    function IconInformation($) {
        var _this = this;
        this.init = function () {
            _this.registerEvents();
        };
        this.registerEvents = function () {
            var self = _this;
            _this.$("#icon-information").on("click", _this.showGuidelines);
        };
        this.showGuidelines = function () {
            var iconInformation = _this.$("#icon-information");
            var guideNote = _this.$("#info-note");
            guideNote.modal({ show: true, backdrop: false });
            var iconHelpPosition = iconInformation.position();
            guideNote.css({
                bottom: "auto",
                top: iconHelpPosition.top + 40,
            });
        };
        this.$ = $;
    }
    return IconInformation;
}());
$(function () {
    new IconInformation($).init();
});
//# sourceMappingURL=icon-note.js.map