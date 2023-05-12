create table profile
(
    name          TEXT    default '',
    rm_count      INTEGER default 0,
    pl_race       INTEGER default 0,
    pl_health     REAL default 0,
    pl_max_health REAL default 0,
    pl_speed      REAL default 5,
    pl_jump_force REAL default 5,
    rm_type       INTEGER default 0,
    rm_race       INTEGER default 0,
    primary key (name)
);

create table equipped
(
    id           INTEGER,
    profile_name TEXT,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);

create table item
(
    id           INTEGER,
    profile_name TEXT,
    item_type    INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);

create table phys_damage
(
    id           INTEGER,
    profile_name TEXT,
    point        REAL,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);

create table phys_protection
(
    id           INTEGER,
    profile_name TEXT,
    point        REAL,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);

create table shape
(
    id           INTEGER,
    profile_name TEXT,
    pref_index   INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);

create table weapon
(
    id           INTEGER,
    profile_name TEXT,
    weapon_type  INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);

create table armor
(
    id           INTEGER,
    profile_name TEXT,
    armor_type   INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade
);