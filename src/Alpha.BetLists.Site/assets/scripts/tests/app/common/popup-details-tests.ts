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

    it("Show -> Expected Behavior", () => {
        // Arrange
        DetailsFrame.off("load").on("load", () => { });

        // Act
        Popup.Show("3423423432432");

        // Assert
        expect(DetailsTitle).toHaveHtml("Details");
        expect(DetailsFrame.contents().find("body")).toHaveHtml("");
        expect(DetailsFrame).toHaveAttr("src", "3423423432432");
        expect(DetailsModal).toBeVisible();
    });

    it("Show_TitleHasValue -> Expected Behavior", () => {
        // Arrange
        DetailsFrame.off("load").on("load", () => { });

        // Act
        Popup.Show("3423423432432", "title");

        // Assert
        expect(DetailsTitle).toHaveHtml("title");
    });

    it("Show_OnLoad -> Expected Behavior", () => {
        // Act
        DetailsFrame.trigger("load");

        // Assert
        expect(DetailsLoadingImage).toBeHidden();
        expect(DetailsTitle.html()).toBe("Details");
    });
})