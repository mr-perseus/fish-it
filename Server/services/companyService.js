const _ = require("lodash")
const Joi = require("joi")
const mongoose = require("mongoose")

const companySchema = new mongoose.Schema({
	name: {
		type: String,
		required: true,
		minlength: 3,
		maxlength: 64
	},
	location: {
		type: String,
		required: true,
		maxlength: 64
	},
	info: {
		type: String,
		maxlength: 512
	},
	description: {
		type: String
	},
	subjects: {
		type: String
	},
	employment: {
		type: String
	},
	tags: {
		type: String
	},
	links: {
		type: String
	}
})

const Company = mongoose.model("Companies", companySchema)

const companyAttr = [
	"name",
	"location",
	"info",
	"description",
	"subjects",
	"employment",
	"links",
	"tags"
]

function validateCompany(body) {
	const schema = {
		name: Joi.string()
			.min(3)
			.max(64)
			.required(),
		location: Joi.string()
			.max(18)
			.required(),
		info: Joi.string().max(512),
		description: Joi.string().min(5),
		subjects: Joi.string(),
		employment: Joi.string(),
		tags: Joi.string(),
		links: Joi.object()
	}

	return Joi.validate(body, schema)
}

module.exports.getCompanies = async function(req, res) {
	await Company.find()
		.then((companies) => {
			companies = companies.map((company) => {
				return _.pick(company, ["_id", ...companyAttr])
			})
			res.send(companies)
		})
		.catch((error) => {
			res.status(404).send(`No companies were found ${error}`)
		})
}

module.exports.getCompany = async function(req, res) {
	const _id = req.params.id

	await Company.findOne({ _id })
		.then((company) => {
			company = _.pick(company, ["_id", ...companyAttr])
			res.send(company)
		})
		.catch((error) => {
			res.status(404).send(`This company doesn't exist ${error}`)
		})
}

module.exports.createCompany = async function(req, res) {
	const { error } = validateCompany(req.body)
	if (error) return res.status(400).send(error.details[0].message)

	let company = await Company.findOne({ name: req.body.name })
	if (company) return res.status(400).send("This company name has already been used.")

	await new Company(_.pick(req.body, companyAttr))
		.save()
		.then((company) => {
			res.send(company)
		})
		.catch((error) => {
			res.status(500).send(`unkown`)
		})
}

module.exports.updateCompany = async function(req, res) {
	const _id = req.params.id

	const { error } = validateCompany(req.body)
	if (error) return res.status(400).send(error.details[0].message)

	await Company.findOneAndUpdate({ _id }, _.pick(req.body, companyAttr))
		.then((company) => {
			res.send(company)
		})
		.catch((error) => {
			res.status(404).send(`Could not update company ${error}`)
		})
}

module.exports.deleteCompany = async function(req, res) {
	const _id = req.params.id

	await Company.findOneAndDelete({ _id })
		.then((company) => {
			res.send(company)
		})
		.catch((error) => {
			res.status(400).send(`Could not delete Company ${error}`)
		})
}

module.exports.Company = Company
