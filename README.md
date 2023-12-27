# RabbitMQ Direct Exchange Sample

This sample includes a server which listens for node events and publishes a simple message to the client using the routing key for that node.

The key take away is the approach to define a specific exchange name for the published message contract, and the publish topology which must be configured to ensure messages are routed via the direct exchange by routing key.


## Scaling the Clients

To see how additional direct exchanges are created, scale the client up:

```sh
docker compose scale client=5
```
