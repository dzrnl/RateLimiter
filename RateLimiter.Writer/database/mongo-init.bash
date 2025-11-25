#!/bin/bash

mongod --replSet rs0 --bind_ip_all >/dev/null 2>&1 &

RETRY_DELAY=3
until mongosh --quiet --eval "res = db.adminCommand({ ping: 1 }); quit(res.ok ? 0 : 1)" >/dev/null 2>&1; do
  echo "Awaiting MongoDB to be ready..."
  sleep "${RETRY_DELAY}"
done

if ! mongosh --quiet --eval "rs.status()" >/dev/null 2>&1; then
  mongosh --quiet --eval "rs.initiate({ _id: 'rs0', members: [{ _id: 0, host: 'limits-db:27017' }] });"
  echo "Replica set initiated"
fi

wait
