using UnityEngine;

public interface IInitializable
{
    public bool IsInit { get; }

    void Initialize();
}
