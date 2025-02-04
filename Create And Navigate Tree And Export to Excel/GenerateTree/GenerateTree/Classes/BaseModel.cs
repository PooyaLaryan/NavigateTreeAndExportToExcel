namespace GenerateTree.Classes
{
    public class BaseModel : IBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
