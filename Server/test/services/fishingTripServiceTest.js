const fishingTripService = require("../../services/fishingTripService");
const jasmine = require("jasmine");

describe("getFishingTrips", function () {
    it("allFishingTrips", async function () {
        const product = await fishingTripService.getFishingTrips(null, null);
        console.log(product);
        // expect(product).toBe({}); // TODO
    });
});
