FROM gitpod/workspace-full

RUN \
    # Install DotNet => https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2004
    \
    # Install Microsoft package feed
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && sudo dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb \
    \
    # Install .NET
    && sudo apt-get update \
    && sudo apt-get install -y --no-install-recommends dotnet-sdk-6.0 \
    \
    # Cleanup
    && sudo rm -rf /var/lib/apt/lists/*
    