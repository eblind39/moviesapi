#!/bin/bash

RUN_SERVER_LOG=logs/run-server.log
MAX_RETRIES=6000
RETRIES=0

stop_server() {
  pids=$(ps -ef | grep net | awk '/Challenge.Api/{print $2}')
  if [ -n "$pids" ]; then
    echo "Stopping current server"
    kill -9 $pids
  fi
}

wait_until_started() {
  echo "Checking for Challenge.Api to start..."

  until [ $(grep -c "Now listening on" $RUN_SERVER_LOG) -ne 0 ] || [ $(grep -c "exception" $RUN_SERVER_LOG) -ne 0 ]; do
    echo "Challenge.Api is unavailable - waiting"
    sleep 1

    RETRIES=$((RETRIES + 1))
    if [ $RETRIES -eq $MAX_RETRIES ]; then
      return
    fi
  done

  echo "Challenge.Api started"
}

mkdir -p logs

if [ -f $RUN_SERVER_LOG ]; then
  rm $RUN_SERVER_LOG
fi

stop_server

dotnet run --project Challenge.Api/Api > $RUN_SERVER_LOG &

wait_until_started

exit 0
