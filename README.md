"# cSharp programs" 

These programs work together

CimCollect runs a series of powershell scripts from files named CIM*.txt, and generates (machinename)_(section).json.gz files.

I suggest the json files be copied to a central respository for the next step.

CimSave will collect the json files into a Microsoft SQL database with a new table for each type of input data.
This allows you to construct reports of system status, and prepare delta scripts to manage a fleet of servers.

