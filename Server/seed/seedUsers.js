const { User } = require("../services/userService")
const mongoose = require("mongoose")
const bcrypt = require("bcryptjs")
const config = require("config")

const users = [
	{
		name: "Maroben",
		email: "maroben@daishen.ch",
		password: config.marobenPw
	},
	{
		name: "Admin",
		email: "admin@hsrstellenboerse.ch",
		password: config.adminPw
	},
	{
		name: "Userino",
		email: "user@hsrstellenboerse.ch",
		password: config.userPw
	}
]

async function seed() {
	mongoose
		.connect(config.db, {
			useNewUrlParser: true,
			useCreateIndex: true
		})
		.then(() => console.info("Connected to MongoDB ..."))
		.catch((error) => {
			console.error("Could not connect to MongoDB ...")
			process.exit(1)
		})

	await User.deleteMany({}).then(() => console.info("Deleted all Users ..."))

	for (let i = 0; i < users.length; i++) {
		let user = new User(users[i])
		const salt = await bcrypt.genSalt(10)
		user.password = await bcrypt.hash(user.password, salt)
		await user
			.save()
			.then((user) => {
				console.info(`User ${user.name} created ...`)
				if (i + 1 >= users.length) {
					console.info("Done")
					process.exit(0)
				}
			})
			.catch((err) => {
				console.error(err)
				process.exit(1)
			})
	}
}

seed()
