const _ = require("lodash")
const FishingTrip = require("../models/FishingTrip")
const { isTest } = require("./services")
const getLogger = require("log4js").getLogger

module.exports.getFishingTrips = async (req, res) => {
	const Model = isTest(req) ? FishingTrip.ModelTest : FishingTrip.Model

	await Model.find()
		.populate({ path: "Catches", populate: { path: "FishType" } })
		.then((fishingTrips) => {
			getLogger().info(
				`fishingTripService; getFishingTrips; End; fishingTrips;`,
				fishingTrips
			)
			res.send(fishingTrips.map((fishingTrip) => _.pick(fishingTrip, FishingTrip.attr)))
		})
		.catch((error) => {
			getLogger().error(`fishingTripService; getFishingTrips; Error; error;`, error)
			res.status(404).send(`404 - No fishing trips were found. ${error}`)
		})
}

module.exports.getFishingTrip = async (req, res) => {
	const Model = isTest(req) ? FishingTrip.ModelTest : FishingTrip.Model
	const _id = req.params.id

	await Model.findOne({ _id })
		.populate({ path: "Catches", populate: { path: "FishType" } })
		.then((fishingTrip) => {
			getLogger().info(
				`fishingTripService; getFishingTrip; End; _id;`,
				_id,
				"; fishingTrip; ",
				fishingTrip
			)
			res.send(_.pick(fishingTrip, FishingTrip.attr))
		})
		.catch((error) => {
			getLogger().error(
				`fishingTripService; getFishingTrip; Error; _id;`,
				_id,
				"; error; ",
				error
			)
			res.status(404).send(`404 -This fishing trip doesn't exist. ${error}`)
		})
}

module.exports.createFishingTrip = async (req, res) => {
	const Model = isTest(req) ? FishingTrip.ModelTest : FishingTrip.Model
	const fishingTrip = _.pick(req.body, FishingTrip.attrNoId)

	const { error } = FishingTrip.validate(fishingTrip)
	getLogger().info(
		`fishingTripService; createFishingTrip; Start; fishingTrip;`,
		fishingTrip,
		"; error; ",
		error
	)
	if (error) return res.status(400).send(error.details[0].message)

	await new Model(fishingTrip)
		.save()
		.then((fishingTrip) => {
			Model.populate(
				fishingTrip,
				{ path: "Catches", populate: { path: "FishType" } },
				(err, fT) => {
					getLogger().info(
						`fishingTripService; createFishingTrip; End; fishingTrip; `,
						fT
					)
					res.send(_.pick(fT, FishingTrip.attr))
				}
			)
		})
		.catch((error) => {
			getLogger().error(
				`fishingTripService; createFishingTrip; Error; fishingTrip;`,
				fishingTrip,
				"; error; ",
				error
			)
			res.status(500).send(`undefined behaviour ${error}`)
		})
}

module.exports.updateFishingTrip = async (req, res) => {
	const Model = isTest(req) ? FishingTrip.ModelTest : FishingTrip.Model
	const fishingTrip = _.pick(req.body, FishingTrip.attrNoId)
	const _id = req.params.id

	const { error } = FishingTrip.validate(fishingTrip)
	getLogger().info(
		`fishingTripService; updateFishingTrip; Start; fishingTrip; `,
		fishingTrip,
		"; _id; " + _id,
		"; error; ",
		error
	)
	if (error) return res.status(400).send(error.details[0].message)

	await Model.findOneAndUpdate({ _id }, fishingTrip, { new: true })
		.populate({ path: "Catches", populate: { path: "FishType" } })
		.then((fishingTrip) => {
			getLogger().info(
				`fishingTripService; updateFishingTrip; End; fishingTrip; `,
				fishingTrip,
				"; _id; " + _id
			)
			res.send(_.pick(fishingTrip, FishingTrip.attr))
		})
		.catch((error) => {
			getLogger().error(
				`fishingTripService; updateFishingTrip; Error; fishingTrip;`,
				fishingTrip,
				"; _id; " + _id,
				"; error; ",
				error
			)
			res.status(400).send(`update error ${error}`)
		})
}

module.exports.deleteFishingTrip = async (req, res) => {
	const Model = isTest(req) ? FishingTrip.ModelTest : FishingTrip.Model
	const _id = req.params.id

	await Model.findOneAndDelete({ _id })
		.populate({ path: "Catches", populate: { path: "FishType" } })
		.then((fishingTrip) => {
			getLogger().info(
				`fishingTripService; deleteFishingTrip; End; fishingTrip; `,
				fishingTrip,
				"; _id; " + _id
			)
			res.send(_.pick(fishingTrip, FishingTrip.attr))
		})
		.catch((error) => {
			getLogger().error(
				`fishingTripService; deleteFishingTrip; Error; _id;`,
				_id,
				"; error; ",
				error
			)
			res.status(400).send(`delete error ${error}`)
		})
}
