/// <reference path="../components/pager.ts" />

"use strict";

class CasionDetailView {
    $: JQueryStatic;
    pagerComponent: PagerComponent;

    constructor(
        $: JQueryStatic,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
    }

    init = () => {
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();
    };
};

$(function () {
    const rngCasinoView = new CasionDetailView(
        $,
        new PagerComponent($));

    rngCasinoView.init();
})
