const jasmine = require("jasmine")
const Request = require("request")

const EndPointUri = "http://localhost:3801/api/fishingtrips"

describe("Server", () => {
	var Server
	beforeAll(() => (Server = require("../../server")))
	afterAll(() => Server.close)

	describe("fishingTrips - GET ALL", () => {
		let data = {}
		beforeAll((done) => {
			Request.get(EndPointUri, (err, res, body) => {
				data.status = res.statusCode
				data.body = body
				done()
			})
		})

		it("- Status 200", async () => {
			expect(data.status).toBe(200)
		})
	})
})
