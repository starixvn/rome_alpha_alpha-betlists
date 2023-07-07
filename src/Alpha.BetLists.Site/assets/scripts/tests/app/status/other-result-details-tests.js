describe("Status_ResultDetails_", function () {
    var matchId = 1;
    var race = 1;
    var betTypeId = 2;
    var sportTypeId = 3;
    var refNo = "324244";
    var userName = "harrison";
    var winLostDate = new Date();
    var refNoMixParlay = 1;
    var league = 3;
    var isOutright = true;
    var betId = 1;
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
});
//# sourceMappingURL=other-result-details-tests.js.map