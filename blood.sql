SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `BSMS` ;
CREATE SCHEMA IF NOT EXISTS `BSMS` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `BSMS` ;

-- -----------------------------------------------------
-- Table `BSMS`.`blood`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BSMS`.`blood` ;

CREATE TABLE IF NOT EXISTS `BSMS`.`blood` (
  `blood_id` INT NOT NULL AUTO_INCREMENT,
  `taken_from` INT NOT NULL,
  `patient_name` VARCHAR(45) NOT NULL DEFAULT '',
  `patient_age` INT NOT NULL DEFAULT 0,
  `date_added` DATETIME NOT NULL,
  `date_expire` DATETIME NOT NULL,
  `date_removed` DATETIME NULL,
  `is_assigned` TINYINT(1) NOT NULL DEFAULT false,
  `is_quarantined` TINYINT(1) NOT NULL DEFAULT false,
  `reason_for_removal` TEXT NOT NULL,
  `component` VARCHAR(45) NOT NULL DEFAULT 'whole',
  PRIMARY KEY (`blood_id`),
  UNIQUE INDEX `idBlood_UNIQUE` (`blood_id` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BSMS`.`donor`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BSMS`.`donor` ;

CREATE TABLE IF NOT EXISTS `BSMS`.`donor` (
  `donor_id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `blood_type` ENUM('ABp','ABn','Ap','An','Bp','Bn','Op','On') NOT NULL,
  `home_province` VARCHAR(45) NOT NULL DEFAULT '',
  `home_city` VARCHAR(45) NOT NULL DEFAULT '',
  `home_street` VARCHAR(45) NOT NULL DEFAULT '',
  `office_province` VARCHAR(45) NOT NULL DEFAULT '',
  `office_city` VARCHAR(45) NOT NULL DEFAULT '',
  `office_street` VARCHAR(45) NOT NULL DEFAULT '',
  `preferred_contact_method` ENUM('home_landline', 'office_landline', 'cellphone', 'email') NOT NULL,
  `home_landline` VARCHAR(45) NOT NULL DEFAULT '',
  `office_landline` VARCHAR(45) NOT NULL DEFAULT '',
  `email` VARCHAR(45) NOT NULL DEFAULT '',
  `cellphone` VARCHAR(45) NOT NULL DEFAULT '',
  `educational_attainment` VARCHAR(45) NOT NULL DEFAULT '',
  `birth_date` DATETIME NOT NULL,
  `date_registered` DATETIME NOT NULL,
  `last_donation` DATETIME NOT NULL,
  `next_available` DATETIME NOT NULL,
  `times_donated` INT NOT NULL DEFAULT 0,
  `times_contacted` INT NOT NULL DEFAULT 0,
  `is_contactable` TINYINT(1) NOT NULL DEFAULT false,
  `is_viable` TINYINT(1) NOT NULL DEFAULT False,
  `reason_for_deferral` TEXT NOT NULL,
  PRIMARY KEY (`donor_id`),
  UNIQUE INDEX `idVolunteer_UNIQUE` (`donor_id` ASC))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
