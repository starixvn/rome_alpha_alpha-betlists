describe("Status_ResultDetails_", function () {
    var popUpDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popUpDetailsShow = spyOn(Common.PopupDetails.Instance(window.top), "Show").and.callFake(function () { });
    });
    describe("IP_", function () {
        it("ShowIpInfo -> Show Popup With Expected Url", function () {
            // Arrange
            var ip = "10.0.0.1";
            var expectedUrl = "old-path/" + '_IPInfo/IpInfo.aspx?ip=' + ip;
            // Act
            BetList.Status.IpDetails.Instance.ShowIpInfo(ip);
            // Assert
            expect(popUpDetailsShow).toHaveBeenCalledWith(expectedUrl, "IP Details", 400, 400);
        });
    });
});
//# sourceMappingURL=ip-details-tests.js.map