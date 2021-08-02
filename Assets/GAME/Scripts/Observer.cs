using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
    public void SubjectUpdate();
}

public interface Subject
{
    public void ResignObserver(Observer observer);
    public void RemoveObserver(Observer observer);
    public void NotifyObservers();
}
