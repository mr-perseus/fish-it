const log4js = require("log4js")

module.exports = (req, res, next) => {
	log4js.configure({
		appenders: {
			// stdout: {type: "stdout"},
			stderr: {
				type: "stderr",
				layout: {
					type: "pattern",
					pattern: "%[[%d]; [%p]; %c; %z; %h;%] %m;%n"
				}
			},
			errors: {
				type: "file",
				filename: "..\\logs\\error.log",
				maxLogSize: 10485760,
				backups: 10,
				layout: {
					type: "pattern",
					pattern: "[%d]; [%p]; %c; %z; %h; %m;"
				}
			},
			infos: {
				type: "file",
				filename: "..\\logs\\info.log",
				maxLogSize: 10485760,
				backups: 10,
				layout: {
					type: "pattern",
					pattern: "[%d]; [%p]; %c; %z; %h; %m;"
				}
			},
			debugs: {
				type: "file",
				filename: "..\\logs\\debug.log",
				maxLogSize: 10485760,
				backups: 10,
				layout: {
					type: "pattern",
					pattern: "[%d]; [%p]; %c; %z; %h; %m;"
				}
			},
			fileFilterError: { type: "logLevelFilter", appender: "errors", level: "ERROR" },
			fileFilterInfo: { type: "logLevelFilter", appender: "infos", level: "INFO" },
			fileFilterDebug: { type: "logLevelFilter", appender: "debugs", level: "DEBUG" },
			stderrFilter: { type: "logLevelFilter", appender: "stderr", level: "ERROR" }
		},
		categories: {
			default: {
				appenders: ["fileFilterError", "fileFilterInfo", "fileFilterDebug", "stderrFilter"],
				level: "DEBUG"
			}
		}
	})

	next()
}
