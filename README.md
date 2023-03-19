
### Background  
The purpose of this project is to provide a solution for backing up 24/7 security video and images from Wyze security cameras onto a central hard drive via FTP. This solution has only been tested on the WyzeCam V3.

### Alternative Solutions

#### WyzeHacks
HclX/WyzeHacks provided an NFS solution where video footage could be saved to a network file share instead of the SD card. 

> **Note** [wz_mini_hacks](https://github.com/gtxaspec/wz_mini_hacks) makes possible a NFS solution as well which is worth exploring

The following [info](https://github.com/HclX/WyzeHacks/blob/master/info/http_server.md) provided by HclX inspired a 'Wyze Http' solution. But it proved to be unreliable in comparison to this FTP solution. 

#### RTSP
Official RTSP firmware from Wyze used to stream footage to a Network Video Recorder (NVR)

## WyzeFtp Solution

### About
The WyzeCam continuously saves video and alarm images to the SD card. The FtpDownloader app or any FTP Client (e.g. FileZilla) can then synchronize such media from the 32 GB SD card of each Wyze Camera onto a larger hard drive connected to the same network. By executing the FtpDownloader daily, at least 2 months worth of 24/7 video from 5 cameras can be archived onto a 5 terabyte hard drive.

### Quick Setup

[wz_mini_hacks](https://github.com/gtxaspec/wz_mini_hacks) provides a simple solution for enabling telnet and an FTP server on the Wyze camera instead of using DNS spoofing to apply modified firmware.

Once the wz_mini_hack has been applied, ssh into the camera and perform the following:
* cd into /opt/wz_mini/etc/configs/ and create the following script file:

*telnet_script.sh*
```bash
#!/bin/sh

until [ -d /media/mmc/record ]
do
	sleep 5
	echo "waiting for mmc to mount"
done
	echo "mmc mounted"

FILE=/media/mmc/busybox
if test -f "$FILE"; then
    echo "$FILE was found."
    cd /media/mmc/ && ./busybox telnetd &
    /media/mmc/busybox tcpsvd -vE 0.0.0.0 21 /media/mmc/busybox ftpd /media/mmc
else
    echo "$FILE not found."
fi

exit
```

* edit the CUSTOM_SCRIPT_PATH in wz_mini/wz_mini.conf  as follows:

```bash
#####SCRIPTING#####
CUSTOM_SCRIPT_PATH="/opt/wz_mini/etc/configs/telnet_script.sh"
```

* Place the provided version of [busybox](https://github.com/ksarfodev/wyze-ftp/blob/main/busybox) in the root directory of SD card

* Reboot the camera then confirm telnet and FTP access works as expected

```bash
telnet root@192.168.1.2

## the following should apper if telnet is working
WCV3 login: root
```

### Advanced Setup
* Instead of using wz_mini_hacks, the WyzeCam firmware will need to be modified to enable telnet and start an FTP server allowing access to the video and alarm folder content from within a private network using telnet credentials. See the following [README](https://github.com/ksarfodev/wyze-ftp/blob/main/Documentation/firmware_modification.md)
  
* DNS spoofing is still an option for applying modified camera firmware . See the following [README](https://github.com/ksarfodev/wyze-ftp/blob/main/Documentation/dns_spoofing.md) for some helpful information.

### Crontab Setup
 * The following [README](https://github.com/ksarfodev/wyze-ftp/blob/main/Documentation/raspberry-pi-setup.md) is helpful for setting up crontab to execute FtpDownloader on a Raspberry Pi. 
 
### Usage

#### FtpDownloader
* Recursively downloads all the video and image content from the Wyze camera SD card via FTP 
* It is not configured to run as a service. Therefore it needs to be set up as a cron job on Linux or a scheduled task on a Windows machine

#### FirmwareUpdater
* FirmwareUpdater is optional and useful for applying modified Wyze firmware using DNS spoofing

#### FileServer
* FileServer is optional and useful for serving modified Wyze firmware when DNS spoofing is being used.
	
	*If python is prefered, the WyzeUpdater python script  works well. The information in the README of the C# projects might be helpful for adjusting the python script*
	
## Project Features

* With this solution, it's possible to perform official over the air (OTA) updates using the Wyze App. For the WyzeCam V3, OTA updates stop at 4.36.9.139. Newer firmware versions have not been tested with this FTP solution

* WyzeCam plus and Wyze app features continue to work after applying the modifications

## Tech Stack

* FtpDownloader targets .NET 6 and can be compiled as a self-contained file. Therefore, it can be launched on a Raspberry Pi without the need to install the .NET runtime.

* ASP.NET minimal API is used for the FileServer project. It can also be compiled on Linux as a self contained file not requiring the .NET runtime 

## Helpful Info

* The following guide by CedBri is helpful for unpacking the camera firmware, modifying, then repacking: https://github.com/CedBri/wyzecamv3
* Leo's notes contains lots of useful information as well: https://leo.leung.xyz/wiki/Wyze_Cam
    
## Future Goals

* Use Secure FTP
* Make the firmware update projects more automated similar to the WyzeUpdater python script
* Add purging functionality to FtpDownloader to remove older media

