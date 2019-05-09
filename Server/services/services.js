const mongoose = require("mongoose")
const { fishTypeSchema } = require("../models/FishType")
const { catchSchema } = require("../models/Catch")
const { fishingTripSchema } = require("../models/FishingTrip")

module.exports.getModel = (path, model, schema) => {
	const collection = path.indexOf("test") > -1 ? model + "Tests" : model
	return mongoose.model(collection, schema)
}

module.exports.cleanDb = (req, res) => {
	if (req.originalUrl.indexOf("test") > -1) {
		const FishType = mongoose.model("FishTypesTests", fishTypeSchema)
		const Catch = mongoose.model("CatchesTests", catchSchema)
		const FishingTrip = mongoose.model("FishingTripsTests", fishingTripSchema)

		FishType.deleteMany({}, (err) => {
			if (err) return res.status(500).send("Couldn't delete FishTypeTests")
		})
		Catch.deleteMany({}, (err) => {
			if (err) return res.status(500).send("Couldn't delete CatchTests")
		})
		FishingTrip.deleteMany({}, (err) => {
			if (err) return res.status(500).send("Couldn't delete FishingTripsTests")
		})

		return res.send("Cleared Test Database")
	}
	return res.status(400).send("Action not allowed!")
}
