using System.Collections.Generic;

namespace UnityEngine
{
    public class GameObject
    {
        public Transform transform = new();
        private List<object> comps = new List<object>();

        public T AddComponent<T>() where T : new()
        {
            T t = new();
            comps.Add(t);
            return t;
        }
    }

    public class Transform
    {
        public Vector3 position;
    }

    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public override string ToString() => $"({x}, {y}, {z})";
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs) => new(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        public static Vector3 one => new(1f, 1f, 1f);
    }

    public class ExampleComponent
    {
        public int Value;
    }
}
