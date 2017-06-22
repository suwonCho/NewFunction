CREATE OR REPLACE PACKAGE AutoUpdater_PKG AS
/******************************************************************************
   NAME:       AutoUpdater_PKG
   PURPOSE:

   REVISIONS:
   Ver        Date        Author           Description
   ---------  ----------  ---------------  ------------------------------------
   1.0        2009-09-17      CSW       1. Created this package.
******************************************************************************/
  TYPE T_CURSOR IS REF CURSOR;
  
  PROCEDURE FileInfo_DeleteFile
(
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE 
);
  
  
  --FUNCTION MyFunction(Param1 IN NUMBER) RETURN NUMBER;
  PROCEDURE FileInfo_Get
  (
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE,
      OUT_CURSOR    OUT T_CURSOR
  );
  
  
  PROCEDURE FileInfo_GetList
  (
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      OUT_CURSOR    OUT T_CURSOR
  );
  
  PROCEDURE FileInfo_GetFileImage
  (
     		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
         ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE,
         ps_FileImage			out	TAUTOUPDATE.FILEIMAGE%TYPE
  );
	

  PROCEDURE FileInfo_Set
  (
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE,
      ps_Version				in		TAUTOUPDATE.VERSION%TYPE,
      ps_FileDate				in		TAUTOUPDATE.FILEDATE%TYPE,
      ps_FileImage			in		TAUTOUPDATE.FILEIMAGE%TYPE,
      ps_CRC					in		TAUTOUPDATE.CRC%TYPE,
      ps_FileSize				in		TAUTOUPDATE.FileSize%TYPE
  );
END AutoUpdater_PKG; 
/

