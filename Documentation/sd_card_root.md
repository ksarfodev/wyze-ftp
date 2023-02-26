## Contents

* A version of Busybox with ftp/ftpd support

* telnet_script.sh
```bash
  # start telnet
 ./busybox telnetd &
  # configure ftp
 ./busybox tcpsvd -vE 0.0.0.0 21 /media/mmc/busybox ftpd /media/mmc
```
