BEGIN TRANSACTION;

DROP TABLE IF EXISTS "NameTable";
CREATE TABLE "NameTable" (
	"Identifier"   TEXT
);

DROP TABLE IF EXISTS "NameTable_Parcels";
CREATE TABLE "NameTable_Parcels" (
	"Identifier"   TEXT,
	"ParcelId"     INTEGER,
	"Weight"       INTEGER
);

DROP TABLE IF EXISTS "NameTable_ParcelEntries";
CREATE TABLE "NameTable_ParcelEntries" (
	"Identifier"   TEXT,
	"ParcelId"     INTEGER,
	"Value"        TEXT
);

INSERT INTO "NameTable"               VALUES('NAMETABLE_QUEEN_BEES');
INSERT INTO "NameTable_Parcels"       VALUES('NAMETABLE_QUEEN_BEES', 1,1);
INSERT INTO "NameTable_ParcelEntries" VALUES('NAMETABLE_QUEEN_BEES', 1,'entity.queen.bee.special.name');

INSERT INTO "NameTable_Parcels"       VALUES('NAMETABLE_QUEEN_BEES', 2,9);
INSERT INTO "NameTable_ParcelEntries" VALUES('NAMETABLE_QUEEN_BEES', 2,'entity.queen.bee.name');

COMMIT;