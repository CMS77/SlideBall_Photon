
create table player(
    pr_id SERIAL not null,
    pr_name varchar(60) not null,
    pr_email varchar(200) not null,
    pr_pass varchar(200) not null, 
    pr_token VARCHAR(255),
    
    primary key (pr_id)
);

create table profile(
    pe_id SERIAL not null,
    pe_wins varchar(200) not null,
    pe_losts varchar(200) not null, 
    pe_historico varchar(200) not null,
    pe_pr_id Int Not null,

    primary key (pe_id)
);

create table leaderboard(
    lb_id SERIAL not null,
    lb_pe_id Int Not null,

    primary key (lb_id)

);

create table match(
    mh_id SERIAL not null,
    mh_points INT not null,
    mh_pr_id Int Not null,

    primary key (mh_id)
    
);

create table chat(
    ct_id SERIAL not null,
    ct_mensagens varchar(200) not null,
    ct_pr_id Int Not null,
    primary key (ct_id)
    
);

create table tourney(
    ty_id SERIAL not null,
    ty_pr_id Int Not null,
    primary key (ty_id)
    
);


--player/chat
alter table chat add constraint chat_fk_player
            foreign key (ct_pr_id) references player(pr_id) 
			ON DELETE NO ACTION ON UPDATE NO ACTION;

--player/match
alter table match add constraint match_fk_player
            foreign key (mh_pr_id) references player(pr_id) 
			ON DELETE NO ACTION ON UPDATE NO ACTION;

--player/tourney
alter table tourney add constraint tourney_fk_player
            foreign key (ty_pr_id) references player(pr_id) 
			ON DELETE NO ACTION ON UPDATE NO ACTION;

--player/profile
alter table profile add constraint profile_fk_player
            foreign key (pe_pr_id) references player(pr_id) 
			ON DELETE NO ACTION ON UPDATE NO ACTION;

--leaderboard/profile
alter table leaderboard add constraint leaderboard_fk_profile
            foreign key (lb_pe_id) references profile(pe_id) 
			ON DELETE NO ACTION ON UPDATE NO ACTION;



