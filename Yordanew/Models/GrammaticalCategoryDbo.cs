using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

[Table("GrammaticalCategories")]
public class GrammaticalCategoryDbo {
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(PartOfSpeech))]
    public Guid PartOfSpeechId { get; set; }
    
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    
    public virtual PartOfSpeechDbo PartOfSpeech { get; set; }
    public virtual ICollection<GrammaticalFeatureDbo> Features { get; set; } = [];
}