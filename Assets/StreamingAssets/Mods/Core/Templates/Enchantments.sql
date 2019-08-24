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

DROP TABLE IF EXISTS "Enchantments_ExplosionParameters";
CREATE TABLE "Enchantments_ExplosionParameters" (
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

DROP TABLE IF EXISTS "Enchantments_ActionCosts";
CREATE TABLE "Enchantments_ActionCosts" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"Health"       INTEGER
);

DROP TABLE IF EXISTS "Enchantments_AppliedEffects";
CREATE TABLE "Enchantments_AppliedEffects" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"AppliedEffect"       TEXT
);

DROP TABLE IF EXISTS "Enchantments_WornEffects";
CREATE TABLE IF NOT EXISTS "Enchantments_WornEffects" (
	"Identifier"	TEXT NOT NULL,
	"Effect"	TEXT
);

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_DOMINATION', 'name.modifier.enchantment.domination', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_DOMINATION', 'Zapped', 0, 0, 'SKILL_SPELLCASTING', 1,'NEGATIVE', 'effect.domination', 5, 1, 'SelectTarget', 'PROJECTILE_APPEARANCE_PURPLE_BALL', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_DOMINATION', 'Zapped', 'EFFECT_DOMINATION');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_LIGHTNING', 'name.modifier.enchantment.lightning', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"  	VALUES ('ENCHANTMENT_LIGHTNING', 'Zapped', 14, 1, 'SKILL_SPELLCASTING', 1,'LIGHTNING', 'attacks.lightning.1', 5, 999, 'Line', 'PROJECTILE_APPEARANCE_LIGHTNING_JET', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_MADNESS', 'name.modifier.enchantment.madness', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_MADNESS', 'Zapped', 0, 0, 'SKILL_SPELLCASTING', 1,'NEGATIVE', 'effect.madness', 5, 1, 'SelectTarget', 'PROJECTILE_APPEARANCE_PURPLE_BALL', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_MADNESS', 'Zapped', 'EFFECT_MADNESS');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_CHARM', 'name.modifier.enchantment.charm', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_CHARM', 'Zapped', 0, 0, 'SKILL_SPELLCASTING', 1,'NEGATIVE', 'effect.charm', 5, 1, 'SelectTarget', 'PROJECTILE_APPEARANCE_PURPLE_BALL', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_CHARM', 'Zapped', 'EFFECT_CHARM');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_FIREBALL', 'name.modifier.enchantment.fireball', 200, 200);
INSERT INTO "Enchantments_CombatActionParameters"   VALUES ('ENCHANTMENT_FIREBALL', 'Zapped', 0, 0, 'SKILL_SPELLCASTING', 1,'FIRE', 'attacks.fire.1', 5, 999, 'Line', 'PROJECTILE_APPEARANCE_FIREBALL', "ACCURACY_STANDARD", "BLOCK_IGNORE", "DODGE_IGNORE", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Enchantments_ExplosionParameters"      VALUES ('ENCHANTMENT_FIREBALL', 'Zapped', 14, 1, 'SKILL_SPELLCASTING', 1,'FIRE', 'attacks.fire.1', 4, 1, 'Line', 'PROJECTILE_APPEARANCE_FIRE_EXPLOSION', "ACCURACY_STANDARD", "BLOCK_IGNORE", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_LUCKY_COIN_LIFE_SAVE', 'name.modifier.enchantment.none', -1, -1);
INSERT INTO "Enchantments_WornEffects"   			VALUES ('ENCHANTMENT_LUCKY_COIN_LIFE_SAVE', 'EFFECT_LUCKY_COIN_LIFE_SAVE');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_STRENGTH_OF_GIANTS', 'name.modifier.enchantment.none', -1, -1);
INSERT INTO "Enchantments_WornEffects"   			VALUES ('ENCHANTMENT_STRENGTH_OF_GIANTS', 'EFFECT_STRENGTH_OF_GIANTS');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_INNATE_WEAK_POISON', 'name.modifier.enchantment.none', -1, -1);
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_INNATE_WEAK_POISON', 'Melee',  'EFFECT_WEAK_POISON');
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_INNATE_WEAK_POISON', 'Thrown', 'EFFECT_WEAK_POISON');

INSERT INTO "Enchantments"           				VALUES ('ENCHANTMENT_CURE_POISON', 'name.modifier.enchantment.cure.poison', -1, -1);
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_CURE_POISON', 'Thrown',      'EFFECT_TEMPORARY_POISON_IMMUNITY');
INSERT INTO "Enchantments_AppliedEffects"           VALUES ('ENCHANTMENT_CURE_POISON', 'ApplyToSelf', 'EFFECT_TEMPORARY_POISON_IMMUNITY');

COMMIT;
