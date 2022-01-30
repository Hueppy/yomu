drop database yomu;

create database yomu;
use yomu;

create table login (
       email         varchar(50)  not null,
       password_hash varchar(256) not null,
       role          int          not null,

       constraint pk_login primary key (email)
);

create table user (
       id           varchar(50) not null,
       email        varchar(50) not null,
       banned_until datetime    not null,

       constraint pk_user       primary key (id),
       constraint fk_user_login foreign key (email) references login (email) on delete cascade
);

create table community (
       id          varchar(50)   not null,
       description text          not null,

       constraint pk_community primary key (id)
);

create table post (
       id           int           not null auto_increment,
       community_id varchar(50)   not null,
       user_id      varchar(50),
       text         text          not null,
       link         varchar(2048),

       constraint pk_post           primary key (id),
       constraint fk_post_community foreign key (community_id) references community (id) on delete cascade,
       constraint fk_post_user      foreign key (user_id)      references user (id)      on delete set null
);

create table image (
       id      varchar(50) not null,
       post_id int         not null,

       constraint pk_image           primary key (id),
       constraint fk_post_image_post foreign key (post_id) references post (id) on delete cascade
);

create table comment (
       id        int         not null auto_increment,
       post_id   int         not null,
       user_id   varchar(50),
       parent_id int,
       message   text        not null,

       constraint pk_comment        primary key (id),
       constraint pk_comment_post   foreign key (post_id)   references post (id)    on delete cascade,
       constraint pk_comment_user   foreign key (user_id)   references user (id)    on delete set null,
       constraint pk_comment_parent foreign key (parent_id) references comment (id)
);

create table friend (
       befriender_id varchar(50) not null,
       friend_id     varchar(50) not null,

       constraint pk_friend            primary key (befriender_id, friend_id),
       constraint fk_friend_befriender foreign key (befriender_id)            references user (id) on delete cascade,
       constraint fk_friend_friend     foreign key (friend_id)                references user (id) on delete cascade
);

create table block (
       blocker_id varchar(50) not null,
       blockee_id varchar(50) not null,
       
       constraint pk_block         primary key (blocker_Id, blockee_id),
       constraint fk_block_blocker foreign key (blocker_id)             references user (id) on delete cascade,
       constraint fk_block_blockee foreign key (blockee_id)             references user (id) on delete cascade
);

create table user_community (
       user_id      varchar(50) not null,
       community_id varchar(50) not null,
       role         int         not null,

       constraint pk_user_community           primary key (user_id, community_id),
       constraint fk_user_community_user      foreign key (user_id)               references user (id)      on delete cascade,
       constraint fk_user_community_community foreign key (community_id)          references community (id) on delete cascade
);

create table post_rating (
       post_id int         not null,
       user_id varchar(50) not null,
       rating  int         not null,

       constraint pk_post_rating      primary key (post_id, user_id),
       constraint fk_post_rating_post foreign key (post_id)          references post (id) on delete cascade,
       constraint fk_post_rating_user foreign key (user_id)          references user (id) on delete cascade
);

create table report (
       id        int         not null auto_increment,
       sender_id varchar(50),
       comment   text        not null,
       reason    int         not null,
       send_at   datetime    not null,

       constraint pk_report        primary key (id),
       constraint fk_report_sender foreign key (sender_id) references user (id) on delete set null
);

create table report_post (
       report_id int not null,
       post_id   int not null,

       constraint pk_report_post        primary key (report_id, post_id),
       constraint fk_report_post_report foreign key (report_id)          references report (id) on delete cascade,
       constraint fk_report_post_post   foreign key (post_id)            references post (id)   on delete cascade
);

create table report_user (
       report_id int         not null,
       user_id   varchar(50) not null,

       constraint pk_report_user        primary key (report_id, user_id),
       constraint fk_report_user_report foreign key (report_id)          references report (id) on delete cascade,
       constraint fk_report_user_user   foreign key (user_id)            references user (id)   on delete cascade
);

create table report_comment (
       report_id  int not null,
       comment_id int not null,

       constraint pk_report_comment         primary key (report_id, comment_id),
       constraint fk_report_comment_report  foreign key (report_id)             references report (id)  on delete cascade,
       constraint fk_report_comment_comment foreign key (comment_id)            references comment (id) on delete cascade
);

create table message (
       id          int         not null auto_increment,
       sender_id   varchar(50) not null,
       receiver_id varchar(50) not null,
       message     text        not null,
       send_at     datetime    not null,

       constraint pk_message          primary key (id),
       constraint fk_message_sender   foreign key (sender_id)   references user (id) on delete cascade,
       constraint fk_message_receiver foreign key (receiver_id) references user (id) on delete cascade
);
