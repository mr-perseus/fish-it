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
	},
	Image: {
		type: String,
		required: false
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

Catch.attr = ["_id", "FishType", "DateTime", "Length", "Weight", "Image"]
Catch.attrNoId = ["FishType", "DateTime", "Length", "Weight", "Image"]

Catch.Joi = {
	FishType: Joi.object(),
	DateTime: Joi.string(),
	Length: Joi.number(),
	Weight: Joi.number(),
	Image: Joi.string().allow("")
}

Catch.validate = (c) => Joi.validate(c, Catch.Joi)

module.exports = Catch
