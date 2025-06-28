namespace Yordanew.Domain.ValueObjects;

public record Translatable {
    public string Content { get; init; }
    public string? Translation { get; init; }
    public string? Transcription { get; init; }
    
    public Translatable(string content, string? translation = null, string? transcription = null) {
        Content = content;
        Translation = translation;
        Transcription = transcription;
    }
};