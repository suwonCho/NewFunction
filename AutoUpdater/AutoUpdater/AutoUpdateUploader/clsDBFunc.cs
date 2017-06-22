using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using Function;
using Function.Db;


namespace AutoUpdater
{
	class clsDBFunc
	{
		/// <summary>
		/// utype
		/// </summary>
		public enum enUType { FILE };

		/// <summary>
		/// 오토 업데이트 관련 db설정이 되어 있는지 확인한다.
		/// </summary>
		/// <param name="strConn"></param>
		public static bool DB_Init_Check(Function.Db.OracleDB.strConnect strConn)
		{
            if (OracleDB.Fnc.Table_Check_Exists(strConn, "TAUTOUPDATE")) return true;

            if (OracleDB.Fnc.Table_Check_Exists(strConn, "TAUTOUPDATEHIS")) return true;

            if (OracleDB.Fnc.PACKAGE_Check_Exists(strConn, "AUTOUPDATER_PKG")) return true;

			return false;

		}

		/// <summary>
		/// autoupdate에서 필요한 tabel, package등을 생성한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <returns></returns>
		public static void DB_Init(Function.Db.OracleDB.strConnect strConn)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn);
			
			//TAUTOUPDATE 테이블 존재 검사 후 없으면 생성 하여 준다.
            string Sql = @"
DECLARE VI_CNT INT;
BEGIN

--TAUTOUPDATE 테이블 확인
SELECT COUNT(*)
    INTO VI_CNT                                
FROM USER_OBJECTS
WHERE OBJECT_TYPE ='TABLE'
    AND OBJECT_NAME = 'TAUTOUPDATE';
    
--테이블이 있으면 삭제
IF VI_CNT > 0 THEN   
   EXECUTE IMMEDIATE  'DROP TABLE TAUTOUPDATE CASCADE CONSTRAINTS';
   VI_CNT := 0;
END IF;

--테이블 생성
    EXECUTE IMMEDIATE  'CREATE TABLE TAUTOUPDATE
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
    )';


    EXECUTE IMMEDIATE  'CREATE UNIQUE INDEX TAUTOUPDATE_PK ON TAUTOUPDATE
    (UPDATETYPE, UTYPE, FILENAME)';


    EXECUTE IMMEDIATE  'ALTER TABLE TAUTOUPDATE ADD (
      CONSTRAINT TAUTOUPDATE_PK
     PRIMARY KEY
     (UPDATETYPE, UTYPE, FILENAME))';



--TAUTOUPDATEHIS 테이블 확인
SELECT COUNT(*)
    INTO VI_CNT                                
FROM USER_OBJECTS
WHERE OBJECT_TYPE ='TABLE'
    AND OBJECT_NAME = 'TAUTOUPDATEHIS';
    
--테이블이 있으면 삭제
IF VI_CNT > 0 THEN   
   EXECUTE IMMEDIATE  'DROP TABLE TAUTOUPDATEHIS CASCADE CONSTRAINTS';
   VI_CNT := 0;
END IF;


--테이블 생성
EXECUTE IMMEDIATE  'CREATE TABLE TAUTOUPDATEHIS
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
)';


EXECUTE IMMEDIATE  'CREATE UNIQUE INDEX TAUTOUPDATEHIS_PK ON TAUTOUPDATEHIS
(UPDATETYPE, UTYPE, FILENAME, VERSION)';

EXECUTE IMMEDIATE  'ALTER TABLE TAUTOUPDATEHIS ADD (
  CONSTRAINT TAUTOUPDATEHIS_PK
 PRIMARY KEY
 (UPDATETYPE, UTYPE, FILENAME, VERSION))';
 
 
--PACKAGE생성..
EXECUTE IMMEDIATE  '
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
          ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_UType                   in        TAUTOUPDATE.UTYPE%TYPE,
      ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE 
);
  
  
  --FUNCTION MyFunction(Param1 IN NUMBER) RETURN NUMBER;
  PROCEDURE FileInfo_Get
  (
          ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
      OUT_CURSOR    OUT T_CURSOR
  );
  
  
  PROCEDURE FileInfo_GetList
  (
          ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
      OUT_CURSOR    OUT T_CURSOR
  );
  
  PROCEDURE FileInfo_GetList_History
