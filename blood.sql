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
  `name` VARCHAR(45) NOT NULL DEFAULT '',
  `blood_type` INT NOT NULL DEFAULT 0,
  `home_province` VARCHAR(45) NOT NULL DEFAULT '',
  `home_city` VARCHAR(45) NOT NULL DEFAULT '',
  `home_street` VARCHAR(45) NOT NULL,
  `office_province` VARCHAR(45) NOT NULL DEFAULT '',
  `office_city` VARCHAR(45) NOT NULL DEFAULT '',
  `office_street` VARCHAR(45) NOT NULL,
  `preferred_contact_method` INT NOT NULL DEFAULT 0,
  `home_landline` VARCHAR(45) NOT NULL DEFAULT '',
  `office_landline` VARCHAR(45) NOT NULL DEFAULT '',
  `email` VARCHAR(45) NOT NULL DEFAULT '',
  `cellphone` VARCHAR(45) NOT NULL DEFAULT '',
  `educational_attainment` INT NOT NULL DEFAULT 0,
  `birth_date` DATETIME NOT NULL,
  `date_registered` DATETIME NOT NULL,
  `next_available` DATETIME NOT NULL,
  `times_donated` INT NOT NULL DEFAULT 0,
  `times_contacted` INT NOT NULL DEFAULT 0,
  `is_contactable` TINYINT(1) NOT NULL DEFAULT 0,
  `is_viable` TINYINT(1) NOT NULL DEFAULT 0,
  `reason_for_deferral` TEXT NOT NULL,
  PRIMARY KEY (`donor_id`),
  UNIQUE INDEX `idVolunteer_UNIQUE` (`donor_id` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BSMS`.`blood`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BSMS`.`blood` ;

CREATE TABLE IF NOT EXISTS `BSMS`.`blood` (
  `accession_number` VARCHAR(45) NOT NULL DEFAULT '',
  `blood_type` INT NOT NULL DEFAULT 0,
  `date_added` DATETIME NOT NULL,
  `donor_id` INT NULL,
  `patient_name` VARCHAR(45) NOT NULL DEFAULT '',
  `patient_age` INT NOT NULL DEFAULT 0,
  `date_expire` DATETIME NOT NULL,
  `date_removed` DATETIME NOT NULL,
  `is_assigned` TINYINT(1) NOT NULL DEFAULT 0,
  `is_processed` TINYINT(1) NOT NULL DEFAULT 0,
  `is_quarantined` TINYINT(1) NOT NULL DEFAULT 0,
  `reason_for_removal` TEXT NOT NULL,
  UNIQUE INDEX `accession_number_UNIQUE` (`accession_number` ASC),
  PRIMARY KEY (`accession_number`),
  INDEX `donor_id_idx` (`donor_id` ASC),
  CONSTRAINT `donor_id`
    FOREIGN KEY (`donor_id`)
    REFERENCES `BSMS`.`donor` (`donor_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
