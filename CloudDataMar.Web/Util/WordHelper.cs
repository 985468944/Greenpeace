using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
namespace CloudDataMar.Web.Util
{
    public class WordHelper
    {
        public WordHelper()
        { 
        
        }
        /// <summary>
        /// Word应用对象  
        /// </summary>
        private Microsoft.Office.Interop.Word.Application _wordApplication;
        /// <summary>
        /// word 文件对象 
        /// </summary>
        private Microsoft.Office.Interop.Word.Document _wordDocument;
        /// <summary>
        ///  创建文档  如果报错：类型“Microsoft.Office.Interop.Word.ApplicationClass”未定义构造函数 ； 解决方法：在其中点开“引用”文件夹，在"Microsoft.Office.Interop.Word" 上点击鼠标右键，选择“属性”，将属性中的“嵌入互操作类型”的值改为“false”即可
        /// </summary>
        public void CreateAWord()
        {
            //实例化word应用对象 
            this._wordApplication = new Microsoft.Office.Interop.Word.ApplicationClass();
            Object myNothing = System.Reflection.Missing.Value;
            this._wordDocument = this._wordApplication.Documents.Add(ref myNothing, ref myNothing, ref myNothing, ref myNothing);
        }
        /// <summary>
        /// 添加页眉 
        /// </summary>
        /// <param name="pPageHeader"></param>
        public void SetPageHeader(string pPageHeader)
        {
            //添加页眉 
            this._wordApplication.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdOutlineView;
            this._wordApplication.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekPrimaryHeader;
            this._wordApplication.ActiveWindow.ActivePane.Selection.InsertAfter(pPageHeader);
            //设置中间对齐 
            this._wordApplication.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //跳出页眉设置 
            this._wordApplication.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;
        }
        /// <summary>
        /// 插入文字
        /// </summary>
        /// <param name="pText">文本信息</param>
        /// <param name="pFontSize">字体大小</param>
        /// <param name="pFontColor">字体颜色</param>
        /// <param name="pFontBold">字体粗体</param>
        /// <param name="ptextAlignment">方向</param>
        public void InsertText(string pText, int pFontSize, Microsoft.Office.Interop.Word.WdColor pFontColor, int pFontBold, Microsoft.Office.Interop.Word.WdParagraphAlignment ptextAlignment)
        {
            //设置字体样式以及方向 
            this._wordApplication.Application.Selection.Font.Size = pFontSize;
            this._wordApplication.Application.Selection.Font.Bold = pFontBold;
            this._wordApplication.Application.Selection.Font.Color = pFontColor;
            this._wordApplication.Application.Selection.ParagraphFormat.Alignment = ptextAlignment;
            this._wordApplication.Application.Selection.TypeText(pText);
        }
        /// <summary>
        /// 换行
        /// </summary>
        public void NewLine()
        {
            //换行 
            this._wordApplication.Application.Selection.TypeParagraph();
        }
        /// <summary>
        /// 插入一个图片 
        /// </summary>
        /// <param name="pPictureFileName"></param>
        public void InsertPicture(string pPictureFileName)
        {
            object myNothing = System.Reflection.Missing.Value;
            //图片居中显示 
            this._wordApplication.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            this._wordApplication.Application.Selection.InlineShapes.AddPicture(pPictureFileName, ref myNothing, ref myNothing, ref myNothing);
        }
        /// <summary>
        /// 插入表格
        /// </summary>
        public void InsertTable()
        {
            object myNothing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Table table1 = _wordDocument.Tables.Add(_wordApplication.Selection.Range, 4, 3, ref myNothing, ref myNothing);
            _wordDocument.Tables[1].Cell(1, 1).Range.Text = "产品\n项目";
            _wordDocument.Tables[1].Cell(1, 2).Range.Text = "电脑";
            _wordDocument.Tables[1].Cell(1, 3).Range.Text = "手机";
            _wordDocument.Tables[1].Cell(2, 1).Range.Text = "重量(kg)";
            _wordDocument.Tables[1].Cell(3, 1).Range.Text = "价格(元)";
            _wordDocument.Tables[1].Cell(4, 1).Range.Text = "共同信息";
            _wordDocument.Tables[1].Cell(4, 2).Range.Text = "信息A";
            _wordDocument.Tables[1].Cell(4, 3).Range.Text = "信息B";
            table1.Select();
            table1.Rows.Alignment = Microsoft.Office.Interop.Word.WdRowAlignment.wdAlignRowCenter;//整个表格居中

            _wordApplication.Selection.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            _wordApplication.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            _wordApplication.Selection.Cells.HeightRule = Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightExactly;
            _wordApplication.Selection.Cells.Height = 40;
            table1.Rows[2].Height = 20;
            table1.Rows[3].Height = 20;
            table1.Rows[4].Height = 20;
            table1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            _wordApplication.Selection.Cells.Width = 150;
            table1.Columns[1].Width = 75;
            table1.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            table1.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;


            _wordApplication.Selection.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            _wordApplication.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            _wordApplication.Selection.Cells.HeightRule = Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightExactly;
            _wordApplication.Selection.Cells.Height = 40;
            table1.Rows[2].Height = 20; table1.Rows[3].Height = 20;
            table1.Rows[4].Height = 20;
            table1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            _wordApplication.Selection.Cells.Width = 150;
            table1.Columns[1].Width = 75;
            table1.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            table1.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

            //表头斜线 
            table1.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Visible = true;
            table1.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
            table1.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;


            //表格边框			 
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal].Visible = true;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;

            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical].Visible = true;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;

            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft].Visible = true;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;

            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDoubleWavy;

            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight].Visible = true;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDoubleWavy;

            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].Visible = true;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;

            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop].Visible = true;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;
            table1.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;


            //合并单元格 
            //_wordDocument.Tables[1].Cell(4, 2).Merge(table1.Cell(4, 3)); 
        }
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, String lpszOp, String lpszFile, String lpszParams, String lpszDir, int FsShowCmd);
        /// <summary>
        /// 关闭文档 
        /// </summary>
        public void CloseDocument(string fileName)
        {
            object myNothing = System.Reflection.Missing.Value;
            //关闭文档 
            object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges; _wordDocument.Close(ref myNothing, ref myNothing, ref myNothing);
            _wordApplication.Application.Quit(ref saveOption, ref myNothing, ref myNothing); _wordDocument = null; _wordApplication = null;
            //ShellExecute(IntPtr.Zero, "open", fileName, "", "", 3);
        }
        /// <summary>
        /// 保存文件 
        /// </summary>
        /// <param name="pFileName">文件名</param>
        public void SaveWord(string pFileName)
        {
            object myNothing = System.Reflection.Missing.Value;
            object myFileName = pFileName;
            object myWordFormatDocument = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
            object myLockd = false;
            object myPassword = "";
            object myAddto = true;
            try
            {
                this._wordDocument.SaveAs(ref myFileName, ref myWordFormatDocument, ref myLockd, ref myPassword,
                    ref myAddto, ref myPassword,
                    ref myLockd, ref myLockd, ref myLockd, ref myLockd, ref myNothing, ref myNothing, ref myNothing,
                    ref myNothing, ref myNothing, ref myNothing);
            }
            catch (Exception exception)
            {
                throw new Exception("保存word文档失败!");
            }
        }
    }
}
