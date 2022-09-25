[![.NET Core Desktop](https://github.com/fkostya/multisqlrun/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/fkostya/multisqlrun/actions/workflows/dotnet-desktop.yml) ![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/fkostya/6139c93439673c361cd0aebd8462e298/raw/code-coverage.json) 
[![cleanup old branches](https://github.com/fkostya/multisqlrun/actions/workflows/housekeeping.yml/badge.svg?branch=master)](https://github.com/fkostya/multisqlrun/actions/workflows/housekeeping.yml)



Run sql query across multiple databases and save result in csv file

To open an issue for this project, please visit the [multisqlrun project issue tracker](https://github.com/fkostya/multisqlrun/issues).

## version 3.1
- [ ] read tenant connections from cvs file[version 3.1][core func]
## version 3.0
- [ ] replace sync to AMQP(async massaging queue protocol) protocol[core func]:
	* add consumer project: [DONE]
	* add rabbitmq dependency: [DONE]
		[how to install](https://medium.com/geekculture/installing-rabbitmq-on-windows-4411f5114a84)
	* impelement one consumer:
	* add producer to main project:
- [ ] upgrade to core7.0[technical debt]:
- [ ] Program.cs: move IServiceCollection.Add...() to feed from extension(shared prj)[technical debt]:
- [ ] add appsettings(PROD|DEV).json[technical debt]:
- [ ] separate tenants by version[core func]:
	* change settings
	* show tenant names on UI
- [X] create a separate file to sql query[core func]:
	* create file in shared project
	* replace mainform to use new class
	* delete old sqlruncommand file
	* add unit tests
## version 2.0
- [X] download file with connections
- [X] convert to List
- [ ] and save to local as cvs[future]
- [X] add refresh option
- [X] add dependecy injection
- [X] create unit test project
- [X] create project shared
- [X] move shared code to new shared project
- [X] add appsettings.json+IOptions pattern
- [X] test online web site
## version 1.0
- [X] POC version
