const _ = require("lodash")
const { User, userCreationAttr, userDataAttr, validateUser } = require("../models/User")
const userService = require("./userService")
const getLogger = require("log4js").getLogger;

module.exports.getFriends = async (req, res) => {
	const _id = req.params.id
	const user = await userService.getUserById(_id)
	getLogger().info(`friendService; getFriends; Start; _id; `, _id, "; user; " + user);
	if (!user) {
		getLogger().error(`friendService; getFriends; Error finding user; _id; `, _id);
		res.status(404).send("User not found")
	}

	let friends = []
	const pushing = await user.friends.map(async (friendId) => {
		friend = await userService.getUserById(friendId)
		if (!friend) {
			getLogger().error(`friendService; getFriends; Error finding friend; friendId; `, friendId);
			res.status(404).send("Friend not found")
		}
		friends.push(friend)
	})

	Promise.all(pushing).then(() => {
		getLogger().info(`friendService; getFriends; End; _id; `, _id, "; user; " + user, "; friends; ", friends);
		res.send(friends)
	})
}

module.exports.addFriend = async (req, res) => {
	const _id = req.params.id
	const user = await userService.getUserById(_id)
	getLogger().info(`friendService; addFriend; Start; _id; `, _id, "; user; " + user);
	if (!user) {
		getLogger().error(`friendService; addFriend; Error finding user; _id; `, _id);
		return res.status(404).send("User not found")
	}

	const friendId = req.params.friendId
	const friend = await userService.getUserById(friendId)
	if (!friend) {
		getLogger().error(`friendService; addFriend; Error finding friend; friendId; `, friendId);
		return res.status(404).send("Friend not found")
	}

	if (user.friends.indexOf(friendId) > -1) return res.status(400).send(`Friend already added`)
	user.friends.push(friendId)

	const updatedUser = await userService.updateUserById(_id, { $set: { friends: user.friends } })
	if (!updatedUser) {
		getLogger().error(`friendService; addFriend; Error Updating user; _id; `, _id, "; user; ", user);
		return res.status(500).send("Update error")
	}

	getLogger().info(`friendService; addFriend; End; _id; `, _id, "; user; " + user, "; updatedUser; ", updatedUser);
	res.send(updatedUser)
}

module.exports.removeFriend = async (req, res) => {
	const _id = req.params.id
	const user = await userService.getUserById(_id)
	getLogger().info(`friendService; removeFriend; Start; _id; `, _id, "; user; " + user);
	if (!user) {
		getLogger().error(`friendService; removeFriend; Error finding user; _id; `, _id);
		return res.status(404).send("User not found")
	}

	const friendId = req.params.friendId
	const friend = await userService.getUserById(friendId)
	if (!friend) {
		getLogger().error(`friendService; removeFriend; Error finding friend; friendId; `, friendId);
		return res.status(404).send("Friend not found")
	}

	if (user.friends.indexOf(friendId) === -1) return res.status(400).send(`Friend already removed`)
	user.friends = user.friends.filter((id) => friendId != id)

	const updatedUser = await userService.updateUserById(_id, { $set: { friends: user.friends } })
	if (!updatedUser) {
		getLogger().error(`friendService; removeFriend; Error Updating user; _id; `, _id, "; user; ", user);
		return res.status(500).send("Update error")
	}

	getLogger().info(`friendService; removeFriend; End; _id; `, _id, "; user; " + user, "; updatedUser; ", updatedUser);
	res.send(updatedUser)
}
