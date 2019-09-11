BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Items";
CREATE TABLE IF NOT EXISTS "Items" (
	"Identifier"      TEXT,
	"LocalizationPrefix"	TEXT,
	"MinStackSize"	INTEGER,
	"MaxStackSize"	INTEGER,
	"DestroyOnUse"	INTEGER,
	"ChanceToSurviveLaunch"	INTEGER,
	"AmmoType"	TEXT,
	"Enchantment"                 TEXT,
	"ItemAppearanceIdentifier"    TEXT
);

DROP TABLE IF EXISTS "Items_SlotsWearable";
CREATE TABLE IF NOT EXISTS "Items_SlotsWearable" (
	"Identifier"	TEXT NOT NULL,
	"Slot"	TEXT
);

DROP TABLE IF EXISTS "Items_SlotsOccupiedByWearing";
CREATE TABLE IF NOT EXISTS "Items_SlotsOccupiedByWearing" (
	"Identifier"	TEXT NOT NULL,
	"Slot"	TEXT
);

DROP TABLE IF EXISTS "Items_TagsThatDescribeThisItem";
CREATE TABLE IF NOT EXISTS "Items_TagsThatDescribeThisItem" (
	"Identifier"	TEXT NOT NULL,
	"Tag"	TEXT
);

DROP TABLE IF EXISTS "Items_TagsAppliedToEntity";
CREATE TABLE IF NOT EXISTS "Items_TagsAppliedToEntity" (
	"Identifier"	TEXT NOT NULL,
	"Tag"	TEXT
);

