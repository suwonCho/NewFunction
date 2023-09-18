using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using function;
using System.IO;
using System.Threading;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace function.doc
{
    /// <summary>
    /// pdf 문서처리
    /// </summary>
    public class pdf
    {
        PdfDocument doc;

        public static string[] ImageExtention = new string[] {".png", ".gif", ".jpg", ".jpeg" };

        /// <summary>
        /// 작업 취소 여부
        /// </summary>
        public bool isWorkCancel
        {
            get; set;
        } = false;


        /// <summary>
        /// pdf 문서 클래스를 생성한다.
        /// </summary>
        public void Document_Create()
        {
            doc = new PdfDocument();
        }

        /// <summary>
        /// pdf 문서를 저장한다.
        /// </summary>
        /// <param name="save_Path"></param>
        public void Document_Save(string save_Path)
        {
            doc.Save(save_Path);
        }

        /// <summary>
        /// pdf에 image를 추가한다.
        /// </summary>
        /// <param name="image_Path"></param>
        public void Image_Add(string image_Path)
        {
            FileInfo fi = new FileInfo(image_Path);

            if(!fi.Exists) throw new Exception($"{image_Path} 파일을 찾을 수 없습니다.");

            var c = (from ex in pdf.ImageExtention
                    where ex == fi.Extension.ToLower()
                    select ex).Count();

            if (c < 1) return;

            PdfPage page = doc.AddPage();
            using (XImage img = XImage.FromFile(image_Path))
            {
                // Change PDF Page size to match image
                page.Width = img.PixelWidth;
                page.Height = img.PixelHeight;

                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(img, 0, 0, page.Width, page.Height);
            }
        }


        /// <summary>
        /// pdf에 폴더에 있는 image들을 추가한다.(Task 비동기 작업용)
        /// </summary>
        /// <param name="o"></param>        
        public void FolderImages_Add(object o)
        {
            pdfPram p = o as pdfPram;

            FolderImages_Add(p.FolderPath, p.SortName, p.save_Path, p.delProgress, p.MaxImgCount);
        }


        /// <summary>
        /// pdf에 폴더에 있는 image들을 추가한다.
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <param name="SortName">파일명(경로포함) 정렬 여부</param>
        /// <param name="save_Path">저장할 파일명(작업 종료 후 자동 저장 된다)</param>
        /// <param name="delProgress">진행 상태 델리게이트 - 리턴값 double[] : 진행수 / 전체수</param>
        /// <param name="MaxImgCount">최대 이미지 수량 0: 분리하지않음 </param>
        public void FolderImages_Add(string FolderPath, bool SortName, string save_Path, delObject delProgress, int MaxImgCount = 0)
        {
            DirectoryInfo di = new DirectoryInfo(FolderPath);

            if (!di.Exists) throw new Exception($"{FolderPath} 폴더을 찾을 수 없습니다.");

            string[] files = Directory.GetFiles(di.FullName);

            Images_Add(files, SortName, save_Path, delProgress, MaxImgCount);
        }


        /// <summary>
        /// pdf에 image들을 추가한다.
        /// </summary>
        /// <param name="images_path"></param>
        /// <param name="SortName">파일명(경로포함) 정렬 여부</param>
        /// <param name="save_Path">저장할 파일명(작업 종료 후 자동 저장 된다)</param>
        /// <param name="delProgress">진행 상태 델리게이트 - 리턴값 double[] : 진행수 / 전체수</param>
        /// <param name="MaxImgCount">최대 이미지 수량 0: 분리하지않음 </param>
        public void Images_Add(string[] images_path, bool SortName, string save_Path,delObject delProgress, int MaxImgCount = 0)
        {
            Document_Create();

            isWorkCancel = false;

            Double[] dbl = new double[2];

            dbl[0] = 0;
            dbl[1] = 0;
            int cnt = 0;
            int SaveCnt = 0;

            IEnumerable<string> pat;

            if (SortName)
            {
                
                pat = from pp in images_path
                            orderby pp
                            select pp;

               
            }
            else
            {
                pat = from pp in images_path                            
                      select pp;
            }


            dbl[1] = pat.LongCount();
            foreach (string s in pat)
            {
                
                dbl[0]++;
                Image_Add(s);

                if (delProgress != null) delProgress.Invoke(dbl);
                
                if(isWorkCancel)
                {
                    dbl[0] = -1;
                    if (delProgress != null) delProgress.Invoke(dbl);
                    break;
                }

                cnt++;
                //이미지 최대수가 되면 파일 저장
                if(MaxImgCount > 0 && cnt >= MaxImgCount)
                {
                    cnt = 0;
                    SaveDoc(save_Path, SaveCnt);
                    SaveCnt++;
                }

                //for test
                //Thread.Sleep(500);
            }

            //이미지 최대수가 되면 파일 저장
            if (cnt > 0)
            {
                cnt = 0;
                SaveDoc(save_Path, SaveCnt);
                SaveCnt++;
            }

        }

        private void SaveDoc(string save_Path, int SaveCnt = 0)
        {
            string file;

            //if (SaveCnt < 1)
            //    file = save_Path;
            //else
            {
                FileInfo f = new FileInfo(save_Path);
                string[] fff = f.Name.Split(new char[] { '.' });
                fff[0] = $"{fff[0]}({SaveCnt})";

                file = $"{f.DirectoryName}\\{fff[0]}.{fff[1]}";
            }

            FileInfo ff = new FileInfo(file);
            if (ff.Exists) ff.Delete();


            Document_Save(ff.FullName);

            Document_Create();
        }

    }   //end class
}
