const { Company } = require("../services/companyService")
const mongoose = require("mongoose")
const config = require("config")
const csv = require("csv-parser")
const fs = require("fs")

const subjects = [
	"Bauingenieurwesen",
	"Elektrotechnik",
	"Erneuerbare Energien & Umwelttechnik",
	"Informatik",
	"Landschaftsarchitektur",
	"Maschinentechnik & Innovation",
	"Raumplanung",
	"Wirtschaftsingenieurwesen"
]
const employment = ["Internship", "Trainee", "Full-Time"]
const links = ["homepage", "linkedin", "xing", "facebook", "twitter", "instagram", "youtube"]
const tags = ["tag_0", "tag_1", "tag_2", "tag_3", "tag_4"]

const headers = ["name", "location", "info", "description"]
	.concat(links)
	.concat(subjects)
	.concat(employment)
	.concat(tags)

getCompany = (data) => {
	let dataSubjects = []
	for (let i = 0; i < subjects.length; i++) {
		if (data[subjects[i]]) {
			dataSubjects.push(subjects[i])
		}
	}

	let dataEmployment = []
	for (let i = 0; i < employment.length; i++) {
		if (data[employment[i]]) {
			dataEmployment.push(employment[i])
		}
	}

	let dataLinks = {}
	for (let i = 0; i < links.length; i++) {
		if (data[links[i]] != null) {
			dataLinks[links[i]] = data[links[i]]
		}
	}

	let dataTags = []
	for (let i = 0; i < tags.length; i++) {
		if (data[tags[i]].length > 0) {
			dataTags.push(data[tags[i]])
		}
	}

	let company = {
		name: data.name,
		location: data.location,
		info: data.info,
		description: data.description,
		subjects: JSON.stringify(dataSubjects),
		employment: JSON.stringify(dataEmployment),
		tags: JSON.stringify(dataTags),
		links: JSON.stringify(dataLinks)
	}

	return company
}

seedCompaniesCSV = () => {
	let companies = []

	fs.createReadStream("seed/data6.csv")
		.pipe(csv(headers))
		.on("data", (data) => {
			companies.push(getCompany(data))
		})
		.on("end", async () => {
			mongoose
				.connect(config.db, {
					useNewUrlParser: true,
					useCreateIndex: true
				})
				.then(() => console.info("Connected to MongoDB ..."))
				.catch((error) => console.error("Could not connect to MongoDB ..."))

			await Company.deleteMany({})
			await Company.insertMany(companies)

			mongoose.disconnect()
			console.info("Done!")
		})
}

seedCompaniesJSON = () => {
	let companies = []

	fs.readFile("seed/data.json", async (err, data) => {
		if (err) throw err

		let parsed = JSON.parse(data)
		console.log(typeof parsed)
		for (let i = 0; i < parsed.length; i++) {
			companies.push(getCompany(parsed[i]))
		}

		mongoose
			.connect(config.db, {
				useNewUrlParser: true,
				useCreateIndex: true
			})
			.then(() => console.info("Connected to MongoDB ..."))
			.catch((error) => console.error("Could not connect to MongoDB ..."))

		await Company.deleteMany({})
		await Company.insertMany(companies)
	})
}

seedCompaniesJSON()
