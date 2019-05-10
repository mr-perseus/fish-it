const Request = require("request")
const config = require("config")

const CatchesUri = config.TestEndPointUri + "catch"

describe("Server", () => {
	var Server
	beforeAll(() => (Server = require("../../server")))
	afterAll(() => Server.close)

	describe("Catches - GET ALL", () => {
		let catches = {}
		beforeAll((done) => {
			Request.get(CatchesUri, (err, res, body) => {
				catches.status = res.statusCode
				catches.body = body
				done()
			})
		})

		it("- Status 200", async () => {
			expect(catches.status).toBe(200)
		})
	})
})
