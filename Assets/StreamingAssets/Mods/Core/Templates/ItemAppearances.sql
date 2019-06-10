BEGIN TRANSACTION;

DROP TABLE IF EXISTS "ItemAppearances";
CREATE TABLE IF NOT EXISTS "ItemAppearances" (
    "Identifier"    TEXT NOT NULL UNIQUE,
    "InventorySprite"   TEXT,
    PRIMARY KEY("Identifier")
);

DROP TABLE IF EXISTS "ItemAppearances_SpriteAttachments";
CREATE TABLE IF NOT EXISTS "ItemAppearances_SpriteAttachments" (
    "Identifier"    TEXT NOT NULL,
    "SpriteAttachment"  TEXT,
    "Sprite"    TEXT
);

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_LONGSWORD','Longsword');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_LONGSWORD','MainHandFront','Longsword');


INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_BOW','Bow');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_BOW','MainHandFront','Bow');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_LUCKY_COIN','LuckyCoin');



INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_GREEN_POTION','GreenPotion');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_RED_POTION','RedPotion');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_PURPLE_POTION','PurplePotion');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_BLUE_POTION','BluePotion');


INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ARROW','Arrow');
INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ACTION_STAFF','ActionStaff');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ACTION_STAFF','MainHandFront','ActionStaff');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ORB_SCEPTER','OrbScepter');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_ORB_SCEPTER','MainHandFront','OrbScepter');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_DAGGER','Dagger');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_DAGGER','MainHandFront','Dagger');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_MACE','Mace');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_MACE','MainHandFront','Mace');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_SWIRL_STAFF','SwirlStaff');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SWIRL_STAFF','MainHandFront','SwirlStaff');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_HOOK_STAFF','HookStaff');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_HOOK_STAFF','MainHandFront','HookStaff');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_SANDALS','SandalsIcon');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','LeftLegFrontSE','ShoeFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','RightLegFrontSE','ShoeFrontSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','LeftLegFrontNE','ShoeFrontNE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SANDALS','RightLegFrontNE','ShoeFrontNE');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_SHIELD_OF_AMALURE','ShieldOfAmalureSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SHIELD_OF_AMALURE','OffHandFrontSE','ShieldOfAmalureSE');
INSERT INTO "ItemAppearances_SpriteAttachments" VALUES ('ITEM_APPEARANCE_SHIELD_OF_AMALURE','OffHandFrontNE','ShieldOfAmalureNE');

INSERT INTO "ItemAppearances"                   VALUES ('ITEM_APPEARANCE_ROBE_OF_WONDERS','RobeOfWondersFrontSE');
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
