BEGIN TRANSACTION;
DROP TABLE IF EXISTS `trigger_behaviour_prototypes`;
CREATE TABLE IF NOT EXISTS `trigger_behaviour_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `trigger_behaviour_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'TRIGGER_BEHAVIOUR_IS_TRIGGER','HasTriggerBehaviour');
INSERT INTO `trigger_behaviour_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'TRIGGER_BEHAVIOUR_IS_NOT_TRIGGER','NoTriggerBehaviour');
DROP TABLE IF EXISTS `token_view_prototypes`;
CREATE TABLE IF NOT EXISTS `token_view_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL,
	`sprite_id`	TEXT
);
INSERT INTO `token_view_prototypes` (id,unique_identifier_id,class_name,sprite_id) VALUES (1,'TOKEN_VIEW_NONE','NoView',NULL);
INSERT INTO `token_view_prototypes` (id,unique_identifier_id,class_name,sprite_id) VALUES (2,'TOKEN_VIEW_SPINE_ANIMATED','SpineAnimatedView',NULL);
INSERT INTO `token_view_prototypes` (id,unique_identifier_id,class_name,sprite_id) VALUES (3,'TOKEN_VIEW_STATIC_SPRITE','StaticSpriteView','Marker');
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
INSERT INTO `token_prototypes` (id,unique_identifier_id,type,width,height,shape,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,token_view_prototype_id) VALUES (1,'Poncy','token',1,1,0,1,2,1,4,2,2,3);
INSERT INTO `token_prototypes` (id,unique_identifier_id,type,width,height,shape,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,token_view_prototype_id) VALUES (2,'Giant Bee','token',1,1,0,4,2,1,4,2,2,3);
INSERT INTO `token_prototypes` (id,unique_identifier_id,type,width,height,shape,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,token_view_prototype_id) VALUES (3,'Queen Bee','token',1,1,0,4,2,1,4,2,2,3);
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
DROP TABLE IF EXISTS `spawn_tables`;
CREATE TABLE IF NOT EXISTS `spawn_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER UNIQUE,
	`resolution`	TEXT NOT NULL,
	`available_on_levels`	TEXT,
	`mandatory`	INTEGER NOT NULL,
	`room_with_tag_constraint`	TEXT,
	`is_unique`	INTEGER NOT NULL
);
INSERT INTO `spawn_tables` (id,unique_identifier_id,resolution,available_on_levels,mandatory,room_with_tag_constraint,is_unique) VALUES (1,'GIANT_BEES','AnyOf','1,2',1,NULL,1);
DROP TABLE IF EXISTS `spawn_table_prototype_entries`;
CREATE TABLE IF NOT EXISTS `spawn_table_prototype_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`spawn_table_id`	INTEGER NOT NULL,
	`token_prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL,
	`number_of_rolls`	INTEGER NOT NULL
);
INSERT INTO `spawn_table_prototype_entries` (id,spawn_table_id,token_prototype_id,weight,number_of_rolls) VALUES (1,1,1,100,1);
DROP TABLE IF EXISTS `spawn_table_entries`;
CREATE TABLE IF NOT EXISTS `spawn_table_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`spawn_table_id`	INTEGER NOT NULL,
	`token_prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL,
	`number_of_rolls`	INTEGER NOT NULL
);
INSERT INTO `spawn_table_entries` (id,spawn_table_id,token_prototype_id,weight,number_of_rolls) VALUES (1,1,2,100,4);
INSERT INTO `spawn_table_entries` (id,spawn_table_id,token_prototype_id,weight,number_of_rolls) VALUES (2,1,3,5,1);
DROP TABLE IF EXISTS `room_prototypes`;
CREATE TABLE IF NOT EXISTS `room_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER NOT NULL,
	`generator`	INTEGER NOT NULL,
	`minimum_height`	INTEGER NOT NULL,
	`minimum_width`	INTEGER NOT NULL,
	`maximum_width`	INTEGER NOT NULL,
	`maximum_height`	INTEGER NOT NULL,
	`fill_tileset_id`	INTEGER NOT NULL,
	`tags`	TEXT,
	`available_on_levels`	TEXT,
	`mandatory`	INTEGER NOT NULL,
	`is_unique`	INTEGER NOT NULL
);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id,tags,available_on_levels,mandatory,is_unique) VALUES (1,'SIMPLE_STONE_ROOM_1','StandardRoomGenerator',4,4,9,9,1,NULL,'1,2,3',1,1);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id,tags,available_on_levels,mandatory,is_unique) VALUES (2,'SIMPLE_STONE_ROOM_2','StandardRoomGenerator',4,4,9,9,1,NULL,'1,2,3',1,1);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id,tags,available_on_levels,mandatory,is_unique) VALUES (3,'SIMPLE_STONE_ROOM_3','StandardRoomGenerator',4,4,9,9,1,NULL,'1,2,3',1,1);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id,tags,available_on_levels,mandatory,is_unique) VALUES (4,'SIMPLE_STONE_ROOM_4','StandardRoomGenerator',4,4,9,9,1,NULL,'1,2,3',1,1);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id,tags,available_on_levels,mandatory,is_unique) VALUES (5,'SIMPLE_STONE_ROOM_5','StandardRoomGenerator',4,4,9,9,1,NULL,'1,2,3',1,1);
INSERT INTO `room_prototypes` (id,unique_identifier_id,generator,minimum_height,minimum_width,maximum_width,maximum_height,fill_tileset_id,tags,available_on_levels,mandatory,is_unique) VALUES (6,'SIMPLE_STONE_ROOM_6','StandardRoomGenerator',4,4,9,9,1,NULL,'1,2,3',1,1);
DROP TABLE IF EXISTS `room_prototype_entries`;
CREATE TABLE IF NOT EXISTS `room_prototype_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	``	INTEGER
);
DROP TABLE IF EXISTS `persona_prototypes`;
CREATE TABLE IF NOT EXISTS `persona_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `persona_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'PERSONA_OBJECT','ObjectPersona');
INSERT INTO `persona_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'PERSONA_PAWN','PawnPersona');
DROP TABLE IF EXISTS `motor_prototypes`;
CREATE TABLE IF NOT EXISTS `motor_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `motor_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'MOTOR_FLYING','FlyingMotor');
INSERT INTO `motor_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'MOTOR_GHOST','GhostMotor');
INSERT INTO `motor_prototypes` (id,unique_identifier_id,class_name) VALUES (3,'MOTOR_NONE','NoMotor');
INSERT INTO `motor_prototypes` (id,unique_identifier_id,class_name) VALUES (4,'MOTOR_WALKING','WalkingMotor');
DROP TABLE IF EXISTS `level_prototypes`;
CREATE TABLE IF NOT EXISTS `level_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER NOT NULL UNIQUE,
	`default_spawn_table_id`	INTEGER NOT NULL,
	`default_tileset_id`	INTEGER NOT NULL
);
INSERT INTO `level_prototypes` (id,unique_identifier_id,default_spawn_table_id,default_tileset_id) VALUES (1,'LEVEL_1_DEFAULT',1,1);
DROP TABLE IF EXISTS `item_view_prototypes`;
CREATE TABLE IF NOT EXISTS `item_view_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE
);
INSERT INTO `item_view_prototypes` (id,unique_identifier_id) VALUES (1,'ITEM_VIEW_NONE');
DROP TABLE IF EXISTS `item_prototypes`;
CREATE TABLE IF NOT EXISTS `item_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`item_properties_prototype_id`	INTEGER NOT NULL,
	`item_view_prototype_id`	INTEGER NOT NULL
);
INSERT INTO `item_prototypes` (id,unique_identifier_id,item_properties_prototype_id,item_view_prototype_id) VALUES (1,'SHIELD_OF_AMALURE_NO_ENCHANTMENTS',1,1);
DROP TABLE IF EXISTS `item_properties_prototypes`;
CREATE TABLE IF NOT EXISTS `item_properties_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
);
INSERT INTO `item_properties_prototypes` (id) VALUES (1);
DROP TABLE IF EXISTS `inventory_tables`;
CREATE TABLE IF NOT EXISTS `inventory_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER UNIQUE,
	`resolution`	TEXT NOT NULL
);
DROP TABLE IF EXISTS `inventory_table_entries`;
CREATE TABLE IF NOT EXISTS `inventory_table_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`inventory_table_id`	INTEGER NOT NULL,
	`item_prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL,
	`number_of_rolls`	INTEGER NOT NULL
);
DROP TABLE IF EXISTS `inventory_prototypes_to_inventory_tables`;
CREATE TABLE IF NOT EXISTS `inventory_prototypes_to_inventory_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`inventory_prototype_id`	INTEGER NOT NULL,
	`inventory_table_id`	INTEGER NOT NULL
);
DROP TABLE IF EXISTS `inventory_prototypes`;
CREATE TABLE IF NOT EXISTS `inventory_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `inventory_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'STARTING_MAGE_INVENTORY','HasInventory');
INSERT INTO `inventory_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'NO_INVENTORY','HasNoInventory');
DROP TABLE IF EXISTS `equipment_tables`;
CREATE TABLE IF NOT EXISTS `equipment_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	INTEGER UNIQUE,
	`resolution`	TEXT NOT NULL
);
INSERT INTO `equipment_tables` (id,unique_identifier_id,resolution) VALUES (1,'ROGUE_WEAPONS_EQUIPMENT','OneOf');
DROP TABLE IF EXISTS `equipment_table_entries`;
CREATE TABLE IF NOT EXISTS `equipment_table_entries` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`equipment_table_id`	INTEGER NOT NULL,
	`item_prototype_id`	INTEGER NOT NULL,
	`weight`	INTEGER NOT NULL,
	`number_of_rolls`	INTEGER NOT NULL
);
INSERT INTO `equipment_table_entries` (id,equipment_table_id,item_prototype_id,weight,number_of_rolls) VALUES (1,1,1,100,1);
DROP TABLE IF EXISTS `equipment_prototypes_to_equipment_tables`;
CREATE TABLE IF NOT EXISTS `equipment_prototypes_to_equipment_tables` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`equipment_prototype_id`	INTEGER NOT NULL,
	`equipment_table_id`	INTEGER NOT NULL
);
INSERT INTO `equipment_prototypes_to_equipment_tables` (id,equipment_prototype_id,equipment_table_id) VALUES (1,2,1);
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
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `behaviour_prototypes` (id,unique_identifier_id,class_name) VALUES (1,'BEHAVIOUR_PLAYER','Player');
INSERT INTO `behaviour_prototypes` (id,unique_identifier_id,class_name) VALUES (2,'BEHAVIOUR_NONE','NoBehaviour');
INSERT INTO `behaviour_prototypes` (id,unique_identifier_id,class_name) VALUES (3,'BEHAVIOUR_AI_SLIME','AISlime');
INSERT INTO `behaviour_prototypes` (id,unique_identifier_id,class_name) VALUES (4,'BEHAVIOUR_AI_BRUTE','AIBrute');
DROP VIEW IF EXISTS `token_prototypes_view`;
CREATE VIEW token_prototypes_view AS 
SELECT    token_prototypes.unique_identifier_id, 
          width, 
          height, 
          shape, 
          behaviour_prototypes.unique_identifier_id         AS behaviour_unique_identifier, 
          equipment_prototypes.unique_identifier_id         AS equipment_unique_identifier, 
          inventory_prototypes.unique_identifier_id         AS inventory_unique_identifier, 
          motor_prototypes.unique_identifier_id             AS motor_unique_identifier, 
          persona_prototypes.unique_identifier_id           AS persona_unique_identifier, 
          trigger_behaviour_prototypes.unique_identifier_id AS trigger_behaviour_unique_identifier, 
          token_view_prototypes.unique_identifier_id        AS view_unique_identifier 
FROM      token_prototypes 
LEFT JOIN behaviour_prototypes, 
          equipment_prototypes, 
          inventory_prototypes, 
          motor_prototypes, 
          persona_prototypes, 
          trigger_behaviour_prototypes, 
          token_view_prototypes 
WHERE     behaviour_prototypes.id = token_prototypes.behaviour_prototype_id 
AND       equipment_prototypes.id = token_prototypes.equipment_prototype_id 
AND       inventory_prototypes.id = token_prototypes.inventory_prototype_id 
AND       motor_prototypes.id = token_prototypes.motor_prototype_id 
AND       persona_prototypes.id = token_prototypes.persona_prototype_id 
AND       trigger_behaviour_prototypes.id = token_prototypes.trigger_behaviour_prototype_id 
AND       token_view_prototypes.id = token_prototypes.token_view_prototype_id;
DROP VIEW IF EXISTS `spawn_table_entries_view`;
CREATE VIEW spawn_table_entries_view AS SELECT st.id as spawn_table_id, st.unique_identifier_id,st.resolution,ste.weight as weight, tp.id as token_prototype_id, tp.unique_identifier_id as token_prototype_unique_identifier, ste.number_of_rolls as number_of_rolls FROM spawn_tables as st LEFT JOIN spawn_table_entries as ste, token_prototypes as tp WHERE ste.spawn_table_id = st.id AND tp.id = ste.token_prototype_id;
DROP VIEW IF EXISTS `room_prototypes_view`;
CREATE VIEW room_prototypes_view AS SELECT room_prototypes.id AS room_prototype_id, room_prototypes.unique_identifier_id, generator, minimum_height, minimum_width, maximum_width, maximum_height, tile.unique_identifier_id AS tileset_unique_identifier, room_prototypes.tags, room_prototypes.available_on_levels, room_prototypes.mandatory, room_prototypes.is_unique FROM room_prototypes LEFT JOIN tilesets AS tile where fill_tileset_id = tile.id;
DROP VIEW IF EXISTS `level_prototypes_view`;
CREATE VIEW level_prototypes_view               AS 
SELECT    level_prototypes.id                   AS level_prototypes_id, 
          level_prototypes.unique_identifier_id AS level_prototypes_unique_identifier, 
          spawn_tables.id                       AS default_spawn_table_id, 
          spawn_tables.unique_identifier_id     AS default_spawn_table_unique_identifier, 
          tilesets.id                           AS default_tileset_id, 
          tilesets.unique_identifier_id         AS default_tileset_unique_identifier 
FROM      level_prototypes 
LEFT JOIN spawn_tables, 
          tilesets 
WHERE     level_prototypes.default_tileset_id = tilesets.id 
AND       level_prototypes.default_spawn_table_id = spawn_tables.id;
DROP VIEW IF EXISTS `item_prototypes_view`;
CREATE VIEW item_prototypes_view AS SELECT item_prototypes.unique_identifier_id as unique_identifier FROM item_prototypes LEFT JOIN item_properties_prototypes, item_view_prototypes WHERE item_properties_prototypes.id = item_prototypes.item_properties_prototype_id AND item_view_prototypes.id = item_prototypes.item_view_prototype_id;
DROP VIEW IF EXISTS `inventory_table_entries_view`;
CREATE VIEW inventory_table_entries_view AS SELECT it.id as inventory_table_id, it.unique_identifier_id,it.resolution,ite.weight as weight, ip.id as item_prototype_id, ip.unique_identifier_id as item_prototype_unique_identifier, ite.number_of_rolls as number_of_rolls FROM inventory_tables as it LEFT JOIN inventory_table_entries as ite, item_prototypes as ip WHERE ite.inventory_table_id = it.id AND ip.id = ite.item_prototype_id;
DROP VIEW IF EXISTS `inventory_prototype_inventory_tables_view`;
CREATE VIEW inventory_prototype_inventory_tables_view AS SELECT inventory_prototypes.id as inventory_prototype_id, inventory_prototypes.unique_identifier_id AS inventory_unique_identifier, inventory_tables.id as inventory_table_id,inventory_tables.resolution as resolution, inventory_tables.unique_identifier_id AS inventory_table_unique_identifier FROM inventory_prototypes_to_inventory_tables LEFT JOIN inventory_tables, inventory_table_entries, inventory_prototypes WHERE inventory_prototypes_to_inventory_tables.inventory_table_id = inventory_tables.id AND inventory_prototypes_to_inventory_tables.inventory_prototype_id = inventory_prototypes.id;
DROP VIEW IF EXISTS `equipment_table_entries_view`;
CREATE VIEW equipment_table_entries_view AS SELECT et.id as equipment_table_id, et.unique_identifier_id,et.resolution,ete.weight as weight, ip.id as item_prototype_id, ip.unique_identifier_id as item_prototype_unique_identifier, ete.number_of_rolls as number_of_rolls FROM equipment_tables as et LEFT JOIN equipment_table_entries as ete, item_prototypes as ip WHERE ete.equipment_table_id = et.id AND ip.id = ete.item_prototype_id;
DROP VIEW IF EXISTS `equipment_prototype_equipment_tables_view`;
CREATE VIEW equipment_prototype_equipment_tables_view AS SELECT equipment_prototypes.id as equipment_prototype_id, equipment_prototypes.unique_identifier_id AS equipment_unique_identifier, equipment_tables.id as equipment_table_id,equipment_tables.resolution as resolution, equipment_tables.unique_identifier_id AS equipment_table_unique_identifier FROM equipment_prototypes_to_equipment_tables LEFT JOIN equipment_tables, equipment_table_entries, equipment_prototypes WHERE equipment_prototypes_to_equipment_tables.equipment_table_id = equipment_tables.id AND equipment_prototypes_to_equipment_tables.equipment_prototype_id = equipment_prototypes.id;
COMMIT;
