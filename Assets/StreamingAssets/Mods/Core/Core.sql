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
DROP TABLE IF EXISTS `room_prototypes`;
CREATE TABLE IF NOT EXISTS `room_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER NOT NULL,
	`generator`	INTEGER NOT NULL,
	`minimum_height`	INTEGER NOT NULL,
	`minimum_width`	INTEGER NOT NULL,
	`maximum_width`	INTEGER NOT NULL,
	`maximum_height`	INTEGER NOT NULL,
	`fill_tileset_id`	INTEGER NOT NULL
);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id) VALUES (1,'SimpleStoneRoom','StandardRoomGenerator',5,5,9,9,1);
DROP TABLE IF EXISTS `probability_tables`;
CREATE TABLE IF NOT EXISTS `probability_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER UNIQUE,
	`type`	TEXT NOT NULL,
	`resolution`	TEXT NOT NULL
);
INSERT INTO `probability_tables` (id,unique_identifier_id,type,resolution) VALUES (1,'ROGUE_WEAPONS','equipment','OneOf');
INSERT INTO `probability_tables` (id,unique_identifier_id,type,resolution) VALUES (2,'PONCY_SPAWN','spawn','OneOf');
DROP TABLE IF EXISTS `probability_table_entries`;
CREATE TABLE IF NOT EXISTS `probability_table_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`probability_table_id`	INTEGER NOT NULL,
	`prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL
);
INSERT INTO `probability_table_entries` (id,probability_table_id,prototype_id,weight) VALUES (1,1,1,1);
INSERT INTO `probability_table_entries` (id,probability_table_id,prototype_id,weight) VALUES (2,2,1,1);
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
DROP TABLE IF EXISTS `inventory_to_inventory_tables`;
CREATE TABLE IF NOT EXISTS `inventory_to_inventory_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`inventory_prototype_id`	INTEGER NOT NULL,
	`inventory_prototype_table_id`	INTEGER NOT NULL
);
DROP TABLE IF EXISTS `inventory_prototypes`;
CREATE TABLE IF NOT EXISTS `inventory_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `inventory_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'STARTING_MAGE_INVENTORY','HasInventory');
INSERT INTO `inventory_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'NO_INVENTORY','HasNoInventory');
DROP TABLE IF EXISTS `equipment_to_equipment_tables`;
CREATE TABLE IF NOT EXISTS `equipment_to_equipment_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`equipment_prototype_id`	INTEGER NOT NULL,
	`equipment_prototype_table_id`	INTEGER NOT NULL
);
INSERT INTO `equipment_to_equipment_tables` (id,equipment_prototype_id,equipment_prototype_table_id) VALUES (1,2,1);
DROP TABLE IF EXISTS `equipment_prototypes`;
CREATE TABLE IF NOT EXISTS `equipment_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `equipment_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'NO_EQUIPMENT','CannotWearEquipment');
INSERT INTO `equipment_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'STARTING_MAGE_EQUIPMENT','WearsEquipment');
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
CREATE VIEW token_prototypes_view AS SELECT token_prototypes.unique_identifier_id, width, height, shape, behaviour_prototypes.class_name AS behaviour_class_name, equipment_prototypes.class_name AS equipment_class_name, inventory_prototypes.class_name AS inventory_class_name, motor_prototypes.class_name AS motor_class_name, persona_prototypes.class_name AS persona_class_name, trigger_behaviour_prototypes.class_name AS trigger_behaviour_class_name, token_view_prototypes.class_name AS view_class_name FROM token_prototypes LEFT JOIN behaviour_prototypes, equipment_prototypes, inventory_prototypes, motor_prototypes, persona_prototypes, trigger_behaviour_prototypes, token_view_prototypes WHERE behaviour_prototypes.id = token_prototypes.behaviour_prototype_id AND equipment_prototypes.id = token_prototypes.equipment_prototype_id AND inventory_prototypes.id = token_prototypes.inventory_prototype_id AND motor_prototypes.id = token_prototypes.motor_prototype_id AND persona_prototypes.id = token_prototypes.persona_prototype_id AND trigger_behaviour_prototypes.id = token_prototypes.trigger_behaviour_prototype_id AND token_view_prototypes.id = token_prototypes.token_view_prototype_id;
DROP VIEW IF EXISTS `spawn_table_entries_view`;
CREATE VIEW spawn_table_entries_view AS SELECT pt.unique_identifier_id,pt.resolution,pte.weight as weight, tp.unique_identifier_id as token_prototype_unique_identifier FROM probability_tables as pt LEFT JOIN probability_table_entries as pte, token_prototypes as tp WHERE pte.probability_table_id = pt.id AND tp.id = pte.prototype_id AND pt.type = 'spawn';
DROP VIEW IF EXISTS `room_prototypes_view`;
CREATE VIEW room_prototypes_view as select room_prototypes.unique_identifier_id, generator, minimum_height, minimum_width, maximum_width, maximum_height, tile.unique_identifier_id as tileset_unique_identifier from room_prototypes left join tilesets as tile where fill_tileset_id = tile.id;
DROP VIEW IF EXISTS `loot_table_entries_view`;
CREATE VIEW loot_table_entries_view AS SELECT pt.unique_identifier_id,pt.resolution,pte.weight as weight, ip.unique_identifier_id as item_prototype_unique_identifier FROM probability_tables as pt LEFT JOIN probability_table_entries as pte, item_prototypes as ip WHERE pte.probability_table_id = pt.id AND ip.id = pte.prototype_id AND pt.type = 'loot';
DROP VIEW IF EXISTS `item_prototypes_view`;
CREATE VIEW item_prototypes_view AS SELECT unique_identifier_id FROM item_prototypes LEFT JOIN item_properties_prototypes, item_view_prototypes WHERE item_properties_prototypes.id = item_prototypes.item_properties_prototype_id AND item_view_prototypes.id = item_prototypes.item_view_prototype_id;
DROP VIEW IF EXISTS `inventory_to_inventory_table_view`;
CREATE VIEW inventory_to_inventory_table_view       AS 
SELECT    inventory_prototypes.unique_identifier_id AS inventory_unique_identifier, 
          probability_tables.unique_identifier_id   AS probability_table_unique_identifier 
