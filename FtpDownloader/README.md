
## appSettings.json

* Manually add in the mac address of cameras to download media from.
* Adjust the LocalRecordFolder and other values accordingly
* FtpUser and FtpPassword correspond to the telnet credentials of the camera
* The WyzeAccessToken is necessary for getting a list of cameras via api. To avoid having to store the token on disk and make frequent api calls, manually create a wyzeCameraList.json file and keep it updated

Sample wyzeCameraList.json

```json
{
//mac address of camera
"ABCDEFGHIJ00":
 {
      //camera nickname
	 "Item1":"Garage Cam",
	 "Item2":"192.168.100.136"
 },
 //mac address of camera
 "MNOPQRSTUV01":
 {
	 "Item1":"Front Porch Cam",
	 "Item2":"192.168.100.137"
```

## CameraList.cs

Gets information from appSettings.json then makes an api call to get the current ip address. The response is then supplied to FtpDownload in order to connect to each camera.

A wyzeCameraList.json file will be written to disk. If new Wyze cameras are added later on, these will either have to be manually added or simply delete wyzeCameraList.json to allow FtpDownloader to recreate a new json file. The app was intentionally designed this way to limit the number of api calls being made to Wyze servers. 

## deviceInfo.json

This file serves as a template and shouldn't be modified.

## FtpDownload.cs

After gathering information from appSettings.json, it is responsible for connecting to each camera using the FluentFTP library. It then downloads content from the respective 'record' and 'alarms' directories and saves these onto the destination drive. The media will be grouped by camera nicknames.

## Program.cs

The main program will get a collection of cameras with details including the IP address. Ftp download will then begin.
