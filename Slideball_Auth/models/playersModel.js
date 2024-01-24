const bcrypt = require('bcrypt');
const pool = require("../config/database");
const auth = require("../config/utils");
const saltRounds = 10; 

function dbUserToUser(dbUser)  {
    let user = new User();
    user.id = dbUser.pr_id;
    user.name = dbUser.pr_name;
    user.email = dbUser.pr_email;
    return user;
}

class User {
    constructor(id, name, email, pass, token) {
        this.id = id;
        this.name = name;
        this.email = email;
        this.pass = pass;
        this.token = token;
    }
    export() {
        let user=new User();
        user.name = this.name;
        return user; 
    }


    static async register(user) {
        try {
            let dbResult =
                await pool.query("Select * from player where pr_email=$1", [user.email]);
            let dbUsers = dbResult.rows;
            if (dbUsers.length)
                return {
                    status: 400, result: [{
                        location: "body", param: "email",
                        msg: "That email is already registered"
                    }]
                };
                
            let encpass = await bcrypt.hash(user.pass,saltRounds);      
            await pool.query(`Insert into player(pr_name, pr_email, pr_pass) values($1,$2,$3)`,
            [user.name, user.email,encpass]);
            dbResult = await pool.query(`Select * from player where pr_email=$1`,
            [user.email]);
            return { status: 200, result: {msg:"Registered! You can now log in.", result: dbResult.rows[0]}} ;
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }
    }
    
        static async getById(id) {
        try {
            let dbResult = await pool.query("Select * from player where pr_id=$1", [id]);
            let dbUsers = dbResult.rows;
            if (!dbUsers.length) 
                return { status: 404, result:{msg: "No user found for that id."} } ;
            let dbUser = dbUsers[0];
            return { status: 200, result: 
                new User(dbUser.pr_id, dbUser.pr_name, dbUser.pr_email , dbUser.pr_pass, dbUser.pr_token)} ;
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }  
    }
    

    //login
    static async checkLogin(user) {
        try {
            let dbResult =
                await pool.query("Select * from player where pr_name=$1", [user.name]);
            let dbUsers = dbResult.rows;
            if (!dbUsers.length)
                return { status: 401, result: { msg: "Wrong username or password!"}};

            let dbUser = dbUsers[0]; 

            let isPass = await bcrypt.compare(user.pass,dbUser.pr_pass);
            if (!isPass) 
                return { status: 401, result: { msg: "Wrong username or password!"}};
            return { status: 200, result: dbUserToUser(dbUser) } ;

        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }
    }

    // No verifications. Only to use internally
    static async saveToken(user) {
        try {
            let dbResult =
                await pool.query(`Update player set pr_token=$1 where pr_id = $2`,
                [user.token,user.id]);
            return { status: 200, result: {msg:"Token saved!"}} ;
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }
    }



    static async getUserByToken(token) {
        try {
            let dbResult = await pool.query(`Select * from player where pr_token = $1`,[token]);
            let dbUsers = dbResult.rows;
            if (!dbUsers.length)
                return { status: 403, result: {msg:"Invalid authentication!"}} ;
            let user = dbUserToUser(dbUsers[0]);
            return { status: 200, result: user} ;
        } catch (err) {
            console.log(err);
            return { status: 500, result: err };
        }
    }
}

module.exports = User;
