const _ = require("lodash")
const { User, userCreationAttr, userDataAttr, validateUser } = require("../models/User")
const userService = require("./userService")

module.exports.getFriends = async (req, res) => {
	const _id = req.params.id
	const user = await userService.getUserById(_id)
	if (!user) res.status(404).send("User not found")

	let friends = []
	const pushing = await user.friends.map(async (friendId) => {
		friend = await userService.getUserById(friendId)
		if (!friend) res.status(404).send("Friend not found")
		friends.push(friend)
	})

	Promise.all(pushing).then(() => {
		res.send(friends)
	})
}

module.exports.addFriend = async (req, res) => {
	const _id = req.params.id
	const user = await userService.getUserById(_id)
	if (!user) return res.status(404).send("User not found")

	const friendId = req.params.friendId
	const friend = await userService.getUserById(friendId)
	if (!friend) return res.status(404).send("Friend not found")

	if (user.friends.indexOf(friendId) > -1) return res.status(400).send(`Friend already added`)
	user.friends.push(friendId)

	const updatedUser = await userService.updateUserById(_id, { $set: { friends: user.friends } })
	if (!updatedUser) return res.status(500).send("Update error")
	res.send(updatedUser)
}

module.exports.removeFriend = async (req, res) => {
	const _id = req.params.id
	const user = await userService.getUserById(_id)
	if (!user) return res.status(404).send("User not found")

	const friendId = req.params.friendId
	const friend = await userService.getUserById(friendId)
	if (!friend) return res.status(404).send("Friend not found")

	if (user.friends.indexOf(friendId) === -1) return res.status(400).send(`Friend already removed`)
	user.friends = user.friends.filter((id) => friendId != id)

	const updatedUser = await userService.updateUserById(_id, { $set: { friends: user.friends } })
	if (!updatedUser) return res.status(500).send("Update error")
	res.send(updatedUser)
}
