using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

















namespace AntiplagiatLib
{
    /// <summary>
    /// Бор, в котором находятся и возвращаются строки. (Бор - корневое дерево, на каждом ребре которого лежит символ, причём
    /// из любой вершины не может выходить два ребра, имеющих один и тот же символ.
    /// Вершины бора называются терминальными, если на них заканчивается какое-то слово)
    /// </summary>
    public class Trie : Trie<string>
    {
        /// <summary>
        /// Добавить строку в бор
        /// </summary>
        /// <param name="s">Строка, которая добавляется.</param>
        public void Add(string s)
        {
            Add(s, s);
        }

        /// <summary>
        /// Добавить несколько строк.
        /// </summary>
        /// <param name="strings">Строки, которые добавляются.</param>
        public void Add(IEnumerable<string> strings)
        {
            foreach (string s in strings)
            {
                Add(s);
            }
        }
    }

    /// <summary>
    /// Бор, в котором находятся строки в тексте и возвращает .
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class Trie<TValue> : Trie<char, TValue>
    {
    }

    /// <summary>
    /// бор, который найдет строки или фразы и вернет значение типа <typeparamref name="T"/>
    /// для каждой найденной строки или фразы.
    /// </summary>
    /// <remarks>
    /// <typeparamref name="T"/> обычно будет символом для нахождения строк или строкой для нахождения фраз или целых слов.
    /// </remarks>
    /// <typeparam name="T">Тип буквы в слове.</typeparam>
    /// <typeparam name="TValue">Тип Value, который возвращается, когда слово найдено.</typeparam>
    public class Trie<T, TValue>
    {
        /// <summary>
        /// Корень бора. У корня нет Value и Parent.
        /// </summary>
        private readonly Node<T, TValue> root = new Node<T, TValue>();

        /// <summary>
        /// Добавить слово в дерево.
        /// </summary>
        /// <remarks>
        /// Слово состоит из букв. Для каждой буквы строится вершина.
        /// Если тип буквы - char, то слово, состоящее из букв - string.
        /// Но буква также может быть string-ом, тогда для каждого слова будет добавлена вершина и слово будет фразой.
        /// </remarks>
        /// <param name="word">Слово, которое ищем.</param>
        /// <param name="value">Value, которое возвращается, когда слово найдено.</param>
        public void Add(IEnumerable<T> word, TValue value)
        {
            // Начинаем с корня
            var node = root;

            // строим ветку для слова, по одной букве за раз
            // если вершины для буквы не существует, добавляем ее
            foreach (T c in word)
            {
                var child = node[c];

                if (child == null)
                    child = node[c] = new Node<T, TValue>(c, node);

                node = child;
            }

            // помечаем конец ветки, добавляя value, возвращаемое,
            // когда это слово будет найдено в тексте
            node.Values.Add(value);
        }


        /// <summary>
        /// Создаем fail или fall links.
        /// </summary>
        public void Build()
        {
            // проходим по бору с помощью BFS (поиск в ширину)
            var queue = new Queue<Node<T, TValue>>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                // перебираем дочерние вершины
                foreach (var child in node)
                    queue.Enqueue(child);

                // fail link корня - корень
                if (node == root)
                {
                    root.Fail = root;
                    continue;
                }

                var fail = node.Parent.Fail;

                while (fail[node.Word] == null && fail != root)
                    fail = fail.Fail;

                node.Fail = fail[node.Word] ?? root;
                if (node.Fail == node)
                    node.Fail = root;
            }
        }

        /// <summary>
        /// Добавить все слова в text.
        /// </summary>
        /// <param name="text">Текст, в котором ищем.</param>
        /// <returns>Values, которые были добавлены для найденных слов.</returns>
        public IEnumerable<TValue> Find(IEnumerable<T> text)
        {
            var node = root;

            foreach (T c in text)
            {
                while (node[c] == null && node != root)
                    node = node.Fail;

                node = node[c] ?? root;

                for (var t = node; t != root; t = t.Fail)
                {
                    foreach (TValue value in t.Values)
                        yield return value;
                }
            }
        }

        /// <summary>
        /// Вершина в бору.
        /// </summary>
        /// <typeparam name="TNode">Тот же тип, что и parent.</typeparam>
        /// <typeparam name="TNodeValue">Тот же тип, что у value parent-а.</typeparam>
        private class Node<TNode, TNodeValue> : IEnumerable<Node<TNode, TNodeValue>>
        {
            private readonly TNode word;
            private readonly Node<TNode, TNodeValue> parent;
            private readonly Dictionary<TNode, Node<TNode, TNodeValue>> children = new Dictionary<TNode, Node<TNode, TNodeValue>>();
            private readonly List<TNodeValue> values = new List<TNodeValue>();

            /// <summary>
            /// Конструктор для корневой вершины.
            /// </summary>
            public Node()
            {
            }

            /// <summary>
            /// Конструктор для вершины со словом.
            /// </summary>
            /// <param name="word"></param>
            /// <param name="parent"></param>
            public Node(TNode word, Node<TNode, TNodeValue> parent)
            {
                this.word = word;
                this.parent = parent;
            }

            /// <summary>
            /// Слово (или буква) в этой вершине.
            /// </summary>
            public TNode Word
            {
                get { return word; }
            }

            /// <summary>
            /// Вершина родителя.
            /// </summary>
            public Node<TNode, TNodeValue> Parent
            {
                get { return parent; }
            }

            /// <summary>
            /// Fail или fall вершина.
            /// </summary>
            public Node<TNode, TNodeValue> Fail
            {
                get;
                set;
            }

            /// <summary>
            /// Дочерние для этой вершины.
            /// </summary>
            /// <param name="c">Дочернее слово.</param>
            /// <returns>Дочерняя вершина.</returns>
            public Node<TNode, TNodeValue> this[TNode c]
            {
                get { return children.ContainsKey(c) ? children[c] : null; }
                set { children[c] = value; }
            }

            /// <summary>
            /// Values для слов, которые заканчиватся в этой вершине.
            /// </summary>
            public List<TNodeValue> Values
            {
                get { return values; }
            }

            /// <inherit/>
            public IEnumerator<Node<TNode, TNodeValue>> GetEnumerator()
            {
                return children.Values.GetEnumerator();
            }

            /// <inherit/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <inherit/>
            public override string ToString()
            {
                return Word.ToString();
            }
        }
    }
}
