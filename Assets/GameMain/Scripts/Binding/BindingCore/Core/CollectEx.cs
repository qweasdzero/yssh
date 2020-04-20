namespace StarForce
{
    public static class CollectEx
    {
        public static T GetOrAddItem<T>(this Collection<T> collection, int index) where T : ItemContext,new()
        {
            T item;
            if (index < 0 || index >= collection.Count )
            {
                item = new T();
                collection.Insert(index, item);
                return item;
            }
            
            return collection.GetItem(index);
        }
    }
}