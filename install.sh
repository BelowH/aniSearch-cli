#!/bin/sh
sudo rm -rf /usr/bin/aniSearch
sudo rm -rf $HOME/.local/share/aniSearch
printf '#!/bin/sh\nexec $HOME/.local/share/aniSearch/aniSearch-cli' > aniSearch
chmod +x aniSearch
sudo mv aniSearch /usr/bin/
wget https://github.com/BelowH/aniSearch-cli/releases/download/0.2.6.0-linux/aniSearch.tar.gz
sudo mkdir $HOME/.local/share/aniSearch
sudo chmod o+wr $HOME/.local/share/aniSearch
sudo tar -xzvf aniSearch.tar.gz --directory $HOME/.local/share/aniSearch
rm -rf aniSearch.tar.gz
echo "Done!"

