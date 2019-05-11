const bcrypt = require("bcryptjs")
const _ = require("lodash")
const { User, userCreationAttr, userDataAttr, validateUser } = require("../models/User")

module.exports.getUsers = async (req, res) => {
	await User.find()
		.then((users) => {
			users = users.map((user) => {
				return _.pick(user, userDataAttr)
			})
			res.send(users)
		})
		.catch((error) => {
			res.status(404).send(`No Users found ${error}`)
		})
}

module.exports.getUser = async (req, res) => {
	const _id = req.params.id

	await User.findOne({ _id })
		.then((user) => {
			user = _.pick(user, userDataAttr)
			res.send(user)
		})
		.catch((error) => {
			res.status(404).send(`User doesn't exist ${error}`)
		})
}

module.exports.getUserById = async (_id) => {
	return await User.findOne({ _id })
		.then((user) => {
			return _.pick(user, userDataAttr)
		})
		.catch((error) => {
			return null
		})
}

module.exports.createUser = async (req, res) => {
	let user = _.pick(req.body, userCreationAttr)

	const { error } = validateUser(user)
	if (error) return res.status(400).send(error.details[0].message)

	const username = await User.findOne({ username: user.username })
	const email = await User.findOne({ email: user.email })
	const errors = {
		username: username ? "This Username is already registered" : "",
		email: email ? "This Email is already registered" : ""
	}

	if (username || email) return res.status(400).send(errors)

	user = new User(user)
	const salt = await bcrypt.genSalt(10)
	user.password = await bcrypt.hash(user.password, salt)

	await user
		.save()
		.then((user) => {
			const token = user.generateAuthToken()
			res.header("x-auth-token", token)
				.header("access-control-expose-headers", "x-auth-token")
				.send(_.pick(user, userDataAttr))
		})
		.catch((error) => {
			res.status(500).send(`An unexpected Error occured ${error}`)
		})
}

module.exports.updateUser = async (req, res) => {
	const _id = req.params.id
	let user = _.pick(req.body, userCreationAttr)

	const { error } = validateUser(user)
	if (error) return res.status(400).send(error.details[0].message)

	const salt = await bcrypt.genSalt(10)
	user.password = await bcrypt.hash(user.password, salt)

	await User.findOneAndUpdate({ _id }, user)
		.then((user) => {
			user = _.pick(user, userDataAttr)
			res.send(user)
		})
		.catch((error) => {
			res.status(404).send(`The selected user doesn't exist ${error}`)
		})
	res.status(500).send(`Updating Users is not yet handled`)
}

module.exports.updateUserById = async (_id, obj) => {
	return await User.findOneAndUpdate({ _id }, obj)
		.then((user) => {
			return _.pick(user, userDataAttr)
		})
		.catch(() => {
			return null
		})
}

module.exports.deleteUser = async (req, res) => {
	const _id = req.params.id

	await User.findOneAndRemove({ _id })
		.then((user) => {
			user = _.pick(user, userDataAttr)
			res.send(user)
		})
		.catch((error) => {
			res.status(404).send(`The selected user doesn't exist ${error}`)
		})
}

module.exports.User = User
