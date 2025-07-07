using Yordanew.Domain.Entity;

namespace Yordanew.Dtos;

public class GrammaticalCategoryDto {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public required string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public IList<GrammaticalFeatureDto> Features { get; set; } = new List<GrammaticalFeatureDto>();
}

public static class CategoryDtoExtensions {
    public static GrammaticalCategoryDto ToDto(this GrammaticalCategory domain, IList<GrammaticalFeature>? features = null) {
        return new GrammaticalCategoryDto {
            Id = domain.Id,
            Name = domain.Name.Value,
            Code = domain.Code.Value,
            Description = domain.Description.Content,
            Features = features?.Select(f => f.ToDto()).ToList() ?? [],
        };
    }
}