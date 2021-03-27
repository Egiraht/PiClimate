#!/usr/bin/env sh

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/.
#
# Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

# This script publishes the Logger and Monitor projects to the "Published" directory.
# The projects are published as framework-dependant.


PUBLISH_DIR="./Published"
LOGGER_PROJECT="PiClimate.Logger"
LOGGER_PROJECT_DIR="./PiClimate.Logger"
MONITOR_PROJECT="PiClimate.Monitor"
MONITOR_PROJECT_DIR="./PiClimate.Monitor"
DOTNET_EXECUTABLE="dotnet"
_7ZIP_EXECUTABLE="7z"


# Getting the project versions.
LOGGER_PROJECT_VERSION=""
if [ -f "./$LOGGER_PROJECT_DIR/$LOGGER_PROJECT.csproj" ]; then
  LOGGER_PROJECT_VERSION=v$(grep -o "<InformationalVersion>.*</InformationalVersion>" "./$LOGGER_PROJECT_DIR/$LOGGER_PROJECT.csproj" | cut -d ">" -f 2 | cut -d "<" -f 1);
fi
echo "Logger project version: ${LOGGER_PROJECT_VERSION}"

MONITOR_PROJECT_VERSION=""
if [ -f "./$MONITOR_PROJECT_DIR/$MONITOR_PROJECT.csproj" ]; then
  MONITOR_PROJECT_VERSION=v$(grep -o "<InformationalVersion>.*</InformationalVersion>" "./$MONITOR_PROJECT_DIR/$MONITOR_PROJECT.csproj" | cut -d ">" -f 2 | cut -d "<" -f 1);
fi
echo "Monitor project version: ${MONITOR_PROJECT_VERSION}"


# Publishing projects to the directory.
if [ ! "$(command -v $DOTNET_EXECUTABLE)" ]; then
  echo "No dotnet executable found, aborting."
  exit
fi

rm -rf ${PUBLISH_DIR:?}
mkdir -p $PUBLISH_DIR

rm -rf "${PUBLISH_DIR:?}/${LOGGER_PROJECT:?}"
$DOTNET_EXECUTABLE publish -c "Release" -o "$PUBLISH_DIR/$LOGGER_PROJECT" --no-self-contained --nologo $LOGGER_PROJECT
echo "Logger is published to the \"$PUBLISH_DIR/$LOGGER_PROJECT\" directory."

rm -rf "${PUBLISH_DIR:?}/${MONITOR_PROJECT:?}"
$DOTNET_EXECUTABLE publish -c "Release" -o "$PUBLISH_DIR/$MONITOR_PROJECT" --no-self-contained --nologo $MONITOR_PROJECT
echo "Monitor is published to the \"$PUBLISH_DIR/$MONITOR_PROJECT\" directory."


# Compressing the published projects into archives.
if [ ! "$(command -v $_7ZIP_EXECUTABLE)" ]; then
  echo "No 7-zip archiver found, aborting."
  exit
fi

LOGGER_ARCHIVE="$LOGGER_PROJECT $LOGGER_PROJECT_VERSION.zip"
cd "$PUBLISH_DIR/$LOGGER_PROJECT" && $_7ZIP_EXECUTABLE a "../$LOGGER_ARCHIVE" && cd - || exit
echo "Logger is compressed into the \"$PUBLISH_DIR/$LOGGER_ARCHIVE\" archive."

MONITOR_ARCHIVE="$MONITOR_PROJECT $MONITOR_PROJECT_VERSION.zip"
cd "$PUBLISH_DIR/$MONITOR_PROJECT" && $_7ZIP_EXECUTABLE a "../$MONITOR_ARCHIVE" && cd - || exit
echo "Monitor is compressed into the \"$PUBLISH_DIR/$LOGGER_ARCHIVE\" archive."

echo "Done."
