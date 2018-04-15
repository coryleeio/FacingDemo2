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
DROP TABLE IF EXISTS `token_prototypes`;
CREATE TABLE IF NOT EXISTS `token_prototypes` (
	`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`unique_identifier_id`	TEXT NOT NULL UNIQUE,
	`behaviour_prototype_id`	INTEGER NOT NULL,
	`equipment_prototype_id`	INTEGER NOT NULL,
	`inventory_prototype_id`	INTEGER NOT NULL,
	`motor_prototype_id`	INTEGER NOT NULL,
	`persona_prototype_id`	INTEGER NOT NULL,
	`trigger_behaviour_prototype_id`	INTEGER NOT NULL,
	`view_prototype_id`	INTEGER NOT NULL
);
INSERT INTO `token_prototypes` (id,unique_identifier_id,behaviour_prototype_id,equipment_prototype_id,inventory_prototype_id,motor_prototype_id,persona_prototype_id,trigger_behaviour_prototype_id,view_prototype_id) VALUES (1,'Poncy',1,2,1,4,2,2,2);
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
DROP VIEW IF EXISTS `token_prototypes_rendered`;
CREATE VIEW token_prototypes_rendered as
SELECT    
                  unique_identifier_id, 
                  behaviour_prototypes.class_name AS behaviour_class_name, 
                  equipment_prototypes.class_name AS equipment_class_name,
                  inventory_prototypes.class_name as inventory_class_name,
                  motor_prototypes.class_name as motor_class_name,
                  persona_prototypes.class_name as persona_class_name,
                  trigger_behaviour_prototypes.class_name as trigger_behaviour_class_name,
                  view_prototypes.class_name as view_class_name
        FROM      token_prototypes 
        LEFT JOIN behaviour_prototypes, 
                  equipment_prototypes, 
                  inventory_prototypes,
                  motor_prototypes,
                  persona_prototypes,
                  trigger_behaviour_prototypes,
                  view_prototypes
        WHERE     behaviour_prototypes.id = token_prototypes.behaviour_prototype_id 
        AND       equipment_prototypes.id = token_prototypes.equipment_prototype_id
        AND       inventory_prototypes.id = token_prototypes.inventory_prototype_id
        AND       motor_prototypes.id = token_prototypes.motor_prototype_id
        AND       persona_prototypes.id = token_prototypes.persona_prototype_id
        AND       trigger_behaviour_prototypes.id = token_prototypes.trigger_behaviour_prototype_id
        AND       view_prototypes.id = token_prototypes.view_prototype_id LIMIT 0, 50000;
COMMIT;
