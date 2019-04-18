const express = require("express")
const router = express.Router()

const service = require("../services/catchService")

router.get("/", service.getCatches)
router.get("/:id", service.getCatch)
router.post("/new", service.createCatch)
router.put("/:id", service.updateCatch)
router.delete("/:id", service.deleteCatch)

module.exports = router
