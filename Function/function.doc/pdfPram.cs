using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace function.doc
{
    /// <summary>
    /// pdf task(비동기 작업) 파라메터용 클래스
    /// </summary>
    public class pdfPram
    {
        /// <summary>
        /// 작업 폴더 경로
        /// </summary>
        public string FolderPath
        { get; set; }

        /// <summary>
        /// 파일명(경로포함) 정렬 여부
        /// </summary>
        public bool SortName
        { get; set; }

        /// <summary>
        /// 저장할 파일명(작업 종료 후 자동 저장 된다)
        /// </summary>
        public string save_Path
        { get; set; }

        /// <summary>
        /// 진행 상태 델리게이트 - 리턴값 double[] : 진행수 / 전체수
        /// </summary>
        public delObject delProgress
        { get; set; }

        /// <summary>
        /// 최대 이미지 수량 0: 분리하지않음
        /// </summary>
        public int MaxImgCount
        { get; set; } = 0;

        /// <summary>
        /// pdf에 폴더에 있는 image들을 추가한다.
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <param name="SortName">파일명(경로포함) 정렬 여부</param>
        /// <param name="save_Path">저장할 파일명(작업 종료 후 자동 저장 된다)</param>
        /// <param name="delProgress">진행 상태 델리게이트 - 리턴값 double[] : 진행수 / 전체수</param>
        /// <param name="MaxImgCount">최대 이미지 수량 0: 분리하지않음 </param>
        public pdfPram(string _FolderPath, bool _SortName, string _save_Path, delObject _delProgress, int _MaxImgCount = 0)
        {
            FolderPath = _FolderPath;
            SortName = _SortName;
            save_Path = _save_Path;
            delProgress = _delProgress;
            MaxImgCount = _MaxImgCount;
        }

        public pdfPram()
        {

        }
    }
}
