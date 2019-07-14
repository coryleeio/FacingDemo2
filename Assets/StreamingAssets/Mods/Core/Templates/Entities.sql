
BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Entities";
CREATE TABLE "Entities" (
	"Identifier"   TEXT,
	"Name"         TEXT,
	"Race"  TEXT,
	"ViewOverride" 
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

INSERT INTO "Entities"                 VALUES("ENTITY_PONCY", "entity.player.name.default", "RACE_HUMAN", "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_PONCY", "PONCY_KIT");

INSERT INTO "Entities"                 VALUES("ENTITY_MASLOW", "entity.dog.name.default", "RACE_DOG", "");

INSERT INTO "Entities"                 VALUES("ENTITY_GIANT_BEE", "entity.bee.name", "RACE_GIANT_BEE", "");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_GIANT_BEE", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_QUEEN_BEE", "NAMETABLE_QUEEN_BEES", "RACE_GIANT_BEE", "VIEW_LARGE_BEE");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_QUEEN_BEE", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_SKELETON", "entity.skeleton.name", "RACE_SKELETON", "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_HUMANOID_CLOTHING");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_GHOST", "entity.ghost.name", "RACE_GHOST", "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_HUMANOID_CLOTHING");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_ANIMATED_WEAPON", "entity.animated.weapon.name", "RACE_GHOST", "");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_ANIMATED_WEAPON", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_ANIMATED_WEAPON", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_STAIRS_UP", "entity.stairs.up.name", "RACE_STAIRS", "");
INSERT INTO "Entities"                 VALUES("ENTITY_STAIRS_DOWN", "entity.stairs.down.name", "RACE_STAIRS", "");
INSERT INTO "Entities"                 VALUES("ENTITY_GROUND_DROP", "", "RACE_GROUND_DROP", "");

COMMIT;