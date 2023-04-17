"use strict";

class ProductAndProductDetailDropdownOption {
    constructor(
        public products: any,
        public productDetails: any,
        public onDropdownHidden = Function(),
        public selectorProductId = "dropdown-product",
        public selectorProductDetailId = "dropdown-product-detail") {
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
    }
}

class ProductAndProductDetailDropdownComponent {
    private dropdownProduct: JQuery;
    private dropdownProductDetail: JQuery;

    render(options: ProductAndProductDetailDropdownOption) {
        this.renderProductDropdown(options);
        this.renderProductDetailDropdown(options);
    }

    private renderProductDropdown(options: ProductAndProductDetailDropdownOption) {
        const self = this;
        const productIdWithSubProductIds = {};
        const productList = options.products.map(function (item) {
            const checkboxItem: any = self.buildCheckboxItem(item);

            if (item.Products) {
                const subProductIds = [];
                productIdWithSubProductIds[item.Id] = subProductIds;

                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds.push(item.Id);
                    return self.buildCheckboxItem(item);
                });
            }

            return checkboxItem;
        });

        const dropdown = $("#" + options.selectorProductId);
        this.dropdownProduct = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            expandGroupsByDefault: false,
            listData: productList,
            onCheck: function () {
                const checked = this.checked;
                const productId = this.value;
                const subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    subProductIds.forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                } else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                self.dropdownProductDetail.fanexMultiSelect("setSelectAll", this.checked);
            },
            onLeave: options.onDropdownHidden
        });
    }

    private handleOnCheckProduct(productId, checked) {
        if (checked) {
            this.dropdownProductDetail.fanexMultiSelect("selectByCustomAttributes", { productId });
        } else {
            this.dropdownProductDetail.fanexMultiSelect("deselectByCustomAttributes", { productId });
        }
    }

    private renderProductDetailDropdown(options: ProductAndProductDetailDropdownOption) {
        const self = this;
        const productIdsHasDetail = {};
        const productDetailList = options.productDetails.map(function (item) {
            productIdsHasDetail[item.ProductId] = true;

            return self.buildCheckboxItem(item, {
                productId: item.ProductId
            });
        });

        const dropdown = $("#" + options.selectorProductDetailId);
        this.dropdownProductDetail = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: productDetailList,
            onCheck: function () {
                const checked = this.checked;
                const productId = $(this).data("productid");

                if (checked) {
                    self.dropdownProduct.fanexMultiSelect("select", productId);
                } else {
                    if (!self.dropdownProductDetail.fanexMultiSelect("getSelectedItemsByCustomAttributes", { productId }).length) {
                        self.dropdownProduct.fanexMultiSelect("deselect", productId);
                    }
                }
            },
            onCheckAll: function () {
                if (this.checked) {
                    self.dropdownProduct.fanexMultiSelect("select", Object.keys(productIdsHasDetail));
                } else {
                    self.dropdownProduct.fanexMultiSelect("deselect", Object.keys(productIdsHasDetail));
                }
            },
            onLeave: options.onDropdownHidden
        });
    }

    private buildCheckboxItem(item, attributes = undefined) {
        return {
            value: item.Id,
            text: item.Name,
            checked: item.Checked,
            attributes
        }
    }

    getSelectedItems() {
        return {
            productIds: this.dropdownProduct.fanexMultiSelect("getSelectedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getSelectedValues")
        };
    }
}