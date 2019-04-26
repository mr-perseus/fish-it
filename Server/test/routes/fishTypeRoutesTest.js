const jasmine = require("jasmine")
const Request = require("request")

const EndPointUri = "http://localhost:3801/api/fishtypes"

describe("Server", () => {
	var Server
	beforeAll(() => (Server = require("../../server")))
	afterAll(() => Server.close)

	describe("fishTypes - GET ALL", () => {
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
