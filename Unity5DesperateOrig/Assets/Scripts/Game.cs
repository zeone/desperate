using UnityEngine;
using System.Collections;
public static class Game
{
   
}
    public static class Angles
    {
        private static Quaternion _zero = Quaternion.identity;
        public static Quaternion zero
        {
            get { return _zero; }
            set { _zero = value; }
        }
        public static float HorizontalAngle(Vector3 direction)
        {
            return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
        public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
        {
            dirA = dirA - Vector3.Project(dirA, axis);
            dirB = dirB - Vector3.Project(dirB, axis);
            float angle = Vector3.Angle(dirA, dirB);
            return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
        }
    }
public static class BonusInfo
{
    private static bool _frize;
    private static float time=1;
    private static Turet _turet;
    private static float _mulSpeed=1;
    public static bool frize
    {
        get{return _frize;}
        set{_frize=value;}
    }
    public static float mulSpeed
    {
        get { return _mulSpeed; }
        set { _mulSpeed = value; }
    }
    public static float slowTime
    {
        get { return time; }
        set { time = value; }
    }
    public static Turet turet
    {
        get { return _turet; }
        set { _turet = value; }
    }
}
