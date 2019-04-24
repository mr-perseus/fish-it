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

const FishType = mongoose.model("FishTypes", fishTypeSchema)

const fishTypeAttr = ["_id", "Name", "Description"]
const fishTypeAttrNoId = ["Name", "Description"]

const fishTypeJoi = {
	Name: Joi.string()
		.max(64)
		.required(),
	Description: Joi.string().max(1024)
}

const validateFishType = (f) => Joi.validate(f, fishTypeJoi)

module.exports = {
	FishType,
	fishTypeAttr,
	fishTypeAttrNoId,
	validateFishType
}
