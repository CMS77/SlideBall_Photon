const socket = io();

// Event listeners
socket.on('connect', () => {
  console.log('Connected to server');
});

socket.on('disconnect', () => {
  console.log('Disconnected from server');
});

socket.on('playerDetails', (player) => {
  console.log('Player details:', player);
});

document.addEventListener('keydown', (event) => {
  if (event.key === 'ArrowRight') {
    const data = 'Move to the right';
    socket.emit('playerMove', data);
  }
});

socket.on('playerMoved', (data) => {
  console.log(`Player ${data.playerId} moved: ${data.data}`);
});

socket.on('playerDisconnected', (playerId) => {
  console.log(`Player ${playerId} disconnected`);
});
