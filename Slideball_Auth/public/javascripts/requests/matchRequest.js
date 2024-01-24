async function getPlayerScore(playerId) {
    try {
        const response = await fetch(`/api/user/${playerId}/score/`, 
        {
            method: "GET",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return { successful: response.status == 200};
    } catch (err) {
        console.log(err);
        return {err: err};
    }
}
async function getLeaderboard() {
    try {
        const response = await fetch(`/api/leaderboard/`, 
        {
            method: "GET",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        }); 
        return { successful: response.status == 200};
    } catch (err) {
        console.log(err);
        return {err: err};
    }
}
async function createUpdate(playerId, points) {
    try {
        const response = await fetch(`/api/user/${playerId}/score/`, 
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        method: "PUT",
        body: JSON.stringify({
            points: points,
        })
        }); 
        return { successful: response.status == 200};
    } catch (err) {
        console.log(err);
        return {err: err};
    }
}