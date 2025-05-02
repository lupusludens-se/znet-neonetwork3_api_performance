set identity_insert [Time_Zone] on;


insert [Time_Zone] ([Time_Zone_Id],[Display_Name],[Created_Ts],[Created_User_Id],[Last_Change_Ts],[Updated_User_Id],[Abbreviation],[Has_DST],[Standard_Name],[UTC_Offset],[Daylight_Abbreviation],[Daylight_Name],[Windows_Name])
select 1,N'(UTC-12:00) International Date Line West',NULL,NULL,NULL,NULL,N'DST',0,N'Dateline Standard Time',-12,NULL,NULL,N'Dateline Standard Time' UNION ALL
select 2,N'(UTC-11:00) Coordinated Universal Time-11',NULL,NULL,NULL,NULL,N'UTC-11',0,N'UTC-11',-11,NULL,NULL,N'UTC-11' UNION ALL
select 3,N'(UTC-10:00) Aleutian Islands',NULL,NULL,NULL,NULL,N'ALST',1,N'Aleutian Standard Time',-10,N'ALDT',N'Aleutian Daylight Time',N'Aleutian Standard Time' UNION ALL
select 4,N'(UTC-10:00) Hawaii',NULL,NULL,NULL,NULL,N'HAST',0,N'Hawaiian Standard Time',-10,NULL,NULL,N'Hawaiian Standard Time' UNION ALL
select 5,N'(UTC-09:30) Marquesas Islands',NULL,NULL,NULL,NULL,N'MIT',0,N'Marquesas Standard Time',-9.5,NULL,NULL,N'Marquesas Standard Time' UNION ALL
select 6,N'(UTC-09:00) Alaska',NULL,NULL,NULL,NULL,N'AKST',1,N'Alaskan Standard Time',-9,N'AKDT',N'Alaska Daylight Time',N'Alaskan Standard Time' UNION ALL
select 7,N'(UTC-09:00) Coordinated Universal Time-09',NULL,NULL,NULL,NULL,N'UTC-09',0,N'UTC-09',-9,NULL,NULL,N'UTC-09' UNION ALL
select 8,N'(UTC-08:00) Baja California',NULL,NULL,NULL,NULL,N'PSTM',1,N'Pacific Standard Time (Mexico)',-8,N'PDTM',N'Pacific Daylight Time (Mexico)',N'Pacific Standard Time (Mexico)' UNION ALL
select 9,N'(UTC-08:00) Coordinated Universal Time-08',NULL,NULL,NULL,NULL,N'UTC-08',0,N'UTC-08',-8,NULL,NULL,N'UTC-08' UNION ALL
select 10,N'(UTC-08:00) Pacific Time (US & Canada)',NULL,NULL,NULL,NULL,N'PST',1,N'Pacific Standard Time',-8,N'PDT',N'Pacific Daylight Time',N'Pacific Standard Time' UNION ALL
select 11,N'(UTC-07:00) Arizona',NULL,NULL,NULL,NULL,N'USMST',0,N'US Mountain Standard Time',-7,NULL,NULL,N'US Mountain Standard Time' UNION ALL
select 12,N'(UTC-07:00) Chihuahua, La Paz, Mazatlan',NULL,NULL,NULL,NULL,N'MSTM',1,N'Mountain Standard Time (Mexico)',-7,N'MDTM',N'Mountain Daylight Time (Mexico)',N'Mountain Standard Time (Mexico)' UNION ALL
select 13,N'(UTC-07:00) Mountain Time (US & Canada)',NULL,NULL,NULL,NULL,N'MST',1,N'Mountain Standard Time',-7,N'MDT',N'Mountain Daylight Time',N'Mountain Standard Time' UNION ALL
select 14,N'(UTC-07:00) Yukon',NULL,NULL,NULL,NULL,N'YST',1,N'Yukon Standard Time',-7,NULL,NULL,N'Yukon Standard Time' UNION ALL
select 15,N'(UTC-06:00) Central America',NULL,NULL,NULL,NULL,N'CAST',0,N'Central America Standard Time',-6,NULL,NULL,N'Central America Standard Time' UNION ALL
select 16,N'(UTC-06:00) Central Time (US & Canada)',NULL,NULL,NULL,NULL,N'CST',1,N'Central Standard Time',-6,N'CDT',N'Central Daylight Time',N'Central Standard Time' UNION ALL
select 17,N'(UTC-06:00) Easter Island',NULL,NULL,NULL,NULL,N'EAST',1,N'Easter Island Standard Time',-6,N'EASST',N'Easter Island Summer Time',N'Easter Island Standard Time' UNION ALL
select 18,N'(UTC-06:00) Guadalajara, Mexico City, Monterrey',NULL,NULL,NULL,NULL,N'CSTM',1,N'Central Standard Time (Mexico)',-6,N'CDTM',N'Central Daylight Time (Mexico)',N'Central Standard Time (Mexico)' UNION ALL
select 19,N'(UTC-06:00) Saskatchewan',NULL,NULL,NULL,NULL,N'SSK',0,N'Canada Central Standard Time',-6,NULL,NULL,N'Canada Central Standard Time' UNION ALL
select 20,N'(UTC-05:00) Bogota, Lima, Quito, Rio Branco',NULL,NULL,NULL,NULL,N'SAPST',0,N'SA Pacific Standard Time',-5,NULL,NULL,N'SA Pacific Standard Time' UNION ALL
select 21,N'(UTC-05:00) Chetumal',NULL,NULL,NULL,NULL,N'ESTM',1,N'Eastern Standard Time (Mexico)',-5,N'EDTM',N'Eastern Daylight Time (Mexico)',N'Eastern Standard Time (Mexico)' UNION ALL
select 22,N'(UTC-05:00) Eastern Time (US & Canada)',NULL,NULL,NULL,NULL,N'EST',1,N'Eastern Standard Time',-5,N'EDT',N'Eastern Daylight Time',N'Eastern Standard Time' UNION ALL
select 23,N'(UTC-05:00) Haiti',NULL,NULL,NULL,NULL,N'HST',1,N'Haiti Standard Time',-5,N'HDT',N'Haiti Daylight Time',N'Haiti Standard Time' UNION ALL
select 24,N'(UTC-05:00) Havana',NULL,NULL,NULL,NULL,N'CUST',1,N'Cuba Standard Time',-5,N'CUDT',N'Cuba Daylight Time',N'Cuba Standard Time' UNION ALL
select 25,N'(UTC-05:00) Indiana (East)',NULL,NULL,NULL,NULL,N'USEST',1,N'US Eastern Standard Time',-5,N'USEDT',N'Eastern Daylight Time',N'US Eastern Standard Time' UNION ALL
select 26,N'(UTC-05:00) Turks and Caicos',NULL,NULL,NULL,NULL,N'TCST',1,N'Turks and Caicos Standard Time',-5,N'TCDT',N'Turks and Caicos Daylight Time',N'Turks And Caicos Standard Time' UNION ALL
select 27,N'(UTC-04:00) Asuncion',NULL,NULL,NULL,NULL,N'PAST',1,N'Paraguay Standard Time',-4,N'PST',N'Paraguay Summer Time',N'Paraguay Standard Time' UNION ALL
select 28,N'(UTC-04:00) Atlantic Time (Canada)',NULL,NULL,NULL,NULL,N'AST',1,N'Atlantic Standard Time',-4,N'ADT',N'Atlantic Daylight Time',N'Atlantic Standard Time' UNION ALL
select 29,N'(UTC-04:00) Caracas',NULL,NULL,NULL,NULL,N'VET',1,N'Venezuela Standard Time',-4,NULL,NULL,N'Venezuela Standard Time' UNION ALL
select 30,N'(UTC-04:00) Cuiaba',NULL,NULL,NULL,NULL,N'CBST',1,N'Central Brazilian Standard Time',-4,NULL,NULL,N'Central Brazilian Standard Time' UNION ALL
select 31,N'(UTC-04:00) Georgetown, La Paz, Manaus, San Juan',NULL,NULL,NULL,NULL,N'SAWST',0,N'SA Western Standard Time',-4,NULL,NULL,N'SA Western Standard Time' UNION ALL
select 32,N'(UTC-04:00) Santiago',NULL,NULL,NULL,NULL,N'PSA',1,N'Pacific SA Standard Time',-4,N'CST',N'Chile Summer Time',N'Pacific SA Standard Time' UNION ALL
select 33,N'(UTC-03:30) Newfoundland',NULL,NULL,NULL,NULL,N'NLT',1,N'Newfoundland Standard Time',-3.5,N'NLDT',N'Newfoundland Standard Time',N'Newfoundland Standard Time' UNION ALL
select 34,N'(UTC-03:00) Araguaina',NULL,NULL,NULL,NULL,N'TST',1,N'Tocantins Standard Time',-3,NULL,NULL,N'Tocantins Standard Time' UNION ALL
select 35,N'(UTC-03:00) Brasilia',NULL,NULL,NULL,NULL,N'ESAST',1,N'E. South America Standard Time',-3,NULL,NULL,N'E. South America Standard Time' UNION ALL
select 36,N'(UTC-03:00) Cayenne, Fortaleza',NULL,NULL,NULL,NULL,N'SAEST',0,N'SA Eastern Standard Time',-3,NULL,NULL,N'SA Eastern Standard Time' UNION ALL
select 37,N'(UTC-03:00) City of Buenos Aires',NULL,NULL,NULL,NULL,N'ART',1,N'Argentina Standard Time',-3,NULL,NULL,N'Argentina Standard Time' UNION ALL
select 38,N'(UTC-03:00) Greenland',NULL,NULL,NULL,NULL,N'CGT',1,N'Greenland Standard Time',-3,N'WGST',N'West Greenland Summer Time',N'Greenland Standard Time' UNION ALL
select 39,N'(UTC-03:00) Montevideo',NULL,NULL,NULL,NULL,N'MVD',1,N'Montevideo Standard Time',-3,NULL,NULL,N'Montevideo Standard Time' UNION ALL
select 40,N'(UTC-03:00) Punta Arenas',NULL,NULL,NULL,NULL,N'PAST',1,N'Magallanes Standard Time',-3,NULL,NULL,N'Magallanes Standard Time' UNION ALL
select 41,N'(UTC-03:00) Saint Pierre and Miquelon',NULL,NULL,NULL,NULL,N'PMST',1,N'Saint Pierre Standard Time',-3,N'PMDT',N'Saint Pierre Daylight Time',N'Saint Pierre Standard Time' UNION ALL
select 42,N'(UTC-03:00) Salvador',NULL,NULL,NULL,NULL,N'BAST',1,N'Bahia Standard Time',-3,NULL,NULL,N'Bahia Standard Time' UNION ALL
select 43,N'(UTC-02:00) Coordinated Universal Time-02',NULL,NULL,NULL,NULL,N'UTC-02',0,N'UTC-02',-2,NULL,NULL,N'UTC-02' UNION ALL
select 45,N'(UTC-01:00) Azores',NULL,NULL,NULL,NULL,N'AZO',1,N'Azores Standard Time',-1,N'AZOST',N'Azores Summer Time',N'Cape Verde Standard Time' UNION ALL
select 46,N'(UTC-01:00) Cabo Verde Is.',NULL,NULL,NULL,NULL,N'CVST',0,N'Cabo Verde Standard Time',-1,NULL,NULL,N'Cabo Verde Standard Time' UNION ALL
select 47,N'(UTC) Coordinated Universal Time',NULL,NULL,NULL,NULL,N'UTC',0,N'Coordinated Universal Time',0,NULL,NULL,N'UTC' UNION ALL
select 48,N'(UTC+00:00) Dublin, Edinburgh, Lisbon, London',NULL,NULL,NULL,NULL,N'GMT',1,N'GMT Standard Time',0,N'BST',N'British Summer Time',N'GMT Standard Time' UNION ALL
select 49,N'(UTC+00:00) Monrovia, Reykjavik',NULL,NULL,NULL,NULL,N'GST',0,N'Greenwich Standard Time',0,NULL,NULL,N'Greenwich Standard Time' UNION ALL
select 50,N'(UTC+00:00) Sao Tome',NULL,NULL,NULL,NULL,N'STST',1,N'Sao Tome Standard Time',0,NULL,NULL,N'Sao Tome Standard Time' UNION ALL
select 51,N'(UTC+01:00) Casablanca',NULL,NULL,NULL,NULL,N'MRC',1,N'Morocco Standard Time',0,N'MRCDT',N'Morocco Daylight Time',N'Morocco Standard Time' UNION ALL
select 52,N'(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna',NULL,NULL,NULL,NULL,N'WET',1,N'W. Europe Standard Time',1,N'CEST',N'Central European Summer Time',N'W. Europe Standard Time' UNION ALL
select 53,N'(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague',NULL,NULL,NULL,NULL,N'CET',1,N'Central Europe Standard Time',1,N'CEST',N'Central European Summer Time',N'Central Europe Standard Time' UNION ALL
select 54,N'(UTC+01:00) Brussels, Copenhagen, Madrid, Paris',NULL,NULL,NULL,NULL,N'ROM',1,N'Romance Standard Time',1,N'CEST',N'Central European Summer Time',N'Romance Standard Time' UNION ALL
select 55,N'(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb',NULL,NULL,NULL,NULL,N'CEST',1,N'Central European Standard Time',1,N'CEST',N'Central European Summer Time',N'Central European Standard Time' UNION ALL
select 56,N'(UTC+01:00) West Central Africa',NULL,NULL,NULL,NULL,N'WCAST',0,N'W. Central Africa Standard Time',1,NULL,NULL,N'W. Central Africa Standard Time' UNION ALL
select 57,N'(UTC+02:00) Amman',NULL,NULL,NULL,NULL,N'JRD',1,N'Jordan Standard Time',2,N'CEST',N'Central European Summer Time',N'Jordan Standard Time' UNION ALL
select 58,N'(UTC+02:00) Athens, Bucharest',NULL,NULL,NULL,NULL,N'GTB',1,N'GTB Standard Time',2,N'EEST',N'Eastern European Summer Time',N'GTB Standard Time' UNION ALL
select 59,N'(UTC+02:00) Beirut',NULL,NULL,NULL,NULL,N'MEST',1,N'Middle East Standard Time',2,N'EEST',N'Eastern European Summer Time',N'Middle East Standard Time' UNION ALL
select 60,N'(UTC+02:00) Cairo',NULL,NULL,NULL,NULL,N'EGY',1,N'Egypt Standard Time',2,NULL,NULL,N'Egypt Standard Time' UNION ALL
select 61,N'(UTC+02:00) Chisinau',NULL,NULL,NULL,NULL,N'EET',1,N'Eastern European Standard Time',2,N'EEST',N'Eastern European Summer Time',N'E. Europe Standard Time' UNION ALL
select 62,N'(UTC+02:00) Damascus',NULL,NULL,NULL,NULL,N'SYST',1,N'Syria Standard Time',2,N'EEST',N'Eastern European Summer Time',N'Syria Standard Time' UNION ALL
select 63,N'(UTC+02:00) Gaza, Hebron',NULL,NULL,NULL,NULL,N'GAST',1,N'West Bank Gaza Standard Time',2,N'EEST',N'Eastern European Summer Time',N'West Bank Standard Time' UNION ALL
select 64,N'(UTC+02:00) Harare, Pretoria',NULL,NULL,NULL,NULL,N'SAST',0,N'South Africa Standard Time',2,NULL,NULL,N'South Africa Standard Time' UNION ALL
select 65,N'(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius',NULL,NULL,NULL,NULL,N'FLE',1,N'FLE Standard Time',2,N'EEST',N'Eastern European Summer Time',N'FLE Standard Time' UNION ALL
select 66,N'(UTC+02:00) Jerusalem',NULL,NULL,NULL,NULL,N'ISRAEL',1,N'Jerusalem Standard Time',2,N'JDT',N'Jerusalem Daylight Time',N'Israel Standard Time' UNION ALL
select 67,N'(UTC+02:00) Juba',NULL,NULL,NULL,NULL,N'SSST',1,N'South Sudan Standard Time',2,NULL,NULL,N'South Sudan Standard Time' UNION ALL
select 68,N'(UTC+02:00) Kaliningrad',NULL,NULL,NULL,NULL,N'KALT',1,N'Russia TZ 1 Standard Time',2,NULL,NULL,N'Kaliningrad Standard Time' UNION ALL
select 69,N'(UTC+02:00) Khartoum',NULL,NULL,NULL,NULL,N'KHST',1,N'Sudan Standard Time',2,NULL,NULL,N'Sudan Standard Time' UNION ALL
select 70,N'(UTC+02:00) Tripoli',NULL,NULL,NULL,NULL,N'LST',1,N'Libya Standard Time',2,NULL,NULL,N'Libya Standard Time' UNION ALL
select 71,N'(UTC+02:00) Windhoek',NULL,NULL,NULL,NULL,N'NMT',1,N'Namibia Standard Time',2,NULL,NULL,N'Namibia Standard Time' UNION ALL
select 72,N'(UTC+03:00) Baghdad',NULL,NULL,NULL,NULL,N'ARABIC',1,N'Arabic Standard Time',3,NULL,NULL,N'Arabic Standard Time' UNION ALL
select 73,N'(UTC+03:00) Istanbul',NULL,NULL,NULL,NULL,N'TRT',1,N'Turkey Standard Time',3,NULL,NULL,N'Turkey Standard Time' UNION ALL
select 74,N'(UTC+03:00) Kuwait, Riyadh',NULL,NULL,NULL,NULL,N'ARAB',0,N'Arab Standard Time',3,NULL,NULL,N'Arab Standard Time' UNION ALL
select 75,N'(UTC+03:00) Minsk',NULL,NULL,NULL,NULL,N'BST',1,N'Belarus Standard Time',3,NULL,NULL,N'Belarus Standard Time' UNION ALL
select 76,N'(UTC+03:00) Moscow, St. Petersburg',NULL,NULL,NULL,NULL,N'MSK',1,N'Russia TZ 2 Standard Time',3,NULL,NULL,N'Russian Standard Time' UNION ALL
select 77,N'(UTC+03:00) Nairobi',NULL,NULL,NULL,NULL,N'EAT',0,N'E. Africa Standard Time',3,NULL,NULL,N'E. Africa Standard Time' UNION ALL
select 78,N'(UTC+03:00) Volgograd',NULL,NULL,NULL,NULL,N'VOLT',1,N'Volgograd Standard Time',3,NULL,NULL,N'Volgograd Standard Time' UNION ALL
select 79,N'(UTC+03:30) Tehran',NULL,NULL,NULL,NULL,N'IRST',1,N'Iran Standard Time',3.5,NULL,NULL,N'Iran Standard Time' UNION ALL
select 80,N'(UTC+04:00) Abu Dhabi, Muscat',NULL,NULL,NULL,NULL,N'ARABIA',0,N'Arabian Standard Time',4,NULL,NULL,N'Arabian Standard Time' UNION ALL
select 81,N'(UTC+04:00) Astrakhan, Ulyanovsk',NULL,NULL,NULL,NULL,N'ASTT',1,N'Astrakhan Standard Time',4,NULL,NULL,N'Astrakhan Standard Time' UNION ALL
select 82,N'(UTC+04:00) Baku',NULL,NULL,NULL,NULL,N'AZT',1,N'Azerbaijan Standard Time',4,NULL,NULL,N'Azerbaijan Standard Time' UNION ALL
select 83,N'(UTC+04:00) Izhevsk, Samara',NULL,NULL,NULL,NULL,N'SAMT',1,N'Russia TZ 3 Standard Time',4,NULL,NULL,N'Russia Time Zone 3' UNION ALL
select 84,N'(UTC+04:00) Port Louis',NULL,NULL,NULL,NULL,N'MUT',1,N'Mauritius Standard Time',4,NULL,NULL,N'Mauritius Standard Time' UNION ALL
select 85,N'(UTC+04:00) Saratov',NULL,NULL,NULL,NULL,N'SAT',1,N'Saratov Standard Time',4,NULL,NULL,N'Saratov Standard Time' UNION ALL
select 86,N'(UTC+04:00) Tbilisi',NULL,NULL,NULL,NULL,N'GET',0,N'Georgian Standard Time',4,NULL,NULL,N'Georgian Standard Time' UNION ALL
select 87,N'(UTC+04:00) Yerevan',NULL,NULL,NULL,NULL,N'CCS',1,N'Caucasus Standard Time',4,NULL,NULL,N'Caucasus Standard Time' UNION ALL
select 88,N'(UTC+04:30) Kabul',NULL,NULL,NULL,NULL,N'AFT',0,N'Afghanistan Standard Time',4.5,NULL,NULL,N'Afghanistan Standard Time' UNION ALL
select 89,N'(UTC+05:00) Ashgabat, Tashkent',NULL,NULL,NULL,NULL,N'WAT',0,N'West Asia Standard Time',5,NULL,NULL,N'West Asia Standard Time' UNION ALL
select 90,N'(UTC+05:00) Ekaterinburg',NULL,NULL,NULL,NULL,N'YEKT',1,N'Russia TZ 4 Standard Time',5,NULL,NULL,N'Ekaterinburg Standard Time' UNION ALL
select 91,N'(UTC+05:00) Islamabad, Karachi',NULL,NULL,NULL,NULL,N'PKT',1,N'Pakistan Standard Time',5,NULL,NULL,N'Pakistan Standard Time' UNION ALL
select 92,N'(UTC+05:00) Qyzylorda',NULL,NULL,NULL,NULL,N'QST',1,N'Qyzylorda Standard Time',5,NULL,NULL,N'Qyzylorda Standard Time' UNION ALL
select 93,N'(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi',NULL,NULL,NULL,NULL,N'IST',0,N'India Standard Time',5.5,NULL,NULL,N'India Standard Time' UNION ALL
select 94,N'(UTC+05:30) Sri Jayawardenepura',NULL,NULL,NULL,NULL,N'SLST',0,N'Sri Lanka Standard Time',5.5,NULL,NULL,N'Sri Lanka Standard Time' UNION ALL
select 95,N'(UTC+05:45) Kathmandu',NULL,NULL,NULL,NULL,N'NPT',0,N'Nepal Standard Time',5.75,NULL,NULL,N'Nepal Standard Time' UNION ALL
select 96,N'(UTC+06:00) Astana',NULL,NULL,NULL,NULL,N'CAT',0,N'Central Asia Standard Time',6,NULL,NULL,N'Central Asia Standard Time' UNION ALL
select 97,N'(UTC+06:00) Dhaka',NULL,NULL,NULL,NULL,N'BST',1,N'Bangladesh Standard Time',6,NULL,NULL,N'Bangladesh Standard Time' UNION ALL
select 98,N'(UTC+06:00) Omsk',NULL,NULL,NULL,NULL,N'OMST',1,N'Omsk Standard Time',6,NULL,NULL,N'Omsk Standard Time' UNION ALL
select 99,N'(UTC+06:30) Yangon (Rangoon)',NULL,NULL,NULL,NULL,N'MMT',0,N'Myanmar Standard Time',6.5,NULL,NULL,N'Myanmar Standard Time' UNION ALL
select 100,N'(UTC+07:00) Bangkok, Hanoi, Jakarta',NULL,NULL,NULL,NULL,N'SEA',0,N'SE Asia Standard Time',7,NULL,NULL,N'SE Asia Standard Time' UNION ALL
select 101,N'(UTC+07:00) Barnaul, Gorno-Altaysk',NULL,NULL,NULL,NULL,N'BAT',1,N'Altai Standard Time',7,NULL,NULL,N'Altai Standard Time' UNION ALL
select 102,N'(UTC+07:00) Hovd',NULL,NULL,NULL,NULL,N'HOVT',1,N'W. Mongolia Standard Time',7,NULL,NULL,N'W. Mongolia Standard Time' UNION ALL
select 103,N'(UTC+07:00) Krasnoyarsk',NULL,NULL,NULL,NULL,N'KRAT',1,N'Russia TZ 6 Standard Time',7,NULL,NULL,N'North Asia Standard Time' UNION ALL
select 104,N'(UTC+07:00) Novosibirsk',NULL,NULL,NULL,NULL,N'NOVT',1,N'Novosibirsk Standard Time',7,NULL,NULL,N'N. Central Asia Standard Time' UNION ALL
select 105,N'(UTC+07:00) Tomsk',NULL,NULL,NULL,NULL,N'TOT',1,N'Tomsk Standard Time',7,NULL,NULL,N'Tomsk Standard Time' UNION ALL
select 106,N'(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi',NULL,NULL,NULL,NULL,N'CHN',0,N'China Standard Time',8,NULL,NULL,N'China Standard Time' UNION ALL
select 107,N'(UTC+08:00) Irkutsk',NULL,NULL,NULL,NULL,N'IRKT',1,N'Russia TZ 7 Standard Time',8,NULL,NULL,N'North Asia East Standard Time' UNION ALL
select 108,N'(UTC+08:00) Kuala Lumpur, Singapore',NULL,NULL,NULL,NULL,N'SST',0,N'Malay Peninsula Standard Time',8,NULL,NULL,N'Singapore Standard Time' UNION ALL
select 109,N'(UTC+08:00) Perth',NULL,NULL,NULL,NULL,N'AWST',1,N'W. Australia Standard Time',8,NULL,NULL,N'W. Australia Standard Time' UNION ALL
select 110,N'(UTC+08:00) Taipei',NULL,NULL,NULL,NULL,N'TWT',0,N'Taipei Standard Time',8,NULL,NULL,N'Taipei Standard Time' UNION ALL
select 111,N'(UTC+08:00) Ulaanbaatar',NULL,NULL,NULL,NULL,N'ULAT',1,N'Ulaanbaatar Standard Time',8,NULL,NULL,N'Ulaanbaatar Standard Time' UNION ALL
select 112,N'(UTC+08:45) Eucla',NULL,NULL,NULL,NULL,N'ACWST',0,N'Aus Central W. Standard Time',8.75,NULL,NULL,N'Aus Central W. Standard Time' UNION ALL
select 113,N'(UTC+09:00) Chita',NULL,NULL,NULL,NULL,N'TBST',1,N'Transbaikal Standard Time',9,NULL,NULL,N'Transbaikal Standard Time' UNION ALL
select 114,N'(UTC+09:00) Osaka, Sapporo, Tokyo',NULL,NULL,NULL,NULL,N'JST',0,N'Tokyo Standard Time',9,NULL,NULL,N'Tokyo Standard Time' UNION ALL
select 115,N'(UTC+09:00) Pyongyang',NULL,NULL,NULL,NULL,N'NKST',1,N'North Korea Standard Time',9,NULL,NULL,N'North Korea Standard Time' UNION ALL
select 116,N'(UTC+09:00) Seoul',NULL,NULL,NULL,NULL,N'KST',0,N'Korea Standard Time',9,NULL,NULL,N'Korea Standard Time' UNION ALL
select 117,N'(UTC+09:00) Yakutsk',NULL,NULL,NULL,NULL,N'YAKT',1,N'Russia TZ 8 Standard Time',9,NULL,NULL,N'Yakutsk Standard Time' UNION ALL
select 118,N'(UTC+09:30) Adelaide',NULL,NULL,NULL,NULL,N'ACDT',1,N'Cen. Australia Standard Time',9.5,N'ACDT',N'Australian Central Daylight Time',N'Cen. Australia Standard Time' UNION ALL
select 119,N'(UTC+09:30) Darwin',NULL,NULL,NULL,NULL,N'ACST',0,N'AUS Central Standard Time',9.5,NULL,NULL,N'AUS Central Standard Time' UNION ALL
select 120,N'(UTC+10:00) Brisbane',NULL,NULL,NULL,NULL,N'AEST',0,N'E. Australia Standard Time',10,NULL,NULL,N'E. Australia Standard Time' UNION ALL
select 121,N'(UTC+10:00) Canberra, Melbourne, Sydney',NULL,NULL,NULL,NULL,N'AEDT',1,N'AUS Eastern Standard Time',10,N'AEDT',N'Australian Eastern Daylight Time',N'AUS Eastern Standard Time' UNION ALL
select 122,N'(UTC+10:00) Guam, Port Moresby',NULL,NULL,NULL,NULL,N'WPT',0,N'West Pacific Standard Time',10,NULL,NULL,N'West Pacific Standard Time' UNION ALL
select 123,N'(UTC+10:00) Hobart',NULL,NULL,NULL,NULL,N'TSM',1,N'Tasmania Standard Time',10,N'TSMDT',N'Australian Eastern Daylight Time',N'Tasmania Standard Time' UNION ALL
select 124,N'(UTC+10:00) Vladivostok',NULL,NULL,NULL,NULL,N'VVS',1,N'Russia TZ 9 Standard Time',10,NULL,NULL,N'Vladivostok Standard Time' UNION ALL
select 125,N'(UTC+10:30) Lord Howe Island',NULL,NULL,NULL,NULL,N'LHST',1,N'Lord Howe Standard Time',10.5,N'LHDT',N'Lord Howe Daylight Time',N'Lord Howe Standard Time' UNION ALL
select 126,N'(UTC+11:00) Bougainville Island',NULL,NULL,NULL,NULL,N'BST',1,N'Bougainville Standard Time',11,NULL,NULL,N'Bougainville Standard Time' UNION ALL
select 127,N'(UTC+11:00) Chokurdakh',NULL,NULL,NULL,NULL,N'CHOT',1,N'Russia TZ 10 Standard Time',11,NULL,NULL,N'Russia Time Zone 10' UNION ALL
select 128,N'(UTC+11:00) Magadan',NULL,NULL,NULL,NULL,N'MAGT',1,N'Magadan Standard Time',11,NULL,NULL,N'Magadan Standard Time' UNION ALL
select 129,N'(UTC+11:00) Norfolk Island',NULL,NULL,NULL,NULL,N'NFT',1,N'Norfolk Standard Time',11,N'NFDT',N'Norfolk Island Daylight Time',N'Norfolk Standard Time' UNION ALL
select 130,N'(UTC+11:00) Sakhalin',NULL,NULL,NULL,NULL,N'SAKT',1,N'Sakhalin Standard Time',11,NULL,NULL,N'Sakhalin Standard Time' UNION ALL
select 131,N'(UTC+11:00) Solomon Is., New Caledonia',NULL,NULL,NULL,NULL,N'CPST',0,N'Central Pacific Standard Time',11,NULL,NULL,N'Central Pacific Standard Time' UNION ALL
select 132,N'(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky',NULL,NULL,NULL,NULL,N'PETT',1,N'Russia TZ 11 Standard Time',12,NULL,NULL,N'Russia Time Zone 11' UNION ALL
select 133,N'(UTC+12:00) Auckland, Wellington',NULL,NULL,NULL,NULL,N'NZST',1,N'New Zealand Standard Time',12,N'NZDT',N'New Zealand Daylight Time',N'New Zealand Standard Time' UNION ALL
select 134,N'(UTC+12:00) Coordinated Universal Time+12',NULL,NULL,NULL,NULL,N'UTC+12',0,N'UTC+12',12,NULL,NULL,N'UTC+12' UNION ALL
select 135,N'(UTC+12:00) Fiji',NULL,NULL,NULL,NULL,N'FJT',1,N'Fiji Standard Time',12,N'FJST',N'Fiji Summer Time',N'Fiji Standard Time' UNION ALL
select 137,N'(UTC+12:45) Chatham Islands',NULL,NULL,NULL,NULL,N'CHAST',1,N'Chatham Islands Standard Time',12.75,N'CHADT',N'Chatham Daylight Time',N'Chatham Islands Standard Time' UNION ALL
select 138,N'(UTC+13:00) Coordinated Universal Time+13',NULL,NULL,NULL,NULL,N'UTC+13',0,N'UTC+13',13,NULL,NULL,N'UTC+13' UNION ALL
select 139,N'(UTC+13:00) Nuku''alofa',NULL,NULL,NULL,NULL,N'TOT',1,N'Tonga Standard Time',13,NULL,NULL,N'Tonga Standard Time' UNION ALL
select 140,N'(UTC+13:00) Samoa',NULL,NULL,NULL,NULL,N'SST',1,N'Samoa Standard Time',13,NULL,NULL,N'Samoa Standard Time' UNION ALL
select 141,N'(UTC+14:00) Kiritimati Island',NULL,NULL,NULL,NULL,N'LINT',0,N'Line Islands Standard Time',14,NULL,NULL,N'Line Islands Standard Time';

set identity_insert [Time_Zone] off;