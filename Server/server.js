const bodyParser = require("body-parser")
const express = require("express")
const server = express()
const config = require("config")
const mongoose = require("mongoose")

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
	.then(() => console.info("Connected to MongoDB..."))
	.catch((error) => console.error("Could not connect to MongoDB..."))

server.disable("x-powered-by")
server.use(bodyParser.json())
server.use(bodyParser.urlencoded({ extended: true }))

const logger = require("./utils/log")
server.use(logger)

const routes = require("./routes/routes")
server.use(["/api/test", "/api"], routes)

const port = config.port
server.listen(port, (error) => {
	if (error) throw error
	console.info(`ready at port ${port}`)
})

module.exports = server
