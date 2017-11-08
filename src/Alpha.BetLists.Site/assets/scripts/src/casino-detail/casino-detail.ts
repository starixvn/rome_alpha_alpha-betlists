/// <reference path="../components/pager.ts" />

"use strict";

class CasionDetailView {
    $: JQueryStatic;
    $tblContainer: JQuery;
    pagerComponent: PagerComponent;
    pageList: number[] = [100, 200, 300, 500, 1000];

    constructor(
        $: JQueryStatic,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.pagerComponent = pagerComponent;
    }

    init = () => {
        if (this.isiOS()) {
            var maxHeight = window.top.window.innerHeight - 260;
            this.$tblContainer = this.$("#table-container");
            this.$tblContainer.css({ 'max-height': maxHeight, overflow: 'auto' });
        }

        this.pagerComponent.pageList = this.pageList;
        this.pagerComponent.render();
    };

    isiOS = () => {
        var isiOS = /iPhone|iPad|iPod/i.test(navigator.userAgent);
        return isiOS;
    };
};

$(function () {
    const rngCasinoView = new CasionDetailView(
        $,
        new PagerComponent($));

    rngCasinoView.init();
})
