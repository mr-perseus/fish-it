const Joi = require("joi")
const mongoose = require("mongoose")

const FishingTrip = {}

FishingTrip.Schema = {
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
}

FishingTrip.Model = mongoose.model("FishingTrips", new mongoose.Schema(FishingTrip.Schema))
FishingTrip.ModelTest = mongoose.model(
	"FishingTripsTests",
	new mongoose.Schema({
		...FishingTrip.Schema,
		Catches: [
			{
				type: mongoose.Schema.Types.ObjectId,
				ref: "CatchesTests"
			}
		]
	})
)

FishingTrip.attr = [
	"_id",
	"PredominantWeather",
	"Location",
	"DateTime",
	"Description",
	"Temperature",
	"Catches"
]

FishingTrip.attrNoId = [
	"PredominantWeather",
	"Location",
	"DateTime",
	"Description",
	"Temperature",
	"Catches"
]

FishingTrip.Joi = {
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

FishingTrip.validate = (fT) => Joi.validate(fT, FishingTrip.Joi)

module.exports = FishingTrip
