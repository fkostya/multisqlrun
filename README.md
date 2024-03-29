# multi sql run

[![.NET Core Desktop](https://github.com/fkostya/multisqlrun/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/fkostya/multisqlrun/actions/workflows/dotnet-desktop.yml) ![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/fkostya/6139c93439673c361cd0aebd8462e298/raw/code-coverage.json) 
[![cleanup old branches](https://github.com/fkostya/multisqlrun/actions/workflows/housekeeping.yml/badge.svg?branch=master)](https://github.com/fkostya/multisqlrun/actions/workflows/housekeeping.yml)

# prototype
### [application capability](https://github.com/fkostya/multisqlrun/blob/master/delivery-process-mapping.md)
<br />
<br />
Run sql query across multiple databases and save result in csv file

To open an issue for this project, please visit the [multisqlrun project issue tracker](https://github.com/fkostya/multisqlrun/issues).

![image](https://github.com/fkostya/multisqlrun/blob/master/appui/assets/catalog.svg)

![image](https://user-images.githubusercontent.com/64334685/192419317-09c501c2-266a-4f82-bc39-7354d23cbf0a.png)

## Roadmap  

## version 5.0
- [ ] bug fixes[technical debt]
## version 4.0
- [ ] migrate windows app to web .core and react[core func]
- [ ] performance improvements[core func]
- [ ] bug fixes[technical debt]
## version 3.1
- [ ] read tenant connections from cvs file[version 3.1][core func]
- [ ] read catalogs from file\database and use it to display on list of catalogs[technical debt]
- [ ] implement full flow with rabbitmq and persist state in database[core func]
- [ ] replace data.sqlclient nuget package to microsoft.data.sqlclient[technical debt]
- [ ] study DAPR architecture
- [ ] add custom log folder[nice to have]
- [ ] bug fixes[technical debt]
## version 3.0 - in development
- [X] review design\architecture to make repository public
- [X] make it public available service
- [X] move configuratino file under %LOCALAPPDATA%\IsolatedStorage\
	- [X] add unit test
- [X] remove logger AppErrorLog
- [X] refactor code(model\BL) layer to support rabbitmq architecture
- [ ] run sql query in 'singlethread' mode - WIP
- [ ] replace sync to AMQP(advanced massage queuing protocol) protocol[core func]:
- [ ] replace sync to AMQP(async massaging queue protocol) protocol[core func] [how to](https://www.rabbitmq.com/documentation.html):
	- [X] add consumer project
	- [X] add rabbitmq dependency [how to install](https://medium.com/geekculture/installing-rabbitmq-on-windows-4411f5114a84)
	- [X] add rabbitmq producer to project:
	- [ ] impelement simple consumers:
		- [ ] consumer to process sql query
		- [ ] consumer to save output to csv file
		- [ ] consumer to write to log file
		- [ ] consumer to test that rabbitmq is online(empty consumer)
	 - [ ] add connectors:
		- [ ] connector to run sql query against ms sql
		- [ ] connector to save sql output to csv file
		- [ ] connector to write log message
		- [ ] empty connector
- [ ] intergare MediatR [how to tutorial](https://medium.com/aeturnuminc/microservices-using-mediatr-on-net-core-3-1-with-exception-handling-c273a7aa4a70)  
- [ ] upgrade to core7.0[technical debt]:
- [ ] add appsettings(PROD|DEV).json[technical debt]:
- [X] Program.cs: move IServiceCollection.Add...() to feed from extension(shared prj)[technical debt]:
- [X] separate tenants by version[core func]: [PR 14](https://github.com/fkostya/multisqlrun/pull/14):
	- [X] change settings via appconfig
	- [X] show tenant names on UI
- [X] create a separate file to sql query[core func][PR 11](https://github.com/fkostya/multisqlrun/pull/11):
	- [X] create file in shared project
	- [X] replace mainform to use new class
	- [X] delete old sqlruncommand file
	- [X] add unit tests
## version 2.0 - MVP
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
## version 1.0 - POC
- [X] POC version
