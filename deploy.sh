#!/usr/bin/env bash
# Start the stack with 1Password refs resolved. A plain `docker compose up` passes literal
# op:// strings (e.g. Email__Password) and breaks all email.
set -euo pipefail
cd /opt/humans
oprun --env-file /opt/humans/.env -- docker compose up -d
