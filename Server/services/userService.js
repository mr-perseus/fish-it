const mongoose = require("mongoose")
const jwt = require("jsonwebtoken")
const bcrypt = require("bcryptjs")
const config = require("config")
const _ = require("lodash")
const Joi = require("joi")

const userSchema = new mongoose.Schema({
	name: {
		type: String,
		required: true,
		unique: true,
		minlength: 5,
		maxlength: 64
	},
	email: {
		type: String,
		required: true,
		unique: true,
		minlength: 3,
		maxlength: 255
	},
	password: {
		type: String,
		required: true,
		minlength: 5,
		maxlength: 1024
	},
	isAdmin: Boolean
})

userSchema.methods.generateAuthToken = function() {
	let token = jwt.sign(
		{ _id: this._id, name: this.name, email: this.email, isAdmin: this.isAdmin },
		config.get("jwtPrivateKey")
	)
	return token
}

const User = mongoose.model("User", userSchema)

function validateUser(body) {
	user = _.pick(body, ["name", "email", "password"])

	const schema = {
		name: Joi.string()
			.min(5)
			.max(64)
			.required(),
		email: Joi.string()
			.min(6)
			.max(255)
			.required()
			.email(),
		password: Joi.string()
			.min(5)
			.max(255)
			.required()
	}

	return Joi.validate(user, schema)
}

module.exports.getUsers = async function(req, res) {
	await User.find()
		.then((users) => {
			users = users.map((user) => {
				return _.pick(user, ["_id", "name", "email", "isAdmin"])
			})
			res.send(users)
		})
		.catch((error) => {
			res.status(404).send(`No Users found ${error}`)
		})
}

module.exports.getUser = async function(req, res) {
	const _id = req.params.id

	await User.findOne({ _id })
		.then((user) => {
			user = _.pick(user, ["name", "email", "isAdmin"])
			res.send(user)
		})
		.catch((error) => {
			res.status(404).send(`User doesn't exist ${error}`)
		})
}

module.exports.createUser = async function(req, res) {
	const { error } = validateUser(req.body)
	if (error) return res.status(400).send(error.details[0].message)

	const name = await User.findOne({ name: req.body.name })
	const email = await User.findOne({ email: req.body.email })
	const errors = {
		name: name ? "This Username is already registered" : "",
		email: email ? "This Email is already registered" : ""
	}

	if (name || email) return res.status(400).send(errors)

	user = new User(_.pick(req.body, ["name", "email", "password"]))
	const salt = await bcrypt.genSalt(10)
	user.password = await bcrypt.hash(user.password, salt)

	await user
		.save()
		.then((user) => {
			const token = user.generateAuthToken()
			res.header("x-auth-token", token)
				.header("access-control-expose-headers", "x-auth-token")
				.send(_.pick(user, ["_id", "name", "email"]))
		})
		.catch((error) => {
			res.status(500).send(`An unexpected Error occured ${error}`)
		})
}

module.exports.updateUser = async function(req, res) {
	const _id = req.params.id

	const { error } = validateUser(req.body)
	if (error) return res.status(400).send(error.details[0].message)

	let user = _.pick(req.body, ["name", "email", "password"])
	const salt = await bcrypt.genSalt(10)
	user.password = await bcrypt.hash(user.password, salt)

	await User.findOneAndUpdate({ _id }, user)
		.then((user) => {
			user = _.pick(user, ["name", "email", "isAdmin"])
			res.send(user)
		})
		.catch((error) => {
			res.status(404).send(`The selected user doesn't exist ${error}`)
		})
	res.status(500).send(`Updating Users is not yet handled`)
}

module.exports.deleteUser = async function(req, res) {
	const _id = req.params.id

	await User.findOneAndRemove({ _id })
		.then((user) => {
			user = _.pick(user, ["name", "email", "isAdmin"])
			res.send(user)
		})
		.catch((error) => {
			res.status(404).send(`The selected user doesn't exist ${error}`)
		})
}

module.exports.User = User
