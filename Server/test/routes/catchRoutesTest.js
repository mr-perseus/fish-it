const Request = require("request")
const config = require("config")
const async = require("async")

const FishTypeUriCreate = config.TestEndPointUri + "fishtype/new"

const CatchUri = config.TestEndPointUri + "catch"
const CatchUriCreate = CatchUri + "/new"
let CatchUriId = CatchUri + "/"

const aFish = {
	Name: "Salmon",
	Description: "So delicious!"
}

const bFish = {
	Name: "Not a Salmon",
	Description: "yikes"
}

const aCatch = {
	DateTime: "2019-04-17T00:00:00",
	Length: 12.2,
	Weight: 15.8
}

const bCatch = {
	DateTime: "2019-04-17T00:00:00",
	Length: 88,
	Weight: 88
}

const cCatch = {
	DateTime: "2019-04-17T00:00:00",
	Length: "NotANumber",
	Weight: "NotANumber"
}

const dCatch = {
	DateTime: "2019-04-17T00:00:00",
	Length: 88,
	Weight: 88
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

	describe("- Setup Catch TestEnv", () => {
		let catchData = {}
		let aFishData = {}
		let bFishData = {}

		beforeAll((done) => {
			async.auto(
				{
					create_afish: (cb) => {
						Request.post({ url: FishTypeUriCreate, form: aFish }, (err, res, body) => {
							aFishData.status = res.statusCode
							aFishData.body = JSON.parse(body)
							aCatch.FishType = aFishData.body
							bCatch.FishType = aFishData.body
							cCatch.FishType = aFishData.body
							cb()
						})
					},
					create_bfish: (cb) => {
						Request.post({ url: FishTypeUriCreate, form: bFish }, (err, res, body) => {
							bFishData.status = res.statusCode
							bFishData.body = JSON.parse(body)
							dCatch.FishType = bFishData.body
							cb()
						})
					},
					create_catch: [
						"create_afish",
						"create_bfish",
						(r, cb) => {
							Request.post(
								{ url: CatchUriCreate, form: aCatch },
								(err, res, body) => {
									catchData.status = res.statusCode
									catchData.body = JSON.parse(body)
									CatchUriId += catchData.body._id
									cb()
								}
							)
						}
					]
				},
				done
			)
		})

		afterAll((done) => {
			Request.delete(config.TestEndPointUri, () => {
				done()
			})
		})

		it("- Creation Successfull", () => {
			expect(catchData.status).toBe(200)
			expect(catchData.body.Weight).toBe(15.8)
		})

		describe("- Various Manipulations", () => {
			let cData = {}
			let dData = {}
			let dFishData = {}

			beforeAll((done) => {
				async.auto(
					{
						NaN: (cb) => {
							Request.put({ url: CatchUriId, form: cCatch }, (err, res, body) => {
								cData.status = res.statusCode
								cData.body = body
								cb()
							})
						},
						update: [
							"NaN",
							(r, cb) => {
								Request.put({ url: CatchUriId, form: bCatch }, (err, res, body) => {
									catchData.status = res.statusCode
									catchData.body = JSON.parse(body)
									cb()
								})
							}
						],
						new_fish: [
							"update",
							(r, cb) => {
								Request.put({ url: CatchUriId, form: dCatch }, (err, res, body) => {
									dData.status = res.statusCode
									dData.body = JSON.parse(body)
									cb()
								})
							}
						],
						get_fish: [
							"new_fish",
							(r, cb) => {
								Request.get(CatchUriId, (err, res, body) => {
									dFishData.status = res.statusCode
									dFishData.body = JSON.parse(body).FishType
									console.log(body)
									cb()
								})
							}
						]
					},
					done
				)
			})

			it("- validation check", () => {
				expect(cData.status).toBe(400)
				expect(catchData.body.Length).toBe(88)
				expect(catchData.body.Weight).toBe(88)
			})

			it("- try changing FishType", () => {
				expect(dData.status).toBe(200)
				expect(dFishData.body.Name).toEqual(bFish.Name)
				expect(dFishData.body.Description).toEqual(bFish.Description)
			})

			describe("- removing", () => {
				let remData = {}

				beforeAll((done) => {
					Request.delete(CatchUriId, (err, res, body) => {
						remData.status = res.statusCode
						remData.body = JSON.parse(body)
						done()
					})
				})

				it(" - a catch", () => {
					expect(remData.status).toBe(200)
					expect(remData.body).toEqual(dData.body)
				})
			})
		})
	})
})
