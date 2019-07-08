BEGIN TRANSACTION;

DROP TABLE IF EXISTS "EncounterTable";
CREATE TABLE "EncounterTable" (
	"Identifier"   TEXT
);
DROP TABLE IF EXISTS "EncounterTable_Parcels";
CREATE TABLE "EncounterTable_Parcels" (
	"Identifier"   TEXT,
	"ParcelId"     INTEGER,
	"Weight"       INTEGER
);
DROP TABLE IF EXISTS "EncounterTable_ParcelEntries";
CREATE TABLE "EncounterTable_ParcelEntries" (
	"Identifier"   TEXT,
	"ParcelId"     INTEGER,
	"Value"        TEXT
);

INSERT INTO "EncounterTable"               VALUES('ENCOUNTER_BEE_SWARM');
INSERT INTO "EncounterTable_Parcels"       VALUES('ENCOUNTER_BEE_SWARM', 1,1);
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_BEE_SWARM', 1,'ENTITY_GIANT_BEE');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_BEE_SWARM', 1,'ENTITY_GIANT_BEE');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_BEE_SWARM', 1,'ENTITY_ANIMATED_WEAPON');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_BEE_SWARM', 1,'ENTITY_GHOST');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_BEE_SWARM', 1,'ENTITY_QUEEN_BEE');


INSERT INTO "EncounterTable"               VALUES('ENCOUNTER_SKELETONS');
INSERT INTO "EncounterTable_Parcels"       VALUES('ENCOUNTER_SKELETONS', 1,1);
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_SKELETONS', 1,'ENTITY_SKELETON');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_SKELETONS', 1,'ENTITY_SKELETON');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_SKELETONS', 1,'ENTITY_SKELETON');
INSERT INTO "EncounterTable_ParcelEntries" VALUES('ENCOUNTER_SKELETONS', 1,'ENTITY_SKELETON');





COMMIT;