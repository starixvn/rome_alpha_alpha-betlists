describe("Common_PopupDetails_", () => {
    let DetailsLoadingImage: JQuery;
    let DetailsFrame: JQuery;
    let DetailsTitle: JQuery;
    let DetailsModal: JQuery;
    let Popup: Common.PopupDetails;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("popup-template.html");

        DetailsLoadingImage = $('#details-modal #loading-image');
        DetailsFrame = $('#details-modal iframe');
        DetailsTitle = $('#details-modal .modal-title');
        DetailsModal = $('#details-modal');
        Popup = new Common.PopupDetails();
    });

    describe("Show()", () => {
        beforeEach(() => {
            // Arrange
            DetailsFrame.off("load").on("load", () => { });

            // Act
            Popup.Show("3423423432432");
        });

        it("Should Have Expected Details Title", () => {
            expect(DetailsTitle).toHaveHtml("Details");
        });

        it("Should Clear Frame Body Content", () => {
            expect(DetailsFrame.contents().find("body")).toHaveHtml("");
        });

        it("Should Have Correct Source of Frame", () => {
            expect(DetailsFrame).toHaveAttr("src", "3423423432432");
        });

        it("Should Show Details Model", () => {
            expect(DetailsModal).toBeVisible();
        });

        describe("When Show Has Title Value", () => {
            it("Should Have Expected Details Title", () => {
                // Act
                Popup.Show("3423423432432", "title");

                // Assert
                expect(DetailsTitle).toHaveHtml("title");
            });
        });

        describe("OnLoad", () => {
            beforeEach(() => {
                DetailsFrame.trigger("load");
            });

            it("Should Hide Details Loading Image", () => {
                expect(DetailsLoadingImage).toBeHidden();
            });

            it("Should Show Details Title With Expected Value", () => {
                expect(DetailsTitle.html()).toBe("Details");
            });

            it("Should Show Details Model", () => {
                expect(DetailsModal).toBeVisible();
            });
        });
    });
});