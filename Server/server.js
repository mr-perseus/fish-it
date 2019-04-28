const bodyParser = require("body-parser")
const express = require("express")
const server = express()
const config = require("config")
const mongoose = require("mongoose")

const auth = require("./routes/authRoutes")
const catches = require("./routes/catchRoutes")
const users = require("./routes/userRoutes")
const friends = require("./routes/friendRoutes")
const fishTypes = require("./routes/fishTypeRoutes")
const fishingTrips = require("./routes/fishingTripRoutes")

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

server.use("/api/auth", auth)
server.use("/api/catch", catches)
server.use("/api/user", users)
server.use("/api/friends", friends)
server.use("/api/fishtype", fishTypes)
server.use("/api/fishingtrip", fishingTrips)

const port = process.env.PORT || config.port
server.listen(port, (error) => {
	if (error) throw error
	console.info(`ready at port ${port}`)
})

module.exports = server
