const Joi = require("joi")
const mongoose = require("mongoose")

const Catch = {}

Catch.Schema = {
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
}

Catch.Model = mongoose.model("Catches", new mongoose.Schema(Catch.Schema))
Catch.ModelTest = mongoose.model(
	"CatchesTests",
	new mongoose.Schema({
		...Catch.Schema,
		FishType: {
			type: mongoose.Schema.Types.ObjectId,
			ref: "FishTypesTests"
		}
	})
)

Catch.attr = ["_id", "FishType", "DateTime", "Length", "Weight"]
Catch.attrNoId = ["FishType", "DateTime", "Length", "Weight"]

Catch.Joi = {
	FishType: Joi.string(),
	DateTime: Joi.string(),
	Length: Joi.number(),
	Weight: Joi.number()
}

Catch.validate = (c) => Joi.validate(c, Catch.Joi)

module.exports = Catch
