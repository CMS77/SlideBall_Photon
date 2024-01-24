const express = require('express');
const router = express.Router();
const Player = require('../models/player');

// Simulated in-memory storage for players
const players = {};

// Route to handle player movement
router.post('/move', (req, res) => {
  const { playerId, direction } = req.body;

  if (players[playerId]) {
    // Update player's position or perform other logic as needed
    // Emit an event to notify other players about the movement
    io.emit('playerMoved', { playerId, direction });

    res.status(200).json({ success: true });
  } else {
    res.status(404).json({ success: false, message: 'Player not found' });
  }
});

module.exports = router;
