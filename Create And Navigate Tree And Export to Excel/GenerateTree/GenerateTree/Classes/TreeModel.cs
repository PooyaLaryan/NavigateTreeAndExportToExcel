namespace GenerateTree.Classes
{
    public class TreeModel : ITreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ITreeNode> Child { get; set; } = new List<ITreeNode>();
    }
}
