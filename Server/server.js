const bodyParser = require("body-parser")
const express = require("express")
const server = express()
const config = require("config")
const mongoose = require("mongoose")
const log4js = require("log4js");

const auth = require("./routes/authRoutes")
const users = require("./routes/usersRoutes")
const friends = require("./routes/friendsRoutes")
const fishingTrips = require("./routes/fishingTripsRoutes")

log4js.configure({
	appenders: {
		// stdout: {type: "stdout"},
		stderr: {
			type: "stderr",
			layout: {
				type: "pattern",
				pattern: "%[[%d]; [%p]; %c; %z; %h;%] %m;%n",
			},
		},
		errors: {
			type: "file", filename: "..\\logs\\error.log", maxLogSize: 10485760, backups: 10,
			layout: {
				type: "pattern",
				pattern: "[%d]; [%p]; %c; %z; %h; %m;",
			},
		},
		infos: {
			type: "file", filename: "..\\logs\\info.log", maxLogSize: 10485760, backups: 10,
			layout: {
				type: "pattern",
				pattern: "[%d]; [%p]; %c; %z; %h; %m;",
			},
		},
		debugs: {
			type: "file", filename: "..\\logs\\debug.log", maxLogSize: 10485760, backups: 10,
			layout: {
				type: "pattern",
				pattern: "[%d]; [%p]; %c; %z; %h; %m;",
			},
		},
		fileFilterError: { type: "logLevelFilter", appender: "errors", level: "ERROR" },
		fileFilterInfo: { type: "logLevelFilter", appender: "infos", level: "INFO" },
		fileFilterDebug: { type: "logLevelFilter", appender: "debugs", level: "DEBUG" },
		stderrFilter: { type: "logLevelFilter", appender: "stderr", level: "ERROR" },
	},
	categories: {
		default: { appenders: ["fileFilterError", "fileFilterInfo", "fileFilterDebug", "stderrFilter"], level: "DEBUG" },
	},
});

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

server.use("/api/auth", auth)
server.use("/api/users", users)
server.use("/api/friends", friends)
server.use("/api/fishingtrips", fishingTrips)

const port = process.env.PORT || config.port
server.listen(port, (error) => {
	if (error) throw error
	console.info(`ready at port ${port}`)
})

module.exports = server
