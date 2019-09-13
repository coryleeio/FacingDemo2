
BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Entities";
CREATE TABLE "Entities" (
	"Identifier"   TEXT,
	"Name"         TEXT,
	"Race"  TEXT,
	"Level"      INTEGER,
	"ViewOverride" TEXT
);

DROP TABLE IF EXISTS "Entities_EquipmentTables";
CREATE TABLE IF NOT EXISTS "Entities_EquipmentTables" (
    "Identifier"    TEXT,
    "Table"   TEXT
);

DROP TABLE IF EXISTS "Entities_InventoryTables";
CREATE TABLE IF NOT EXISTS "Entities_InventoryTables" (
    "Identifier"    TEXT,
    "Table"   TEXT
);

INSERT INTO "Entities"                 VALUES("ENTITY_PONCY", "entity.player.name.default", "ENTITY_TYPE_HUMAN",1, "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_PONCY", "PONCY_KIT");

INSERT INTO "Entities"                 VALUES("ENTITY_MASLOW", "entity.dog.name.default", "ENTITY_TYPE_DOG",1, "");

INSERT INTO "Entities"                 VALUES("ENTITY_GIANT_BEE", "entity.bee.name", "ENTITY_TYPE_GIANT_BEE",1, "");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_GIANT_BEE", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_QUEEN_BEE", "NAMETABLE_QUEEN_BEES", "ENTITY_TYPE_GIANT_BEE",1, "VIEW_LARGE_BEE");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_QUEEN_BEE", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_SKELETON", "entity.skeleton.name", "ENTITY_TYPE_SKELETON",1, "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_HUMANOID_CLOTHING");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_GHOST", "entity.ghost.name", "ENTITY_TYPE_GHOST",1, "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_HUMANOID_CLOTHING");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_ANIMATED_WEAPON", "entity.animated.weapon.name", "ENTITY_TYPE_GHOST",1, "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_ANIMATED_WEAPON", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_ANIMATED_WEAPON", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_STAIRS_UP", "entity.stairs.up.name", "ENTITY_TYPE_STAIRS",0, "");
INSERT INTO "Entities"                 VALUES("ENTITY_WELL", "entity.well.name", "ENTITY_TYPE_WELL", 0, "");
INSERT INTO "Entities"                 VALUES("ENTITY_STAIRS_DOWN", "entity.stairs.down.name", "ENTITY_TYPE_STAIRS",0, "VIEW_STAIRCASE_DOWN");
INSERT INTO "Entities"                 VALUES("ENTITY_GROUND_DROP", "", "ENTITY_TYPE_GROUND_DROP",0, "");

COMMIT;