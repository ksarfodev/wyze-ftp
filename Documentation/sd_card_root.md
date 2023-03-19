The information on this repository is for general informational purposes only. There is no representation or warranty, express or implied. Your use of the repository is solely at your own risk. This repository may contain links to third party content, which I do not warrant, or assume liability for.

## Contents

* A version of Busybox with ftp/ftpd support

* telnet_script.sh
```bash
  # start telnet
 ./busybox telnetd &
  # configure ftp
 ./busybox tcpsvd -vE 0.0.0.0 21 /media/mmc/busybox ftpd /media/mmc
```
