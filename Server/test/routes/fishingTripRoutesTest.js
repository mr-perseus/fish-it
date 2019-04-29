const Request = require("request")
const config = require("config")

const FishingTripsUri = config.EndPointUri + "fishingtrip"

describe("Server", () => {
	var Server
	beforeAll(() => (Server = require("../../server")))
	afterAll(() => Server.close)

	describe("FishingTrips - GET ALL", () => {
		let fishingtrips = {}

		beforeAll((done) => {
			Request.get(FishingTripsUri, (err, res, body) => {
				fishingtrips.status = res.statusCode
				fishingtrips.body = body
				done()
			})
		})

		it("- Status 200", async () => {
			expect(fishingtrips.status).toBe(200)
		})
	})
})
