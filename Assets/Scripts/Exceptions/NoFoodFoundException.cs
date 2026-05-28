using System;
using UnityEngine;

public class NoFoodFoundException : Exception
{
    public NoFoodFoundException() : base() { }

    public NoFoodFoundException(string message) : base(message) { }

    public NoFoodFoundException(string message, Exception inner) : base(message, inner) { }
}