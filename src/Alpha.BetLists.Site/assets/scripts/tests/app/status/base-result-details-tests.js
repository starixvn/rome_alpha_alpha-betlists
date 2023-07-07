describe("Status_ResultDetails_", function () {
    describe("Factory_", function () {
        [9, 29, 2799].forEach(function (betType) {
            it("CreateInstance_ByBetType -> Return MixParlayResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.MixParlayResultDetails));
            });
        });
        var _loop_1 = function (betType) {
            it("CreateInstance_ByBetType -> Return FishingWorldResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.FishingWorldResultDetails));
            });
        };
        for (var betType = 1011; betType <= 1020; betType++) {
            _loop_1(betType);
        }
        ;
        var _loop_2 = function (betType) {
            it("CreateInstance_ByBetType -> Return RnGCasinoResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.RnGCasinoResultDetails));
            });
        };
        for (var betType = 2101; betType <= 2399; betType++) {
            _loop_2(betType);
        }
        ;
        var _loop_3 = function (betType) {
            it("CreateInstance_ByBetType -> Return RnGCasinoResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.RnGCasinoResultDetails));
            });
        };
        for (var betType = 2901; betType <= 2999; betType++) {
            _loop_3(betType);
        }
        ;
        var _loop_4 = function (betType) {
            it("CreateInstance_ByBetType -> Return MacauGamesResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.MacauGamesResultDetails));
            });
        };
        for (var betType = 4001; betType <= 4100; betType++) {
            _loop_4(betType);
        }
        ;
        var _loop_5 = function (betType) {
            it("CreateInstance_ByBetType -> Return MacauGamesResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.MacauGamesResultDetails));
            });
        };
        for (var betType = 5800; betType <= 5999; betType++) {
            _loop_5(betType);
        }
        ;
        var _loop_6 = function (betType) {
            it("CreateInstance_ByBetType -> Return AllbetResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.AllbetResultDetails));
            });
        };
        for (var betType = 2401; betType <= 2405; betType++) {
            _loop_6(betType);
        }
        ;
        var _loop_7 = function (betType) {
            it("CreateInstance_ByBetType -> Return FastMarketsResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.FastMarketsResultDetails));
            });
        };
        for (var betType = 221; betType <= 227; betType++) {
            _loop_7(betType);
        }
        ;
        var _loop_8 = function (betType) {
            it("CreateInstance_ByBetType -> Return PPResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.PPResultDetails));
            });
        };
        for (var betType = 4101; betType <= 4300; betType++) {
            _loop_8(betType);
        }
        ;
        [1, 2, 3, 4, 5].forEach(function (betType) {
            it("CreateInstance_ByBetType -> Return SportsbookResultDetails Instance", function () {
                // Act
                var actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.SportsbookResultDetails));
            });
        });
    });
    describe("Base_", function () {
        it("ViewResultDetails -> Create Instance View Result Details", function () {
            // Arrange
            var betType = 1;
            var date = new Date();
            var instance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
            var viewResultDetailsSpy = spyOn(instance, "ViewResultDetails");
            // Act
            BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails(1, 1, betType, 1, "1", "user", date, 1, 1, false, 1, "1", 1, "transDesc", "siteId");
            // Assert
            expect(viewResultDetailsSpy).toHaveBeenCalledWith(1, 1, betType, 1, "1", "user", date, 1, 1, false, 1, "1", 1, "transDesc", "siteId");
        });
    });
    describe("GlobalResultsFunction_", function () {
        it("ViewResult -> Assign Expected Function", function () {
            // Arrange
            var expectedResultDetailsFunc = BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails;
            // Act & Assert
            expect(ViewResult).toBe(expectedResultDetailsFunc);
        });
    });
});
//# sourceMappingURL=base-result-details-tests.js.map