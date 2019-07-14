
BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Races";
CREATE TABLE "Races" (
	"Identifier"   TEXT,
	"Name"         TEXT,
	"IsCombatant"  INTEGER,
	"DefaultWeapon" TEXT,
	"BlocksPathing" INTEGER,
	"DefaultViewTemplate"  TEXT,
	"DefaultAIClassName"   TEXT,
	"AlwaysVisible" INTEGER,
	"Trigger"       TEXT,
	"isFloating" TEXT,
	"CastsShadow" TEXT
);
DROP TABLE IF EXISTS "Races_Attributes";
CREATE TABLE IF NOT EXISTS "Races_Attributes" (
    "Identifier"    TEXT,
    "Attribute" TEXT,
    "Value"     INTEGER
);

INSERT INTO "Races"                 VALUES("RACE_HUMAN", "race.name.human", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_HUMAN_ANY", "Gamepackage.Archer", 0, "", "NotFloating", "CastsShadow");
INSERT INTO "Races_Attributes"      VALUES("RACE_HUMAN", "MAX_HEALTH", 10);
INSERT INTO "Races_Attributes"      VALUES("RACE_HUMAN", "VISION_RADIUS", 4);
INSERT INTO "Races_Attributes"      VALUES("RACE_HUMAN", "SHOUT_RADIUS", 4);

INSERT INTO "Races"                 VALUES("RACE_GIANT_BEE", "race.name.giant.bee", 1, "ITEM_BEE_STINGER", 1, "VIEW_BEE", "Gamepackage.DumbMelee", 0, "", "IsFloating", "CastsShadow");
INSERT INTO "Races_Attributes"      VALUES("RACE_GIANT_BEE", "MAX_HEALTH", 10);
INSERT INTO "Races_Attributes"      VALUES("RACE_GIANT_BEE", "VISION_RADIUS", 4);
INSERT INTO "Races_Attributes"      VALUES("RACE_GIANT_BEE", "SHOUT_RADIUS", 4);

INSERT INTO "Races"                 VALUES("RACE_DOG", "race.name.dog", 1, "ITEM_DOG_MAW", 1, "VIEW_MARKER_BLUE", "Gamepackage.DumbMelee", 0, "", "NotFloating", "CastsShadow");
INSERT INTO "Races_Attributes"      VALUES("RACE_DOG", "MAX_HEALTH", 45);
INSERT INTO "Races_Attributes"      VALUES("RACE_DOG", "VISION_RADIUS", 4);
INSERT INTO "Races_Attributes"      VALUES("RACE_DOG", "SHOUT_RADIUS", 4);

INSERT INTO "Races"                 VALUES("RACE_SKELETON", "race.name.skeleton", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_SKELETON_WHITE", "Gamepackage.Archer", 0, "", "NotFloating", "CastsShadow");
INSERT INTO "Races_Attributes"      VALUES("RACE_SKELETON", "MAX_HEALTH", 10);
INSERT INTO "Races_Attributes"      VALUES("RACE_SKELETON", "VISION_RADIUS", 4);
INSERT INTO "Races_Attributes"      VALUES("RACE_SKELETON", "SHOUT_RADIUS", 4);

INSERT INTO "Races"                 VALUES("RACE_GHOST", "race.name.ghost", 1, "ITEM_HUMANOID_FIST", 1, "VIEW_GHOST", "Gamepackage.Archer", 0, "", "IsFloating", "CastsShadow");
INSERT INTO "Races_Attributes"      VALUES("RACE_GHOST", "MAX_HEALTH", 10);
INSERT INTO "Races_Attributes"      VALUES("RACE_GHOST", "VISION_RADIUS", 4);
INSERT INTO "Races_Attributes"      VALUES("RACE_GHOST", "SHOUT_RADIUS", 4);

INSERT INTO "Races"                 VALUES("RACE_STAIRS", "race.name.stairs", 0, "", 0, "VIEW_STAIRCASE_UP","", 1, "TRIGGER_CHANGE_LEVEL_ON_PRESS", "NotFloating", "NoShadow");
INSERT INTO "Races"                 VALUES("RACE_GROUND_DROP", "race.name.prop", 0, "", 0, "VIEW_CORPSE", "", 0, "TRIGGER_LOOTABLE", "NotFloating", "NoShadow");

COMMIT;