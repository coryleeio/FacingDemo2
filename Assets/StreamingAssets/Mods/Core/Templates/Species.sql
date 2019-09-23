
BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Species";
CREATE TABLE "Species" (
	"Identifier"   TEXT,
	"Name"         TEXT,
	"DefaultWeapon" TEXT,
	"DefaultAIClassName"   TEXT
);

DROP TABLE IF EXISTS "Species_Attributes";
CREATE TABLE IF NOT EXISTS "Species_Attributes" (
    "Identifier"    TEXT,
    "Attribute" TEXT,
    "Value"     INTEGER
);

INSERT INTO "Species"                 VALUES("ENTITY_TYPE_HUMAN", "race.name.human",  "ITEM_HUMANOID_FIST", "Gamepackage.Archer");
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_HUMAN", "MaxHealth", 10);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_HUMAN", "VisionRadius", 4);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_HUMAN", "ShoutRadius", 4);

INSERT INTO "Species"                 VALUES("ENTITY_TYPE_GIANT_BEE", "race.name.giant.bee", "ITEM_BEE_STINGER", "Gamepackage.DumbMelee");
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_GIANT_BEE", "MaxHealth", 10);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_GIANT_BEE", "VisionRadius", 4);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_GIANT_BEE", "ShoutRadius", 4);

INSERT INTO "Species"                 VALUES("ENTITY_TYPE_DOG", "race.name.dog", "ITEM_DOG_MAW", "Gamepackage.DumbMelee");
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_DOG", "MaxHealth", 45);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_DOG", "VisionRadius", 4);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_DOG", "ShoutRadius", 4);

INSERT INTO "Species"                 VALUES("ENTITY_TYPE_SKELETON", "race.name.skeleton", "ITEM_HUMANOID_FIST", "Gamepackage.Archer");
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_SKELETON", "MaxHealth", 10);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_SKELETON", "VisionRadius", 4);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_SKELETON", "ShoutRadius", 4);

INSERT INTO "Species"                 VALUES("ENTITY_TYPE_GHOST", "race.name.ghost", "ITEM_HUMANOID_FIST", "Gamepackage.Archer");
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_GHOST", "MaxHealth", 10);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_GHOST", "VisionRadius", 4);
INSERT INTO "Species_Attributes"      VALUES("ENTITY_TYPE_GHOST", "ShoutRadius", 4);

COMMIT;