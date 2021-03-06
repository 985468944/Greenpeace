﻿using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using System.Data;

namespace CloudDataMar.Web
{
   public class AsposeExcel
{
    public AsposeExcel() //导出构造数
    {
      
    }
   
    public static void OutFileToDisk(DataSet dsTab,string title, string xlsPath)
    {
        Workbook workbook = new Workbook(); //工作簿
        #region 遍历表数据
        for (int t = 0; t < dsTab.Tables.Count; t++)
        {
            Worksheet sheet = null;
            DataTable dt = dsTab.Tables[t];
            sheet = workbook.Worksheets.Add(dsTab.Tables[t].TableName);
            Cells cells = sheet.Cells;//单元格
            //为标题设置样式    
            Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            styleTitle.Font.Name = "宋体";//文字字体
            styleTitle.Font.Size = 18;//文字大小
            styleTitle.Font.IsBold = true;//粗体

            //列样式
            Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
            style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style2.Font.Name = "宋体";//文字字体
            style2.Font.Size = 14;//文字大小
            style2.Font.IsBold = true;//粗体
            style2.IsTextWrapped = true;//单元格内容自动换行
            style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
         //   ------内容区-----
            Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
            style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style3.Font.Name = "宋体";//文字字体
            style3.Font.Size = 12;//文字大小
            style3.IsTextWrapped = true;//单元格内容自动换行
            style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            int Colnum = dt.Columns.Count;//表格列数
            int Rownum = dt.Rows.Count;//表格行数
            //生成行1 标题行   
            cells.Merge(0, 0, 1, Colnum);//合并单元格
            cells[0, 0].PutValue(title);//填写内容
            cells[0, 0].SetStyle(styleTitle);
            cells.SetRowHeight(0, 38);

            //生成行2 列名行
            for (int i = 0; i < Colnum; i++)
            {
                cells[1, i].PutValue(dt.Columns[i].ColumnName);
                cells[1, i].SetStyle(style2);
                cells.SetRowHeight(1, 25);
            }
            //生成数据行
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum; k++)
                {
                    cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                    cells[2 + i, k].SetStyle(style3);
                    cells.SetColumnWidth(k, 30);
                }
                cells.SetRowHeight(2 + i, 22);

            }
            workbook.Save(xlsPath);
        }
        #endregion
        workbook.Worksheets.RemoveAt(0);
        workbook.Save(xlsPath);
    }
 
} 
}