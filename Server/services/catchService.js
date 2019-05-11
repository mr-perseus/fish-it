const _ = require("lodash")
const Catch = require("../models/Catch")
const { isTest } = require("./services")
const getLogger = require("log4js").getLogger

module.exports.getCatches = async (req, res) => {
	const Model = isTest(req) ? Catch.ModelTest : Catch.Model

	await Model.find()
		.populate("FishType")
		.then((catches) => {
			getLogger().info(`catchService; getCatches; End; catch;`, catches)
			res.send(catches)
		})
		.catch((error) => {
			getLogger().error(`catchService; getCatches; Error; error;`, error)
			res.status(404).send(`404 - No Catches were found. ${error}`)
		})
}

module.exports.getCatch = async (req, res) => {
	const Model = isTest(req) ? Catch.ModelTest : Catch.Model
	const _id = req.params.id

	await Model.findOne({ _id })
		.populate("FishType")
		.then((aCatch) => {
			getLogger().info(`catchService; getCatch; End; _id;`, _id, "; catch; ", aCatch)
			res.send(_.pick(aCatch, Catch.attr))
		})
		.catch((error) => {
			getLogger().error(`catchService; getCatch; Error; _id;`, _id, "; error; ", error)
			res.status(404).send(`404 -This catch doesn't exist. ${error}`)
		})
}

module.exports.createCatch = async (req, res) => {
	const Model = isTest(req) ? Catch.ModelTest : Catch.Model

	const aCatch = _.pick(req.body, Catch.attrNoId)
	aCatch.FishType = aCatch.FishType._id

	const { error } = Catch.validate(aCatch)
	getLogger().info(`catchService; createCatch; Start; catch;`, aCatch, "; error; ", error)
	if (error) return res.status(400).send(error.details[0].message)

	await new Model(aCatch)
		.save()
		.then((aCatch) => {
			getLogger().info(`catchService; createcatch; End; catch; `, aCatch)
			res.send(_.pick(aCatch, Catch.attr))
		})
		.catch((error) => {
			getLogger().error(
				`catchService; createCatch; Error; catch;`,
				aCatch,
				"; error; ",
				error
			)
			res.status(500).send(`undefined behaviour ${error}`)
		})
}

module.exports.updateCatch = async (req, res) => {
	const Model = isTest(req) ? Catch.ModelTest : Catch.Model
	const _id = req.params.id

	const aCatch = _.pick(req.body, Catch.attrNoId)
	aCatch.FishType = aCatch.FishType._id

	const { error } = Catch.validate(aCatch)
	getLogger().info(
		`catchService; updateCatch; Start; catch; `,
		aCatch,
		"; _id; " + _id,
		"; error; ",
		error
	)
	if (error) return res.status(400).send(error.details[0].message)

	await Model.findOneAndUpdate({ _id }, aCatch, { new: true })
		.populate("FishType")
		.then((aCatch) => {
			getLogger().info(`catchService; updateCatch; End; catch; `, aCatch, "; _id; " + _id)
			res.send(_.pick(aCatch, Catch.attr))
		})
		.catch((error) => {
			getLogger().error(
				`catchService; updateCatch; Error; catch;`,
				aCatch,
				"; _id; " + _id,
				"; error; ",
				error
			)
			res.status(400).send(`update error ${error}`)
		})
}

module.exports.deleteCatch = async (req, res) => {
	const Model = isTest(req) ? Catch.ModelTest : Catch.Model
	const _id = req.params.id

	await Model.findOneAndDelete({ _id })
		.populate("FishType")
		.then((aCatch) => {
			getLogger().info(`catchService; deleteCatch; End; catch; `, aCatch, "; _id; " + _id)
			res.send(_.pick(aCatch, Catch.attr))
		})
		.catch((error) => {
			getLogger().error(`catchService; deleteCatch; Error; _id;`, _id, "; error; ", error)
			res.status(400).send(`delete error ${error}`)
		})
}
