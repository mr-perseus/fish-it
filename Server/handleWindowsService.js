const Service = require("node-windows").Service

const file = "server.js"
const PATH = process.argv[1]

const dir = PATH.slice(0, PATH.lastIndexOf("\\"))
const filePath = `${dir}\\${file}`

const action = process.argv[2]

const svc = new Service({
	name: "NodeJs HSR Career",
	description: "HSR Stellenboerse",
	script: filePath,
	nodeOptions: ["--harmony", "--max_old_space_size=4096"]
})

svc.on("install", () => {
	console.info("Installing Service ...")
	svc.start()
})

svc.on("uninstall", () => {
	console.info("Removing Service ...")
})

switch (action) {
	case "install":
		svc.install()
		break
	case "remove":
		svc.uninstall()
		break
	default:
		console.error("No arguments were given. Ie 'node handleService.js install'")
		break
}
