#!/usr/bin/env sh

PUBLISH_DIR="./Published"
LOGGER_PROJECT="PiClimate.Logger"
MONITOR_PROJECT="PiClimate.Monitor"

if [ ! -d $PUBLISH_DIR ]; then
  mkdir -p $PUBLISH_DIR
fi

rm -rf "${PUBLISH_DIR:?}/$LOGGER_PROJECT"
rm -rf "${PUBLISH_DIR:?}/$MONITOR_PROJECT"

dotnet publish -c "Release" -o "$PUBLISH_DIR/$LOGGER_PROJECT" --no-self-contained --nologo $LOGGER_PROJECT
dotnet publish -c "Release" -o "$PUBLISH_DIR/$MONITOR_PROJECT" --no-self-contained --nologo $MONITOR_PROJECT
