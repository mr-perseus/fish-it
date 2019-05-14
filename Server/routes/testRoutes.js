const express = require("express")
const router = express.Router()

const service = require("../services/services")

router.delete("/", service.cleanDb)

module.exports = router
