using Fingers10.ExcelExport.Attributes;

namespace CreateTree.Models
{
    public class TreeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [NestedIncludeInReport]
        public List<TreeModel> Child { get; set; } = new List<TreeModel>();
    }
}
