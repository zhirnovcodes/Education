public class IndexedQueue<T>
{
    private readonly T[] _array;
    private int _firstIndex = 0;
    private int _lastIndex = -1;

    public int Count { get; private set; }

    public IndexedQueue(int capacity)
    {
        _array = new T[capacity];
    }

    public void Clear()
    {
        _firstIndex = 0;
        _lastIndex = -1;
        Count = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true if overflown</returns>
    public bool Add(T value)
    {
        var result = false;

        if (Count + 1 == _array.Length)
        {
            _firstIndex = (_firstIndex + 1) % _array.Length;
            result = true;
        }
        else
        {
            Count++;
        }

        _lastIndex = (_lastIndex + 1) % _array.Length;

        _array[_lastIndex] = value;
        return result;
    }

    public T this [int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new System.IndexOutOfRangeException();
            }
            index = (_firstIndex + index) % _array.Length;
            return _array[index];
        }
    }
}
