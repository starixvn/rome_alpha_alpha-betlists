"use strict";
var ProductAndProductDetailDropdownOption = /** @class */ (function () {
    function ProductAndProductDetailDropdownOption(products, productDetails, onDropdownHidden, selectorProductId, selectorProductDetailId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorProductId === void 0) { selectorProductId = "dropdown-product"; }
        if (selectorProductDetailId === void 0) { selectorProductDetailId = "dropdown-product-detail"; }
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
    }
    return ProductAndProductDetailDropdownOption;
}());
var ProductAndProductDetailDropdownComponent = /** @class */ (function () {
    function ProductAndProductDetailDropdownComponent() {
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function (options) {
        this.renderProductDropdown(options);
        this.renderProductDetailDropdown(options);
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDropdown = function (options) {
        var self = this;
        var productIdWithSubProductIds = {};
        var productList = options.products.map(function (item) {
            var checkboxItem = self.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds_1.push(item.Id);
                    return self.buildCheckboxItem(item);
                });
            }
            return checkboxItem;
        });
        var dropdown = $("#" + options.selectorProductId);
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
                var checked = this.checked;
                var productId = this.value;
                var subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    subProductIds.forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
                else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                self.dropdownProductDetail.fanexMultiSelect("setSelectAll", this.checked);
            },
            onLeave: options.onDropdownHidden
        });
    };
    ProductAndProductDetailDropdownComponent.prototype.handleOnCheckProduct = function (productId, checked) {
        if (checked) {
            this.dropdownProductDetail.fanexMultiSelect("selectByCustomAttributes", { productId: productId });
        }
        else {
            this.dropdownProductDetail.fanexMultiSelect("deselectByCustomAttributes", { productId: productId });
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetailDropdown = function (options) {
        var self = this;
        var productIdsHasDetail = {};
        var productDetailList = options.productDetails.map(function (item) {
            productIdsHasDetail[item.ProductId] = true;
            return self.buildCheckboxItem(item, {
                productId: item.ProductId
            });
        });
        var dropdown = $("#" + options.selectorProductDetailId);
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
                var checked = this.checked;
                var productId = $(this).data("productid");
                if (checked) {
                    self.dropdownProduct.fanexMultiSelect("select", productId);
                }
                else {
                    if (!self.dropdownProductDetail.fanexMultiSelect("getSelectedItemsByCustomAttributes", { productId: productId }).length) {
                        self.dropdownProduct.fanexMultiSelect("deselect", productId);
                    }
                }
            },
            onCheckAll: function () {
                if (this.checked) {
                    self.dropdownProduct.fanexMultiSelect("select", Object.keys(productIdsHasDetail));
                }
                else {
                    self.dropdownProduct.fanexMultiSelect("deselect", Object.keys(productIdsHasDetail));
                }
            },
            onLeave: options.onDropdownHidden
        });
    };
    ProductAndProductDetailDropdownComponent.prototype.buildCheckboxItem = function (item, attributes) {
        if (attributes === void 0) { attributes = undefined; }
        return {
            value: item.Id,
            text: item.Name,
            checked: item.Checked,
            attributes: attributes
        };
    };
    ProductAndProductDetailDropdownComponent.prototype.getSelectedItems = function () {
        return {
            productIds: this.dropdownProduct.fanexMultiSelect("getSelectedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getSelectedValues")
        };
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map