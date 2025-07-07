using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

[Table("GrammaticalFeatures")]
public class GrammaticalFeatureDbo {
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    
    public virtual GrammaticalCategoryDbo Category { get; set; }
}