BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Campaigns";
CREATE TABLE IF NOT EXISTS "Campaigns" (
    "Identifier"    TEXT UNIQUE,
    "RulesEngineClassName"   TEXT
);

DROP TABLE IF EXISTS "Campaigns_XpForLevel";
CREATE TABLE IF NOT EXISTS "Campaigns_XpForLevel" (
    "Identifier"    TEXT,
    "Level"    INTEGER,
    "XpNeeded"    INTEGER
);

DROP TABLE IF EXISTS "Campaigns_XpForSkillRank";
CREATE TABLE IF NOT EXISTS "Campaigns_XpForSkillRank" (
    "Identifier"    TEXT,
    "Level"    INTEGER,
    "XpNeeded"    INTEGER
);

DROP TABLE IF EXISTS "Campaigns_XpAwardedForKillingEntityOfLevel";
CREATE TABLE IF NOT EXISTS "Campaigns_XpAwardedForKillingEntityOfLevel" (
    "Identifier"    TEXT,
    "Level"    INTEGER,
    "XpNeeded"    INTEGER
);

DROP TABLE IF EXISTS "Campaigns_Settings";
CREATE TABLE IF NOT EXISTS "Campaigns_Settings" (
    "Identifier"    TEXT,
    "SettingKey"    TEXT,
    "SettingValue"  TEXT
);

INSERT INTO "Campaigns" VALUES ('CAMPAIGN_CORE','Gamepackage.CoreRulesEngine');

INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',0, 0);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',1, 0);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',2, 200);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',3, 400);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',4, 800);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',5, 1600);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',6, 3200);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',7, 6400);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',8, 12600);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',9, 25200);
INSERT INTO "Campaigns_XpForLevel" VALUES ('CAMPAIGN_CORE',10, 50400);

INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',0, 0);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',1, 0);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',2, 200);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',3, 400);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',4, 800);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',5, 1600);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',6, 3200);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',7, 6400);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',8, 12600);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',9, 25200);
INSERT INTO "Campaigns_XpForSkillRank" VALUES ('CAMPAIGN_CORE',10, 50400);

INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',0, 0);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',1, 10);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',2, 20);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',3, 40);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',4, 80);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',5, 160);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',6, 320);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',7, 640);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',8, 1260);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',9, 2520);
INSERT INTO "Campaigns_XpAwardedForKillingEntityOfLevel" VALUES ('CAMPAIGN_CORE',10, 5040);

INSERT INTO "Campaigns_Settings" VALUES ('CAMPAIGN_CORE','MaxLevel', 10);
INSERT INTO "Campaigns_Settings" VALUES ('CAMPAIGN_CORE','MaxSkillRank', 10);
INSERT INTO "Campaigns_Settings" VALUES ('CAMPAIGN_CORE','GlobalXpModifier', "1.0");

COMMIT;
