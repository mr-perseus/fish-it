const Joi = require("joi")
const mongoose = require("mongoose")

const fishTypeSchema = new mongoose.Schema({
	Name: {
		type: String,
		required: true,
		maxlength: 64
	},
	Description: {
		type: String,
		maxlength: 1024
	}
})

mongoose.model("FishTypes", fishTypeSchema)
mongoose.model("FishTypesTests", fishTypeSchema)

const fishTypeAttr = ["_id", "Name", "Description"]
const fishTypeAttrNoId = ["Name", "Description"]

const fishTypeJoi = {
	Name: Joi.string()
		.max(64)
		.required(),
	Description: Joi.string()
		.allow("")
		.max(1024)
}

const validateFishType = (f) => Joi.validate(f, fishTypeJoi)

module.exports = {
	fishTypeSchema,
	fishTypeAttr,
	fishTypeAttrNoId,
	validateFishType
}
