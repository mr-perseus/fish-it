const Request = require("request")
const config = require("config")
const async = require("async")

const FishTypeUri = config.TestEndPointUri + "fishtype"
const FishTypeUriCreate = FishTypeUri + "/new"

const aFish = {
	Name: "Salmon",
	Description: "So delicious!"
}

const bFish = {
	Name: "Salmono",
	Description: "So delicious!"
}

const emptyFish = {
	Name: "",
	Description: ""
}

const longFish = {
	Name: "ThisIsAVeryLongFishNameThatDoesn'tMakeAnySenseSoGoAndGetYourFish!",
	Description: ""
}

describe("Server", () => {
	let Server

	beforeAll(() => {
		Server = require("../../server")
		process.setMaxListeners(100)
	})

	afterAll(() => {
		Server.close
	})

	describe("Setup FishType TestEnv", () => {
		let aData = {}
		let IdUri = FishTypeUri

		beforeAll((done) => {
			Request.post({ url: FishTypeUriCreate, form: aFish }, (err, res, body) => {
				aData.status = res.statusCode
				aData.body = JSON.parse(body)
				IdUri += "/" + aData.body._id
				done()
			})
		})

		afterAll((done) => {
			Request.delete(config.TestEndPointUri, () => {
				done()
			})
		})

		it("Creation Successfull", () => {
			expect(aData.status).toBe(200)
			expect(aData.body.Name).toBe("Salmon")
			expect(aData.body.Description).toBe("So delicious!")
		})

		describe("various updates", () => {
			let emptyData = {}
			let longData = {}

			beforeAll((done) => {
				async.parallel(
					[
						(cb) => {
							Request.put({ url: IdUri, form: emptyFish }, (err, res, body) => {
								emptyData.status = res.statusCode
								emptyData.body = body
								cb()
							})
						},
						(cb) => {
							Request.put({ url: IdUri, form: longFish }, (err, res, body) => {
								longData.status = res.statusCode
								longData.body = body
								cb()
							})
						},
						(cb) => {
							Request.put({ url: IdUri, form: bFish }, (err, res, body) => {
								aData.status = res.statusCode
								aData.body = JSON.parse(body)
								cb()
							})
						}
					],
					done
				)
			})

			it("Updating bad Request", (done) => {
				expect(emptyData.status).toBe(400)
				expect(longData.status).toBe(400)
				expect(aData.body.Name).toBe("Salmono")
				done()
			})
		})

		describe("removing", () => {
			let remData = {}

			beforeAll((done) => {
				Request.delete(IdUri, (err, res, body) => {
					remData.status = res.statusCode
					remData.body = JSON.parse(body)
					done()
				})
			})

			it("a Fishtype", (done) => {
				expect(remData.status).toBe(200)
				expect(remData.body).toEqual(aData.body)
				done()
			})
		})
	})
})