FROM      inventory_to_inventory_tables 
LEFT JOIN probability_tables, 
          inventory_prototypes 
WHERE     inventory_to_inventory_tables.inventory_prototype_table_id = probability_tables.id 
AND       probability_tables.type = 'inventory' 
AND       inventory_to_inventory_tables.inventory_prototype_id = inventory_prototypes.id;
DROP VIEW IF EXISTS `equipment_to_equipment_table_view`;
CREATE VIEW equipment_to_equipment_table_view       AS 
SELECT    equipment_prototypes.unique_identifier_id AS equipment_unique_identifier, 
          probability_tables.unique_identifier_id   AS probability_table_unique_identifier 
FROM      equipment_to_equipment_tables 
LEFT JOIN probability_tables, 
          equipment_prototypes 
WHERE     equipment_to_equipment_tables.equipment_prototype_table_id = probability_tables.id 
AND       probability_tables.type = 'equipment' 
AND       equipment_to_equipment_tables.equipment_prototype_id = equipment_prototypes.id;
DROP VIEW IF EXISTS `equipment_table_entries_view`;
CREATE VIEW equipment_table_entries_view AS SELECT pt.unique_identifier_id,pt.resolution,pte.weight as weight, ip.unique_identifier_id as item_prototype_unique_identifier FROM probability_tables as pt LEFT JOIN probability_table_entries as pte, item_prototypes as ip WHERE pte.probability_table_id = pt.id AND ip.id = pte.prototype_id AND pt.type = 'equipment';
COMMIT;
