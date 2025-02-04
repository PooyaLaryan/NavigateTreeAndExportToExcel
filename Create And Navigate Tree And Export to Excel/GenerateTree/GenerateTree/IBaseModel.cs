using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTree
{
    public interface IBaseModel
    {
        int Id { get; set; }
        int ParentCategoryId { get; set; }
    }
}
