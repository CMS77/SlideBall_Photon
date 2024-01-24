const pool = require("../config/database");
const auth = require("../config/utils");

function dbMatchToMatch(dbMatch)  {
    let match = new Match();
    match.id = dbMatch.mh_id;
    match.playerId = dbMatch.mh_pr_id;
    match.points = dbMatch.mh_points;
    match.playerName = dbMatch.pr_name;

    return match;
}

class Match {

    constructor(id, points, playerId) {
        this.id = id;
        this.points = points;
        this.playerId = playerId;
    }
    export() {
            return this; 
    }

    static async createUpdate(playerId, points) {
        try {
            let dbResult =
                await pool.query("Select * from match where mh_pr_id=$1", [playerId]);
            let dbMatch = dbResult.rows;
            if (dbMatch.length){
                let updateResult =
                await pool.query("Update match SET mh_points=$1 where mh_pr_id=$2", [dbMatch[0].mh_points+points, playerId]);
            }
            else {
                let insertResult =
                await pool.query("Insert into match (mh_points, mh_pr_id) values ($1, $2)", [points, playerId]);
            }
                return {
                    status: 200, result: {
                        msg: "Create/update sucessfully!"
                    }
                };
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }
    } 
    static async getPlayerScore(playerId) {
        try {
            let dbplayerResult = await pool.query("Select match.*, player.pr_name from match inner join player on match.mh_pr_id = player.pr_id where mh_pr_id=$1", [playerId]);
            let dbMatch = dbplayerResult.rows;
            if (!dbMatch.length) 
                return { status: 404, result:{msg: "No player found for that id."} } ;
            let dbplayerScore = dbMatch[0];
            return { status: 200, result: dbMatchToMatch(dbplayerScore)} ;
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }  
    }
    static async getLeaderboard() {
        try {
            let leaderboard = await pool.query(`Select match.mh_points,player.pr_name from match inner join player 
            on match.mh_pr_id = player.pr_id order by match.mh_points desc limit 10`);
            return { status: 200, result: leaderboard.rows} ;
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        } 
    }
}

module.exports = Match;