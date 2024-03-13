using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangedProperty<TValue> : IEquatable<ChangedProperty<TValue>>
{
    [SerializeField] private TValue _value;

    public TValue Value
    {
        get => _value;
        set
        {
            if (_value.Equals(value))
                return;

            _value = value;
            OnChanged?.Invoke();
        }
    }

    public delegate void ChangedHandler();
    public event ChangedHandler OnChanged;

    public ChangedProperty()
    {
    }

    public ChangedProperty(TValue value)
    {
        _value = value;
    }

    public bool Equals(ChangedProperty<TValue> other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return EqualityComparer<TValue>.Default.Equals(_value, other._value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        return Equals((ChangedProperty<TValue>)obj);
    }

    public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(_value);

    public static bool operator ==(ChangedProperty<TValue> left, ChangedProperty<TValue> right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ChangedProperty<TValue> left, ChangedProperty<TValue> right)
    {
        return !(left == right);
    }

    public static implicit operator TValue(ChangedProperty<TValue> obj)
    {
        return obj.Value;
    }

    public static explicit operator ChangedProperty<TValue>(TValue obj)
    {
        return new ChangedProperty<TValue>(obj);
    }
}