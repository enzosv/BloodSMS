SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `BSMS` ;
CREATE SCHEMA IF NOT EXISTS `BSMS` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `BSMS` ;

-- -----------------------------------------------------
-- Table `BSMS`.`donor`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BSMS`.`donor` ;

CREATE TABLE IF NOT EXISTS `BSMS`.`donor` (
  `donor_id` INT NOT NULL AUTO_INCREMENT,
  `last_name` VARCHAR(45) NOT NULL,
  `first_name` VARCHAR(45) NOT NULL,
  `middle_initial` VARCHAR(45) NOT NULL,
  `blood_type` INT NOT NULL,
  `home_province` INT NOT NULL,
  `home_city` INT NOT NULL,
  `home_street` VARCHAR(45) NOT NULL,
  `office_province` INT NOT NULL,
  `office_city` INT NOT NULL,
  `office_street` VARCHAR(45) NOT NULL,
  `home_landline` VARCHAR(45) NOT NULL,
  `office_landline` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `cellphone` VARCHAR(45) NOT NULL,
  `educational_attainment` INT NOT NULL,
  `birth_date` DATETIME NOT NULL,
  `date_registered` DATETIME NOT NULL,
  `next_available` DATETIME NOT NULL,
  `times_contacted` INT NOT NULL,
  `is_contactable` TINYINT(1) NOT NULL,
  `is_viable` TINYINT(1) NOT NULL,
  `reason_for_deferral` TEXT NOT NULL,
  PRIMARY KEY (`donor_id`),
  UNIQUE INDEX `idVolunteer_UNIQUE` (`donor_id` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BSMS`.`blood`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BSMS`.`blood` ;

CREATE TABLE IF NOT EXISTS `BSMS`.`blood` (
  `accession_number` VARCHAR(45) NOT NULL,
  `blood_type` INT NOT NULL,
  `donor_id` INT NULL,
  `date_donated` DATETIME NOT NULL,
  `date_removed` DATETIME NOT NULL,
  UNIQUE INDEX `accession_number_UNIQUE` (`accession_number` ASC),
  PRIMARY KEY (`accession_number`),
  INDEX `donor_id_idx` (`donor_id` ASC),
  CONSTRAINT `donor_id`
    FOREIGN KEY (`donor_id`)
    REFERENCES `BSMS`.`donor` (`donor_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BSMS`.`component`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BSMS`.`component` ;

CREATE TABLE IF NOT EXISTS `BSMS`.`component` (
  `accession_number` VARCHAR(45) NOT NULL,
  `component_name` INT NOT NULL,
  `date_processed` DATETIME NOT NULL,
  `date_reprocessed` DATETIME NOT NULL,
  `date_expired` DATETIME NOT NULL,
  `date_quarantined` DATETIME NOT NULL,
  `date_assigned` DATETIME NOT NULL,
  `date_released` DATETIME NOT NULL,
  `patient_last_name` VARCHAR(45) NOT NULL,
  `patient_first_name` VARCHAR(45) NOT NULL,
  `patient_middle_initial` VARCHAR(45) NOT NULL,
  `patient_age` VARCHAR(45) NOT NULL,
  `reason_for_removal` TEXT NOT NULL,
  PRIMARY KEY (`accession_number`),
  CONSTRAINT `accession_number`
    FOREIGN KEY (`accession_number`)
    REFERENCES `BSMS`.`blood` (`accession_number`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
