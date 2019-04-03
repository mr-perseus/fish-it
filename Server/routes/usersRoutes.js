const express = require("express")
const router = express.Router()

const service = require("../services/userService")

router.get("/", service.getUsers)
router.get("/:id", service.getUser)
router.post("/register", service.createUser)
router.put("/:id", service.updateUser)
router.delete("/:id", service.deleteUser)

module.exports = router
