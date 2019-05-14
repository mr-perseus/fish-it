const _ = require("lodash")
const FishType = require("../models/FishType")
const { isTest } = require("./services")
const getLogger = require("log4js").getLogger

module.exports.getFishTypes = async (req, res) => {
	const Model = isTest(req) ? FishType.ModelTest : FishType.Model

	await Model.find()
		.then((fishTypes) => {
			getLogger().info(`fishTypeService; getFishTypes; End; fishType;`, fishTypes)
			res.send(fishTypes.map((fishType) => _.pick(fishType, FishType.attr)))
		})
		.catch((error) => {
			getLogger().error(`fishTypeService; getFishTypes; Error; error;`, error)
			res.status(404).send(`404 - No fishTypeTypes were found. ${error}`)
		})
}

module.exports.getFishType = async (req, res) => {
	const Model = isTest(req) ? FishType.ModelTest : FishType.Model
	const _id = req.params.id

	await Model.findOne({ _id })
		.then((fishType) => {
			getLogger().info(`fishTypeService; getFish; End; _id;`, _id, "; fishType; ", fishType)
			res.send(_.pick(fishType, FishType.attr))
		})
		.catch((error) => {
			getLogger().error(`fishTypeService; getFish; Error; _id;`, _id, "; error; ", error)
			res.status(404).send(`404 -This fishType doesn't exist. ${error}`)
		})
}

module.exports.createFishType = async (req, res) => {
	const Model = isTest(req) ? FishType.ModelTest : FishType.Model

	const fishType = _.pick(req.body, FishType.attrNoId)

	const { error } = FishType.validate(fishType)
	getLogger().info(`fishTypeService; createFish; Start; fishType;`, fishType, "; error; ", error)
	if (error) return res.status(400).send(error.details[0].message)

	await new Model(fishType)
		.save()
		.then((fishType) => {
			getLogger().info(`fishTypeService; createfishType; End; fishType; `, fishType)
			res.send(_.pick(fishType, FishType.attr))
		})
		.catch((error) => {
			getLogger().error(
				`fishTypeService; createFish; Error; fishType;`,
				fishType,
				"; error; ",
				error
			)
			res.status(500).send(`undefined behaviour ${error}`)
		})
}

module.exports.updateFishType = async (req, res) => {
	const Model = isTest(req) ? FishType.ModelTest : FishType.Model
	const _id = req.params.id

	const fishType = _.pick(req.body, FishType.attrNoId)
	const { error } = FishType.validate(fishType)
	getLogger().info(
		`fishTypeService; updateFish; Start; fishType; `,
		fishType,
		"; _id; " + _id,
		"; error; ",
		error
	)
	if (error) return res.status(400).send(error.details[0].message)

	await Model.findOneAndUpdate({ _id }, fishType, { new: true })
		.then((fishType) => {
			getLogger().info(
				`fishTypeService; updateFish; End; fishType; `,
				fishType,
				"; _id; " + _id
			)
			res.send(_.pick(fishType, FishType.attr))
		})
		.catch((error) => {
			getLogger().error(
				`fishTypeService; updateFish; Error; fishType;`,
				fishType,
				"; _id; " + _id,
				"; error; ",
				error
			)
			res.status(400).send(`update error ${error}`)
		})
}

module.exports.deleteFishType = async (req, res) => {
	const Model = isTest(req) ? FishType.ModelTest : FishType.Model
	const _id = req.params.id

	await Model.findOneAndDelete({ _id })
		.then((fishType) => {
			getLogger().info(
				`fishTypeService; deleteFish; End; fishType; `,
				fishType,
				"; _id; " + _id
			)
			res.send(_.pick(fishType, FishType.attr))
		})
		.catch((error) => {
			getLogger().error(`fishTypeService; deleteFish; Error; _id;`, _id, "; error; ", error)
			res.status(400).send(`delete error ${error}`)
		})
}
