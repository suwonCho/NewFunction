using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace function.wpf
{
    /// <summary>
    /// 폼을 닫을 경우 처리 하는 방법
    /// </summary>
    public enum enCloseWindowToType
    {
        /// <summary>
        /// 걍 닫음
        /// </summary>
        None,
        /// <summary>
        /// 예, 아니요 선택
        /// </summary>
        PromptYN,
        /// <summary>
        /// 트레이 아이콘으로
        /// </summary>
        TrayIcon,
        /// <summary>
        /// 예, 아니요, 트레이 아이콘 선택
        /// </summary>
        Prompt

    }

    public enum enMsgType
    {
        /// <summary>
        /// 타입 없음, 하얀 동그라미
        /// </summary>
        None,
        /// <summary>
        /// 타입 정상, 녹색 동그라미
        /// </summary>
        OK,
        /// <summary>
        /// 타입 에러, 빨강 동그라미
        /// </summary>
        Error
    }

    public enum enTextType { All, NumberOlny, EngOnly, None };

    public enum enElementType { Label, Progress }


    public enum enStrachtType
    {
        None,
        Left,
        Right,
        Top,
        Bottom,
        LeftTop,
        RigthTop,
        LeftBottom,
        RightBottom
    }

    public enum enWindowStateMaximizedMode
    {
        /// <summary>
        /// 보통 모드
        /// </summary>
        Normal,
        /// <summary>
        /// 테스크바 위에 표시(테스크바 안보임)
        /// </summary>
        OverTaskbar,
        /// <summary>
        /// 전체화면 허용 안함
        /// </summary>
        NoAllow
    }

}   //end namespace
