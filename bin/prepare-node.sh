#!/bin/bash

if [ $(npm list -g | grep -Ec '\sn@') -eq 0 ]; then
  npm i -g --slient --loglevel=error n
fi

node_version="$(node -v)"

if [ "$node_version" != "v$(n --latest)" ]; then
  npm i -g --slient --loglevel=error npm
  n --quiet latest
fi

if [ $(npm list -g | grep -c newman) -eq 0 ]; then
  npm i -g --slient --loglevel=error newman@5.2.2
fi

exit 0
