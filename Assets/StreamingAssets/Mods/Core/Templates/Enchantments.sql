BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Enchantments";
CREATE TABLE IF NOT EXISTS "Enchantments" (
	"Identifier"      TEXT,
	"NameModifier"    TEXT,
	"MinCharges"	INTEGER,
	"MaxCharges"	INTEGER
);

DROP TABLE IF EXISTS "Enchantments_CombatActionParameters";
CREATE TABLE "Enchantments_CombatActionParameters" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"DyeSize"	INTEGER,
	"DyeNumber"	INTEGER,
	"DamageType"	TEXT,
	"AppliedEffectTemplate"	TEXT,
	"AttackMessage"	TEXT,
	"Range"	INTEGER,
	"NumberOfTargetsToPierce"	INTEGER,
	"TargetingType"	TEXT,
	"ProjectileAppearanceIdentifier"	TEXT,
	"InteractionProperties"	TEXT
);

DROP TABLE IF EXISTS "Enchantments_ExplosionParameters";
CREATE TABLE "Enchantments_ExplosionParameters" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"DyeSize"	INTEGER,
	"DyeNumber"	INTEGER,
	"DamageType"	TEXT,
	"AppliedEffectTemplate"	TEXT,
	"AttackMessage"	TEXT,
	"Range"	INTEGER,
	"NumberOfTargetsToPierce"	INTEGER,
	"TargetingType"	TEXT,
	"ProjectileAppearanceIdentifier"	TEXT,
	"InteractionProperties"	TEXT
);

DROP TABLE IF EXISTS "Enchantments_ActionCosts";
CREATE TABLE "Enchantments_ActionCosts" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"Health"       INTEGER
);

DROP TABLE IF EXISTS "Enchantments_WornEffects";
CREATE TABLE IF NOT EXISTS "Enchantments_WornEffects" (
	"Identifier"	TEXT NOT NULL,
	"Effect"	TEXT
);

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_DOMINATION', 'name.modifier.enchantment.domination', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_DOMINATION', 'Zapped', 0, 0, 'NEGATIVE', 'EFFECT_DOMINATION', '', 5, 1, 'SelectTarget', 'PROJECTILE_APPEARANCE_PURPLE_BALL', "");

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_LIGHTNING', 'name.modifier.enchantment.lightning', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"  	VALUES ('ENCHANTMENT_LIGHTNING', 'Zapped', 14, 1, 'LIGHTNING', '', 'attacks.lightning.1', 5, 999, 'Line', 'PROJECTILE_APPEARANCE_LIGHTNING_JET', "");

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_MADNESS', 'name.modifier.enchantment.madness', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_MADNESS', 'Zapped', 0, 0, 'NEGATIVE', 'EFFECT_MADNESS', '', 5, 1, 'SelectTarget', 'PROJECTILE_APPEARANCE_PURPLE_BALL', "");

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_CHARM', 'name.modifier.enchantment.charm', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_CHARM', 'Zapped', 0, 0, 'NEGATIVE', 'EFFECT_CHARM', '', 5, 1, 'SelectTarget', 'PROJECTILE_APPEARANCE_PURPLE_BALL', "");

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_FIREBALL', 'name.modifier.enchantment.fireball', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_FIREBALL', 'Zapped', 0, 0, 'FIRE', '', 'attacks.fire.1', 5, 999, 'Line', 'PROJECTILE_APPEARANCE_FIREBALL', "Unavoidable");
INSERT INTO "Enchantments_ExplosionParameters"      VALUES ('ENCHANTMENT_FIREBALL', 'Zapped', 14, 1, 'FIRE', '', 'attacks.fire.1', 4, 1, 'Line', 'PROJECTILE_APPEARANCE_FIRE_EXPLOSION', 'IgnoresBlock');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_LUCKY_COIN_LIFE_SAVE', 'name.modifier.enchantment.none', -1, -1);
INSERT INTO "Enchantments_WornEffects"   			VALUES ('ENCHANTMENT_LUCKY_COIN_LIFE_SAVE', 'EFFECT_LUCKY_COIN_LIFE_SAVE');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_STRENGTH_OF_GIANTS', 'name.modifier.enchantment.none', -1, -1);
INSERT INTO "Enchantments_WornEffects"   			VALUES ('ENCHANTMENT_STRENGTH_OF_GIANTS', 'EFFECT_STRENGTH_OF_GIANTS');

COMMIT;
