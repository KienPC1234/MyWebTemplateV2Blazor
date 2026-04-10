#!/bin/bash

# Define paths
ROOT_DIR="/data/MyWebTemplateV2Blazor"
PROJECT_DIR="$ROOT_DIR/MyWebTemplateV2/MyWebTemplateV2"
PORT=3002

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_USE_POLLING_FILE_WATCHER=1

# Kill process using the port
echo "==> Checking if port $PORT is occupied..."
PID=$(lsof -ti:$PORT)
if [ ! -z "$PID" ]; then
    echo "    Found process $PID on port $PORT. Killing it..."
    kill -9 $PID
fi

echo "==> Navigating to project directory..."
cd $PROJECT_DIR

echo "==> Building Tailwind CSS..."
npm run build

echo "==> Building .NET Project..."
dotnet build

echo "==> Running project on http://localhost:$PORT..."
dotnet run --project MyWebTemplateV2.csproj --urls http://localhost:$PORT
