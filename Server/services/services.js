const async = require("async")

const FishType = require("../models/FishType")
const Catch = require("../models/Catch")
const FishingTrip = require("../models/FishingTrip")

module.exports.isTest = (req) => {
	return req.originalUrl.indexOf("test") > -1
}

module.exports.cleanDb = (req, res) => {
	if (req.originalUrl.indexOf("test") === -1) {
		return res.status(400).send("Action not allowed!")
	}

	async.parallel(
		[
			(cb) => {
				FishType.ModelTest.deleteMany({}, (err) => {
					if (err) return res.status(500).send("Couldn't delete FishTypeTests")
					cb()
				})
			},
			(cb) => {
				Catch.ModelTest.deleteMany({}, (err) => {
					if (err) return res.status(500).send("Couldn't delete CatchTests")
					cb()
				})
			},
			(cb) => {
				FishingTrip.ModelTest.deleteMany({}, (err) => {
					if (err) return res.status(500).send("Couldn't delete FishingTripsTests")
					cb()
				})
			}
		],
		() => {
			return res.send("Cleared Test Database")
		}
	)
}
