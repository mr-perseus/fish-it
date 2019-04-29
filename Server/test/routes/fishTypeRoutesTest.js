const Request = require("request")
const config = require("config")

const FishTypesUri = config.EndPointUri + "fishtype"

describe("Server", () => {
	var Server
	beforeAll(() => (Server = require("../../server")))
	afterAll(() => Server.close)

	describe("fishTypes - GET ALL", () => {
		let data = {}
		beforeAll((done) => {
			Request.get(FishTypesUri, (err, res, body) => {
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
