BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Tilesets";
CREATE TABLE IF NOT EXISTS "Tilesets" (
	"Identifier"	TEXT NOT NULL UNIQUE,
	"FloorSprite"	TEXT,
	"TeeSprite"	TEXT,
	"NorthCornerSprite"	TEXT,
	"EastCornerSprite"	TEXT,
	"SouthCornerSprite"	TEXT,
	"WestCornerSprite"	TEXT,
	"NorthEastWallSprite"	TEXT,
	"SouthEastWallSprite"	TEXT,
	"SouthWestWallSprite"	TEXT,
	"NorthWestWallSprite"	TEXT,
	"NorthEastTeeSprite"	TEXT,
	"SouthEastTeeSprite"	TEXT,
	"SouthWestTeeSprite"	TEXT,
	"NorthWestTeeSprite"	TEXT,
	PRIMARY KEY("Identifier")
);
INSERT INTO "Tilesets" VALUES ( "TILESET_STONE", "StoneFloor", "StoneTee", "StoneCornerNorth", "StoneCornerEast", "StoneCornerSouth", "StoneCornerWest", "StoneNorthEastWall", "StoneSouthEastWall", "StoneSouthWestWall", "StoneNorthWestWall", "StoneNorthEastTee", "StoneSouthEastTee", "StoneSouthWestTee", "StoneNorthWestTee");
COMMIT;
