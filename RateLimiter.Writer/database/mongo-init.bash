#!/bin/bash

mongod --replSet rs0 --bind_ip_all &

RETRY_DELAY=3
until mongosh --quiet --eval "res = db.adminCommand({ ping: 1 }); quit(res.ok ? 0 : 1)" >/dev/null 2>&1; do
  echo "Awaiting MongoDB to be ready..."
  sleep "${RETRY_DELAY}"
done

mongosh --quiet --eval "
  if (!rs.status().ok) { 
    print('Initializing replica set...'); 
    rs.initiate({ _id: 'rs0', members: [{ _id: 0, host: 'limits-db:27017' }] }); 
  } else { 
    print('Replica set already initialized.'); 
  }"

wait
