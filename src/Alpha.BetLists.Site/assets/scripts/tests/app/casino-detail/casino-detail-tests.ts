describe("CasinoDetail", () => {
    const jQueryFake = $;
    const pagerComponent = new PagerComponent($);
    let casinoDetailView: CasionDetailView,
        renderPagerComponent: jasmine.Spy;

    beforeEach(() => {
        casinoDetailView = new CasionDetailView(jQueryFake, pagerComponent);
        renderPagerComponent = spyOn(pagerComponent, "render");
    });

    describe("_init", () => {
        beforeEach(() => {
            // act
            casinoDetailView.init();
        });
        it("_Calls pagerComponent.render with small page size list", () => {
            // assert
            expect(pagerComponent.pageList).toEqual([50, 100, 150, 200]);
            expect(pagerComponent.render).toHaveBeenCalled();
        });
    });
});
