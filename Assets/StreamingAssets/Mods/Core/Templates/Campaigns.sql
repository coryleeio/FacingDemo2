BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Campaigns";
CREATE TABLE IF NOT EXISTS "Campaigns" (
    "Identifier"    TEXT UNIQUE,
    "RulesEngineClassName"   TEXT
);
INSERT INTO "Campaigns" VALUES ('CAMPAIGN_CORE','Gamepackage.CoreRulesEngine');

COMMIT;
