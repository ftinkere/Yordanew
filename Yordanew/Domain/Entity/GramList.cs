namespace Yordanew.Domain.Entity;

public class GramList {
    public required PartOfSpeech PartOfSpeech { get; set; }
    public IEnumerable<Tuple<GrammaticCategory, GrammaticValue>> Grammatics { get; set; } = new List<Tuple<GrammaticCategory, GrammaticValue>>();
    
}