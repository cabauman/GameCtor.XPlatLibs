using System.Collections.Generic;

namespace GameCtor.Repository
{
    public struct RepoItemCollection<T>
        where T : IModel
    {
        public RepoItemCollection(int cursor, IEnumerable<T> items)
        {
            Cursor = cursor;
            Items = items;
        }

        public int Cursor { get; }

        public IEnumerable<T> Items { get; }
    }
}
