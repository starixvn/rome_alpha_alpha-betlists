describe("Status_ResultDetails_", () => {
    describe("Factory_", () => {
        it("CreateInstance_ByBetType -> Return Expected Instance", () => {
            // Arrange
            const betType = 71;

            // Act
            let actualInstance = BetList.Status.ResultDetails.ResultDetailsFactory.CreateInstance(betType);

            // Assert
            expect(actualInstance).toEqual(jasmine.any(BetList.Status.ResultDetails.CasinoResultDetails));
        });
    });
});