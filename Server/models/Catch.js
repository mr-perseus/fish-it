const Joi = require("joi")
const mongoose = require("mongoose")

const catchSchema = new mongoose.Schema({
	FishType: {
		type: mongoose.Schema.Types.ObjectId,
		ref: "FishTypes"
	},
	DateTime: {
		type: String,
		required: true,
		maxlength: 64
	},
	Length: {
		type: Number
	},
	Weight: {
		type: Number
	}
})

mongoose.model("Catches", catchSchema)
mongoose.model("CatchesTests", catchSchema)

const catchAttr = ["_id", "FishType", "DateTime", "Length", "Weight"]
const catchAttrNoId = ["FishType", "DateTime", "Length", "Weight"]

const catchJoi = {
	FishType: Joi.string(),
	DateTime: Joi.string(),
	Length: Joi.number(),
	Weight: Joi.number()
}

const validateCatch = (c) => Joi.validate(c, catchJoi)

module.exports = {
	catchSchema,
	catchAttr,
	catchAttrNoId,
	validateCatch
}
