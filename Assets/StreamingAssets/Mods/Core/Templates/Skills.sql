BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Skills";
CREATE TABLE IF NOT EXISTS "Skills" (
	"Identifier"      TEXT,
	"Name"            TEXT,
	"UISprite"        TEXT,
	"SkillXpModifier" TEXT
);

INSERT INTO "Skills" VALUES('SKILL_LONG_BLADES', 'skill.long.blades', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_SHORT_BLADES', 'skill.short.blades', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_MACES', 'skill.maces', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_STAVES', 'skill.staves', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_UNARMED', 'skill.unarmed', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_SPELLCASTING', 'skill.spellcasting', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_SHIELDS', 'skill.shields', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_ARCHERY', 'skill.archery', 'Longsword', "1.0");
INSERT INTO "Skills" VALUES('SKILL_THROWING', 'skill.throwing', 'Longsword', "1.0");
COMMIT;
