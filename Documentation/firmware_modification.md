### Firmware Modification
The WyzeCam V3 firmware 4.36.8.15 can be modified and applied to the WyzeCam V3 successfully using DNS spoofing

## Download 4.36.8.15 from Wyze 
Official firmware from Wyze: [wcv3_4.36.8.15](https://download.wyzecam.com/firmware/v3/demo_wcv3_4.36.8.15.zip)

* Follow [CedBri's](https://github.com/CedBri/wyzecamv3) guide to unpack
* After unpacking 4.36.8.15
	 * Change the password 
	 * openssl passwd -1 -salt <YOUR SALT> <YOUR PASSWORD>
	     * replace shadow file
 
## update the rCS file

add the following to the end of /etc/init.d/rcS
```bash
FILE1='/media/mmc/telnet_script.sh'

while true
do
    if test -f "$FILE1"; then
        echo "$FILE1 was found.."
        break
    else
        echo "$FILE1 not found"
    fi

    sleep 1
done

if test -f "$FILE1"; then
    echo "$FILE1 was found."
    cd /media/mmc && sh telnet_script.sh
else
    echo "$FILE1 not found."
fi
```

Before packing the tar file, edit upgraderun.sh as follows:
	
```bash
#!/bin/sh

echo "erase rootfs !!!!!!!!!!!"
flash_eraseall /dev/mtd2
sync
echo "write backk !!!!!!!!!!!"
flashcp -v /tmp/Upgrade/mtd2 /dev/mtd2

sync

reboot
```


Sample  folder structure of modified tar file. Note that mtd2 represents what was modified.

* app
* config_repair_up.sh
* kernel
* mtd2
* PARA
* upgraderun.sh

The tar file is now ready to be applied to a camera via the DNS spoofing method with the help of FileServer and FirmwareUpdater.


