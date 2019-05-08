const mongoose = require("mongoose")

module.exports.getModel = (path, model, schema) => {
	const collection = path.indexOf("test") > -1 ? model + "Test" : model
	return mongoose.model(collection, schema)
}
