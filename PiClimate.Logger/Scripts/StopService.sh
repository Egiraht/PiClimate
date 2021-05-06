#!/usr/bin/env bash

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/.
#
# Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

PICLIMATE_LOGGER_ROOT_DIR=/home/pi/PiClimate.Logger

echo "Stopping the PiClimate.Logger service..."
if [[ -f ${PICLIMATE_LOGGER_ROOT_DIR}/.pid ]]; then
  kill `cat ${PICLIMATE_LOGGER_ROOT_DIR}/.pid`
  rm ${PICLIMATE_LOGGER_ROOT_DIR}/.pid
fi
