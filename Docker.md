# Create Docker Container

[Connect a Docker container to your tailnet with Docker Compose · Tailscale Docs](https://tailscale.com/docs/features/containers/docker/how-to/connect-docker-container)

Once you have your Tailscale Service set up, you can connect a Docker container to your tailnet using Docker Compose. Here's how you can do it:
docker compose up -d
This command will start your Docker container in detached mode. The container will be able to access the Tailscale Service you created,
allowing it to communicate with other devices on your tailnet.

Acquire a TLS certificate for your Tailscale Service using Let's Encrypt. This will enable secure HTTPS communication between your clients and the service.
Enter the following command in your terminal:
tailscale cert --service=svc:antplus-service