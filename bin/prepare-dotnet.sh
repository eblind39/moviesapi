#!/bin/bash

if [ $(dotnet --version | grep -e ^3.1 | wc -l) -eq 0 ]; then
  sudo apt-get -y update
  sudo apt-get install -y dotnet-sdk-3.1
fi

exit 0