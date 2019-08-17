BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Triggers";
CREATE TABLE IF NOT EXISTS "Triggers" (
	"Identifier"	TEXT NOT NULL UNIQUE,
	"TriggerShape" TEXT,
	"TriggerMode" TEXT,
	"PressInputHint" TEXT,
	"TriggerableActionClassName" TEXT,
	PRIMARY KEY("Identifier")
);

DROP TABLE IF EXISTS "Triggers_CombatActionParameters";
CREATE TABLE "Triggers_CombatActionParameters" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"DyeSize"	INTEGER,
	"DyeNumber"	INTEGER,
	"DamageType"	TEXT,
	"AttackMessage"	TEXT,
	"Range"	INTEGER,
	"NumberOfTargetsToPierce"	INTEGER,
	"TargetingType"	TEXT,
	"ProjectileAppearanceIdentifier"	TEXT,
	"InteractionProperties"	TEXT
);

INSERT INTO "Triggers"                         VALUES ( "TRIGGER_CHANGE_LEVEL_ON_PRESS", "SingleSquare", "Press", "traverse.staircase", "Gamepackage.ChangeLevel");
INSERT INTO "Triggers"                         VALUES ( "TRIGGER_LOOTABLE", "SingleSquare", "Press", "show.loot.message", "Gamepackage.LootEntitiesInPosition");
COMMIT;
