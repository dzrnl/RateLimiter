#!/bin/bash
set -e

KAFKA_CONFIG=/etc/kafka/server.properties
DATA_DIR=/var/lib/kafka/data
META_FILE="$DATA_DIR/meta.properties"

if [ ! -f "$META_FILE" ]; then
  CLUSTER_ID=$(/opt/kafka/bin/kafka-storage.sh random-uuid)
  /opt/kafka/bin/kafka-storage.sh format -t "$CLUSTER_ID" -c "$KAFKA_CONFIG"
fi

exec /opt/kafka/bin/kafka-server-start.sh "$KAFKA_CONFIG"
