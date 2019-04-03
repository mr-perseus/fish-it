const express = require("express")
const router = express.Router()

const service = require("../services/authService")

router.post("/", service.login)

module.exports = router
