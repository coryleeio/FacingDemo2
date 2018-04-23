BEGIN TRANSACTION;
DROP TABLE IF EXISTS `trigger_behaviour_prototypes`;
CREATE TABLE IF NOT EXISTS `trigger_behaviour_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `trigger_behaviour_prototypes` (id,class_name) VALUES (1,'HasTriggerBehaviour');
INSERT INTO `trigger_behaviour_prototypes` (id,class_name) VALUES (2,'NoTriggerBehaviour');
DROP TABLE IF EXISTS `token_view_prototypes`;
CREATE TABLE IF NOT EXISTS `token_view_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `token_view_prototypes` (id,class_name) VALUES (1,'NoView');
INSERT INTO `token_view_prototypes` (id,class_name) VALUES (2,'SpineAnimatedView');
INSERT INTO `token_view_prototypes` (id,class_name) VALUES (3,'StaticSpriteView');
DROP TABLE IF EXISTS `token_prototypes`;
CREATE TABLE IF NOT EXISTS `token_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`type`	TEXT NOT NULL,
	`width`	INTEGER NOT NULL,
	`height`	INTEGER NOT NULL,
	`shape`	INTEGER NOT NULL,
	`behaviour_prototype_id`	INTEGER NOT NULL,
	`equipment_prototype_id`	INTEGER NOT NULL,
	`inventory_prototype_id`	INTEGER NOT NULL,
	`motor_prototype_id`	INTEGER NOT NULL,
	`persona_prototype_id`	INTEGER NOT NULL,
	`trigger_behaviour_prototype_id`	INTEGER NOT NULL,
	`token_view_prototype_id`	INTEGER NOT NULL
);
INSERT INTO `token_prototypes` (id,unique_identifier_id,type,width,height,shape,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,token_view_prototype_id) VALUES (1,'Poncy','token',1,1,0,1,2,1,4,2,2,1);
DROP TABLE IF EXISTS `tilesets`;
CREATE TABLE IF NOT EXISTS `tilesets` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`floor_sprite`	TEXT,
	`tee_sprite`	TEXT,
	`north_corner_sprite`	TEXT,
	`east_corner_sprite`	TEXT,
	`south_corner_sprite`	TEXT,
	`west_corner_sprite`	TEXT,
	`north_east_wall_sprite`	TEXT,
	`south_east_wall_sprite`	TEXT,
	`south_west_wall_sprite`	TEXT,
	`north_west_wall_sprite`	TEXT,
	`north_east_tee_sprite`	TEXT,
	`south_east_tee_sprite`	TEXT,
	`south_west_tee_sprite`	TEXT,
	`north_west_tee_sprite`	TEXT
);
INSERT INTO `tilesets` (id,unique_identifier_id,floor_sprite,tee_sprite,north_corner_sprite,east_corner_sprite,south_corner_sprite,west_corner_sprite,north_east_wall_sprite,south_east_wall_sprite,south_west_wall_sprite,north_west_wall_sprite,north_east_tee_sprite,south_east_tee_sprite,south_west_tee_sprite,north_west_tee_sprite) VALUES (1,'Stone','StoneFloor','StoneTee','StoneCornerNorth','StoneCornerEast','StoneCornerSouth','StoneCornerWest','StoneNorthEastWall','StoneSouthEastWall','StoneSouthWestWall','StoneNorthWestWall','StoneNorthEastTee','StoneSouthEastTee','StoneSouthWestTee','StoneNorthWestTee');
DROP TABLE IF EXISTS `persona_prototypes`;
CREATE TABLE IF NOT EXISTS `persona_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `persona_prototypes` (id,class_name) VALUES (1,'ObjectPersona');
INSERT INTO `persona_prototypes` (id,class_name) VALUES (2,'PawnPersona');
DROP TABLE IF EXISTS `motor_prototypes`;
CREATE TABLE IF NOT EXISTS `motor_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `motor_prototypes` (id,class_name) VALUES (1,'FlyingMotor');
INSERT INTO `motor_prototypes` (id,class_name) VALUES (2,'GhostMotor');
INSERT INTO `motor_prototypes` (id,class_name) VALUES (3,'NoMotor');
INSERT INTO `motor_prototypes` (id,class_name) VALUES (4,'WalkingMotor');
DROP TABLE IF EXISTS `loot_tables`;
CREATE TABLE IF NOT EXISTS `loot_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER UNIQUE
);
INSERT INTO `loot_tables` (id,unique_identifier_id) VALUES (1,'LOOT_ROGUE_WEAPONS');
DROP TABLE IF EXISTS `loot_table_entries`;
CREATE TABLE IF NOT EXISTS `loot_table_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`loot_table_id`	INTEGER NOT NULL,
	`item_prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL
);
INSERT INTO `loot_table_entries` (id,loot_table_id,item_prototype_id,weight) VALUES (1,1,1,1);
DROP TABLE IF EXISTS `item_view_prototypes`;
CREATE TABLE IF NOT EXISTS `item_view_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
);
INSERT INTO `item_view_prototypes` (id) VALUES (1);
DROP TABLE IF EXISTS `item_prototypes`;
CREATE TABLE IF NOT EXISTS `item_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`item_properties_prototype_id`	INTEGER NOT NULL,
	`item_view_prototype_id`	INTEGER NOT NULL
);
INSERT INTO `item_prototypes` (id,unique_identifier_id,item_properties_prototype_id,item_view_prototype_id) VALUES (1,'Shield of Amalure',1,1);
DROP TABLE IF EXISTS `item_properties_prototypes`;
CREATE TABLE IF NOT EXISTS `item_properties_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
);
INSERT INTO `item_properties_prototypes` (id) VALUES (1);
DROP TABLE IF EXISTS `inventory_prototypes`;
CREATE TABLE IF NOT EXISTS `inventory_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `inventory_prototypes` (id,class_name) VALUES (1,'HasInventory');
INSERT INTO `inventory_prototypes` (id,class_name) VALUES (2,'HasNoInventory');
DROP TABLE IF EXISTS `equipment_tables`;
CREATE TABLE IF NOT EXISTS `equipment_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER UNIQUE
);
DROP TABLE IF EXISTS `equipment_table_entries`;
CREATE TABLE IF NOT EXISTS `equipment_table_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`equipment_table_id`	INTEGER NOT NULL,
	`item_prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL
);
DROP TABLE IF EXISTS `equipment_prototypes`;
CREATE TABLE IF NOT EXISTS `equipment_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `equipment_prototypes` (id,class_name) VALUES (1,'CannotWearEquipment');
INSERT INTO `equipment_prototypes` (id,class_name) VALUES (2,'WearsEquipment');
DROP TABLE IF EXISTS `behaviour_prototypes`;
CREATE TABLE IF NOT EXISTS `behaviour_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `behaviour_prototypes` (id,class_name) VALUES (1,'Player');
INSERT INTO `behaviour_prototypes` (id,class_name) VALUES (2,'NoBehaviour');
INSERT INTO `behaviour_prototypes` (id,class_name) VALUES (3,'AISlime');
INSERT INTO `behaviour_prototypes` (id,class_name) VALUES (4,'AIBrute');
DROP VIEW IF EXISTS `token_prototypes_view`;
CREATE VIEW token_prototypes_view AS SELECT unique_identifier_id, width, height, shape, behaviour_prototypes.class_name AS behaviour_class_name, equipment_prototypes.class_name AS equipment_class_name, inventory_prototypes.class_name AS inventory_class_name, motor_prototypes.class_name AS motor_class_name, persona_prototypes.class_name AS persona_class_name, trigger_behaviour_prototypes.class_name AS trigger_behaviour_class_name, token_view_prototypes.class_name AS view_class_name FROM token_prototypes LEFT JOIN behaviour_prototypes, equipment_prototypes, inventory_prototypes, motor_prototypes, persona_prototypes, trigger_behaviour_prototypes, token_view_prototypes WHERE behaviour_prototypes.id = token_prototypes.behaviour_prototype_id AND equipment_prototypes.id = token_prototypes.equipment_prototype_id AND inventory_prototypes.id = token_prototypes.inventory_prototype_id AND motor_prototypes.id = token_prototypes.motor_prototype_id AND persona_prototypes.id = token_prototypes.persona_prototype_id AND trigger_behaviour_prototypes.id = token_prototypes.trigger_behaviour_prototype_id AND token_view_prototypes.id = token_prototypes.token_view_prototype_id;
DROP VIEW IF EXISTS `loot_table_entries_view`;
CREATE VIEW loot_table_entries_view AS SELECT lt.unique_identifier_id,lte.weight as weight, ip.unique_identifier_id as item_prototype_unique_identifier FROM loot_tables as lt LEFT JOIN loot_table_entries as lte,item_prototypes as ip where lte.loot_table_id = lt.id AND ip.id = lte.item_prototype_id;
DROP VIEW IF EXISTS `item_prototypes_view`;
CREATE VIEW item_prototypes_view AS SELECT unique_identifier_id FROM item_prototypes LEFT JOIN item_properties_prototypes, item_view_prototypes WHERE item_properties_prototypes.id = item_prototypes.item_properties_prototype_id AND item_view_prototypes.id = item_prototypes.item_view_prototype_id;
DROP VIEW IF EXISTS `equipment_table_entries_view`;
CREATE VIEW equipment_table_entries_view AS SELECT et.unique_identifier_id,ete.weight as weight, ip.unique_identifier_id as item_prototype_unique_identifier FROM equipment_tables as et LEFT JOIN equipment_table_entries as ete,item_prototypes as ip where ete.equipment_table_id = et.id AND ip.id = ete.item_prototype_id;
COMMIT;
