describe("Status_ResultDetails_", () => {
    describe("Factory_", () => {
        [71, 72].forEach((betType: number) => {
            it("CreateInstance_ByBetType -> Return CasinoResultDetails Instance", () => {
                // Act
                let actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);

                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.CasinoResultDetails));
            });
        });

        for (let betType = 2501; betType <= 2699; betType++) {
            it("CreateInstance_ByBetType -> Return NG2ResultDetails Instance", () => {
                // Act
                let actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);

                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.NG2ResultDetails));
            });
        };

        [9, 29, 2799].forEach((betType: number) => {
            it("CreateInstance_ByBetType -> Return MixParlayResultDetails Instance", () => {
                // Act
                let actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);

                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.MixParlayResultDetails));
            });
        });

        [1, 2, 3, 4, 5].forEach((betType: number) => {
            it("CreateInstance_ByBetType -> Return SportsbookResultDetails Instance", () => {
                // Act
                let actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);

                // Assert
                expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.SportsbookResultDetails));
            });
        });
    });

    describe("Base_", () => {
        it("ViewResultDetails -> Create Instance View Result Details", () => {
            // Arrange
            const betType = 1;
            const date = new Date();
            let instance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);
            let viewResultDetailsSpy = spyOn(instance, "ViewResultDetails");

            // Act
            BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails(1, 1, betType, 1, "1", "user", date, 1, 1, false, 1, "1", 1, "transDesc");

            // Assert
            expect(viewResultDetailsSpy).toHaveBeenCalledWith(1, 1, betType, 1, "1", "user", date, 1, 1, false, 1, "1", 1, "transDesc");
        });
    });

    describe("GlobalResultsFunction_", () => {
        it("ViewResult -> Assign Expected Function", () => {
            // Arrange
            const expectedResultDetailsFunc = BetList.Status.ResultDetails.BaseResultDetails.Instance.ViewResultDetails;

            // Act & Assert
            expect(ViewResult).toBe(expectedResultDetailsFunc);
        });
    });
});
