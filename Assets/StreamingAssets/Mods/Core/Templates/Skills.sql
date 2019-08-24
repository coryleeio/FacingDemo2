BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Skills";
CREATE TABLE IF NOT EXISTS "Skills" (
	"Identifier"      TEXT,
	"Name"            TEXT,
	"UISprite"        TEXT,
	"SkillXpModifier" TEXT
);

INSERT INTO "Skills" VALUES('SKILL_LONG_BLADES', 'skill.long.blades', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_SPELLCASTING', 'skill.spellcasting', 'Longsword', "1.0");

COMMIT;
