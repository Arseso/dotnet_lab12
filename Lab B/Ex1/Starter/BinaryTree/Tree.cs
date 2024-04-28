using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class Tree<TItem> : IBinaryTree<TItem> where TItem : IComparable<TItem>
    {
        public TItem NodeData { get; set; }
        public Tree<TItem> LeftTree { get; set; }
        public Tree<TItem> RightTree { get; set; }

        public Tree(TItem nodeValue)
        {
            NodeData = nodeValue;
            LeftTree = null;
            RightTree = null;
        }

        public void Add(TItem newItem)
        {
            TItem currentNodeValue = NodeData;
            // Check if the item should be inserted in the left tree.
            if (currentNodeValue.CompareTo(newItem) > 0)
            {
                // Is the left tree null?
                if (LeftTree == null)
                {
                    LeftTree = new Tree<TItem>(newItem);
                }
                else // Call the Add method recursively.
                {
                    LeftTree.Add(newItem);
                }
            }
            else // Insert in the right tree.
            {
                // Is the right tree null?
                if (RightTree == null)
                {
                    RightTree = new Tree<TItem>(newItem);
                }
                else // Call the Add method recursively.
                {
                    RightTree.Add(newItem);
                }
            }
        }

        public void WalkTree()
        {
            // Recursive descent of the left tree.
            if (LeftTree != null)
            {
                LeftTree.WalkTree();
            }
            Console.WriteLine(NodeData.ToString());
            // Recursive descent of the right tree.
            if (RightTree != null)
            {
                RightTree.WalkTree();
            }
        }
        public void Remove(TItem itemToRemove)
        {
            // Cannot remove null.
            if (itemToRemove == null)
            {
                return;
            }
            // Check if the item could be in the left tree.
            if (NodeData.CompareTo(itemToRemove) > 0 && LeftTree != null)
            {
                // Check the left tree.
                // Check 2 levels down the tree - cannot remove
                // 'this', only the LeftTree or RightTree properties.
                if (LeftTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    // The LeftTree property has no children - set the
                    // LeftTree property to null.
                    if (LeftTree.LeftTree == null && LeftTree.RightTree == null)
                    {
                        LeftTree = null;
                    }
                    else // Remove LeftTree.
                    {
                        RemoveNodeWithChildren(LeftTree);
                    }
                }
                else
                {
                    // Keep looking - call the Remove method recursively.
                    LeftTree.Remove(itemToRemove);
                }
            }
            // Check if the item could be in the right tree.?
            if (NodeData.CompareTo(itemToRemove) < 0 && RightTree != null)
            {
                // Check the right tree.
                // Check 2 levels down the tree - cannot remove
                // 'this', only the LeftTree or RightTree properties.
                if (RightTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    // The RightTree property has no children – set the
                    // RightTree property to null.
                    if (RightTree.LeftTree == null && RightTree.RightTree == null)
                    {
                        RightTree = null;
                    }
                    else // Remove the RightTree.
                    {
                        RemoveNodeWithChildren(RightTree);
                    }
                }
                else
                {
                    // Keep looking - call the Remove method recursively.
                    RightTree.Remove(itemToRemove);
                }
            }
            // This will only apply at the root node.
            if (NodeData.CompareTo(itemToRemove) == 0)
            {
                // No children - do nothing, a tree must have at least
                // one node.
                if (LeftTree == null && RightTree == null)
                {
                    return;
                }
                else // The root node has children.
                {
                    RemoveNodeWithChildren(this);
                }
            }
        }
        private void RemoveNodeWithChildren(Tree<TItem> node)
        {
            // Check whether the node has children.
            if (node.LeftTree == null && node.RightTree == null)
            {
                throw new ArgumentException("Node has no children");
            }
            // The tree node has only one child - replace the
            // tree node with its child node.
            if (node.LeftTree == null ^ node.RightTree == null)
            {
                if (node.LeftTree == null)
                {
                    node.CopyNodeToThis(node.RightTree);
                }
                else
                {
                    node.CopyNodeToThis(node.LeftTree);
                }
            }
            else
            // The tree node has two children - replace the tree node's value
            // with its "in order successor" node value and then remove the
            // in order successor node.
            {
                // Find the in order successor – the leftmost descendant of
                // its RightTree node.
                Tree<TItem> successor = GetLeftMostDescendant(node.RightTree);
                // Copy the node value from the in order successor.
                node.NodeData = successor.NodeData;
                // Remove the in order successor node.
                if (node.RightTree.RightTree == null &&
                node.RightTree.LeftTree == null)
                {
                    node.RightTree = null; // The successor node had no
                                           // children.
                }
                else
                {
                    node.RightTree.Remove(successor.NodeData);
                }
            }
        }
        private void CopyNodeToThis(Tree<TItem> node)
        {
            NodeData = node.NodeData;
            LeftTree = node.LeftTree;
            RightTree = node.RightTree;
        }
        private Tree<TItem> GetLeftMostDescendant(Tree<TItem> node)
        {
            while (node.LeftTree != null)
            {
                node = node.LeftTree;
            }
            return node;
        }
    }
}
