/// <reference path="../components/client-pager.ts" />

"use strict";

class RNGCasionView {
    $: JQueryStatic;
    pagerComponent: ClientPagerComponent;

    constructor(
        $: JQueryStatic,
        pagerComponent: ClientPagerComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
    }

    init = () => {
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();
    };
};

$(function () {
    const rngCasinoView = new RNGCasionView(
        $,
        new ClientPagerComponent($));

    rngCasinoView.init();
})
