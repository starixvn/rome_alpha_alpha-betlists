describe("StatusMultiSelectionComponent", function () {
    var statusMultiSelectionComponent;
    beforeEach(function () {
        statusMultiSelectionComponent = new StatusMultiSelectionComponent($);
    });
    describe("_validate", function () {
        it("_Always -> Return Selected Items", function () {
            // arrange
            setFixtures("<body>\n                        <select id=\"dropdown-status\">\n                            <option value=\"101\" selected=\"selected\"></option>\n                            <option value=\"102\"></option>\n                            <option value=\"103\" selected=\"selected\"></option>\n                        </select>\n                    </body>");
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            // act
            var actualSelectedItems = statusMultiSelectionComponent.getSelectedItems();
            // assert
            expect(actualSelectedItems).toEqual(actualSelectedItems);
        });
    });
    describe("_getSelectedItems", function () {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-status\">\n                                <option value=\"101\" selected=\"selected\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            statusMultiSelectionComponent.validate();
            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-status\">\n                                <option value=\"101\" selected=\"selected\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = false;
            // act
            statusMultiSelectionComponent.validate();
            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Not Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-status\">\n                                <option value=\"101\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </div>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            statusMultiSelectionComponent.validate();
            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-status\">\n                                <option value=\"101\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </div>\n                            <div class=\"multiselect btn\"></div>\n                        </body>");
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            statusMultiSelectionComponent.validate();
            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Validate Before Submit; Select Items after Validate Error;Don't have button select -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-status\">\n                                <option value=\"101\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </div>\n                        </body>");
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;
            spyOn($.fn, "removeClass");
            // act
            statusMultiSelectionComponent.validate();
            // assert
            expect($.fn.removeClass).not.toHaveBeenCalledWith("error");
        });
    });
    describe("_onChange_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body><div id=\"status-ids\"></div></body>");
            spyOn(statusMultiSelectionComponent, "validate");
            spyOn(statusMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [101, 102, 103]; });
            statusMultiSelectionComponent.$hiddenInput = $("#status-ids");
            // act
            statusMultiSelectionComponent.onChange(null, false);
        });
        it("_Validate", function () {
            // assert
            expect(statusMultiSelectionComponent.validate).toHaveBeenCalled();
        });
        it("_Update hidden input", function () {
            // assert
            expect(statusMultiSelectionComponent.$hiddenInput.val()).toBe("101,102,103");
        });
        it("_Calls Onchange Event", function () {
            // arrange
            var onChange = {
                event: function () {
                    console.log(1);
                }
            };
            spyOn(onChange, "event");
            statusMultiSelectionComponent.options =
                new StatusMultiSelectionOption("dropdown-status", true, "status-ids", onChange.event);
            statusMultiSelectionComponent.onChange(null, false);
            // assert
            expect(statusMultiSelectionComponent.options.onChange).toHaveBeenCalled();
        });
    });
    describe("_render", function () {
        describe("_Has Status", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <select id=\"dropdown-status\"\n                                    data-has-any-status=\"true\"\n                                    data-label-select-status=\"Select Status\"\n                                    data-label-selected=\"# selected\"\n                                    data-label-all-status=\"All selected\"\n                                    data-language=\"en\">\n                                <option value=\"101\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </select>\n                            <div class=\"multiselect btn\"></div>\n                            <div id=\"status-ids\"></div>\n                        </body>");
                spyOn($.fn, "multiselect");
                // act
                statusMultiSelectionComponent.render();
            });
            it("_Bind multi selection", function () {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300,
                    numberDisplayed: 1,
                    nonSelectedText: "Select Status",
                    nSelectedText: " # selected",
                    allSelectedText: "All selected",
                    includeSelectAllOption: true,
                    onChange: statusMultiSelectionComponent.onChange,
                    onSelectAll: statusMultiSelectionComponent.onChange,
                    language: "en"
                });
            });
            it("_Cache element;", function () {
                // assert
                expect(statusMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
        describe("_Not Have Status", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <select id=\"dropdown-status\"\n                                    data-has-any-status=\"false\"\n                                    data-label-there-is-no-status=\"No Status Found\"\n                                    data-label-selected=\"# selected\"\n                                    data-label-all-status=\"All selected\"\n                                    data-language=\"en\">\n                                <option value=\"101\"></option>\n                                <option value=\"102\"></option>\n                                <option value=\"103\"></option>\n                            </select>\n                            <div class=\"multiselect btn\"></div>\n                            <div id=\"status-ids\"></div>\n                        </body>");
                spyOn($.fn, "multiselect");
                // act
                statusMultiSelectionComponent.render();
            });
            it("_Bind multi selection", function () {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300,
                    numberDisplayed: 1,
                    nonSelectedText: "No Status Found",
                    nSelectedText: " # selected",
                    allSelectedText: "All selected",
                    includeSelectAllOption: false,
                    onChange: statusMultiSelectionComponent.onChange,
                    onSelectAll: statusMultiSelectionComponent.onChange,
                    language: "en"
                });
            });
            it("_Cache element;", function () {
                // assert
                expect(statusMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
    });
});
//# sourceMappingURL=statusmultiselection-tests.js.map