DROP TABLE IF EXISTS "Items_CombatActionParameters";
CREATE TABLE "Items_CombatActionParameters" (
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

DROP TABLE IF EXISTS "Items_ExplosionParameters";
CREATE TABLE "Items_ExplosionParameters" (
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

DROP TABLE IF EXISTS "Items_ActionCosts";
CREATE TABLE "Items_ActionCosts" (
	"Identifier"	TEXT,
	"InteractionType"	TEXT,
	"Health"       INTEGER
);

DROP TABLE IF EXISTS "Items_EnchantmentTables";
CREATE TABLE "Items_EnchantmentTables" (
	"Identifier"   TEXT
);
DROP TABLE IF EXISTS "Items_EnchantmentTables_Parcels";
CREATE TABLE "Items_EnchantmentTables_Parcels" (
	"Identifier"   TEXT,
	"ParcelId"     INTEGER,
	"Weight"       INTEGER
);
DROP TABLE IF EXISTS "Items_EnchantmentTables_ParcelEntries";
CREATE TABLE "Items_EnchantmentTables_ParcelEntries" (
	"Identifier"   TEXT,
	"ParcelId"     INTEGER,
	"Value"        TEXT
);

INSERT INTO "Items_EnchantmentTables"               VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS');
INSERT INTO "Items_EnchantmentTables_Parcels"       VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS', 1,1);
INSERT INTO "Items_EnchantmentTables_ParcelEntries" VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS', 1,'ENCHANTMENT_MADNESS');

INSERT INTO "Items_EnchantmentTables_Parcels"       VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS', 2,1);
INSERT INTO "Items_EnchantmentTables_ParcelEntries" VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS', 2,'ENCHANTMENT_CHARM');

INSERT INTO "Items_EnchantmentTables_Parcels"       VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS', 3,1);
INSERT INTO "Items_EnchantmentTables_ParcelEntries" VALUES('ENCHANTMENT_TABLE_MIND_EFFECTS', 3,'ENCHANTMENT_DOMINATION');

INSERT INTO "Items"                         VALUES ('ITEM_LONGSWORD','item.longsword',1,1,0,100,'None', "", "ITEM_APPEARANCE_LONGSWORD");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_LONGSWORD', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_LONGSWORD', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_LONGSWORD', 'Melee', 8, 1, 'SKILL_LONG_BLADES', 1,'SLASHING', 'attacks.slashing.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_LONGSWORD', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'SLASHING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_DAGGER','item.dagger',1,1,0,100,'None', "", "ITEM_APPEARANCE_DAGGER");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_DAGGER', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_DAGGER', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_DAGGER', 'Melee', 8, 1, 'SKILL_SHORT_BLADES', 1,'PIERCING', 'attacks.piercing.1',1,1,'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_DAGGER', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'PIERCING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_FANG_OF_JAHABI','item.fang.of.jahabi',1,1,0,100,'None', "ENCHANTMENT_INNATE_WEAK_POISON", "ITEM_APPEARANCE_DAGGER");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_FANG_OF_JAHABI', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_FANG_OF_JAHABI', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_FANG_OF_JAHABI', 'Melee', 8, 1, 'SKILL_SHORT_BLADES', 1,'PIERCING','attacks.piercing.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_FANG_OF_JAHABI', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'PIERCING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_MACE','item.mace',1,1,0,100,'None', "", "ITEM_APPEARANCE_MACE");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_MACE', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_MACE', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_MACE', 'Melee', 8, 1, 'SKILL_MACES', 1,'BLUDGEONING', 'attacks.bludgeoning.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_MACE', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'BLUDGEONING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_STAFF_OF_FIREBALLS','item.wand',1,1,0,100,'None', "ENCHANTMENT_FIREBALL", "ITEM_APPEARANCE_ACTION_STAFF");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_STAFF_OF_FIREBALLS', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_STAFF_OF_FIREBALLS', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_STAFF_OF_FIREBALLS', 'Melee', 8, 1, 'SKILL_STAVES', 1,'BLUDGEONING', 'attacks.bludgeoning.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_STAFF_OF_FIREBALLS', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'BLUDGEONING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_SHORTBOW','item.bow',1,1,0,100,'Arrow', "", "ITEM_APPEARANCE_BOW");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_SHORTBOW', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_SHORTBOW', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_SHORTBOW', 'OffHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_SHORTBOW', 'Melee', 3, 1, '', 1,'BLUDGEONING', 'attacks.bludgeoning.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_SHORTBOW', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'BLUDGEONING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_SHORTBOW', 'Ranged', 12, 1, 'SKILL_ARCHERY', 1,'PIERCING', 'attacks.piercing.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_ARROW', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_WAND_OF_LIGHTNING','item.wand',1,1,0,100,'None', "ENCHANTMENT_LIGHTNING", "ITEM_APPEARANCE_SWIRL_STAFF");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_WAND_OF_LIGHTNING', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_WAND_OF_LIGHTNING', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_WAND_OF_LIGHTNING', 'Melee', 8, 1, 'SKILL_STAVES', 1,'BLUDGEONING', 'attacks.bludgeoning.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_WAND_OF_LIGHTNING', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'BLUDGEONING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_MIND_WAND','item.wand',1,1,0,100,'None', "ENCHANTMENT_TABLE_MIND_EFFECTS", "ITEM_APPEARANCE_ORB_SCEPTER");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_MIND_WAND', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_MIND_WAND', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_MIND_WAND', 'Melee', 8, 1, 'SKILL_STAVES', 1,'BLUDGEONING', 'attacks.bludgeoning.1', 1, 1, 'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_MIND_WAND', 'Thrown', 3, 1, 'SKILL_THROWING', 1,'BLUDGEONING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_PURPLE_POTION','item.potion',1,1,1, 0,'None', "ENCHANTMENT_CURE_POISON", "ITEM_APPEARANCE_PURPLE_POTION");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_PURPLE_POTION', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_PURPLE_POTION', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_PURPLE_POTION', 'ApplyToSelf', 0, 0, '', 1,'HEALING', 'attacks.bludgeoning.1', 1, 1, 'SelectTarget', NULL, "ACCURACY_GUARANTEED", "BLOCK_IGNORE", "DODGE_IGNORE", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_PURPLE_POTION', 'Thrown', 0, 0, '', 1,'HEALING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_BROKEN_FLASK', "ACCURACY_GUARANTEED", "BLOCK_IGNORE", "DODGE_IGNORE", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_LUCKY_COIN','item.lucky.coin',1,1,0,100,'None',"ENCHANTMENT_LUCKY_COIN_LIFE_SAVE", "ITEM_APPEARANCE_LUCKY_COIN");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_LUCKY_COIN', 'Thrown', 0, 0, '', 1,'BLUDGEONING', 'attacks.throw.useless.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_AUTO',"ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_TagsThatDescribeThisItem"VALUES ('ITEM_LUCKY_COIN', 'ItemEffectsApplyFromInventory');

