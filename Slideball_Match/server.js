const express = require('express');
const http = require('http');
const path = require('path');
const socketIo = require('socket.io');
const Player = require('./models/player');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

// Middleware to parse JSON in request bodies
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(express.static(path.join(__dirname, 'public')));

// Simulated in-memory storage for players
const players = {};

// Function to handle player connection
function handleConnection(socket) {
  console.log('A user connected');

  // Simulate creating a player and sending player information to the client
  const playerId = socket.id;
  const username = `Player_${playerId.substr(0, 4)}`;
  const player = new Player(playerId, username);

  players[playerId] = player;

  // Emit an event to notify the client about the player details
  socket.emit('playerDetails', player);

  // Handle player events
  socket.on('disconnect', () => {
    handleDisconnection(playerId);
  });

  socket.on('playerMove', (data) => {
    handlePlayerMove(socket, playerId, data);
  });
}

// Function to handle player disconnection
function handleDisconnection(playerId) {
  console.log('User disconnected');

  // Remove the player from the in-memory storage
  delete players[playerId];

  // Emit an event to notify other players about the disconnection
  io.emit('playerDisconnected', playerId);
}

// Function to handle player movement
function handlePlayerMove(socket, playerId, data) {
  console.log(`Player ${playerId} moved: ${data}`);
  // Broadcast the player's movement to other connected players
  socket.broadcast.emit('playerMoved', { playerId, data });
}

// Socket.io connection event
io.on('connection', handleConnection);

// Use player routes
const playerRoutes = require('./routes/playerRoutes');
app.use('/api/player', playerRoutes);

const port = process.env.PORT || 3000;
server.listen(port, '0.0.0.0', () => {
  console.log(`Server running at http://localhost:${port}`);
});
