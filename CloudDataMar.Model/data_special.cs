
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------


namespace CloudDataMar.Model
{

using System;
    using System.Collections.Generic;
    
public partial class data_special
{

    public int ID { get; set; }

    public string Data_Special_Name { get; set; }

    public string Data_Special_Url { get; set; }

    public string Data_Special_Desc { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }

    public Nullable<System.DateTime> UpdateDateTime { get; set; }

    public string Data_Image { get; set; }

    public Nullable<int> DownloadCount { get; set; }

    public Nullable<int> PageViewCount { get; set; }

    public Nullable<int> Data_IsPublish { get; set; }

}

}