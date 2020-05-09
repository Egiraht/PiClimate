#!/usr/bin/env bash

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/.
#
# Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

PICLIMATE_LOGGER_ROOT_DIR=/home/pi/PiClimate.Logger
DOTNET_BIN_PATH=/home/pi/dotnet/dotnet

echo "Starting the PiClimate.Logger service..."
if [[ ! -f ${PICLIMATE_LOGGER_ROOT_DIR}/.pid ]]; then
  ${DOTNET_BIN_PATH} ${PICLIMATE_LOGGER_ROOT_DIR}/PiClimate.Logger.dll > ${PICLIMATE_LOGGER_ROOT_DIR}/Output.log 2>> ${PICLIMATE_LOGGER_ROOT_DIR}/Error.log &
  echo $! > ${PICLIMATE_LOGGER_ROOT_DIR}/.pid
fi
