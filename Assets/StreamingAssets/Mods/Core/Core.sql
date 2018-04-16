BEGIN TRANSACTION;
DROP TABLE IF EXISTS `view_prototypes`;
CREATE TABLE IF NOT EXISTS `view_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `view_prototypes` (id,class_name) VALUES (1,'NoView');
INSERT INTO `view_prototypes` (id,class_name) VALUES (2,'SpineAnimatedView');
INSERT INTO `view_prototypes` (id,class_name) VALUES (3,'StaticSpriteView');
DROP TABLE IF EXISTS `trigger_behaviour_prototypes`;
CREATE TABLE IF NOT EXISTS `trigger_behaviour_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `trigger_behaviour_prototypes` (id,class_name) VALUES (1,'HasTriggerBehaviour');
INSERT INTO `trigger_behaviour_prototypes` (id,class_name) VALUES (2,'NoTriggerBehaviour');
DROP TABLE IF EXISTS `prototypes`;
CREATE TABLE IF NOT EXISTS `prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`type`	TEXT,
	`width`	INTEGER,
	`height`	INTEGER,
	`shape`	INTEGER,
	`behaviour_prototype_id`	INTEGER,
	`equipment_prototype_id`	INTEGER,
	`inventory_prototype_id`	INTEGER,
	`motor_prototype_id`	INTEGER,
	`persona_prototype_id`	INTEGER,
	`trigger_behaviour_prototype_id`	INTEGER,
	`view_prototype_id`	INTEGER,
	`item_view_prototype_id`	INTEGER,
	`item_properties_prototype_id`	INTEGER
);
INSERT INTO `prototypes` (id,unique_identifier_id,type,width,height,shape,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,view_prototype_id,item_view_prototype_id,item_properties_prototype_id) VALUES (1,'Poncy','token',1,1,0,1,2,1,4,2,2,2,NULL,NULL);
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
DROP TABLE IF EXISTS `item_properties_prototypes`;
CREATE TABLE IF NOT EXISTS `item_properties_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
);
DROP TABLE IF EXISTS `inventory_prototypes`;
CREATE TABLE IF NOT EXISTS `inventory_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`class_name`	TEXT NOT NULL
);
INSERT INTO `inventory_prototypes` (id,class_name) VALUES (1,'HasInventory');
INSERT INTO `inventory_prototypes` (id,class_name) VALUES (2,'HasNoInventory');
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
DROP VIEW IF EXISTS `token_prototypes`;
CREATE VIEW token_prototypes AS 
SELECT    unique_identifier_id, 
          width,
		  height,
		  shape,
          behaviour_prototypes.class_name         AS behaviour_class_name, 
          equipment_prototypes.class_name         AS equipment_class_name, 
          inventory_prototypes.class_name         AS inventory_class_name, 
          motor_prototypes.class_name             AS motor_class_name, 
          persona_prototypes.class_name           AS persona_class_name, 
          trigger_behaviour_prototypes.class_name AS trigger_behaviour_class_name, 
          view_prototypes.class_name              AS view_class_name 
FROM      prototypes 
LEFT JOIN behaviour_prototypes, 
          equipment_prototypes, 
          inventory_prototypes, 
          motor_prototypes, 
          persona_prototypes, 
          trigger_behaviour_prototypes, 
          view_prototypes 
WHERE     prototypes.type = 'token' 
AND       behaviour_prototypes.id = prototypes.behaviour_prototype_id 
AND       equipment_prototypes.id = prototypes.equipment_prototype_id 
AND       inventory_prototypes.id = prototypes.inventory_prototype_id 
AND       motor_prototypes.id = prototypes.motor_prototype_id 
AND       persona_prototypes.id = prototypes.persona_prototype_id 
AND       trigger_behaviour_prototypes.id = prototypes.trigger_behaviour_prototype_id 
AND       view_prototypes.id = prototypes.view_prototype_id;
DROP VIEW IF EXISTS `item_prototypes`;
CREATE VIEW item_prototypes AS 
SELECT    unique_identifier_id 
FROM      prototypes 
LEFT JOIN item_properties_prototypes, 
          item_view_prototypes 
WHERE     prototypes.type = 'item' 
AND       item_properties_prototypes.id = prototypes.item_properties_prototype_id 
AND       item_view_prototypes.id = prototypes.item_view_prototype_id;
COMMIT;
