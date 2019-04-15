const Joi = require("joi")
const mongoose = require("mongoose")

const fishingTripSchema = new mongoose.Schema({
	PredominantWeather: {
		type: String,
		required: true,
		maxlength: 64
	},
	Location: {
		type: String,
		required: true,
		maxlength: 64
	},
	DateTime: {
		type: String,
		required: true
	},
	Description: {
		type: String,
		maxlength: 1024
	},
	Temperature: {
		type: Number
	},
	Catches: [
		{
			type: mongoose.Schema.Types.ObjectId,
			ref: "Catches"
		}
	]
})

const FishingTrip = mongoose.model("FishingTrips", fishingTripSchema)

const fishingTripAttr = [
	"_id",
	"PredominantWeather",
	"Location",
	"DateTime",
	"Description",
	"Temperature",
	"Catches"
]

const fishingTripJoi = {
	PredominantWeather: Joi.string()
		.max(64)
		.required(),
	Location: Joi.string()
		.max(64)
		.required(),
	DateTime: Joi.string().required(),
	Description: Joi.string().max(1024),
	Temperature: Joi.number(),
	Catches: Joi.array()
}

const validateFishingTrip = (fT) => Joi.validate(fT, fishingTripJoi)

module.exports = {
	FishingTrip,
	fishingTripAttr,
	validateFishingTrip
}
