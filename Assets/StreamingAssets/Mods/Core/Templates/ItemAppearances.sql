BEGIN TRANSACTION;

DROP TABLE IF EXISTS "ItemAppearances";
CREATE TABLE IF NOT EXISTS "ItemAppearances" (
    "Identifier"    TEXT NOT NULL UNIQUE,
    "InventorySprite"   TEXT,
    "GroundSprite"      TEXT,
    PRIMARY KEY("Identifier")
);

DROP TABLE IF EXISTS "ItemAppearances_SpriteAttachments";
CREATE TABLE IF NOT EXISTS "ItemAppearances_SpriteAttachments" (
    "Identifier"    TEXT NOT NULL,
    "SpriteAttachment"  TEXT,
    "Sprite"    TEXT
);

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_NONE','', '');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_LONGSWORD','LongswordIcon', 'Longsword');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_LONGSWORD','MainHandFront','Longsword');


INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_BOW','BowIcon','Bow');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_BOW','MainHandFront','Bow');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_LUCKY_COIN','LuckyCoin','LuckyCoin');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_GREEN_POTION','GreenPotion','GreenPotion');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_RED_POTION','RedPotion','RedPotion');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_PURPLE_POTION','PurplePotion','PurplePotion');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_BLUE_POTION','BluePotion','BluePotion');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ARROW','ArrowIcon','Arrow');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ACTION_STAFF','ActionStaffIcon','ActionStaff');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ACTION_STAFF','MainHandFront','ActionStaff');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ORB_SCEPTER','OrbScepterIcon','OrbScepter');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ORB_SCEPTER','MainHandFront','OrbScepter');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_DAGGER','DaggerIcon','Dagger');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_DAGGER','MainHandFront','Dagger');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_MACE','MaceIcon','Mace');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_MACE','MainHandFront','Mace');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_SWIRL_STAFF','SwirlStaffIcon','SwirlStaff');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SWIRL_STAFF','MainHandFront','SwirlStaff');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_HOOK_STAFF','HookStaffIcon','HookStaff');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_HOOK_STAFF','MainHandFront','HookStaff');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_SANDALS','SandalsIcon','SandalsIcon');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','LeftLegFrontSE','ShoeFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','RightLegFrontSE','ShoeFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','LeftLegFrontNE','ShoeFrontNE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','RightLegFrontNE','ShoeFrontNE');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_SHIELD_OF_AMALURE','ShieldOfAmalureIcon','ShieldOfAmalureSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SHIELD_OF_AMALURE','OffHandFrontSE','ShieldOfAmalureSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SHIELD_OF_AMALURE','OffHandFrontNE','ShieldOfAmalureNE');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','RobeOfWondersIcon','RobeOfWondersFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','ChestFrontNE','RobeOfWondersFrontNE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','ChestFrontSE','RobeOfWondersFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','HelmetBackSE','RobeOfWondersHoodBackSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','HelmetFrontNE','RobeOfWondersHoodFrontNE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','HelmetFrontSE','RobeOfWondersHoodFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','LeftArmFrontSE','RobeOfWondersLeftArmFront');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','LeftArmFrontNE','RobeOfWondersLeftArmFront');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','RightArmFrontSE','RobeOfWondersRightArmFront');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','RightArmFrontNE','RobeOfWondersRightArmFront');

COMMIT;
