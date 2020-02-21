#!/usr/bin/env bash

PICLIMATE_ROOT_DIR=/home/pi/PiClimate
DOTNET_BIN_PATH=/home/pi/dotnet/dotnet

echo "Starting the PiClimate.Logger service..."
if [[ ! -f ${PICLIMATE_ROOT_DIR}/.pid ]]; then
  ${DOTNET_BIN_PATH} ${PICLIMATE_ROOT_DIR}/PiClimate.Logger.dll > /dev/null 2>> ${PICLIMATE_ROOT_DIR}/Error.log &
  echo $! > ${PICLIMATE_ROOT_DIR}/.pid
fi
