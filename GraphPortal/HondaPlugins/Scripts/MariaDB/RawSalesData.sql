CREATE TABLE `rawsalesdata` (
  `CarModel` VARCHAR(40) NULL,
  `CarType` CHAR(3) NULL,
  `ModelYear` VARCHAR(4) NULL,
  `FrameNo` VARCHAR(34) NULL,
  `ProductionDate` VARCHAR(8) NULL,
  `InitialRegistDate` VARCHAR(8) NULL,
  `Timestamp` TIMESTAMP NULL DEFAULT NOW());