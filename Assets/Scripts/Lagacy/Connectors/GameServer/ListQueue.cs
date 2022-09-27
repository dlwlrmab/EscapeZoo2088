using System.Collections.Generic;

public sealed class ListQueue<T> : List<T>
{
    public new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
    }

    public void Enqueue(T item)
    {
        base.Add(item);
    }

    public T Dequeue()
    {
        var t = base[0];
        base.RemoveAt(0);
        return t;
    }

    public T Peek()
    {
        return base[0];
    }
}