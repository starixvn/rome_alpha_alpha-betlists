# escape=`
FROM hub.nexdev.net/base/rocketor/buildservice AS builder

ARG TRANSFORM_CONFIG
ARG ROCKETOR_API
ARG REDMINE_IDS

SHELL ["cmd", "/S", "/C"]

WORKDIR /app

# Copy files *.sln, *.csproj, packages.config to restore nuget packages
COPY *.sln .
COPY src\Alpha.BetLists.Site\*.csproj src\Alpha.BetLists.Site\
COPY src\Alpha.BetLists.Site\packages.config src\Alpha.BetLists.Site\

COPY src\Alpha.BetLists\*.csproj src\Alpha.BetLists\
COPY src\Alpha.BetLists\packages.config src\Alpha.BetLists\

COPY src\Alpha.BetLists.Builders\*.csproj src\Alpha.BetLists.Builders\
COPY src\Alpha.BetLists.Builders\packages.config src\Alpha.BetLists.Builders\

COPY src\Alpha.BetLists.StaticData\*.csproj src\Alpha.BetLists.StaticData\
COPY src\Alpha.BetLists.StaticData\packages.config src\Alpha.BetLists.StaticData\

RUN nuget.exe restore Alpha.BetLists.sln

COPY src src\
COPY SonarQube SonarQube\

RUN MSBuild.exe "src\Alpha.BetLists.Site\Alpha.BetLists.Site.csproj" `
    /t:Build;WebPublish /m `
    /p:DebugType=None `
    /p:Configuration=Release `
    /p:WebPublishMethod=FileSystem `
    /p:TransformWebConfigEnabled=False `
    /p:LocalizedBuild=true `
    /p:AutoParameterizationWebConfigConnectionStrings=False `
    /p:MarkWebConfigAssistFilesAsExclude=false `
    /p:publishUrl="C:\Compiled" `
    /consoleloggerparameters:ErrorsOnly

WORKDIR C:\Compiled

# Change SP Version
RUN `
    if defined REDMINE_IDS `
    FOR %P IN ( `
        App_Data\SPNames\Docker\DBACC.bodb02.BODBDownlineNet.xml, `		
		App_Data\SPNames\Docker\DBAGE.bodb_AGE.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBAGE.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBARC.bodb_Archive.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBCust.CustDB.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBCust.IPDB.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBLOG.bodb_CustomerLog.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBONUSER.ON_USER.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBPOLICY.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Internal\DBACC.bodb02.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBAGE.bodb_AGE.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBAGE.bodb02.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBARC.bodb_Archive.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBCust.CustDB.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBCust.IPDB.BODBDownlineNet.xml, `
		App_Data\SPNames\Internal\DBLOG.bodb_CustomerLog.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBONUSER.ON_USER.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBPOLICY.bodb02.BODBDownlineNetView.xml `
    ) `
    DO curl --fail -o C:\sp-result.xml --form file=@"%P" --form "redmineIds=%REDMINE_IDS%" -X POST %ROCKETOR_API% `
        && xcopy C:\sp-result.xml "%P" /Y

RUN FOR %P IN ( `
		LOCAL2, `
		SIT2, `
		UAT2, `
		PRO2, `
		backupMNLNap2, `
		backupMNL2, `
		napinternal2 `
    ) `
    DO ( `
        if "%P" == "%TRANSFORM_CONFIG%"; `
		echo "Copying files..." && xcopy "assets/styles/bet-list-2.css" "assets/styles/bet-list.css" /Y `
		&& xcopy "assets/styles/bet-list-2.min.css" "assets/styles/bet-list.min.css" /Y `
    )

# Transform config
RUN echo "%TRANSFORM_CONFIG%" `
    && xcopy "Web.%TRANSFORM_CONFIG%.config" "Web.config" /Y `
    && xcopy "Web.ConnectionStrings.%TRANSFORM_CONFIG%.config" "Web.ConnectionStrings.config" /Y `
	&& xcopy "Web.AppSettings.%TRANSFORM_CONFIG%.config" "Web.AppSettings.config" /Y `
	&& xcopy "App_Data\activitytracking.%TRANSFORM_CONFIG%.json" "App_Data\activitytracking.json" /Y `
	&& xcopy "App_Data\securitycode.%TRANSFORM_CONFIG%.json" "App_Data\securitycode.json" /Y `
	&& xcopy "App_Data\ProductSection.%TRANSFORM_CONFIG%.config" "App_Data\ProductSection.config" /Y

FROM hub.nexdev.net/base/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019 AS base

WORKDIR C:\inetpub\wwwroot

COPY --from=builder C:\Compiled .

RUN dir .\bin\

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]