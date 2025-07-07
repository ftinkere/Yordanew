using Yordanew.Domain.Entity;

namespace Yordanew.Dtos;

public class PartOfSpeechDto {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public required string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public IList<GrammaticalCategoryDto> Categories { get; set; } = new List<GrammaticalCategoryDto>();
}

public static class PosDtoExtensions {
    public static PartOfSpeechDto ToDto(this PartOfSpeech domain, IDictionary<GrammaticalCategory, IList<GrammaticalFeature>>? categories = null) {
        return new PartOfSpeechDto {
            Id = domain.Id,
            Name = domain.Name.Value,
            Code = domain.Code.Value,
            Description = domain.Description.Content,
            Categories = categories?.Select(kvp => kvp.Key.ToDto(kvp.Value)).ToList() ?? [],
        };
    }
}