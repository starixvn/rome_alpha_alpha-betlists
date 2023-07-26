"use strict";
var FindTicketView = /** @class */ (function () {
    function FindTicketView($) {
        var _this = this;
        this.init = function () {
            _this.cacheDOMElements();
            _this.registerEvents();
        };
        this.cacheDOMElements = function () {
            _this.$refNo = _this.$("#ref-no");
            _this.$submitButton = _this.$("#submit-betList-form");
        };
        this.onChangeRefNoTextBox = function () {
            _this.$refNo.on('change keyup paste', function () {
                var content = _this.$refNo.val().trim();
                if (content) {
                    _this.$("#RefNo").val(content);
                    _this.$submitButton.prop("disabled", false);
                }
                else {
                    _this.$submitButton.prop("disabled", true);
                }
            });
        };
        this.$ = $;
    }
    FindTicketView.prototype.registerEvents = function () {
        this.$submitButton.prop("disabled", true);
        this.onChangeRefNoTextBox();
    };
    return FindTicketView;
}());
$(function () {
    var findTicketView = new FindTicketView($);
    findTicketView.init();
});
//# sourceMappingURL=find-tickets.js.map