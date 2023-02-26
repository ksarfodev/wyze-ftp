# FirmwareUpdater

## appSettings.json

It's necessary to create a hashed version of your Wyze password then update the appSettings file in order to make api calls.

The following code describes how to hash your Wyze user account password using Python or C#:

```python
 password = "password"
 
 for i in range(0, 3):
    password = hashlib.md5(password.encode('ascii')).hexdigest()
```

```c#
string password = "password";

for(int i = 0; i < 3; i++)
{
  password = ComputeMD5(password).ToLower();	
}
```

## deviceInfo.json

This file serves as a template and shouldn't be modified.

## Program.cs

Main program to generate a token file and trigger a firmware update on select Wyze cameras by making an api call

## Update.cs
References the WyzeFtpLibrary to make api call to initiate a firmware update

## WyzeAuth.cs
References the WyzeFtpLibrary to make api call to return an access token
