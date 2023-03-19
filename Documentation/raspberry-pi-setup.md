The information on this repository is for general informational purposes only. There is no representation or warranty, express or implied. Your use of the repository is solely at your own risk. This repository may contain links to third party content, which I do not warrant, or assume liability for.

## FtpDownloader

### Connect to Raspberry Pi via ssh then execute crontab -e and add the following:

```bash
# uncomment the following to test
#* * * * * echo "test" >> ~/test.txt

# run a bash script which will execute the dotnet app once a day at 1:42 am. Log the FtpDownloader console output to a file named cron.log
42 01 * * * /bin/sh /home/pi/Documents/FtpDownloader/script.sh >/home/pi/Documents/FtpDownloader/cron.log 2>&1
```

### Create script.sh file

```bash
#!/bin/bash
#run FtpDownloader
cd /home/pi/Documents/FtpDownloader/ && ./FtpDownloader

```

### Copy FtpDownloader published files onto Raspberry Pi

From Visual Studio- FtpDownloader published as a self contained file for linux-arm
* Select the option to produce a single file

![vs profile setting](https://github.com/ksarfodev/wyze-ftp/blob/main/Documentation/screenshots/Pasted%20image%2020230225193325.png)

* WinSCP is a great tool for copying files between Windows and Linux
* Run the following on Raspberry Pi after each new published release: 
```bash
pi@raspberrypi:~/Documents/FtpDownloader $ chmod 777 ./FtpDownloader
```

### App Settings
Update appSettings.json file accordingly i.e. include the mac address of cameras to download from, update the paths for LocalRecordFolder, add Wyze account token, as an option add Pushover credentials

Note * if a wyzeCameraList.json file fails to be created, try generating a new access token

### Configure Storage Drive

If using an external hard drive, confirm that it is mounted properly.

```bash
pi@raspberrypi:/mnt/share/Wyze/record $ ls
```

https://www.digikey.com/en/maker/blogs/2022/how-to-connect-a-drive-hddssd-to-a-raspberry-pi-or-other-linux-computers

### Crontab Test

Modify crontab as follows:


```bash

# run script.sh at 6 pm and save the console output to cron.log
00 18 * * * /bin/sh /home/pi/Documents/FtpDownloader/script.sh >/home/pi/cron.log 2>&1
```


Confirm files successfully downloaded using a tool such as WinSCP:

![WinSCP ftp directory](https://github.com/ksarfodev/wyze-ftp/blob/main/Documentation/screenshots/Pasted%20image%2020230225193942.png)



### Summary
A cron job to download media from all cameras can be scheduled to run once a day. However, note that depending on the number of cameras configured with FTP access, the process can take several hours to complete over Wifi.
		
Once the cron job is confirmed to be working,  adjust the start time as needed. The following crontab setting will run FtpDownloader once a day at 10 pm
```bash
# execute crontab -e
00 22 * * * /bin/sh /home/pi/Documents/FtpDownloader/script.sh >/home/pi/cron.log 2>&1
```
