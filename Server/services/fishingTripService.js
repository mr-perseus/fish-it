const _ = require("lodash")
const { FishingTrip, fishingTripAttr, validateFishingTrip } = require("../models/FishingTrip")
const getLogger = require("log4js").getLogger;

module.exports.getFishingTrips = async (req, res) => {
	await FishingTrip.find()
		.then((fishingTrips) => {
			getLogger().info(`getFishingTrips; End; fishingTrips;`, fishingTrips);
			res.send(fishingTrips)
		})
		.catch((error) => {
			getLogger().error(`getFishingTrips; Error; error;`, error);
			res.status(404).send(`404 - No fishing trips were found. ${error}`)
		})
}

module.exports.getFishingTrip = async (req, res) => {
	const _id = req.params.id

	await FishingTrip.findOne({ _id })
		.then((fishingTrip) => {
			getLogger().info(`getFishingTrip; End; _id;`, _id, "; fishingTrip; ", fishingTrip);
			res.send(fishingTrip)
		})
		.catch((error) => {
			getLogger().error(`getFishingTrip; Error; _id;`, _id, "; error; ", error);
			res.status(404).send(`404 -This fishing trip doesn't exist. ${error}`)
		})
}

module.exports.createFishingTrip = async (req, res) => {
	const fishingTrip = _.pick(req.body, fishingTripAttr)
	const { error } = validateFishingTrip(fishingTrip)
	getLogger().info(`createFishingTrip; Start; fishingTrip;`, fishingTrip, "; error; ", error);
	if (error) return res.status(400).send(error.details[0].message)

	await new FishingTrip(fishingTrip)
		.save()
		.then((fishingTrip) => {
			getLogger().info(`createFishingTrip; End; fishingTrip; `, fishingTrip);
			res.send(fishingTrip)
		})
		.catch((error) => {
			getLogger().error(`createFishingTrip; Error; fishingTrip;`, fishingTrip, "; error; ", error);
			res.status(500).send(`undefined behaviour ${error}`)
		})
}

module.exports.updateFishingTrip = async (req, res) => {
	const _id = req.params.id
	const fishingTrip = _.pick(req.body, fishingTripAttr)

	const { error } = validateFishingTrip(fishingTrip)
	getLogger().info(`updateFishingTrip; Start; fishingTrip; `, fishingTrip, "; _id; " + _id, "; error; ", error);
	if (error) return res.status(400).send(error.details[0].message)

	await FishingTrip.findOneAndUpdate({ _id }, fishingTrip)
		.then((fishingTrip) => {
			getLogger().info(`updateFishingTrip; End; fishingTrip; `, fishingTrip, "; _id; " + _id);
			res.send(fishingTrip)
		})
		.catch((error) => {
			getLogger().error(`updateFishingTrip; Error; fishingTrip;`, fishingTrip, "; _id; " + _id, "; error; ", error);
			res.status(400).send(`update error ${error}`)
		})
}

module.exports.deleteFishingTrip = async (req, res) => {
	const _id = req.params.id

	await FishingTrip.findOneAndDelete({ _id })
		.then((fishingTrip) => {
			getLogger().info(`deleteFishingTrip; End; fishingTrip; `, fishingTrip, "; _id; " + _id);
			res.send(fishingTrip)
		})
		.catch((error) => {
			getLogger().error(`deleteFishingTrip; Error; _id;`, _id, "; error; ", error);
			res.status(400).send(`delete error ${error}`)
		})
}
