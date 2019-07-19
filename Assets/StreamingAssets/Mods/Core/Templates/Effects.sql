BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Effects";
CREATE TABLE IF NOT EXISTS "Effects" (
    "Identifier"    TEXT UNIQUE,
    "LocalizationPrefix"    TEXT,
    "EffectImplClassName"   TEXT,
    "HasUnlimitedDuration"  INTEGER,
    "Duration"  INTEGER,
    "StackingStrategy"  TEXT
);
DROP TABLE IF EXISTS "Effects_Data";
CREATE TABLE IF NOT EXISTS "Effects_Data" (
    "Identifier"    TEXT,
    "Key"   TEXT,
    "Value" TEXT
);
DROP TABLE IF EXISTS "Effects_TagsAppliedToEntity";
CREATE TABLE IF NOT EXISTS "Effects_TagsAppliedToEntity" (
    "Identifier"    TEXT,
    "Tag"   TEXT
);
DROP TABLE IF EXISTS "Effects_TagsThatBlockThisEffect";
CREATE TABLE IF NOT EXISTS "Effects_TagsThatBlockThisEffect" (
    "Identifier"    TEXT,
    "Tag"   TEXT
);

DROP TABLE IF EXISTS "Effects_Attributes";
CREATE TABLE IF NOT EXISTS "Effects_Attributes" (
    "Identifier"    TEXT,
    "Attribute" TEXT,
    "Value"     INTEGER
);

INSERT INTO "Effects"                         VALUES ('EFFECT_TEMPORARY_POISON_IMMUNITY','effect.poison.immunity','Gamepackage.SimpleEffect',0,20,'AddDuration');
INSERT INTO "Effects_TagsAppliedToEntity"     VALUES ('EFFECT_TEMPORARY_POISON_IMMUNITY','POISON_IMMUNITY');


INSERT INTO "Effects"                         VALUES ('EFFECT_LUCKY_COIN_LIFE_SAVE','effect.lucky.coin.lifesave','Gamepackage.LuckyCoinLifeSave',1,0,'IgnoreDuplicates');


INSERT INTO "Effects"                         VALUES ('EFFECT_WEAK_POISON','effect.weak.poison','Gamepackage.Poison',0,4,'AddDuration');
INSERT INTO "Effects_Data"                    VALUES ('EFFECT_WEAK_POISON','PoisonAmount','1');
INSERT INTO "Effects_TagsThatBlockThisEffect" VALUES ('EFFECT_WEAK_POISON','POISON_IMMUNITY');


INSERT INTO "Effects"                         VALUES ('EFFECT_STRONG_POISON','effect.strong.poison','Gamepackage.Poison',0,4,'AddDuration');
INSERT INTO "Effects_Data"                    VALUES ('EFFECT_STRONG_POISON','PoisonAmount','3');
INSERT INTO "Effects_TagsThatBlockThisEffect" VALUES ('EFFECT_STRONG_POISON','POISON_IMMUNITY');


INSERT INTO "Effects"                         VALUES ('EFFECT_MADNESS','effect.madness','Gamepackage.Madness',0,3,'AddDuration');


INSERT INTO "Effects"                         VALUES ('EFFECT_CHARM','effect.charm','Gamepackage.Charm',0,3,'AddDuration');


INSERT INTO "Effects"                         VALUES ('EFFECT_DOMINATION','effect.domination','Gamepackage.Domination',1,0,'AddDuplicate');


INSERT INTO "Effects"                         VALUES ('EFFECT_TEMPORARY_REGENERATION','effect.weak.regeneration','Gamepackage.Regeneration',0,10,'AddDuration');
INSERT INTO "Effects_Data"                    VALUES ('EFFECT_TEMPORARY_REGENERATION','HealAmount','1');


INSERT INTO "Effects"                         VALUES ('EFFECT_STRENGTH_OF_GIANTS','effect.strength.of.giants','Gamepackage.SimpleEffect',1,0,'IgnoreDuplicates');
INSERT INTO "Effects_Attributes"              VALUES ('EFFECT_STRENGTH_OF_GIANTS', 'MaxHealth', 100);

COMMIT;
