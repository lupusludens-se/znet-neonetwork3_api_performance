DECLARE @env varchar(20) = 'preprod'

deleteMoreHealth:
DELETE TOP (10000) Log_Event WHERE Message like 'HTTP "GET" "/neonetwork/' + @env + '/api/health" responded 200%' 
IF @@ROWCOUNT != 0
	GOTO deleteMoreHealth

deleteMoreBadge:
DELETE TOP (10000) Log_Event WHERE Message like 'HTTP "GET" "/neonetwork/' + @env + '/api/users/current/badge-counters" responded 200%'
IF @@ROWCOUNT != 0
	GOTO deleteMoreBadge

deleteMoreCounters:
DELETE TOP (10000) Log_Event WHERE Message like 'HTTP "GET" "/neonetwork/' + @env + '/api/users/current/unseen-counters" responded 200%'
IF @@ROWCOUNT != 0
	GOTO deleteMoreCounters

deleteMoreVerbose:
DELETE TOP (10000) Log_Event WHERE Level = 'Verbose' and DATEADD(day, 2, TimeStamp) = getdate()
IF @@ROWCOUNT != 0
	GOTO deleteMoreVerbose
	
deleteMoreDebug:
DELETE TOP (10000) Log_Event WHERE Level = 'Debug' and DATEADD(day, 2, TimeStamp) = getdate()
IF @@ROWCOUNT != 0
	GOTO deleteMoreDebug
	
deleteMoreInformation:
DELETE TOP (10000) Log_Event WHERE Level = 'Information' and DATEADD(day, 14, TimeStamp) = getdate()
IF @@ROWCOUNT != 0
	GOTO deleteMoreInformation
	
deleteMoreWarn:
DELETE TOP (10000) Log_Event WHERE Level = 'Warn' and DATEADD(day, 30, TimeStamp) = getdate()
IF @@ROWCOUNT != 0
	GOTO deleteMoreWarn
	
deleteMoreError:
DELETE TOP (10000) Log_Event WHERE Level = 'Error' and DATEADD(day, 180, TimeStamp) = getdate()
IF @@ROWCOUNT != 0
	GOTO deleteMoreError
	
deleteMoreFatal:
DELETE TOP (10000) Log_Event WHERE Level = 'Fatal' and DATEADD(day, 180, TimeStamp) = getdate()
IF @@ROWCOUNT != 0
	GOTO deleteMoreFatal
	
	