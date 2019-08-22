BEGIN TRANSACTION;

DROP TABLE IF EXISTS "Skills";
CREATE TABLE IF NOT EXISTS "Skills" (
	"Identifier"      TEXT,
	"Name"            TEXT,
	"UISprite"          TEXT
);

COMMIT;
