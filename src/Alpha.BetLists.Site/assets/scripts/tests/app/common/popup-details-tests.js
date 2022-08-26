describe("Common_PopupDetails_", function () {
    var Popup;
    var topWindow = {
        popupManager: {
            openMultiple: jasmine.any(Function)
        }
    };
    beforeEach(function () {
        Popup = Common.PopupDetails.Instance(topWindow);
    });
    describe("Show()", function () {
        beforeEach(function () {
            // Arrange
            spyOn(topWindow.popupManager, "openMultiple");
            // Act
            Popup.Show("3423423432432", "Title", 1120, 80);
        });
        it("Calls openMultiple", function () {
            // Assert
            expect(topWindow.popupManager.openMultiple).toHaveBeenCalledWith("3423423432432", "Title", 1120, 80, undefined, undefined, undefined, undefined, undefined, undefined, undefined);
        });
    });
});
//# sourceMappingURL=popup-details-tests.js.map