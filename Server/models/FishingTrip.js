const Joi = require("joi")
const mongoose = require("mongoose")

const fishingTripSchema = new mongoose.Schema({
	PredominantWeather: {
		type: Number,
		required: true
	},
	Location: {
		type: String,
		required: true,
		maxlength: 64
	},
	DateTime: {
		type: Date,
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

const fishingTripAttrNoId = [
	"PredominantWeather",
	"Location",
	"DateTime",
	"Description",
	"Temperature",
	"Catches"
]

const fishingTripJoi = {
	PredominantWeather: Joi.number().required(),
	Location: Joi.string()
		.max(64)
		.required(),
	DateTime: Joi.date().required(),
	Description: Joi.string()
		.allow("")
		.max(1024),
	Temperature: Joi.number(),
	Catches: Joi.array()
}

const validateFishingTrip = (fT) => Joi.validate(fT, fishingTripJoi)

module.exports = {
	FishingTrip,
	fishingTripAttr,
	fishingTripAttrNoId,
	validateFishingTrip
}
