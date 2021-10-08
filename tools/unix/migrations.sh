set -o allexport; source .env; set +o allexport

# Menu

help() {
  echo ""
  echo "Usage:"
  echo "create" "\t" "Will create or update default migrations."
  echo "update" "\t" "It will apply migrations to the target database."
  exit 1
}

# Tasks

create() {
  rm -rf src/Monolith.Web.Identity/Data/Migrations/
  dotnet ef migrations add Default --project "src/Monolith.Web.Identity" -s "src/Monolith.Web.Identity" -o "Data/Migrations"  -c "DefaultDbContext" --verbose
}

update() {
  dotnet ef database update Default --project "src/Monolith.Web.Identity" -s "src/Monolith.Web.Identity" -c "DefaultDbContext" --verbose
}

# Main

case "${1}" in
  create ) create ;;
  update ) update ;;
  *      ) help   ;;
esac
