set -o allexport; source .env; set +o allexport

declare -a CONTAINERS=(
  "identity"
  "default"
  "worker"
)

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
  for CONTAINER in "${CONTAINERS[@]}"
  do
    docker build --no-cache -t $REGISTRY/$CONTAINER -f ./docker/build/$CONTAINER/Dockerfile .
    docker tag $CONTAINER $REGISTRY/$CONTAINER
  done
}

push() {
  for CONTAINER in "${CONTAINERS[@]}"
  do
    docker push $REGISTRY/$CONTAINER
  done
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
