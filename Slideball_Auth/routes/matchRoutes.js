const express = require('express');
const router = express.Router();
const Match = require("../models/matchModel");
const auth = require("../middleware/auth");




router.get('',auth.verifyAuth,  async function (req, res, next) {
    try {
        let result = await Match.getLeaderboard();
            res.status(result.status).send(result.result);
    } catch (err) {
        console.log(err);
        res.status(500).send(err);
    }
});


module.exports = router;