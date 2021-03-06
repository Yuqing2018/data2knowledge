CREATE TABLE `rawtechsupportdata` (
  `ID` INTEGER auto_increment NOT NULL,
  `WR_NO` VARCHAR(50) NOT NULL,
  `SUBJECT` VARCHAR(300) NULL,
  `CONSULTATION_DATE` TIMESTAMP NULL,
  `DEALER_CD` CHAR(6) NULL,
  `FRAME_NO` CHAR(17) NULL,
  `CAR_TYPE` CHAR(3) NULL,
  `CAR_TYPE_NAME` CHAR(3) NULL,
  `MILE_AGE` INT NULL,
  `SHIPPING_DATE` VARCHAR(8) NULL,
  `RANK` CHAR(1) NULL,
  `PRODUCTION_DATE` VARCHAR(8) NULL,
  `FAULT_DATE` TIMESTAMP NULL,
  `SYMPTOM_DESC` VARCHAR(2500) NULL,
  `SPECIFIC_PHENOMENON` VARCHAR(2500) NULL,
  `MAINTENANCE_PROCESS` VARCHAR(2500) NULL,
  `MAINTENANCE_PROGRAM` VARCHAR(2500) NULL,
  `AUDIT_OPINION` INT NULL,
  `Timestamp` TIMESTAMP NULL DEFAULT NOW(),
  PRIMARY KEY (`ID`));