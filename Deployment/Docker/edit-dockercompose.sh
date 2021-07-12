sed -i "s/musickg.webapi:.*$/musickg.webapi:$2/" $1
sed -i "s/musickg.webui:.*$/musickg.webui:$2/" $1
sed -i "s/honda.webapi:.*$/honda.webapi:$2/" $1
sed -i "s/honda.modeltrainer:.*$/honda.modeltrainer:$2/" $1
sed -i "s/honda.offlineworker:.*$/honda.offlineworker:$2/" $1