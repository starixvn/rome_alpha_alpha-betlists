
describe("LastBets", () => {
    const shortcut = {
        add: jasmine.any(Function)
    };

    const store = {
        get: jasmine.any(Function),
        set: jasmine.any(Function)
    };

    let lastBetsView: BetList.LastBetsView,
        cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        loadLastBets: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates/last-bets";
        lastBetsView = BetList.LastBetsView.Instance;

        cacheElements = spyOn(lastBetsView, "cacheElements");
        registerEvents = spyOn(lastBetsView, "registerEvents");
        loadLastBets = spyOn(lastBetsView, "loadLastBets");
    });

    describe("_init", () => {
        beforeEach(() => {
            // act
            lastBetsView.Init(shortcut, store);
        });

        it("_Always_Assign shortcut library", () => {
            // assert
            expect(lastBetsView.shortcut).toBe(shortcut);
        });

        it("_Always_Assign store library", () => {
            // assert
            expect(lastBetsView.store).toBe(store);
        });

        it("_Always_Calls cacheElements", () => {
            // assert
            expect(lastBetsView.cacheElements).toHaveBeenCalled();
        });

        it("_Always_Calls registerEvents", () => {
            // assert
            expect(lastBetsView.registerEvents).toHaveBeenCalled();
        });

        it("_Always_Calls loadLastBets", () => {
            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalledWith(false);
        });
    });

    describe("_init_reset trans id", () => {
        beforeEach(() => {
            spyOn(lastBetsView.localStorage, "set");
            spyOn(lastBetsView.localStorage, "get").and.returnValues("1");
        });

        it("_Must Reset_Set Trans Id = 0 to local storage", () => {
            // arrange
            loadFixtures("reset-trans-id-template.html");

            // act
            lastBetsView.Init(shortcut, store);

            // assert
            expect(lastBetsView.localStorage.set).toHaveBeenCalledWith("LastTransIdCacheKey", 0);
        });

        it("_Not Reset_Not call lastBetsView.localStorage.set", () => {
            // arrange
            loadFixtures("not-reset-trans-id-template.html");

            // act
            lastBetsView.Init(shortcut, store);

            // assert
            expect(lastBetsView.localStorage.set).not.toHaveBeenCalledWith("LastTransIdCacheKey", 0);
        });
    });
    describe("_cacheElements", () => {
        beforeEach(() => {
            // arrange
            loadFixtures("normal-template.html");
            lastBetsView.Init(shortcut, store);
            cacheElements.and.callThrough();

            // act
            lastBetsView.cacheElements();
        });

        it("_Always_All neccessary elements have been cached", () => {
            // assert
            expect(lastBetsView.$pageSize).toBeDefined();
            expect(lastBetsView.$loadingIcon).toBeDefined();
            expect(lastBetsView.$msgError).toBeDefined();
            expect(lastBetsView.$betListTableContainer).toBeDefined();
            expect(lastBetsView.betType).toBe(0);
            expect(lastBetsView.getBetsUrl).toBeDefined("GetBetData");
        });
    });
    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            loadFixtures("normal-template.html");

            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);

            spyOn(lastBetsView, "changeLastBetRange");
            spyOn(shortcut, "add");
            registerEvents.and.callThrough();

            // act
            lastBetsView.registerEvents();
        });

        it("_Always_Bind changeLastBetRange event", () => {
            // act
            lastBetsView.$pageSize.click();

            // assert
            expect(lastBetsView.changeLastBetRange).toHaveBeenCalled();
        });

        it("_Always_Binds shortcut.add", () => {
            // assert
            expect(lastBetsView.shortcut.add).toHaveBeenCalledWith("Shift+P", lastBetsView.setGetLastBetsTimeOutByShortcut);
        });
        it("_Always_Binds $lblSwitch.click", () => {
            // arrange
            spyOn(lastBetsView.$chbSwitch, "click");

            // act
            lastBetsView.$lblSwitch.click();

            // assert
            expect(lastBetsView.$chbSwitch.click).toHaveBeenCalled();
        });
    });
    describe("_changeLastBetRange", () => {
        it("_Always_Active selected element", () => {
            // arrange
            loadFixtures("normal-template.html");

            let element = $(".page-size-20");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            lastBetsView.getLastBetsTimeOut = 0;

            // act
            lastBetsView.changeLastBetRange(element);

            // assert
            expect(element.hasClass("active")).toBeTruthy();
            expect($(".page-size:not(.page-size-20)").hasClass("active")).toBeFalsy();
        });
        it("_Always_loadLastBets", () => {
            // arrange
            loadFixtures("normal-template.html");

            let element = $(".page-size-20");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            lastBetsView.getLastBetsTimeOut = 0;

            // act
            lastBetsView.changeLastBetRange(element);

            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalledWith(false);
        });
        it("_Has getLastBetsTimeOut_Clear getLastBetsTimeOut", () => {
            // arrange
            jasmine.clock().install();
            loadFixtures("normal-template.html");

            let element = $(".page-size-20");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);

            let spy = { func: jasmine.any(Function) };
            spyOn(spy, "func");
            lastBetsView.getLastBetsTimeOut = setTimeout(spy.func, 1000);

            // act
            lastBetsView.changeLastBetRange(element);
            jasmine.clock().tick(1001);

            // assert
            expect(spy.func).not.toHaveBeenCalled();
            jasmine.clock().uninstall();
        });
    });
    describe("_loadLastBets", () => {
        beforeEach(() => {
            // arrange
            loadFixtures("normal-template.html");

            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            spyOn($, "ajax");
            spyOn(lastBetsView, "abortRequest");

            loadLastBets.and.callThrough();
            spyOn(lastBetsView.localStorage, "set");
            spyOn(lastBetsView.localStorage, "get").and.returnValues("1");
        });

        it("_Always_Show loading icon", () => {
            // act
            lastBetsView.loadLastBets(false);

            // assert
            expect(lastBetsView.$loadingIcon.hasClass("hide")).toBeFalsy();
        });
        it("_Is Calling_abort request", () => {
            // arrange
            lastBetsView.isCalling = true;

            // act
            lastBetsView.loadLastBets(false);

            // assert
            expect(lastBetsView.abortRequest).toHaveBeenCalled();
        });
        it("_Is not Calling_Not abort request", () => {
            // arrange
            lastBetsView.isCalling = false;

            // act
            lastBetsView.loadLastBets(false);

            // assert
            expect(lastBetsView.abortRequest).not.toHaveBeenCalled();
        });
        it("_Always_Set isCalling=true", () => {
            // act
            lastBetsView.loadLastBets(false);

            // assert
            expect(lastBetsView.isCalling).toBeTruthy();
        });
        it("_Always_Call Ajax", () => {
            // act
            lastBetsView.loadLastBets(false);

            // assert
            expect($.ajax).toHaveBeenCalledWith({
                method: "GET",
                data: {
                    BetType: lastBetsView.betType,
                    PageSize: lastBetsView.pageSize,
                    IsAutoLoad: false,
                    TransId: 0
                },
                url: lastBetsView.getBetsUrl,
                success: lastBetsView.successGetLastBets,
                error: lastBetsView.showError,
                complete: lastBetsView.completeGetLastBets
            });
        });
    });
    describe("_abortRequest", () => {
        beforeEach(() => {
            // arrange
            loadFixtures("normal-template.html");

            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
        });

        it("_Not null ajaxRequest_Call abort", () => {
            // arrange
            lastBetsView.ajaxRequest = $.ajax({});
            spyOn(lastBetsView.ajaxRequest, "abort");

            // act
            lastBetsView.abortRequest();

            // assert
            expect(lastBetsView.ajaxRequest.abort).toHaveBeenCalled();
        });

        it("_Not null ajaxRequest_Call abort", () => {
            // arrange
            lastBetsView.ajaxRequest = null;

            // act & assert
            expect(() => lastBetsView.abortRequest()).not.toThrow();
        });
    });
    describe("_successGetLastBets", () => {
        beforeEach(() => {
            spyOn(lastBetsView.localStorage, "set");
            spyOn(lastBetsView.localStorage, "get").and.returnValues("1");
        });

        it("_Has error_Show error", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = { ErrorCode: 1 };

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.showError).toHaveBeenCalled();
        });
        it("_Not Have error_Hide error", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html";

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.$msgError.hasClass("hide")).toBeTruthy();
        });

        it("_Abort Calling_Not append html to container", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html";
            lastBetsView.isCalling = false;
            lastBetsView.$betListTableContainer.empty();

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.$betListTableContainer.html()).toBe("");
        });
        it("_Not Abort Calling_Append html to container", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.$betListTableContainer.html()).toBe("Html");
        });
        it("_Not Have data_Not set lastTransId", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.localStorage.set).not.toHaveBeenCalled();
        });
        it("_Have data_Set lastTransId", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html<input id='last-trans-id' type='hidden' value='2' />";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.localStorage.set).toHaveBeenCalledWith("LastTransIdCacheKey", 2);
        });
        it("_Always_setTimeout", () => {
            // arrange
            loadFixtures("normal-template.html");

            jasmine.clock().install();
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();

            // act
            lastBetsView.successGetLastBets(responseHtml);
            jasmine.clock().tick(2001);

            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalled();
            jasmine.clock().uninstall();
        });
        it("_Always_Set isCalling=false", () => {
            // arrange
            spyOn(lastBetsView, "showError");
            let responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();

            // act
            lastBetsView.successGetLastBets(responseHtml);

            // assert
            expect(lastBetsView.isCalling).toBeFalsy();
        });
    });
    describe("_showError", () => {
        beforeEach(() => {
            // act
            lastBetsView.showError();
        });
        it("_Always_remove class hide from error message", () => {
            // assert
            expect(lastBetsView.$msgError.hasClass("hide")).toBeFalsy();
        });
        // it("_Always_setTimeout with long time", () => {
        //    // assert
        //    expect(lastBetsView.setTimeoutFunc.calls.mostRecent().args).toEqual([jasmine.any(Function), 600000]);
        // });
    });
    describe("_completeGetLastBets", () => {
        it("_Always_Add class hide to loading icon", () => {
            // act
            lastBetsView.completeGetLastBets();

            // assert
            expect(lastBetsView.$loadingIcon.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_setGetLastBetsTimeOut", () => {
        it("_Has getLastBetsTimeOut_Clear getLastBetsTimeOut", () => {
            // arrange
            jasmine.clock().install();
            let spy = { func: jasmine.any(Function) };
            spyOn(spy, "func");
            lastBetsView.getLastBetsTimeOut = setTimeout(spy.func, 1000);

            // act
            lastBetsView.setGetLastBetsTimeOut();
            jasmine.clock().tick(1001);

            // assert
            expect(spy.func).not.toHaveBeenCalled();
            expect(lastBetsView.getLastBetsTimeOut).toBe(0);
            jasmine.clock().uninstall();
        });
        it("_Not Have getLastBetsTimeOut_Call setTimeout", () => {
            // arrange
            jasmine.clock().install();
            lastBetsView.getLastBetsTimeOut = 0;

            // act
            lastBetsView.setGetLastBetsTimeOut();
            jasmine.clock().tick(2001);

            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalled();
            jasmine.clock().uninstall();
        });
    });
    describe("_setGetLastBetsTimeOutByShortcut", () => {
        beforeEach(() => {
            // arrange
            loadFixtures("normal-template.html");

            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);

            spyOn(lastBetsView, "setGetLastBetsTimeOut");
            spyOn(lastBetsView.$lblSwitch, "click");

            // act
            lastBetsView.setGetLastBetsTimeOutByShortcut();
        });
        it("_Always_Calls $lblSwitch.click", () => {
            // assert
            expect(lastBetsView.$lblSwitch.click).toHaveBeenCalled();
        });
    });

    describe("_localStorage.get", () => {
        it("_Enable store -> Gets data from store", () => {
            // arrange
            lastBetsView.store.enabled = true;
            spyOn($, "cookie");
            spyOn(store, "get").and.returnValue("SomeValue");

            // act
            let actualValue = lastBetsView.localStorage.get("SomeKey");

            // assert
            expect(actualValue).toEqual("SomeValue");
            expect($.cookie).not.toHaveBeenCalled();
        });

        it("_Disable store -> Gets data from cookie", () => {
            // arrange
            lastBetsView.store.enabled = false;
            lastBetsView.localStorage.set("SomeKey", "SomeValue");
            spyOn($, "cookie").and.callFake(() => "SomeValue");

            // act
            let actualValue = lastBetsView.localStorage.get("SomeKey");

            // assert
            expect($.cookie).toHaveBeenCalled();
            expect(actualValue).toEqual("SomeValue");
        });
    });
    describe("_localStorage.set", () => {
        it("_Enable store -> Sets data to store", () => {
            // arrange
            lastBetsView.store.enabled = true;
            spyOn(store, "set");
            spyOn($, "cookie");

            // act
            lastBetsView.localStorage.set("SomeKey", "SomeValue");

            // assert
            expect(store.set).toHaveBeenCalled();
            expect($.cookie).not.toHaveBeenCalled();
        });
        it("_Disable store -> Sets data to cookie", () => {
            // arrange
            lastBetsView.store.enabled = false;
            spyOn(store, "set");
            spyOn($, "cookie");

            // act
            lastBetsView.localStorage.set("SomeKey", "SomeValue");

            // assert
            expect($.cookie).toHaveBeenCalled();
            expect(store.set).not.toHaveBeenCalled();
        });
    });
    describe("_localStorage.getCookieExpireOptions", () => {
        it("_IE -> Returns expire days", () => {
            // arrange
            spyOn(lastBetsView, "isIe").and.callFake(() => true);
            let expectedCookieExpireOptions = { expires: 365 };

            // act
            let actualCookieExpireOptions = lastBetsView.localStorage.getCookieExpireOptions();

            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
        it("_Not IE -> Returns expire days and path", () => {
            // arrange
            spyOn(lastBetsView, "isIe").and.callFake(() => false);
            let expectedCookieExpireOptions = { expires: 365, path: window.location.pathname };

            // act
            let actualCookieExpireOptions = lastBetsView.localStorage.getCookieExpireOptions();

            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
    });

    // isIE: can't be tested
    // window.navigator.userAgent: can't be spied
    describe("_replaceQueryStringValue", () => {
        it("_Url Already Had Param -> Replaces Value", () => {
            // arrange
            let queryString = "http://demo?ParamName1=ParamValue1&ParamName=ParamValue";
            let expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName=NewParamValue";

            // act
            let actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName", "NewParamValue");

            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });

        it("_Url Did Not Have Param -> Appends New Param", () => {
            // arrange
            let queryString = "http://demo?ParamName1=ParamValue1";
            let expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName2=ParamValue2";

            // act
            let actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });

        it("_Url Did Not Have Query String -> Appends New Param With Question Mark", () => {
            // arrange
            let queryString = "http://demo";
            let expectNewQueryString = "http://demo?ParamName2=ParamValue2";

            // act
            let actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });

        it("_Url Is Null -> Return String empty", () => {
            // arrange
            let queryString = null;

            // act
            let actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual("");
        });

        it("_Url Is Undifined -> Return String empty", () => {
            // arrange
            let queryString;

            // act
            let actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual("");
        });
    });
    describe("_replaceHrefParam", () => {
        let replaceQueryStringValueSpy;

        beforeEach(() => {
            // arrange
            loadFixtures("normal-template.html");

            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            replaceQueryStringValueSpy = spyOn(lastBetsView, "replaceQueryStringValue");

            // act
            lastBetsView.replaceHrefParam("paramName", "paramValue");
        });
        it("_Always_Calls replaceQueryStringValue", () => {
            // assert
            expect(replaceQueryStringValueSpy.calls.allArgs()).toEqual([["?paramName=paramValue", "paramName", "paramValue"], ["?paramName1=paramValue1", "paramName", "paramValue"]]);
        });
    });
});