(
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_UType                 in        TAUTOUPDATE.UTYPE%TYPE,
    ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
    OUT_CURSOR            OUT         T_CURSOR
);
  
  PROCEDURE FileInfo_GetFileImage
  (
             ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
         ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
         ps_FileImage            out    TAUTOUPDATE.FILEIMAGE%TYPE
  );
    
PROCEDURE FileInfo_Restore_OldVersion
(
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_UType                 in        TAUTOUPDATE.UTYPE%TYPE,
    ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,    
    ps_OldVersion                in        TAUTOUPDATE.VERSION%TYPE,
    ps_Msg                   out       Varchar2
);

  PROCEDURE FileInfo_Set
  (
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_UType                 in        TAUTOUPDATE.UTYPE%TYPE,
    ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
    ps_FileName2               in        TAUTOUPDATE.FILENAME2%TYPE,
    ps_Version                in        TAUTOUPDATE.VERSION%TYPE,
    ps_FileDate                in        TAUTOUPDATE.FILEDATE%TYPE,
    ps_FileImage            in        TAUTOUPDATE.FILEIMAGE%TYPE,
    ps_Text                in        TAUTOUPDATE.TEXT%TYPE,
    ps_CRC                    in        TAUTOUPDATE.CRC%TYPE,
    ps_FileSize            in        TAUTOUPDATE.FileSize%TYPE
  );
END AutoUpdater_PKG;
';

EXECUTE IMMEDIATE 'CREATE OR REPLACE PACKAGE BODY AutoUpdater_PKG AS

--저장된 파일을 삭제 한다.
PROCEDURE FileInfo_DeleteFile
(
      ps_UpdateType              in        TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_UType                   in        TAUTOUPDATE.UTYPE%TYPE,
      ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE 
)
IS
BEGIN
    
        Delete                
            FROM TAUTOUPDATE
         WHERE     UPDATETYPE = ps_UpdateType
             and    UType = ps_Utype
             and    FileName = ps_FileName;
             
             
         Delete                
            FROM TAUTOUPDATEHIS
         WHERE     UPDATETYPE = ps_UpdateType
             and    UType = ps_Utype
             and    FileName = ps_FileName;
       
END;


--저장된 파일에 대한 정보를 조회 한다.
PROCEDURE FileInfo_Get
(
          ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
      OUT_CURSOR    OUT T_CURSOR
)
IS
BEGIN
    OPEN OUT_CURSOR FOR
        SELECT        UPDATETYPE, 
                        FILENAME,
                        FILENAME2, 
                  VERSION,
                       FILEDATE,
                        FILESIZE,                   
                  UPLOADDATE,
                  CRC,
                  FileSize
            FROM TAUTOUPDATE
         WHERE     UPDATETYPE = ps_UpdateType
             and    FileName = ps_FileName;
       
END;


--update 목록을 조회 한다. old client에서 사용함..
PROCEDURE FileInfo_GetList
(
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    OUT_CURSOR            OUT     T_CURSOR
)
IS
BEGIN
    OPEN OUT_CURSOR FOR
        SELECT    UPDATETYPE,
                  UTYPE,                  
                  FILENAME,
                  FILENAME2, 
                  VERSION,
                  FILEDATE,                   
                  UPLOADDATE,                  
                  CRC,
                  TEXT,
                  FileSize
            FROM TAUTOUPDATE
         WHERE     UPDATETYPE = ps_UpdateType;
       
END;


--update history 목록을 조회 한다.
PROCEDURE FileInfo_GetList_History
(
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_UType                 in        TAUTOUPDATE.UTYPE%TYPE,
    ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
    OUT_CURSOR            OUT        T_CURSOR
)
IS
BEGIN
    OPEN OUT_CURSOR FOR
        SELECT    UPDATETYPE,
                  UTYPE, 
                  FILENAME,
                  FILENAME2, 
                  VERSION,
                  FILEDATE,                   
                  UPLOADDATE,
                  CRC,
                  FileSize
            FROM TAUTOUPDATEHIS
         WHERE     UPDATETYPE = ps_UpdateType
              AND  UTYPE = PS_UTYPE
              AND  FILENAME = PS_FILENAME;
       
END;







