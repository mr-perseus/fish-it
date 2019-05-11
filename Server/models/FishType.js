const Joi = require("joi")
const mongoose = require("mongoose")

const FishType = {}

FishType.Schema = {
	Name: {
		type: String,
		required: true,
		maxlength: 64
	},
	Description: {
		type: String,
		maxlength: 1024
	}
}

FishType.Model = mongoose.model("FishTypes", new mongoose.Schema(FishType.Schema))
FishType.ModelTest = mongoose.model("FishTypesTests", new mongoose.Schema(FishType.Schema))

FishType.attr = ["_id", "Name", "Description"]
FishType.attrNoId = ["Name", "Description"]

FishType.Joi = {
	Name: Joi.string()
		.max(64)
		.required(),
	Description: Joi.string()
		.allow("")
		.max(1024)
}

FishType.validate = (f) => Joi.validate(f, FishType.Joi)

module.exports = FishType
