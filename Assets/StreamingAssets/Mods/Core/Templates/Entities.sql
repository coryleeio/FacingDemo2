
BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Entities";
CREATE TABLE "Entities" (
	"Identifier"   TEXT,
	"Name"         TEXT,
	"IsCombatant"  INTEGER,
	"DefaultWeapon" TEXT,
	"BlocksPathing" INTEGER,
	"ViewTemplate"  TEXT,
	"AIClassName"   TEXT,
	"AlwaysVisible" INTEGER,
	"Trigger"       TEXT,
	"isFloating" TEXT,
	"CastsShadow" TEXT
);

DROP TABLE IF EXISTS "Entities_Attributes";
CREATE TABLE IF NOT EXISTS "Entities_Attributes" (
    "Identifier"    TEXT,
    "Attribute" TEXT,
    "Value"     INTEGER
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





INSERT INTO "Entities"                 VALUES("ENTITY_PONCY", "entity.player.name.default", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_HUMAN_ANY", "", 0, "", "NotFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_PONCY", "MAX_HEALTH", 10);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_PONCY", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_PONCY", "SHOUT_RADIUS", 4);
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_PONCY", "PONCY_KIT");


INSERT INTO "Entities"                 VALUES("ENTITY_MASLOW", "entity.dog.name.default", 1, "ITEM_DOG_MAW", 1, "VIEW_MARKER_BLUE", "Gamepackage.DumbMelee", 0, "", "NotFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_MASLOW", "MAX_HEALTH", 45);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_MASLOW", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_MASLOW", "SHOUT_RADIUS", 4);

INSERT INTO "Entities"                 VALUES("ENTITY_GIANT_BEE", "entity.bee.name", 1, "ITEM_BEE_STINGER", 1, "VIEW_BEE","Gamepackage.DumbMelee", 0, "", "IsFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_GIANT_BEE", "MAX_HEALTH", 10);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_GIANT_BEE", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_GIANT_BEE", "SHOUT_RADIUS", 4);
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_GIANT_BEE", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_SKELETON", "entity.skeleton.name", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_SKELETON_WHITE", "Gamepackage.Archer", 0, "", "NotFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_SKELETON", "MAX_HEALTH", 10);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_SKELETON", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_SKELETON", "SHOUT_RADIUS", 4);
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_HUMANOID_CLOTHING");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_SKELETON", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_GHOST", "entity.ghost.name", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_GHOST", "Gamepackage.Archer", 0, "", "IsFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_GHOST", "MAX_HEALTH", 10);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_GHOST", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_GHOST", "SHOUT_RADIUS", 4);
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_HUMANOID_CLOTHING");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_GHOST", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_ANIMATED_WEAPON", "entity.animated.weapon.name", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_GHOST", "Gamepackage.Archer", 0, "", "IsFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_ANIMATED_WEAPON", "MAX_HEALTH", 10);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_ANIMATED_WEAPON", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_ANIMATED_WEAPON", "SHOUT_RADIUS", 4);
INSERT INTO "Entities_EquipmentTables" VALUES("ENTITY_ANIMATED_WEAPON", "LOOT_TABLE_HUMANOID_WEAPONS");
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_ANIMATED_WEAPON", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_QUEEN_BEE", "NAMETABLE_BEES", 1, "ITEM_BEE_STINGER", 1, "VIEW_BEE", "Gamepackage.DumbMelee", 0, "", "IsFloating", "CastsShadow");
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_QUEEN_BEE", "MAX_HEALTH", 15);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_QUEEN_BEE", "VISION_RADIUS", 4);
INSERT INTO "Entities_Attributes"      VALUES("ENTITY_QUEEN_BEE", "SHOUT_RADIUS", 4);
INSERT INTO "Entities_InventoryTables" VALUES("ENTITY_QUEEN_BEE", "LOOT_TABLE_TRASH");

INSERT INTO "Entities"                 VALUES("ENTITY_STAIRS_UP", "entity.stairs.up.name", 0, "", 0, "VIEW_STAIRCASE_UP","", 1, "TRIGGER_CHANGE_LEVEL_ON_PRESS", "NotFloating", "NoShadow");
INSERT INTO "Entities"                 VALUES("ENTITY_STAIRS_DOWN", "entity.stairs.down.name", 0, "", 0, "VIEW_STAIRCASE_DOWN", "", 1, "TRIGGER_CHANGE_LEVEL_ON_PRESS", "NotFloating", "NoShadow");
INSERT INTO "Entities"                 VALUES("ENTITY_GROUND_DROP", "", 0, "", 0, "VIEW_CORPSE", "", 0, "TRIGGER_LOOTABLE", "NotFloating", "NoShadow");

COMMIT;