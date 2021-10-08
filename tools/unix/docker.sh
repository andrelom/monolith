set -o allexport; source .env; set +o allexport

# Menu

help() {
  echo ""
  echo "Usage:"
  echo "build" "\t" "It will build all docker containers."
  echo "push"  "\t" "It will push all docker containers to the target registry."
  echo "up"    "\t" "It will start all docker containers."
  echo "down"  "\t" "It will stop all docker containers."
  echo "prune" "\t" "It will prune."
  exit 1
}

# Tasks

build() {
  docker build --no-cache -t identity -f ./docker/build/identity/Dockerfile .
  docker tag identity $REGISTRY/identity

  docker build --no-cache -t worker -f ./docker/build/worker/Dockerfile .
  docker tag worker $REGISTRY/worker
}

push() {
  docker push $REGISTRY/identity
  docker push $REGISTRY/worker
}

up() {
  docker compose up -d
}

down() {
  docker compose down
}

prune() {
  docker system prune
}

# Main

case "${1}" in
  build ) build ;;
  push  ) push  ;;
  up    ) up    ;;
  down  ) down  ;;
  prune ) prune ;;
  *     ) help  ;;
esac
