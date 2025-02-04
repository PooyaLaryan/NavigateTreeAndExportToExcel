using GenerateTree.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTree
{
    internal class TreeGenerator
    {
        public void navigation()
        {
            var rawList = GenerateData.Generate();
            TreeModel tree = new TreeModel() { Id = 0, Name = "Category" };
            GenerateTree(tree, rawList, (x) => new TreeModel { Name = x.Name, Id = x.Id }
                );
            string json = JsonConvert.SerializeObject(tree);
            NavigateTree(tree, 0);
        }

        private void GenerateTree(TreeModel tree, List<BaseModel> rawTreeInfo)
        {
            tree.Child.AddRange(rawTreeInfo.Where(x => x.ParentCategoryId == tree.Id).Select(x => new TreeModel { Name = x.Name, Id = x.Id }).ToList());
            foreach (TreeModel treeModel in tree.Child)
            {
                if (rawTreeInfo.Any(x => x.ParentCategoryId == treeModel.Id))
                {
                    GenerateTree(treeModel, rawTreeInfo);
                }
            }
        }

        private void GenerateTree<T, U>(T tree, IList<U> rawTreeInfo, Func<U, T> newTreeNode) where T : ITreeNode where U : IBaseModel
        {
            var child = rawTreeInfo.Where(x => x.ParentCategoryId == tree.Id)
                           .Select(x => newTreeNode(x))
                           .ToList();

            tree.Child.AddRange(child.Cast<ITreeNode>());

            foreach (T treeModel in tree.Child)
            {
                if (rawTreeInfo.Any(x => x.ParentCategoryId == treeModel.Id))
                {
                    GenerateTree(treeModel, rawTreeInfo, newTreeNode);
                }
            }
        }

        public void NavigateTree(TreeModel tree, int level)
        {
            for (int i = 0; i < level; i++)
            {
                Console.Write("------");
            }
            Console.WriteLine(level + " " + tree.Name);
            foreach (TreeModel node in tree.Child)
            {
                NavigateTree(node, level + 1);
            }
        }
    }
}