INSERT INTO "Items"                         VALUES ('ITEM_ROBE_OF_WONDERS','item.robe.of.wonders',1,1,0,100,'None', "", "ITEM_APPEARANCE_ROBE_OF_WONDERS");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_ROBE_OF_WONDERS', 'Chest');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_ROBE_OF_WONDERS', 'Helmet');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_ROBE_OF_WONDERS', 'Chest');

INSERT INTO "Items"                         VALUES ('ITEM_SANDALS','item.sandals',1,1,0,100,'None',  "", "ITEM_APPEARANCE_SANDALS");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_SANDALS', 'Shoes');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_SANDALS', 'Shoes');

INSERT INTO "Items"                         VALUES ('ITEM_SHIELD_OF_AMALURE','item.shield.of.amalure',1,1,0,100,'None',  "ENCHANTMENT_STRENGTH_OF_GIANTS", "ITEM_APPEARANCE_SHIELD_OF_AMALURE");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_SHIELD_OF_AMALURE', 'OffHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_SHIELD_OF_AMALURE', 'OffHand');

INSERT INTO "Items"                         VALUES ('ITEM_ARROW','item.arrow',1,60,0,50,'Arrow', "", "ITEM_APPEARANCE_ARROW");
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_ARROW', 'Thrown', 1, 1, 'SKILL_THROWING', 1,'PIERCING', 'attacks.piercing.1', 5, 1, 'Line', 'PROJECTILE_APPEARANCE_ARROW_SPIN',"ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_ARROW', 'Ammo');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_ARROW', 'Ammo');

INSERT INTO "Items"                         VALUES ('ITEM_HUMANOID_FIST','item.fist',1,1,0,100,'None', "", "ITEM_APPEARANCE_NONE");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_HUMANOID_FIST', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_HUMANOID_FIST', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_HUMANOID_FIST', 'Melee', 1, 1, 'SKILL_UNARMED', 1,'BLUDGEONING', 'attacks.humanoid.1',1,1,'Line', '',"ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_DOG_MAW','item.dog.maw',1,1,0,100,'None', "", "ITEM_APPEARANCE_NONE");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_DOG_MAW', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_DOG_MAW', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_DOG_MAW', 'Melee', 4, 2, 'SKILL_UNARMED', 1,'PIERCING', 'attacks.dog.1',1,1,'Line', '', "ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

INSERT INTO "Items"                         VALUES ('ITEM_BEE_STINGER','item.bee.stinger',1,1,0,100,'None', "ENCHANTMENT_INNATE_WEAK_POISON", "ITEM_APPEARANCE_NONE");
INSERT INTO "Items_SlotsWearable"           VALUES ('ITEM_BEE_STINGER', 'MainHand');
INSERT INTO "Items_SlotsOccupiedByWearing"  VALUES ('ITEM_BEE_STINGER', 'MainHand');
INSERT INTO "Items_CombatActionParameters"  VALUES ('ITEM_BEE_STINGER', 'Melee', 1, 1, 'SKILL_UNARMED', 1,'PIERCING', 'attacks.stinger.1',1,1,'Line', '',"ACCURACY_STANDARD", "BLOCK_STANDARD", "DODGE_STANDARD", "FAILURE_STANDARD", "DAMAGE_STANDARD");

COMMIT;
