describe("LastBets", function () {
    var shortcut = {
        add: jasmine.any(Function)
    };
    var store = {
        get: jasmine.any(Function),
        set: jasmine.any(Function)
    };
    var lastBetsView, cacheElements, registerEvents, loadLastBets;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates/last-bets";
        lastBetsView = BetList.LastBetsView.Instance;
        cacheElements = spyOn(lastBetsView, "cacheElements");
        registerEvents = spyOn(lastBetsView, "registerEvents");
        loadLastBets = spyOn(lastBetsView, "loadLastBets");
    });
    describe("_init", function () {
        beforeEach(function () {
            // act
            lastBetsView.Init(shortcut, store);
        });
        it("_Always_Assign shortcut library", function () {
            // assert
            expect(lastBetsView.shortcut).toBe(shortcut);
        });
        it("_Always_Assign store library", function () {
            // assert
            expect(lastBetsView.store).toBe(store);
        });
        it("_Always_Calls cacheElements", function () {
            // assert
            expect(lastBetsView.cacheElements).toHaveBeenCalled();
        });
        it("_Always_Calls registerEvents", function () {
            // assert
            expect(lastBetsView.registerEvents).toHaveBeenCalled();
        });
        it("_Always_Calls loadLastBets", function () {
            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalledWith(false);
        });
    });
    describe("_init_reset trans id", function () {
        beforeEach(function () {
            spyOn(lastBetsView.localStorage, "set");
            spyOn(lastBetsView.localStorage, "get").and.returnValues("1");
        });
        it("_Must Reset_Set Trans Id = 0 to local storage", function () {
            // arrange
            loadFixtures("reset-trans-id-template.html");
            // act
            lastBetsView.Init(shortcut, store);
            // assert
            expect(lastBetsView.localStorage.set).toHaveBeenCalledWith("LastTransIdCacheKey", 0);
        });
        it("_Not Reset_Not call lastBetsView.localStorage.set", function () {
            // arrange
            loadFixtures("not-reset-trans-id-template.html");
            // act
            lastBetsView.Init(shortcut, store);
            // assert
            expect(lastBetsView.localStorage.set).not.toHaveBeenCalledWith("LastTransIdCacheKey", 0);
        });
    });
    describe("_cacheElements", function () {
        beforeEach(function () {
            // arrange
            loadFixtures("normal-template.html");
            lastBetsView.Init(shortcut, store);
            cacheElements.and.callThrough();
            // act
            lastBetsView.cacheElements();
        });
        it("_Always_All neccessary elements have been cached", function () {
            // assert
            expect(lastBetsView.$pageSize).toBeDefined();
            expect(lastBetsView.$loadingIcon).toBeDefined();
            expect(lastBetsView.$msgError).toBeDefined();
            expect(lastBetsView.$betListTableContainer).toBeDefined();
            expect(lastBetsView.betType).toBe(0);
            expect(lastBetsView.getBetsUrl).toBeDefined("GetBetData");
        });
    });
    describe("_registerEvents", function () {
        beforeEach(function () {
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
        it("_Always_Bind changeLastBetRange event", function () {
            // act
            lastBetsView.$pageSize.click();
            // assert
            expect(lastBetsView.changeLastBetRange).toHaveBeenCalled();
        });
    });
    describe("_changeLastBetRange", function () {
        it("_Always_Active selected element", function () {
            // arrange
            loadFixtures("normal-template.html");
            var element = $(".page-size-20");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            lastBetsView.getLastBetsTimeOut = 0;
            // act
            lastBetsView.changeLastBetRange(element);
            // assert
            expect(element.hasClass("active")).toBeTruthy();
            expect($(".page-size:not(.page-size-20)").hasClass("active")).toBeFalsy();
        });
        it("_Always_loadLastBets", function () {
            // arrange
            loadFixtures("normal-template.html");
            var element = $(".page-size-20");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            lastBetsView.getLastBetsTimeOut = 0;
            // act
            lastBetsView.changeLastBetRange(element);
            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalledWith(false);
        });
        it("_Has getLastBetsTimeOut_Clear getLastBetsTimeOut", function () {
            // arrange
            jasmine.clock().install();
            loadFixtures("normal-template.html");
            var element = $(".page-size-20");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            var spy = { func: jasmine.any(Function) };
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
    describe("_loadLastBets", function () {
        var localStorageSetSpy;
        beforeEach(function () {
            // arrange
            loadFixtures("normal-template.html");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            spyOn($, "ajax");
            spyOn(lastBetsView, "abortRequest");
            loadLastBets.and.callThrough();
            localStorageSetSpy = spyOn(lastBetsView.localStorage, "set");
            spyOn(lastBetsView.localStorage, "get").and.callFake(function () { return 1; });
        });
        it("_Always_Show loading icon", function () {
            // act
            lastBetsView.loadLastBets(false);
            // assert
            expect(lastBetsView.$loadingIcon.hasClass("hide")).toBeFalsy();
        });
        it("_Is Calling_abort request", function () {
            // arrange
            lastBetsView.isCalling = true;
            // act
            lastBetsView.loadLastBets(false);
            // assert
            expect(lastBetsView.abortRequest).toHaveBeenCalled();
        });
        it("_Is not Calling_Not abort request", function () {
            // arrange
            lastBetsView.isCalling = false;
            // act
            lastBetsView.loadLastBets(false);
            // assert
            expect(lastBetsView.abortRequest).not.toHaveBeenCalled();
        });
        it("_Always_Set isCalling=true", function () {
            // act
            lastBetsView.loadLastBets(false);
            // assert
            expect(lastBetsView.isCalling).toBeTruthy();
        });
        it("_Always_Call Ajax", function () {
            // act
            lastBetsView.loadLastBets(false);
            // assert
            expect($.ajax).toHaveBeenCalledWith({
                method: "GET",
                data: {
                    BetType: lastBetsView.betType,
                    PageSize: lastBetsView.pageSize,
                    IsAutoLoad: false,
                    TransId: 0,
                    LatestTransId: 1
                },
                url: lastBetsView.getBetsUrl,
                success: lastBetsView.successGetLastBets,
                error: lastBetsView.errorRequest,
                complete: lastBetsView.completeGetLastBets
            });
        });
        it("_Auto Load_Not Reset Last Trans Id", function () {
            // arrange
            var lastTransId = 10;
            lastBetsView.localStorage.set(lastBetsView.lastTransIdCacheKey, lastTransId);
            var callCount = localStorageSetSpy.calls.count();
            // act
            lastBetsView.loadLastBets(true);
            // assert
            expect(localStorageSetSpy.calls.count()).toBe(callCount);
        });
        it("_Not Auto Load_Reset Last Trans Id", function () {
            // arrange
            var lastTransId = 10;
            lastBetsView.localStorage.set(lastBetsView.lastTransIdCacheKey, lastTransId);
            // act
            lastBetsView.loadLastBets(false);
            // assert
            expect(lastBetsView.localStorage.set).toHaveBeenCalledWith("LastTransIdCacheKey", 0);
        });
    });
    describe("_abortRequest", function () {
        beforeEach(function () {
            // arrange
            loadFixtures("normal-template.html");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
        });
        it("_Not null ajaxRequest_Call abort", function () {
            // arrange
            lastBetsView.ajaxRequest = $.ajax({});
            spyOn(lastBetsView.ajaxRequest, "abort");
            // act
            lastBetsView.abortRequest();
            // assert
            expect(lastBetsView.ajaxRequest.abort).toHaveBeenCalled();
        });
        it("_Not null ajaxRequest_Call abort", function () {
            // arrange
            lastBetsView.ajaxRequest = null;
            // act & assert
            expect(function () { return lastBetsView.abortRequest(); }).not.toThrow();
        });
    });
    describe("_successGetLastBets", function () {
        beforeEach(function () {
            loadFixtures("normal-template.html");
            spyOn(lastBetsView.localStorage, "set");
            spyOn(lastBetsView.localStorage, "get").and.returnValues("1");
        });
        it("_Has error_Show error", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = { ErrorCode: 1 };
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.showError).toHaveBeenCalled();
        });
        it("_Has error_Show error", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = { ErrCode: 1 };
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.showError).toHaveBeenCalled();
        });
        it("_Not Have error_Hide error", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html";
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.$msgError.hasClass("hide")).toBeTruthy();
        });
        it("_Abort Calling_Not append html to container", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html";
            lastBetsView.isCalling = false;
            lastBetsView.$betListTableContainer.empty();
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.$betListTableContainer.html()).toBe("");
        });
        it("_Not Abort Calling_Append html to container", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.$betListTableContainer.html()).toBe("Html");
        });
        it("_Not Have data_Not set lastTransId", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.localStorage.set).not.toHaveBeenCalledWith("LastTransIdCacheKey", jasmine.any(Number));
        });
        it("_Have data_Set lastTransId", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html<input id='last-trans-id' type='hidden' value='2' />";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.localStorage.set).toHaveBeenCalledWith("LastTransIdCacheKey", 2);
        });
        it("_Always_setTimeout", function () {
            // arrange
            loadFixtures("normal-template.html");
            jasmine.clock().install();
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();
            // act
            lastBetsView.successGetLastBets(responseHtml);
            jasmine.clock().tick(2001);
            // assert
            expect(lastBetsView.loadLastBets).toHaveBeenCalled();
            jasmine.clock().uninstall();
        });
        it("_Always_Set isCalling=false", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            var responseHtml = "Html";
            lastBetsView.isCalling = true;
            lastBetsView.$betListTableContainer.empty();
            // act
            lastBetsView.successGetLastBets(responseHtml);
            // assert
            expect(lastBetsView.isCalling).toBeFalsy();
        });
    });
    describe("_showError", function () {
        beforeEach(function () {
            // act
            lastBetsView.showError();
        });
        it("_Always_remove class hide from error message", function () {
            // assert
            expect(lastBetsView.$msgError.hasClass("hide")).toBeFalsy();
        });
        // it("_Always_setTimeout with long time", () => {
        //    // assert
        //    expect(lastBetsView.setTimeoutFunc.calls.mostRecent().args).toEqual([jasmine.any(Function), 600000]);
        // });
    });
    describe("_completeGetLastBets", function () {
        it("_Always_Add class hide to loading icon", function () {
            // act
            lastBetsView.completeGetLastBets();
            // assert
            expect(lastBetsView.$loadingIcon.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_localStorage.get", function () {
        it("_Enable store -> Gets data from store", function () {
            // arrange
            lastBetsView.store.enabled = true;
            spyOn($, "cookie");
            spyOn(store, "get").and.returnValue("SomeValue");
            // act
            var actualValue = lastBetsView.localStorage.get("SomeKey");
            // assert
            expect(actualValue).toEqual("SomeValue");
            expect($.cookie).not.toHaveBeenCalled();
        });
        it("_Disable store -> Gets data from cookie", function () {
            // arrange
            lastBetsView.store.enabled = false;
            lastBetsView.localStorage.set("SomeKey", "SomeValue");
            spyOn($, "cookie").and.callFake(function () { return "SomeValue"; });
            // act
            var actualValue = lastBetsView.localStorage.get("SomeKey");
            // assert
            expect($.cookie).toHaveBeenCalled();
            expect(actualValue).toEqual("SomeValue");
        });
    });
    describe("_localStorage.set", function () {
        it("_Enable store -> Sets data to store", function () {
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
        it("_Disable store -> Sets data to cookie", function () {
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
    describe("_localStorage.getCookieExpireOptions", function () {
        it("_IE -> Returns expire days", function () {
            // arrange
            spyOn(lastBetsView, "isIe").and.callFake(function () { return true; });
            var expectedCookieExpireOptions = { expires: 365 };
            // act
            var actualCookieExpireOptions = lastBetsView.localStorage.getCookieExpireOptions();
            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
        it("_Not IE -> Returns expire days and path", function () {
            // arrange
            spyOn(lastBetsView, "isIe").and.callFake(function () { return false; });
            var expectedCookieExpireOptions = { expires: 365, path: window.location.pathname };
            // act
            var actualCookieExpireOptions = lastBetsView.localStorage.getCookieExpireOptions();
            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
    });
    // isIE: can't be tested
    // window.navigator.userAgent: can't be spied
    describe("_replaceQueryStringValue", function () {
        it("_Url Already Had Param -> Replaces Value", function () {
            // arrange
            var queryString = "http://demo?ParamName1=ParamValue1&ParamName=ParamValue";
            var expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName=NewParamValue";
            // act
            var actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName", "NewParamValue");
            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });
        it("_Url Did Not Have Param -> Appends New Param", function () {
            // arrange
            var queryString = "http://demo?ParamName1=ParamValue1";
            var expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName2=ParamValue2";
            // act
            var actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });
        it("_Url Did Not Have Query String -> Appends New Param With Question Mark", function () {
            // arrange
            var queryString = "http://demo";
            var expectNewQueryString = "http://demo?ParamName2=ParamValue2";
            // act
            var actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });
        it("_Url Is Null -> Return String empty", function () {
            // arrange
            var queryString = null;
            // act
            var actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual("");
        });
        it("_Url Is Undifined -> Return String empty", function () {
            // arrange
            var queryString;
            // act
            var actualNewQueryString = lastBetsView.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual("");
        });
    });
    describe("_replaceHrefParam", function () {
        var replaceQueryStringValueSpy;
        beforeEach(function () {
            // arrange
            loadFixtures("normal-template.html");
            cacheElements.and.callThrough();
            lastBetsView.Init(shortcut, store);
            replaceQueryStringValueSpy = spyOn(lastBetsView, "replaceQueryStringValue");
            // act
            lastBetsView.replaceHrefParam("paramName", "paramValue");
        });
        it("_Always_Calls replaceQueryStringValue", function () {
            // assert
            expect(replaceQueryStringValueSpy.calls.allArgs()).toEqual([["?paramName=paramValue", "paramName", "paramValue"], ["?paramName1=paramValue1", "paramName", "paramValue"]]);
        });
    });
    describe("errorRequest", function () {
        it("_Cancel Request_Not Show Error", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            // act
            lastBetsView.errorRequest({ statusText: "abort" });
            // assert
            expect(lastBetsView.showError).not.toHaveBeenCalled();
        });
        it("_Not Cancel Request_Show Error", function () {
            // arrange
            spyOn(lastBetsView, "showError");
            // act
            lastBetsView.errorRequest({ statusText: "error" });
            // assert
            expect(lastBetsView.showError).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=last-bets-tests.js.map