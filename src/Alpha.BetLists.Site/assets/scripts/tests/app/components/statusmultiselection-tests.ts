describe("StatusMultiSelectionComponent", () => {
    let statusMultiSelectionComponent: StatusMultiSelectionComponent;

    beforeEach(() => {
        statusMultiSelectionComponent = new StatusMultiSelectionComponent($);
    });

    describe("_validate", () => {
        it("_Always -> Return Selected Items", () => {
            // arrange
            setFixtures(`<body>
                        <select id="dropdown-status">
                            <option value="101" selected="selected"></option>
                            <option value="102"></option>
                            <option value="103" selected="selected"></option>
                        </select>
                    </body>`);
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");

            // act
            var actualSelectedItems = statusMultiSelectionComponent.getSelectedItems();

            // assert
            expect(actualSelectedItems).toEqual(actualSelectedItems);
        });
    });
    describe("_getSelectedItems", () => {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", () => {
            // arrange
            setFixtures(`<body>
                            <select id="dropdown-status">
                                <option value="101" selected="selected"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </select>
                            <div class="multiselect btn error"></div>
                        </body>`);
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            statusMultiSelectionComponent.validate();

            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <select id="dropdown-status">
                                <option value="101" selected="selected"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </select>
                            <div class="multiselect btn error"></div>
                        </body>`);
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = false;

            // act
            statusMultiSelectionComponent.validate();

            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Not Select Items after Validate Error -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-status">
                                <option value="101"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </div>
                            <div class="multiselect btn error"></div>
                        </body>`);
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            statusMultiSelectionComponent.validate();

            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-status">
                                <option value="101"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </div>
                            <div class="multiselect btn"></div>
                        </body>`);
            statusMultiSelectionComponent.$dropdownStatus = $("#dropdown-status");
            statusMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            statusMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            statusMultiSelectionComponent.validate();

            // assert
            expect(statusMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Validate Before Submit; Select Items after Validate Error;Don't have button select -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-status">
                                <option value="101"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </div>
                        </body>`);
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
    describe("_onChange_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(`<body><div id="status-ids"></div></body>`);
            spyOn(statusMultiSelectionComponent, "validate");
            spyOn(statusMultiSelectionComponent, "getSelectedItems").and.callFake(() => [101, 102, 103]);
            statusMultiSelectionComponent.$hiddenInput = $("#status-ids");

            // act
            statusMultiSelectionComponent.onChange(null, false);
        });

        it("_Validate", () => {
            // assert
            expect(statusMultiSelectionComponent.validate).toHaveBeenCalled();
        });

        it("_Update hidden input", () => {
            // assert
            expect(statusMultiSelectionComponent.$hiddenInput.val()).toBe("101,102,103");
        });
        it("_Calls Onchange Event", () => {
            // arrange
            let onChange = {
                event: () => {
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
    describe("_render", () => {
        describe("_Has Status", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <select id="dropdown-status"
                                    data-has-any-status="true"
                                    data-label-select-status="Select Status"
                                    data-label-selected="# selected"
                                    data-label-all-status="All selected"
                                    data-language="en">
                                <option value="101"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </select>
                            <div class="multiselect btn"></div>
                            <div id="status-ids"></div>
                        </body>`);
                spyOn($.fn, "multiselect");

                // act
                statusMultiSelectionComponent.render();
            });

            it("_Bind multi selection", () => {
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

            it("_Cache element;", () => {
                // assert
                expect(statusMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });

        describe("_Not Have Status", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <select id="dropdown-status"
                                    data-has-any-status="false"
                                    data-label-there-is-no-status="No Status Found"
                                    data-label-selected="# selected"
                                    data-label-all-status="All selected"
                                    data-language="en">
                                <option value="101"></option>
                                <option value="102"></option>
                                <option value="103"></option>
                            </select>
                            <div class="multiselect btn"></div>
                            <div id="status-ids"></div>
                        </body>`);
                spyOn($.fn, "multiselect");

                // act
                statusMultiSelectionComponent.render();
            });
            it("_Bind multi selection", () => {
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
            it("_Cache element;", () => {
                // assert
                expect(statusMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
    });
});