const Request = require("request")
const config = require("config")
const async = require("async")

const FishTypeUriCreate = config.TestEndPointUri + "fishtype/new"
const CatchUriCreate = config.TestEndPointUri + "catch/new"

const FishingTripUri = config.TestEndPointUri + "fishingtrip"
const FishingTripUriCreate = FishingTripUri + "/new"
let FishingTripUriUriId = FishingTripUri + "/"

const aFish = {
	Name: "Salmon",
	Description: "So delicious!"
}

const aCatch = {
	DateTime: "2019-08-06T11:58:00",
	Length: 3.7,
	Weight: 25
}

const bCatch = {
	DateTime: "2020-11-20T13:25:00",
	Length: 99,
	Weight: 99
}

const aTrip = {
	PredominantWeather: 1,
	DateTime: "2020-11-20",
	Location: "Bodensee",
	Description: "A wonderful trip",
	Temperature: "8.7",
	Catches: []
}

const bTrip = {
	PredominantWeather: "Sunny",
	DateTime: "2020-11-20",
	Location: "Bodensee",
	Description: "",
	Temperature: 15,
	Catches: []
}

const cTrip = {
	PredominantWeather: 1,
	DateTime: "2021-01-02",
	Location: "Obersee",
	Description: "A surprisingly bad trip",
	Temperature: "12",
	Catches: []
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

	describe("- Setup FishingTrip TestEnv", () => {
		let catchData = {}
		let tripData = {}

		beforeAll((done) => {
			async.auto(
				{
					create_afish: (cb) => {
						Request.post({ url: FishTypeUriCreate, form: aFish }, (err, res, body) => {
							let fish = JSON.parse(body)
							aCatch.FishType = fish
							bCatch.FishType = fish
							cb()
						})
					},
					create_aCatch: (cb) => {
						Request.post({ url: CatchUriCreate, form: aCatch }, (err, res, body) => {
							let aCatch = JSON.parse(body)
							aTrip.Catches.push(aCatch)
							bTrip.Catches.push(aCatch)
							cb()
						})
					},
					create_bCatch: (cb) => {
						Request.post({ url: CatchUriCreate, form: bCatch }, (err, res, body) => {
							catchData.status = res.statusCode
							catchData.body = JSON.parse(body)
							cTrip.Catches.push(catchData.body)
							cb()
						})
					},
					create_catch: [
						"create_afish",
						"create_aCatch",
						"create_bCatch",
						(r, cb) => {
							Request.post(
								{ url: FishingTripUriCreate, form: aTrip },
								(err, res, body) => {
									tripData.status = res.statusCode
									tripData.body = JSON.parse(body)
									FishingTripUriUriId += tripData.body._id
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
			expect(tripData.status).toBe(200)
			expect(tripData.body.PredominantWeather).toBe(1)
			expect(tripData.body.Temperature).toBe(8.7)
		})

		describe("- Various Manipulations", () => {
			let bTripData = {}
			let cTripData = {}

			beforeAll((done) => {
				async.auto(
					{
						bad_data: (cb) => {
							Request.put(
								{ url: FishingTripUriUriId, form: bTrip },
								(err, res, body) => {
									bTripData.status = res.statusCode
									bTripData.body = body
									cb()
								}
							)
						},
						change_catch: [
							"bad_data",
							(r, cb) => {
								aTrip.Catches[0] = catchData.body
								Request.put(
									{ url: FishingTripUriUriId, form: aTrip },
									(err, res, body) => {
										tripData.status = res.statusCode
										tripData.body = JSON.parse(body)
										cb()
									}
								)
							}
						],
						update: [
							"change_catch",
							(r, cb) => {
								Request.put(
									{ url: FishingTripUriUriId, form: cTrip },
									(err, res, body) => {
										cTripData.status = res.statusCode
										cTripData.body = JSON.parse(body)
										cb()
									}
								)
							}
						]
					},
					done
				)
			})

			it("- bad data check", () => {
				expect(bTripData.status).toBe(400)
			})

			it("- change catch to bCatch", () => {
				expect(tripData.status).toBe(200)
				expect(tripData.body.Catches[0].Length).toEqual(catchData.body.Length)
			})

			it("- update", () => {
				expect(cTripData.status).toBe(200)
				expect(cTripData.body.Location).toBe("Obersee")
				expect(cTripData.body.Temperature).toBe(12)
			})

			describe("- removing", () => {
				let remData = {}

				beforeAll((done) => {
					Request.delete(FishingTripUriUriId, (err, res, body) => {
						remData.status = res.statusCode
						remData.body = JSON.parse(body)
						done()
					})
				})

				it(" - a fishingTrip", () => {
					expect(remData.status).toBe(200)
					expect(remData.body).toEqual(cTripData.body)
				})
			})
		})
	})
})
