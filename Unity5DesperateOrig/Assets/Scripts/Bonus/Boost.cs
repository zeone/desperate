using UnityEngine;

public class Boost : Bonus
{
    private static int _boost = 1;
    public static int boost
    {
        get { return _boost; }
        set { _boost = value; }
    }
    void Start()
    {
        boost = 2;
    }
    void Update()
    {
        TimeOff();
        if (_willDestroed) boost = 1;
    }
}
