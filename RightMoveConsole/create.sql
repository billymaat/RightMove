﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "ResultsTable" (
    "ResultsTableId" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text,
    CONSTRAINT "PK_ResultsTable" PRIMARY KEY ("ResultsTableId")
);

CREATE TABLE "Properties" (
    "RightMovePropertyId" integer GENERATED BY DEFAULT AS IDENTITY,
    "RightMoveId" integer NOT NULL,
    "HouseInfo" text,
    "Address" text,
    "DateAdded" timestamp with time zone NOT NULL,
    "DateReduced" timestamp with time zone NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "ResultsTableId" integer NOT NULL,
    CONSTRAINT "PK_Properties" PRIMARY KEY ("RightMovePropertyId"),
    CONSTRAINT "FK_Properties_ResultsTable_ResultsTableId" FOREIGN KEY ("ResultsTableId") REFERENCES "ResultsTable" ("ResultsTableId") ON DELETE CASCADE
);

CREATE TABLE "Prices" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Date" timestamp with time zone NOT NULL,
    "Price" integer NOT NULL,
    "RightMovePropertyEntityRightMovePropertyId" integer,
    CONSTRAINT "PK_Prices" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Prices_Properties_RightMovePropertyEntityRightMovePropertyId" FOREIGN KEY ("RightMovePropertyEntityRightMovePropertyId") REFERENCES "Properties" ("RightMovePropertyId")
);

CREATE INDEX "IX_Prices_RightMovePropertyEntityRightMovePropertyId" ON "Prices" ("RightMovePropertyEntityRightMovePropertyId");

CREATE INDEX "IX_Properties_ResultsTableId" ON "Properties" ("ResultsTableId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240613183943_InitialCreatePostGre', '8.0.6');

COMMIT;

