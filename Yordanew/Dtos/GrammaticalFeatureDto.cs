using Yordanew.Domain.Entity;

namespace Yordanew.Dtos;

public class GrammaticalFeatureDto {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public required string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public static class FeatureDtoExtensions {
    public static GrammaticalFeatureDto ToDto(this GrammaticalFeature domain) {
        return new GrammaticalFeatureDto {
            Id = domain.Id,
            Name = domain.Name.Value,
            Code = domain.Code.Value,
            Description = domain.Description.Content,
        };
    }
}