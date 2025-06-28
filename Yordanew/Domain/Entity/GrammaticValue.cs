using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class GrammaticValue(GrammaticCategory grammaticCategory, Transcriptable name) {
    public Transcriptable Name { get; set; } = name;
    public RichText Description { get; set; } = new RichText("");
}