using UnityEngine;
using System;

[Serializable]
public struct IntXY
{
    [SerializeField] public int x;
    [SerializeField] public int y;

    public IntXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Gets the value at an index.
    /// </summary>
    /// <param name="index">The index you are trying to get.</param>
    /// <returns>The value at that index.</returns>
    public int this[int index]
    {
        get
        {
            int result;
            if (index != 0)
            {
                if (index != 1)
                {
                    throw new IndexOutOfRangeException("Index " + index.ToString() + " is out of range.");
                }
                result = y;
            }
            else
            {
                result = x;
            }
            return result;
        }
        set
        {
            if (index != 0)
            {
                if (index != 1)
                {
                    throw new IndexOutOfRangeException("Index " + index.ToString() + " is out of range.");
                }
                y = value;
            }
            else
            {
                x = value;
            }
        }
    }


    public static IntXY zero
    {
        get
        {
            return new IntXY (0, 0);
        }
    }


    public static IntXY one
    {
        get
        {
            return new IntXY(1, 1);
        }
    }

    public static explicit operator Vector2(IntXY point)
    {
        return new Vector2((float)point.x, (float)point.y);
    }

    public static explicit operator IntXY(Vector2 vector2)
    {
        return new IntXY((int)vector2.x, (int)vector2.y);
    }

    public static IntXY operator +(IntXY lhs, IntXY rhs)
    {
        lhs.x += rhs.x;
        lhs.y += rhs.y;
        return lhs;
    }

    public static IntXY operator -(IntXY lhs, IntXY rhs)
    {
        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        return lhs;
    }

    public static IntXY operator *(IntXY lhs, IntXY rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        return lhs;
    }

    public static IntXY operator /(IntXY lhs, IntXY rhs)
    {
        lhs.x /= rhs.x;
        lhs.y /= rhs.y;
        return lhs;
    }

    public static bool operator ==(IntXY lhs, IntXY rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.x;
    }

    public static bool operator !=(IntXY lhs, IntXY rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.x;
    }

    public override bool Equals(object other)
    {
        if (!(other is IntXY))
        {
            return false;
        }

        IntXY point = (IntXY)other;
        return x == point.x && y == point.y;
    }

    public override string ToString()
    {
        return string.Join(", ", new string[] { x.ToString(), y.ToString() });
    }
}