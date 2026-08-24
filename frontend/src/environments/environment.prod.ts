export const environment = {
  production: true,
  // Points at the Dockerized backend (docker-compose maps it to host port 5080, since 5000
  // collides with macOS's AirPlay Receiver). Override this before shipping to a real deployment.
  apiUrl: 'http://localhost:5080/'
};
