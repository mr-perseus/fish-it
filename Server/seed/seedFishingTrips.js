const mongoose = require("mongoose")
const config = require("config")
const fs = require("fs")

const { FishingTrip, validateFishingTrip } = require("../models/FishingTrip")

seedFishingTrips = () => {
	fishingTrips = []

	fs.readFile("seed/fishingTrips.json", async (err, data) => {
		if (err) throw err

		const raw = JSON.parse(data)
		raw.map((fishingTrip) => {
			const { error } = validateFishingTrip(fishingTrip)
			if (error) return res.status(400).send(error.details[0].message)
			fishingTrips.push(fishingTrip)
		})

		mongoose
			.connect(config.db, {
				useNewUrlParser: true,
				useCreateIndex: true
			})
			.then(() => console.info("Connected to MongoDB ..."))
			.catch((error) => console.error("Could not connect to MongoDB ..."))

		await FishingTrip.deleteMany({})
		await FishingTrip.insertMany(fishingTrips)
	})
}

seedFishingTrips()
