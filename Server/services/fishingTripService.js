const _ = require("lodash")
const { FishingTrip, fishingTripAttr, validateFishingTrip } = require("../models/FishingTrip")

module.exports.getFishingTrips = async (req, res) => {
	await FishingTrip.find()
		.then((fishingTrips) => {
			res.send(fishingTrips)
		})
		.catch((error) => {
			res.status(404).send(`404 - No fishing trips were found. ${error}`)
		})
}

module.exports.getFishingTrip = async (req, res) => {
	const _id = req.params.id

	await FishingTrip.findOne({ _id })
		.then((fishingTrip) => {
			res.send(fishingTrip)
		})
		.catch((error) => {
			res.status(404).send(`404 -This fishing trip doesn't exist. ${error}`)
		})
}

module.exports.createFishingTrip = async (req, res) => {
	const fishingTrip = _.pick(req.body, fishingTripAttr)
	const { error } = validateFishingTrip(fishingTrip)
	if (error) return res.status(400).send(error.details[0].message)

	await new FishingTrip(fishingTrip)
		.save()
		.then((fishingTrip) => {
			res.send(fishingTrip)
		})
		.catch((error) => {
			res.status(500).send(`undefined behaviour ${error}`)
		})
}

module.exports.updateFishingTrip = async (req, res) => {
	const _id = req.params.id
	const fishingTrip = _.pick(req.body, fishingTripAttr)

	const { error } = validateFishingTrip(fishingTrip)
	if (error) return res.status(400).send(error.details[0].message)

	await FishingTrip.findOneAndUpdate({ _id }, fishingTrip)
		.then((fishingTrip) => {
			res.send(fishingTrip)
		})
		.catch((error) => {
			res.status(400).send(`update error ${error}`)
		})
}

module.exports.deleteFishingTrip = async (req, res) => {
	const _id = req.params.id

	await FishingTrip.findOneAndDelete({ _id })
		.then((fishingTrip) => {
			res.send(fishingTrip)
		})
		.catch((error) => {
			res.status(400).send(`delete error ${error}`)
		})
}
