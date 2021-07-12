CREATE TABLE `rawregiondata` (
  `DealerCD` CHAR(6) NULL,
  `Region` VARCHAR(40) NULL,
  `Province` VARCHAR(200) NULL,
  `Timestamp` TIMESTAMP NULL DEFAULT NOW());