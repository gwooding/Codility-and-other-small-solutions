using System.Collections.Generic;
using System.Linq;

namespace WordCounter
{
    class Program
    {
        static Tree wordTree = new Tree();

        static void Main(string[] args)
        {
            string[] words = args[0].Split(new char[] {' ', '/', '\\', '_' });
            foreach (string word in words)
            {
                if (word != (""))
                {
                    AddToTree(word.ToLower());
                }
            }

            int i = FindWordCount("aaa");
            int j = FindWordCount("a");
            int k = FindWordCount("efgh");
            int l = FindWordCount("zzz");
        }

        private static int FindWordCount(string word)
        {

            Node currentNode = Tree.root;
            for (int i = 0; i < word.Length; i++)
            {
                if(currentNode.nextNodes.Contains(new Node(word[i], 0)))
                {
                    currentNode = currentNode.nextNodes.First(n => n.letter == word[i]);
                }
                else
                {
                    return -1;
                }
            }
            return currentNode.count;
        }

        private static void AddToTree(string word)
        {
            Node currentNode = Tree.root;
            bool validWord = false;

            for (int i = 0; i < word.Length; i++)
            {
                if (("\"',;:@.!?(){}[]£&+=%").Contains(word[i]))
                {
                    continue;
                }
                else
                {
                    if (!currentNode.nextNodes.Contains(new Node(word[i], 0))){
                        currentNode.nextNodes.Add(new Node(word[i], 0));
                    }
                    currentNode = currentNode.nextNodes.First(n =>n.letter == word[i]);
                        
                    validWord = true;
                }
            }

            if (validWord)
            {
                currentNode.count++;
            }
        }
    }

    internal class Tree
    {
        static internal Node root;
        static Tree()
        {
            root = new Node('+', 0);
        }
    }

    internal class Node
    {
        internal char letter { get; set; }
        internal int count { get; set; }
        internal SortedSet<Node> nextNodes { get; set; }

        internal Node(char letter, int count)
        {
            this.letter = letter;
            this.count = count;
            nextNodes = new SortedSet<Node>(new NodeComparer());
        }
    }

    internal class NodeComparer : IComparer<Node>
    {
        public int Compare(Node node1, Node node2)
        {
            return node1.letter.CompareTo(node2.letter);
        }
    }
}
