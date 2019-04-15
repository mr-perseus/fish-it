const _ = require("lodash")
const { Catch, catchAttr, validateCatch } = require("../models/Catch")
const getLogger = require("log4js").getLogger

module.exports.getCatches = async (req, res) => {
	await Catch.find()
		.then((catches) => {
			getLogger().info(`catchService; getCatches; End; catch;`, catches)
			catches = catches.map((c) => _.pick(c, catchAttr))
			res.send(catches)
		})
		.catch((error) => {
			getLogger().error(`catchService; getCatches; Error; error;`, error)
			res.status(404).send(`404 - No Catches were found. ${error}`)
		})
}

module.exports.getCatchesById = async (catchesId) => {
	let catches = []

	const result = catchesId.map(async (_id) => {
		await Catch.findOne({ _id })
			.then((aCatch) => {
				getLogger().info(`catchService; getCatch; End; _id;`, _id, "; catch; ", aCatch)
				catches.push(_.pick(aCatch, catchAttr))
			})
			.catch((error) => {
				getLogger().error(`catchService; getCatch; Error; _id;`, _id, "; error; ", error)
			})
	})

	return Promise.all(result).then(() => {
		return catches
	})
}

module.exports.getCatch = async (req, res) => {
	const _id = req.params.id

	await Catch.findOne({ _id })
		.then((aCatch) => {
			getLogger().info(`catchService; getCatch; End; _id;`, _id, "; catch; ", aCatch)
			res.send(_.pick(aCatch, catchAttr))
		})
		.catch((error) => {
			getLogger().error(`catchService; getCatch; Error; _id;`, _id, "; error; ", error)
			res.status(404).send(`404 -This catch doesn't exist. ${error}`)
		})
}

module.exports.createCatch = async (req, res) => {
	const aCatch = _.pick(req.body, catchAttr)
	const { error } = validateCatch(aCatch)
	getLogger().info(`catchService; createCatch; Start; catch;`, aCatch, "; error; ", error)
	if (error) return res.status(400).send(error.details[0].message)

	await new Catch(aCatch)
		.save()
		.then((aCatch) => {
			getLogger().info(`catchService; createcatch; End; catch; `, aCatch)
			res.send(_.pick(aCatch, "_id"))
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
	const _id = req.params.id
	const aCatch = _.pick(req.body, catchAttr)

	const { error } = validateCatch(aCatch)
	getLogger().info(
		`catchService; updateCatch; Start; catch; `,
		aCatch,
		"; _id; " + _id,
		"; error; ",
		error
	)
	if (error) return res.status(400).send(error.details[0].message)

	await Catch.findOneAndUpdate({ _id }, aCatch)
		.then((aCatch) => {
			getLogger().info(`catchService; updateCatch; End; catch; `, aCatch, "; _id; " + _id)
			res.send(_.pick(aCatch, "_id"))
		})
		.catch((error) => {
			getLogger().error(
				`catchService; updateCatch; Error; catch;`,
				vatch,
				"; _id; " + _id,
				"; error; ",
				error
			)
			res.status(400).send(`update error ${error}`)
		})
}

module.exports.deleteCatch = async (req, res) => {
	const _id = req.params.id

	await Catch.findOneAndDelete({ _id })
		.then((aCatch) => {
			getLogger().info(`catchService; deleteCatch; End; catch; `, aCatch, "; _id; " + _id)
			res.send(_.pick(aCatch, "_id"))
		})
		.catch((error) => {
			getLogger().error(`catchService; deleteCatch; Error; _id;`, _id, "; error; ", error)
			res.status(400).send(`delete error ${error}`)
		})
}