--저장된 파일에 대한 정보를 조회 한다.
PROCEDURE FileInfo_GetFileImage
(
          ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
      ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
      ps_FileImage            out    TAUTOUPDATE.FILEIMAGE%TYPE
)
IS
BEGIN
    
        SELECT        FILEIMAGE
                Into    ps_FileImage
            FROM TAUTOUPDATE
         WHERE     UPDATETYPE = ps_UpdateType
             AND    UTYPE = ''FILE''
             and    FileName = ps_FileName;
       
END;




--update항목을 기존 버전으로 되돌린다.
PROCEDURE FileInfo_Restore_OldVersion
(
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_UType                 in        TAUTOUPDATE.UTYPE%TYPE,
    ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,    
    ps_OldVersion                in        TAUTOUPDATE.VERSION%TYPE,
    ps_Msg                   out       Varchar2
)
IS
    vi_Count            int;
    vs_CurrVersion          varchar2(20);
BEGIN
    
    --OLD VERSION 데이터가 있는지 확인한다.
    Select count(*)
        into vi_Count
    From tAutoUpdateHis
    Where      UpdateType = ps_UpdateType
        and    UType = ps_UType
        and    FileName = ps_FileName
        and    Version = ps_oldVersion;
   
    if vi_Count < 1 then
       ps_Msg := ''원복할 기존 데이터가 존재하지 않습니다. v.'' || ps_oldVersion;
       goto End_Proc;
    end if;
    
   --현재 버전을 history로 옮긴다.
   Begin
      Select Version
         into vs_CurrVersion
      From tAutoUpdate 
      where  UpdateType = ps_UpdateType
         and UType = ps_UType
         and FileName = ps_FileName;           
      
      --history에서 같은 버젼 삭제..
      Delete  
      From tAutoUpdateHis
      Where       UpdateType = ps_UpdateType
            and    UType = ps_UType
            and    FileName = ps_FileName
            and    Version = vs_CurrVersion;
            
      --insert
      INSERT INTO  tAutoUpdatehis
                 ( UpdateType,
                   UType, 
                   FileName,
                   FileName2,
                   Version,
                   FileDate,
                   FileImage,
                   TEXT,
                   UploadDate,
                   CRC,
                   FileSize
                 )
            SELECT UpdateType,
                   UType, 
                   FileName,
                   FileName2,
                   Version,
                   FileDate,
                   FileImage,
                   TEXT,
                   UploadDate,
                   CRC,
                   FileSize
            From tAutoUpdate
            Where     UpdateType = ps_UpdateType
                and   UType = ps_Utype
                and   FileName = ps_FileName;
   
   Exception When No_data_found then
      vi_Count := 0;
   End; 
   
   --현재 데이터 삭제..
   delete
      From tAutoUpdate 
      where  UpdateType = ps_UpdateType
         and UType = ps_UType
         and FileName = ps_FileName;
         
   --retore
    INSERT INTO  tAutoUpdate
                 ( UpdateType,
                   UType, 
                   FileName,
                   FileName2,
                   Version,
                   FileDate,
                   FileImage,
                   TEXT,
                   UploadDate,
                   CRC,
                   FileSize
                 )
            SELECT UpdateType,
                   UType, 
                   FileName,
                   FileName2,
                   Version,
                   FileDate,
                   FileImage,
                   TEXT,
                   UploadDate,
                   CRC,
                   FileSize
            From tAutoUpdateHis
            Where      UpdateType = ps_UpdateType
                and    UType = ps_UType
                and    FileName = ps_FileName
                and    Version = ps_oldVersion;
    
    --원복한 데이터 histoty에서 삭제..
     delete
     From tAutoUpdateHis
            Where      UpdateType = ps_UpdateType
                and    UType = ps_UType
                and    FileName = ps_FileName
                and    Version = ps_oldVersion;
    
    ps_msg := '''';                
    commit;
    
    <<End_Proc>>
    vi_Count := 0;

Exception When others then
    rollback;
    raise;
    
END;






--update항목을 저장 한다.
PROCEDURE FileInfo_Set
(
    ps_UpdateType            in        TAUTOUPDATE.UPDATETYPE%TYPE,
    ps_UType                 in        TAUTOUPDATE.UTYPE%TYPE,
    ps_FileName                in        TAUTOUPDATE.FILENAME%TYPE,
    ps_FileName2               in        TAUTOUPDATE.FILENAME2%TYPE,
    ps_Version                in        TAUTOUPDATE.VERSION%TYPE,
    ps_FileDate                in        TAUTOUPDATE.FILEDATE%TYPE,
    ps_FileImage            in        TAUTOUPDATE.FILEIMAGE%TYPE,
    ps_Text                in        TAUTOUPDATE.TEXT%TYPE,
    ps_CRC                    in        TAUTOUPDATE.CRC%TYPE,
    ps_FileSize            in        TAUTOUPDATE.FileSize%TYPE
)
IS
    vi_Count            int;
    vs_OldVersion          varchar2(20);
BEGIN
    Select count(*)
        into vi_Count
    From tAutoUpdate
    Where     UpdateType = ps_UpdateType
        and    FileName = ps_FileName;
      
      
    if vi_Count < 1 then
        Insert into    tAutoUpdate
              (     UpdateType,
               UType, 
               FileName,
               FileName2,
               Version,
               FileDate,
               FileImage,
               TEXT,
               UploadDate,
               CRC,
               FileSize )
        Values ( ps_UpdateType,
               ps_UType,
                ps_FileName,
               ps_FileName2,
               ps_Version,
               ps_FileDate,
               ps_FileImage,
               ps_Text,
               sysdate,
               ps_CRC,
               ps_FileSize );    
    else
        
        Select version
            into vs_OldVersion
        From tAutoUpdate
        Where     UpdateType = ps_UpdateType
            and    FileName = ps_FileName;
        
        --버젼이 변경되면 history에 추가 한다.
        if(nvl(vs_OldVersion, '' '') != nvl(ps_Version, '' '')) then 
        
            Delete
            From tAutoUpdateHis
            Where     UpdateType = ps_UpdateType
                and   Version in (ps_Version, vs_OldVersion)             
                and    FileName = ps_FileName;
        
            INSERT INTO  tAutoUpdateHis
                 ( UpdateType,
                   UType, 
                   FileName,
                   FileName2,
                   Version,
                   FileDate,
                   FileImage,
                   TEXT,
                   UploadDate,
                   CRC,
                   FileSize
                 )
            SELECT UpdateType,
                   UType, 
                   FileName,
                   FileName2,
                   Version,
                   FileDate,
                   FileImage,
                   TEXT,
                   UploadDate,
                   CRC,
                   FileSize
            From tAutoUpdate
            Where     UpdateType = ps_UpdateType
                and    FileName = ps_FileName;
                
        end if;
        
        
    
        Update  tAutoUpdate
          Set  Version = ps_Version,
               FileDate = ps_FileDate,
               FileImage = ps_FileImage,
               TEXT = ps_Text,
               UploadDate = sysdate,
               CRC = ps_CRC,
               FileSize = ps_FileSize
         where UpdateType = ps_UpdateType 
          and     FileName = ps_FileName;
    
    end if;
    
    commit;
    
END;

END AutoUpdater_PKG;';

END;
";
            clsDB.intExcute_Query(OracleDB.Fnc.RemoveSpLetter(Sql));

		}


		/// <summary>
		/// db에 저장된 파일 정보를 삭제를 한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		public static int FileInfo_DeleteFile(Function.Db.OracleDB.strConnect strConn, string strUpdateType, string strUType, string strFileName)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_UType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100) };

			param[0].Value = strUpdateType;
			param[1].Value = strUType;
			param[2].Value = strFileName;

			return clsDB.intExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_DeleteFile", param);
		}


		/// <summary>
		/// db에 저장된 파일 정보를 조회 한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		public static DataSet FileInfo_Get(Function.Db.OracleDB.strConnect strConn, string strUpdateType, string strFileName)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),												
                                                new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor)      };


			param[2].Direction = ParameterDirection.Output;

			param[0].Value = strUpdateType;
			param[1].Value = strFileName;


			return clsDB.dsExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_Get", param);
		}


		/// <summary>
		/// 파일 update history 조회
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strUtype"></param>
		/// <param name="strFileName"></param>
		/// <returns></returns>
		public static DataSet FileInfo_Get_History(Function.Db.OracleDB.strConnect strConn, string strUpdateType, enUType UType, string strFileName)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_UType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),												
                                                new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor)      };


			param[3].Direction = ParameterDirection.Output;

			param[0].Value = strUpdateType;
			param[1].Value = UType.ToString();
			param[2].Value = strFileName;


			return clsDB.dsExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_GetList_History", param);
		}



		/// <summary>
		/// db에 저장된 파일 정보 목록을 조회한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>		
		public static DataSet FileInfo_GetList(Function.Db.OracleDB.strConnect strConn, string strUpdateType)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),																							
                                                new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor)      };


			param[1].Direction = ParameterDirection.Output;

			param[0].Value = strUpdateType;



			return clsDB.dsExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_GetList", param);
		}


		/// <summary>
		/// db에 파일정보를 저장한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		public static void FileInfo_Save(Function.Db.OracleDB.strConnect strConn, string strUpdateType, System.IO.FileInfo fi, string strCRC)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),												
												new OracleParameter("ps_VERSION", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileDATE", OracleDbType.Date, 8),
                                                new OracleParameter("ps_FileImage", OracleDbType.Blob, Convert.ToInt32(fi.Length)),
												new OracleParameter("ps_CRC", OracleDbType.Varchar2, 100)};

			param[0].Value = strUpdateType;
			param[1].Value = fi.Name.ToUpper();
			param[2].Value = system.clsFile.FileGetVersion(fi.FullName);
			param[3].Value = fi.LastWriteTime;
			param[5].Value = strCRC;
			

			clsDB.Excute_StoredProcedure("AutoUpdater_PKG.FileInfo_Get", param, 4, fi);
		}



		

		/// <summary>
		/// db에 파일정보를 저장한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		public static void FileInfo_Save(Function.Db.OracleDB.strConnect strConn, enUType UType, string strUpdateType, System.IO.FileInfo fi,
			string strCRC, string strText, Function.Db.OracleDB.delExcuteProcedure_Progress evtP)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);


			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),						//0
												new OracleParameter("ps_UType", OracleDbType.Varchar2, 20),								//1
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),							//2
												new OracleParameter("ps_FileName2", OracleDbType.Varchar2, 100),						//3
												new OracleParameter("ps_Version", OracleDbType.Varchar2, 20),							//4
												new OracleParameter("ps_FileDate", OracleDbType.Date, 8),								//5
                                                new OracleParameter("ps_FileImage", OracleDbType.Blob, Convert.ToInt32(fi.Length)),		//6
												new OracleParameter("ps_Text", OracleDbType.Varchar2, 400),								//7
												new OracleParameter("ps_CRC", OracleDbType.Varchar2, 100),								//8
												new OracleParameter("ps_FileSize", OracleDbType.Int32, 8) };							//9

			param[0].Value = strUpdateType;
			param[1].Value = UType.ToString();
			param[2].Value = fi.Name.ToUpper();
			param[3].Value = fi.Name;
			param[4].Value = system.clsFile.FileGetVersion(fi.FullName);
			param[5].Value = fi.LastWriteTime;

			param[7].Value = Function.Db.OracleDB.Fnc.StringEmpty2DbNull(strText);
			param[8].Value = strCRC;
			param[9].Value = fi.Length;


			clsDB.Excute_StoredProcedure("AutoUpdater_PKG.FileInfo_Set", param, 6, fi, evtP);


		}

		/// <summary>
		/// 기존 버젼으로 되돌린다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="UType"></param>
		/// <param name="strFileName"></param>
		/// <param name="strOldVersion"></param>
		/// <returns></returns>
		public static string FileInfo_Restore_OldVersion(Function.Db.OracleDB.strConnect strConn, string strUpdateType, enUType UType, string strFileName, string strOldVersion)
		{
			Function.Db.OracleDB clsDB = new Function.Db.OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_UType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),												
												new OracleParameter("ps_OldVersion", OracleDbType.Varchar2, 100),
												new OracleParameter("ps_Msg", OracleDbType.Varchar2, 400) };


			param[4].Direction = ParameterDirection.Output;

			param[0].Value = strUpdateType;
			param[1].Value = UType.ToString();
			param[2].Value = strFileName;
			param[3].Value = strOldVersion;


			clsDB.intExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_Restore_OldVersion", param);

			return Fnc.obj2String(param[4].Value);


		}


	}
}
