#!/usr/bin/env bash

PICLIMATE_ROOT_DIR=/home/pi/PiClimate

echo "Stopping the PiClimate.Logger service..."
if [[ -f ${PICLIMATE_ROOT_DIR}/.pid ]]; then
  kill `cat ${PICLIMATE_ROOT_DIR}/.pid`
  rm ${PICLIMATE_ROOT_DIR}/.pid
fi
