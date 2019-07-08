BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Views";
CREATE TABLE "Views" (
	"Identifier"	TEXT,
	"ResourceIdentifier" TEXT,
	"SpineSkinName"      TEXT,
	"Scale"              TEXT,
	"ShadowScale"        TEXT,
	"SortableWeight"     INTEGER
);

DROP TABLE IF EXISTS "MultitileViews";
CREATE TABLE "MultitileViews" (
	"Identifier"	TEXT
);

DROP TABLE IF EXISTS "MultitileViews_Component";
CREATE TABLE "MultitileViews_Component" (
	"Identifier"	TEXT,
	"Sprite"                 TEXT,
    "EngineOffsetX"          TEXT,
    "EngineOffsetY"          TEXT,
    "Layer"                TEXT,
    "GridOffsetX"          INTEGER,
    "GridOffsetY"          INTEGER,
    "Weight"               INTEGER,
    "Height"               INTEGER
);

INSERT INTO "Views"                                 VALUES ("VIEW_MARKER_RED", "RedMarker", "", "1.0", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_MARKER_GREEN", "GreenMarker", "", "1.0", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_MARKER_YELLOW", "YellowMarker", "", "1.0", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_MARKER_BLUE", "BlueMarker", "", "1.0", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_CHESS_PIECE", "MULTITILE_CHESSPIECE", "", "1.0", "1.5", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_RUG", "MULTITILE_RUG", "", "1.0", "1.5", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_STAIRCASE_UP", "StaircaseUp", "", "1.0", "1.0", 0);
INSERT INTO "Views"                                 VALUES ("VIEW_STAIRCASE_DOWN", "StaircaseDown", "", "1.0", "1.0", 0);
INSERT INTO "Views"                                 VALUES ("VIEW_HUMAN_ASIAN", "Humanoid_SkeletonData", "HumanAsian", "0.5", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_HUMAN_WHITE", "Humanoid_SkeletonData", "HumanWhite", "0.5", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_GHOST", "Humanoid_SkeletonData", "Ghost", "0.5", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_HUMAN_BLACK", "Humanoid_SkeletonData", "HumanBlack", "0.5", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_SKELETON_WHITE", "Humanoid_SkeletonData", "SkeletonWhite", "0.5", "1.0", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_BEE", "Bee_SkeletonData", "Template", "0.4", "0.7", 1);
INSERT INTO "Views"                                 VALUES ("VIEW_LARGE_BEE", "Bee_SkeletonData", "Template", "0.6", "1.0", 1);

INSERT INTO "MultitileViews"           VALUES("MULTITILE_CHESSPIECE");
INSERT INTO "MultitileViews_Component" VALUES("MULTITILE_CHESSPIECE", "ChessPieceBottom", "0.0", "0.0", "EntitiesAndProps", 0,0,0,0);
INSERT INTO "MultitileViews_Component" VALUES("MULTITILE_CHESSPIECE", "ChessPieceTop", "0.0", "1.143", "EntitiesAndProps", 0,0,0,1);

INSERT INTO "MultitileViews" VALUES("MULTITILE_RUG");
INSERT INTO "MultitileViews_Component" VALUES("MULTITILE_RUG", "Rug2_3", "0.0", "-0.5", "Ground", 0,0,1,0);
INSERT INTO "MultitileViews_Component" VALUES("MULTITILE_RUG", "Rug2_2", "0.0", "-0.5", "Ground", 1,0,1,1);
INSERT INTO "MultitileViews_Component" VALUES("MULTITILE_RUG", "Rug2_1", "0.0", "-0.5", "Ground", 0,1,1,0);
INSERT INTO "MultitileViews_Component" VALUES("MULTITILE_RUG", "Rug2_0", "0.0", "-0.5", "Ground", 1,1,1,1);

COMMIT;
