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
	},
	temperature: {
		type: String,
		maxlength: 8
	},
	catches: {
		type: Array
	}
})

const FishingTrip = mongoose.model("FishingTrips", fishingTripSchema)

function validateFishingTrip(fishingTrip) {
	const equipmentsJoi = {
		type: Joi.string().max(64),
		name: Joi.string().max(128)
	}

	const catchesJoi = {
		fish: Joi.string()
			.max(64)
			.required(),
		weight: Joi.string().max(16),
		length: Joi.string().max(8),
		datetime: Joi.string().length(16),
		equipments: Joi.array().items(equipmentsJoi)
	}

	const fishingTripJoi = {
		weather: Joi.string()
			.max(64)
			.required(),
		location: Joi.string()
			.max(64)
			.required(),
		date: Joi.string().required(),
		description: Joi.string().max(1024),
		temperature: Joi.string().max(8),
		catches: Joi.array().items(catchesJoi)
	}

	return Joi.validate(fishingTrip, fishingTripJoi)
}

const fishingTripAttr = ["weather", "location", "date", "description", "temperature", "catches"]

module.exports = {
	FishingTrip,
	fishingTripAttr,
	validateFishingTrip
}
