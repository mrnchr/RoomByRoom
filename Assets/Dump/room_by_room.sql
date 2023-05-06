create table profile
(
    name          TEXT    default '',
    rm_count      INTEGER default 0,
    pl_race       INTEGER default 0,
    pl_health     INTEGER default 100,
    pl_speed      INTEGER default 5,
    pl_jump_force INTEGER default 5,
    rm_type       INTEGER default 0,
    rm_race       INTEGER default 0,
    primary key (name),
    check (pl_health >= 0),
    check (pl_jump_force >= 0),
    check (pl_race >= 0 AND pl_race < 4),
    check (pl_speed >= 0),
    check (rm_count >= 0),
    check (rm_race >= 0 AND rm_race < 4),
    check (rm_type >= 0 AND rm_type < 3)
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
        on update cascade on delete cascade,
    check (item_type >= 0 AND item_type < 3)
);

create table phys_damage
(
    id           INTEGER,
    profile_name TEXT,
    point        INTEGER,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade,
    check (point >= 0)
);

create table phys_protection
(
    id           INTEGER,
    profile_name TEXT,
    point        INTEGER,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade,
    check (point >= 0)
);

create table shape
(
    id           INTEGER,
    profile_name TEXT,
    pref_index   INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade,
    check (pref_index >= -1)
);

create table weapon
(
    id           INTEGER,
    profile_name TEXT,
    weapon_type  INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade,
    check (weapon_type >= 0 AND weapon_type < 4)
);

create table armor
(
    id           INTEGER,
    profile_name TEXT,
    armor_type   INTEGER default 0,
    primary key (id, profile_name),
    foreign key (profile_name) references profile
        on update cascade on delete cascade,
    check (armor_type >= 0 AND armor_type < 6)
);