describe("CasinoDetail", function () {
    var jQueryFake = $;
    var pagerComponent = new PagerComponent($);
    var casinoDetailView, renderPagerComponent;
    beforeEach(function () {
        casinoDetailView = new CasionDetailView(jQueryFake, pagerComponent);
        renderPagerComponent = spyOn(pagerComponent, "render");
    });
    describe("_init", function () {
        beforeEach(function () {
            // act
            casinoDetailView.init();
        });
        it("_Calls pagerComponent.render with small page size list", function () {
            // assert
            expect(pagerComponent.pageList).toEqual([100, 200, 300, 500, 1000]);
            expect(pagerComponent.render).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=casino-detail-tests.js.map