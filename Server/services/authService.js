const bcrypt = require("bcryptjs")
const Joi = require("joi")
const _ = require("lodash")

const { User } = require("./userService")
const getLogger = require("log4js").getLogger;

function validateAuth(body) {
	const user = _.pick(body, ["email", "password"])

	const schema = {
		email: Joi.string()
			.min(5)
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

module.exports.login = async function(req, res) {
	const { error } = validateAuth(req.body)
	if (error) {
		getLogger().error(`authService; login; Error validating; error;`, error);
		return res.status(400).send(error.details[0].message)
	}

	let user = await User.findOne({ email: req.body.email })
	if (!user) {
		getLogger().error(`authService; login; Error finding user; req.body.email; `, req.body.email);
		return res.status(400).send("Invalid email or password.")
	}

	const validPassword = await bcrypt.compare(req.body.password, user.password)
	if (!validPassword) {
		getLogger().error(`authService; login; Error validPassword`);
		return res.status(400).send("Invalid email or password.")
	}

	const token = user.generateAuthToken()
	getLogger().debug(`authService; login; End successful;`);
	res.send(token)
}
