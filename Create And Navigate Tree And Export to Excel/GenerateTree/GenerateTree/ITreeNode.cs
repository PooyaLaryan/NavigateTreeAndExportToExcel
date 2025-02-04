using GenerateTree.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTree
{
    public interface ITreeNode
    {
        int Id { get; set; }
        List<ITreeNode> Child { get; set; }
    }
}
