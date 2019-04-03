const express = require("express")
const router = express.Router()

const service = require("../services/fishingTripService")

router.get("/", service.getFishingTrips)
router.get("/:id", service.getFishingTrip)
router.post("/new", service.createFishingTrip)
router.put("/:id", service.updateFishingTrip)
router.delete("/:id", service.deleteFishingTrip)

module.exports = router
