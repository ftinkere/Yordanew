namespace Yordanew.Domain.Entity;

public class GramList {
    public required PartOfSpeech PartOfSpeech { get; set; }
    public IEnumerable<Tuple<GrammaticalCategory, GrammaticalFeature>> Grammatics { get; set; } = new List<Tuple<GrammaticalCategory, GrammaticalFeature>>();
}