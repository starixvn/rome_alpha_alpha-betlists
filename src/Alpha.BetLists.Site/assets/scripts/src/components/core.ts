"use strict";

class AlphaBetLists {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    format = {
        shortDateWithSlash_DatePicker: "MM/DD/YYYY",
    };

    numberDisplayedProductsOnFilter = 1;

    convertAttributeToBool = (attrValue: string) => {
        return !!attrValue && attrValue.toString().toLowerCase() === "true";
    };
}

const alphaBetLists = new AlphaBetLists($);