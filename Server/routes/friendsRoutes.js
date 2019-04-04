const express = require("express")
const router = express.Router()

const service = require("../services/friendService")

router.get("/:id", service.getFriends)
router.post("/:id/:friendId", service.addFriend)
// router.delete("/:id/:friendId", service.removeFriend)

module.exports = router
