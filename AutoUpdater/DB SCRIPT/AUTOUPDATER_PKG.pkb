CREATE OR REPLACE PACKAGE BODY AutoUpdater_PKG AS

--저장된 파일을 삭제 한다.
PROCEDURE FileInfo_DeleteFile
(
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE 
)
IS
BEGIN
    
        Delete        		
			FROM TAUTOUPDATE
         WHERE 	UPDATETYPE = ps_UpdateType
         	and	FileName = ps_FileName;
       
END;


--저장된 파일에 대한 정보를 조회 한다.
PROCEDURE FileInfo_Get
(
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE,
      OUT_CURSOR    OUT T_CURSOR
)
IS
BEGIN
    OPEN OUT_CURSOR FOR
        SELECT		UPDATETYPE, 
        				FILENAME, 
                  VERSION,
   					FILEDATE,
        				FILESIZE,                   
                  UPLOADDATE,
                  CRC,
                  FileSize
			FROM TAUTOUPDATE
         WHERE 	UPDATETYPE = ps_UpdateType
         	and	FileName = ps_FileName;
       
END;


--update 목록을 조회 한다.
PROCEDURE FileInfo_GetList
(
	 ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
    OUT_CURSOR    		OUT 	T_CURSOR
)
IS
BEGIN
    OPEN OUT_CURSOR FOR
        SELECT		UPDATETYPE, 
        				FILENAME, 
                  VERSION,
   					FILEDATE,                   
                  UPLOADDATE,
                  CRC,
                  FileSize
			FROM TAUTOUPDATE
         WHERE 	UPDATETYPE = ps_UpdateType;
       
END;


--저장된 파일에 대한 정보를 조회 한다.
PROCEDURE FileInfo_GetFileImage
(
  		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE,
      ps_FileImage			out	TAUTOUPDATE.FILEIMAGE%TYPE
)
IS
BEGIN
    
        SELECT		FILEIMAGE
        		Into	ps_FileImage
			FROM TAUTOUPDATE
         WHERE 	UPDATETYPE = ps_UpdateType
         	and	FileName = ps_FileName;
       
END;


--update항목을 저장 한다.
PROCEDURE FileInfo_Set
(
		ps_UpdateType			in		TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_FileName				in		TAUTOUPDATE.FILENAME%TYPE,
    ps_Version				in		TAUTOUPDATE.VERSION%TYPE,
    ps_FileDate				in		TAUTOUPDATE.FILEDATE%TYPE,
    ps_FileImage			in		TAUTOUPDATE.FILEIMAGE%TYPE,
    ps_CRC					in		TAUTOUPDATE.CRC%TYPE,
    ps_FileSize			in		TAUTOUPDATE.FileSize%TYPE
)
IS
	vi_Count			int;
BEGIN
    Select count(*)
    	into vi_Count
    From tAutoUpdate
    Where 	UpdateType = ps_UpdateType
    	and	FileName = ps_FileName;
      
      
    if vi_Count < 1 then
    	Insert into	tAutoUpdate
      		( 	UpdateType, 
            	FileName,
               Version,
               FileDate,
               FileImage,
               UploadDate,
               CRC,
               FileSize )
		Values ( ps_UpdateType, 
            	ps_FileName,
               ps_Version,
               ps_FileDate,
               ps_FileImage,
               sysdate,
               ps_CRC,
               ps_FileSize );    
    else
    	Update  tAutoUpdate
      	Set   Version = ps_Version,
               FileDate = ps_FileDate,
               FileImage = ps_FileImage,
               UploadDate = sysdate,
               CRC = ps_CRC,
               FileSize = ps_FileSize
         where UpdateType = ps_UpdateType 
          and 	FileName = ps_FileName;
    
    end if;
    
    commit;
    
END;


END AutoUpdater_PKG; 
/

