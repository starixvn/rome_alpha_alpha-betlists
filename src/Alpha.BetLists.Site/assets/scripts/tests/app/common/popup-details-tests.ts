describe("Common_PopupDetails_", () => {
    let Popup: Common.PopupDetails;

    const topWindow = {
        popupManager: {
            openMultiple: jasmine.any(Function)
        }
    };

    beforeEach(() => {
        Popup = Common.PopupDetails.Instance(topWindow);
    });

    describe("Show()", () => {
        beforeEach(() => {
            // Arrange
            spyOn(topWindow.popupManager, "openMultiple");

            // Act
            Popup.Show("3423423432432", "Title", 1120, 80);
        });

        it("Calls openMultiple", () => {
            // Assert
            expect(topWindow.popupManager.openMultiple).toHaveBeenCalledWith("3423423432432", "Title", 1120, 80);
        });
    });
});