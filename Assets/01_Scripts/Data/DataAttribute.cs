using System;

/// <summary>
/// 어떤 구글 시트를 파싱할지 명시
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class SheetTargetAttribute : Attribute
{
    public string SheetName { get; }
    public SheetTargetAttribute(string sheetName) => SheetName = sheetName;
}

/// <summary>
/// 어떤 colum과 매칭할지 명시
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class SheetColumnAttribute : Attribute
{
    public string ColumnName { get; }
    public SheetColumnAttribute(string columnName) => ColumnName = columnName;
}

