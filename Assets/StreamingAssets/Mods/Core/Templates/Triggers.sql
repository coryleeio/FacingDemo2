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
	"BaseDamage"	INTEGER,
	"ClusteringFactor"	INTEGER,
	"SkillIdentifier" TEXT,
	"NumberOfTurnsToExerciseSkill"    INTEGER,
	"DamageType"	TEXT,
	"AttackMessage"	TEXT,
	"Range"	INTEGER,
	"NumberOfTargetsToPierce"	INTEGER,
	"TargetingType"	TEXT,
	"ProjectileAppearanceIdentifier"	TEXT,
	"AccuracyFormula" 		TEXT,
	"BlockChanceFormula" 	TEXT,
	"DodgeChanceFormula" 	TEXT,
	"FailureFormula" 		TEXT,
	"DamageFormula" 		TEXT
);

DROP TABLE IF EXISTS "Triggers_TemplateData";
CREATE TABLE IF NOT EXISTS "Triggers_TemplateData" (
    "Identifier"    TEXT,
    "Key"   TEXT,
    "Value" TEXT
);

INSERT INTO "Triggers"                         VALUES ( "TRIGGER_CHANGE_LEVEL_ON_PRESS", "SingleSquare", "Press", "traverse.staircase", "Gamepackage.ChangeLevel");
INSERT INTO "Triggers"                         VALUES ( "TRIGGER_LOOTABLE", "SingleSquare", "Press", "show.loot.message", "Gamepackage.LootEntitiesInPosition");

INSERT INTO "Triggers"                         VALUES ( "TRIGGER_OPEN_WELL_DIALOG", "OrthogonalOrDiagonal", "Press", "show.inspect.message", "Gamepackage.OpenDialog");
INSERT INTO "Triggers_TemplateData"            VALUES ( "TRIGGER_OPEN_WELL_DIALOG", "DIALOG_TEMPLATE", "DIALOG_WELL");

COMMIT;
