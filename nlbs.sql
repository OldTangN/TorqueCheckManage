/*
Navicat MySQL Data Transfer

Source Server         : MySQl
Source Server Version : 50519
Source Host           : localhost:3306
Source Database       : nlbs

Target Server Type    : MYSQL
Target Server Version : 50519
File Encoding         : 65001

Date: 2015-04-07 17:57:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `department`
-- ----------------------------
DROP TABLE IF EXISTS `department`;
CREATE TABLE `department` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `departmentName` varchar(50) NOT NULL,
  `parentDepartmentID_id` int(11) DEFAULT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`),
  KEY `department_d80600e3` (`parentDepartmentID_id`),
  CONSTRAINT `parentDepartmentID_id_refs_id_82b20a37` FOREIGN KEY (`parentDepartmentID_id`) REFERENCES `department` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of department
-- ----------------------------

-- ----------------------------
-- Table structure for `django_session`
-- ----------------------------
DROP TABLE IF EXISTS `django_session`;
CREATE TABLE `django_session` (
  `session_key` varchar(40) NOT NULL,
  `session_data` longtext NOT NULL,
  `expire_date` datetime NOT NULL,
  PRIMARY KEY (`session_key`),
  KEY `django_session_b7b81f0c` (`expire_date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of django_session
-- ----------------------------

-- ----------------------------
-- Table structure for `torquecheckrecord`
-- ----------------------------
DROP TABLE IF EXISTS `torquecheckrecord`;
CREATE TABLE `torquecheckrecord` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `TorqueCheckTargetID_id` varchar(50) NOT NULL,
  `analyserValue` decimal(6,2) NOT NULL,
  `checkTime` datetime NOT NULL,
  `passedFlag` tinyint(1) NOT NULL,
  `isEffective` tinyint(1) NOT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`),
  KEY `torquecheckrecord_86ac7ecf` (`TorqueCheckTargetID_id`),
  CONSTRAINT `TorqueCheckTargetID_id_refs_guid_e40845e6` FOREIGN KEY (`TorqueCheckTargetID_id`) REFERENCES `torquechecktarget` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of torquecheckrecord
-- ----------------------------

-- ----------------------------
-- Table structure for `torquechecktarget`
-- ----------------------------
DROP TABLE IF EXISTS `torquechecktarget`;
CREATE TABLE `torquechecktarget` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `wrenchID_id` varchar(50) NOT NULL,
  `checkDate` datetime NOT NULL,
  `qaID_id` varchar(50) NOT NULL,
  `operatorID_id` varchar(50) NOT NULL,
  `torqueTargetValue` decimal(6,2) NOT NULL,
  `errorRange` varchar(10) NOT NULL,
  `is_good` tinyint(1) NOT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`),
  KEY `torquechecktarget_32a0c3f5` (`wrenchID_id`),
  KEY `torquechecktarget_25b454eb` (`qaID_id`),
  KEY `torquechecktarget_1640affb` (`operatorID_id`),
  CONSTRAINT `wrenchID_id_refs_guid_b442afe1` FOREIGN KEY (`wrenchID_id`) REFERENCES `wrench` (`guid`),
  CONSTRAINT `operatorID_id_refs_guid_adfa76ac` FOREIGN KEY (`operatorID_id`) REFERENCES `users` (`guid`),
  CONSTRAINT `qaID_id_refs_guid_adfa76ac` FOREIGN KEY (`qaID_id`) REFERENCES `users` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of torquechecktarget
-- ----------------------------

-- ----------------------------
-- Table structure for `userrole`
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `roleName` varchar(50) NOT NULL,
  `dm` varchar(4) NOT NULL,
  `comment` longtext,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userrole
-- ----------------------------

-- ----------------------------
-- Table structure for `users`
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `is_superuser` tinyint(1) NOT NULL,
  `is_staff` tinyint(1) NOT NULL,
  `empID` varchar(50) NOT NULL,
  `cardID` varchar(50) NOT NULL,
  `departmentID_id` varchar(50) NOT NULL,
  `roleID_id` varchar(50) NOT NULL,
  `phoneNumber` varchar(50) NOT NULL,
  `comment` longtext NOT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`),
  KEY `users_85dc2e2e` (`departmentID_id`),
  KEY `users_c7af1352` (`roleID_id`),
  CONSTRAINT `departmentID_id_refs_guid_e6325826` FOREIGN KEY (`departmentID_id`) REFERENCES `department` (`guid`),
  CONSTRAINT `roleID_id_refs_guid_b3b89012` FOREIGN KEY (`roleID_id`) REFERENCES `userrole` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users
-- ----------------------------

-- ----------------------------
-- Table structure for `wrench`
-- ----------------------------
DROP TABLE IF EXISTS `wrench`;
CREATE TABLE `wrench` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `wrenchCode` varchar(50) NOT NULL,
  `wrenchBarCode` varchar(50) NOT NULL,
  `rangeMin` decimal(6,2) NOT NULL,
  `rangeMax` decimal(6,2) NOT NULL,
  `factory` varchar(50) NOT NULL,
  `createDate` datetime NOT NULL,
  `IP` varchar(50) DEFAULT NULL,
  `port` varchar(10) DEFAULT NULL,
  `species_id` varchar(50) NOT NULL,
  `status_id` varchar(50) NOT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`),
  KEY `wrench_e1800d51` (`species_id`),
  KEY `wrench_48fb58bb` (`status_id`),
  CONSTRAINT `species_id_refs_guid_99d0cc6f` FOREIGN KEY (`species_id`) REFERENCES `wrenchspecies` (`guid`),
  CONSTRAINT `status_id_refs_guid_c17ee1b5` FOREIGN KEY (`status_id`) REFERENCES `wrenchstatus` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of wrench
-- ----------------------------

-- ----------------------------
-- Table structure for `wrenchspecies`
-- ----------------------------
DROP TABLE IF EXISTS `wrenchspecies`;
CREATE TABLE `wrenchspecies` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `speciesName` varchar(50) NOT NULL,
  `speciesCode` varchar(50) NOT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of wrenchspecies
-- ----------------------------

-- ----------------------------
-- Table structure for `wrenchstatus`
-- ----------------------------
DROP TABLE IF EXISTS `wrenchstatus`;
CREATE TABLE `wrenchstatus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `statusName` varchar(50) NOT NULL,
  `guid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of wrenchstatus
-- ----------------------------
