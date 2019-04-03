const bodyParser = require("body-parser")
const express = require("express")
const server = express()
const path = require("path")
const config = require("config")
const mongoose = require("mongoose")

const auth = require("./routes/authRoutes")
const users = require("./routes/usersRoutes")
const companies = require("./routes/companiesRoutes")

// IMPORTANT: set your json web token private key as an environmental variable.
if (!config.get("jwtPrivateKey")) {
	console.error("FATAL ERROR: jwtPrivateKey is not defined.")
	process.exit(1)
}

mongoose
	.connect(config.db, {
		useNewUrlParser: true,
		useCreateIndex: true,
		useFindAndModify: false
	})
	.then(() => console.info("Connected to MongoDB ..."))
	.catch((error) => console.error("Could not connect to MongoDB ..."))

server.disable("x-powered-by")
server.use(bodyParser.json())
server.use(bodyParser.urlencoded({ extended: true }))

if (process.env.NODE_ENV != "production") {
	server.use((req, res, next) => {
		res.setHeader("Access-Control-Allow-Origin", config.corsEndpoint)
		res.setHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE")
		res.setHeader("Access-Control-Allow-Headers", "x-auth-token, content-type")
		next()
	})
}

server.use("/api/auth", auth)
server.use("/api/companies", companies)
server.use("/api/users", users)

server.use(express.static(path.join(__dirname, "..", "client", "build")))

server.use("/*", (req, res) => {
	res.sendFile(path.join(__dirname, "..", "client", "build", "index.html"))
})

const port = process.env.PORT || config.port
server.listen(port, (error) => {
	if (error) throw error
	console.info(`ready at port ${port}`)
})

module.exports = server
