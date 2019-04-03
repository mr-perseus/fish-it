const Joi = require("joi")
const mongoose = require("mongoose")

const fishingTripSchema = new mongoose.Schema({
	weather: {
		type: String,
		required: true,
		maxlength: 64
	},
	location: {
		type: String,
		required: true,
		maxlength: 64
	},
	date: {
		type: String,
		required: true
	},
	description: {
		type: String,
		maxlength: 1024
	}
})

const FishingTrip = mongoose.model("FishingTrips", fishingTripSchema)

function validateFishingTrip(fishingTrip) {
	const schema = {
		weather: Joi.string().max(64).required,
		location: Joi.string()
			.max(64)
			.required(),
		date: Joi.string().required(),
		description: Joi.string().max(1024)
	}

	return Joi.validate(fishingTrip, schema)
}

const fishingTripAttr = ["weather", "location", "date", "description"]

module.exports = {
	FishingTrip,
	fishingTripAttr,
	validateFishingTrip
}
