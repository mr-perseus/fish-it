const express = require("express")
const router = express.Router()

const service = require("../services/fishTypeService")

router.get("/", service.getFishTypes)
router.get("/:id", service.getFishType)
router.post("/new", service.createFishType)
router.put("/:id", service.updateFishType)
router.delete("/:id", service.deleteFishType)

module.exports = router
