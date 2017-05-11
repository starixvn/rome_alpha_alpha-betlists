describe("Status_ResultDetails_", () => {
    let popUpDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popUpDetailsShow = spyOn(Common.PopupDetails.Instance, "Show").and.callFake(() => { });
    })

    describe("IP_", () => {
        it("ShowIpInfo -> Show Popup With Expected Url", () => {
            // Arrange
            const ip = "10.0.0.1";
            const expectedUrl = "old-path/" + '_IPInfo/IpInfo.aspx?ip=' + ip;

            // Act
            BetList.Status.IpDetails.Instance.ShowIpInfo(ip);

            // Assert
            expect(popUpDetailsShow).toHaveBeenCalledWith(expectedUrl, "IP Details");
        })
    });
});