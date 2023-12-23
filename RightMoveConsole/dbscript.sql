CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `ResultsTable` (
    `ResultsTableId` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_ResultsTable` PRIMARY KEY (`ResultsTableId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Properties` (
    `RightMovePropertyId` int NOT NULL AUTO_INCREMENT,
    `RightMoveId` int NOT NULL,
    `HouseInfo` longtext CHARACTER SET utf8mb4 NULL,
    `Address` longtext CHARACTER SET utf8mb4 NULL,
    `DateAdded` datetime(6) NOT NULL,
    `DateReduced` datetime(6) NOT NULL,
    `Date` datetime(6) NOT NULL,
    `Prices` longtext CHARACTER SET utf8mb4 NULL,
    `Dates` longtext CHARACTER SET utf8mb4 NULL,
    `ResultsTableId` int NOT NULL,
    CONSTRAINT `PK_Properties` PRIMARY KEY (`RightMovePropertyId`),
    CONSTRAINT `FK_Properties_ResultsTable_ResultsTableId` FOREIGN KEY (`ResultsTableId`) REFERENCES `ResultsTable` (`ResultsTableId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Properties_ResultsTableId` ON `Properties` (`ResultsTableId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20231214235930_InitialCreate', '7.0.13');

COMMIT;

