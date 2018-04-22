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
	`token_view_prototype_id`	INTEGER NOT NULL,
	FOREIGN KEY(`trigger_behaviour_prototype_id`) REFERENCES `trigger_behaviour_prototypes`,
	FOREIGN KEY(`persona_prototype_id`) REFERENCES `persona_prototypes`,
	FOREIGN KEY(`motor_prototype_id`) REFERENCES `motor_prototypes`,
	FOREIGN KEY(`equipment_prototype_id`) REFERENCES `equipment_prototypes`,
	FOREIGN KEY(`behaviour_prototype_id`) REFERENCES `behaviour_prototypes`,
	FOREIGN KEY(`inventory_prototype_id`) REFERENCES `inventory_prototypes`
);
INSERT INTO `token_prototypes` (id,unique_identifier_id,type,width,height,shape,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,token_view_prototype_id) VALUES (1,'Poncy','token',1,1,0,1,2,1,4,2,2,1);
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
DROP TABLE IF EXISTS `item_prototypes`;
CREATE TABLE IF NOT EXISTS `item_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`item_properties_prototype_id`	INTEGER NOT NULL,
	`item_view_prototype_id`	INTEGER NOT NULL,
	FOREIGN KEY(`item_properties_prototype_id`) REFERENCES `item_properties_prototypes`
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
DROP VIEW IF EXISTS `token_prototypes_view`;
CREATE VIEW token_prototypes_view AS SELECT unique_identifier_id, width, height, shape, behaviour_prototypes.class_name AS behaviour_class_name, equipment_prototypes.class_name AS equipment_class_name, inventory_prototypes.class_name AS inventory_class_name, motor_prototypes.class_name AS motor_class_name, persona_prototypes.class_name AS persona_class_name, trigger_behaviour_prototypes.class_name AS trigger_behaviour_class_name, token_view_prototypes.class_name AS view_class_name FROM token_prototypes LEFT JOIN behaviour_prototypes, equipment_prototypes, inventory_prototypes, motor_prototypes, persona_prototypes, trigger_behaviour_prototypes, token_view_prototypes WHERE behaviour_prototypes.id = token_prototypes.behaviour_prototype_id AND equipment_prototypes.id = token_prototypes.equipment_prototype_id AND inventory_prototypes.id = token_prototypes.inventory_prototype_id AND motor_prototypes.id = token_prototypes.motor_prototype_id AND persona_prototypes.id = token_prototypes.persona_prototype_id AND trigger_behaviour_prototypes.id = token_prototypes.trigger_behaviour_prototype_id AND token_view_prototypes.id = token_prototypes.token_view_prototype_id;
DROP VIEW IF EXISTS `item_prototypes_view`;
CREATE VIEW item_prototypes_view AS SELECT unique_identifier_id FROM item_prototypes LEFT JOIN item_properties_prototypes, item_view_prototypes WHERE item_properties_prototypes.id = item_prototypes.item_properties_prototype_id AND item_view_prototypes.id = item_prototypes.item_view_prototype_id;
COMMIT;
