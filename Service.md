# Create Tailscale Service

[Tailscale Services](https://tailscale.com/docs/features/tailscale-services) allow you to expose a local service on your tailnet without
opening ports or configuring firewalls. This is ideal for services that are only intended to be accessed by devices on your tailnet, such
as a home media server, a development environment, or a private API.

Open Docker Desktop and select the Exec tab. Enter tailscale serve --service=svc:antplus-service --https=443 http://host.docker.internal:5073
This will create a Tailscale Service named antplus-service that listens on port 443 and forwards traffic to host.docker.internal:5073.
