using System.Collections.Generic;

public interface IProvider<T>
{
    public List<T> Provide();
}