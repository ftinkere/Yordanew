using System.ComponentModel.DataAnnotations.Schema;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace Yordanew.Models;

[Table("PartsOfSpeech")]
public class PartOfSpeechDbo {
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Language))]
    public Guid? LanguageId { get; set; }
    
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    
    public virtual LanguageDbo? Language { get; set; }
    public virtual ICollection<GrammaticalCategoryDbo> Categories { get; set; } = [];
}