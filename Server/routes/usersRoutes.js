const express = require("express")
const router = express.Router()

const service = require("../services/userService")
const auth = require("../utils/auth")
const admin = require("../utils/admin")

router.get("/", [auth, admin], service.getUsers)
router.get("/:id", [auth, admin], service.getUser)
router.post("/register", [auth, admin], service.createUser)
router.put("/:id", [auth, admin], service.updateUser)
router.delete("/:id", [auth, admin], service.deleteUser)

module.exports = router
