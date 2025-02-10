# Add .NET global tools to PATH  
export PATH="$PATH:$HOME/.dotnet/tools/"

# This will be used to allow EF Core to read from the environment variable  
# when running EF Core migration in the host machine  
if [ -f "$PWD/.env" ]; then
    # Automatically export all variables from .env
    set -a
    source "$PWD/.env"
    set +a

    # Assign the value of ConnectionStrings__ClubSyncDb_LOCAL from .env 
    # to ConnectionStrings__ClubSyncDb as an environment variable
    export ConnectionStrings__ClubSyncDb="$CLUBSYNC_DB_CONNECTION_STRING_LOCAL"
fi
