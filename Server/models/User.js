const mongoose = require("mongoose")
const jwt = require("jsonwebtoken")
const config = require("config")
const Joi = require("joi")

const userSchema = new mongoose.Schema({
	username: {
		type: String,
		required: true,
		unique: true,
		minlength: 3,
		maxlength: 32
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
	friends: {
		type: Array
	},
	isAdmin: Boolean
})

userSchema.methods.generateAuthToken = function() {
	let token = jwt.sign(
		{ _id: this._id, username: this.username, email: this.email, isAdmin: this.isAdmin },
		config.get("jwtPrivateKey")
	)
	return token
}

const User = mongoose.model("User", userSchema)

const userCreationAttr = ["username", "email", "password"]
const userDataAttr = ["_id", "username", "email", "isAdmin", "friends"]

function validateUser(user) {
	const userJoi = {
		username: Joi.string()
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

	return Joi.validate(user, userJoi)
}

module.exports = {
	User,
	userCreationAttr,
	userDataAttr,
	validateUser
}
