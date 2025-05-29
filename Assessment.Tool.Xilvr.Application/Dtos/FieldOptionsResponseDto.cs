namespace Assessment.Tool.Xilvr.Application.Dtos;

/// <summary>
/// Defines FieldOptionsResponseDto
/// </summary>
public class FieldOptionsResponseDto
{
    /// <summary>
    /// Specifies the total documents
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// Specifies the field options
    /// </summary>
    public List<FieldOptionsDto> FieldOptions { get; set; } = default!;
}
