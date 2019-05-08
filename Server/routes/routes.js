const express = require("express")
const server = express()

const auth = require("./authRoutes")
const catches = require("./catchRoutes")
const users = require("./userRoutes")
const friends = require("./friendRoutes")
const fishTypes = require("./fishTypeRoutes")
const fishingTrips = require("./fishingTripRoutes")

server.use("/auth", auth)
server.use("/catch", catches)
server.use("/user", users)
server.use("/friends", friends)
server.use("/fishtype", fishTypes)
server.use("/fishingtrip", fishingTrips)

module.exports = server
