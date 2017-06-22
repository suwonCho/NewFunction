CREATE TABLE TAUTOUPDATE
(
  UPDATETYPE  VARCHAR2(20 BYTE),
  UTYPE       VARCHAR2(20 BYTE),
  FILENAME    VARCHAR2(100 BYTE),
  FILENAME2   VARCHAR2(100 BYTE),
  VERSION     VARCHAR2(20 BYTE),
  FILEDATE    DATE,
  FILEIMAGE   BLOB,
  TEXT        VARCHAR2(4000 BYTE),
  UPLOADDATE  DATE,
  CRC         VARCHAR2(100 BYTE),
  FILESIZE    INTEGER
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE TABLE TAUTOUPDATEHIS
(
  UPDATETYPE  VARCHAR2(20 BYTE),
  UTYPE       VARCHAR2(20 BYTE),
  VERSION     VARCHAR2(20 BYTE),
  FILENAME    VARCHAR2(100 BYTE),
  FILENAME2   VARCHAR2(100 BYTE),
  FILEDATE    DATE,
  FILEIMAGE   BLOB,
  TEXT        VARCHAR2(4000 BYTE),
  UPLOADDATE  DATE,
  CRC         VARCHAR2(100 BYTE),
  FILESIZE    INTEGER
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE UNIQUE INDEX TAUTOUPDATE_PK ON TAUTOUPDATE
(UPDATETYPE, UTYPE, FILENAME)
LOGGING
NOPARALLEL;


CREATE UNIQUE INDEX TAUTOUPDATEHIS_PK ON TAUTOUPDATEHIS
(UPDATETYPE, UTYPE, FILENAME, VERSION)
LOGGING
NOPARALLEL;


ALTER TABLE TAUTOUPDATE ADD (
  CONSTRAINT TAUTOUPDATE_PK
 PRIMARY KEY
 (UPDATETYPE, UTYPE, FILENAME));


ALTER TABLE TAUTOUPDATEHIS ADD (
  CONSTRAINT TAUTOUPDATEHIS_PK
 PRIMARY KEY
 (UPDATETYPE, UTYPE, FILENAME, VERSION));
