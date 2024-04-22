using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public List<GameServiceWrapper> Services = new();
    private List<GameServiceDelegate> Delegates = new();
    private Dictionary<Type, GameServiceWrapper> InternalServices = new();

    public static Game Instance;

    public void OnEnable()
    {
        Instance = this;
    }

    public void Start()
    {
        ConvertToDictionary();
        InitMode();
    }
    private void ConvertToDictionary()
    {
        foreach (GameServiceWrapper Wrapper in Services)
        {
            Type Type = Wrapper.TargetScript.GetType();
            InternalServices.Add(Type, Wrapper);
        }
    }

    private void InitMode()
    {
        foreach (GameServiceWrapper Wrapper in InternalServices.Values)
        {
            Wrapper.TargetScript.StartService();
        }
    }

    public static T GetService<T>() where T : GameService
    {
        if (!Instance)
            return null;

        if (!Instance.InternalServices.ContainsKey(typeof(T)))
            return null;

        return Instance.InternalServices[typeof(T)].TargetScript as T;
    }

    public static bool TryGetService<T>(out T Service, bool ForceLoad = false) where T: GameService
    {
        Service = GetService<T>();
        if (Service == null && ForceLoad)
        {
            throw new Exception("Missing Service: " + typeof(T).ToString());
        }
        return Service != null;
    }

    public static bool TryGetServices<X, Y>(out X ServiceX, out Y ServiceY, bool ForceLoad = false) where X : GameService where Y : GameService
    {
        ServiceX = GetService<X>();
        ServiceY = GetService<Y>();
        if ((ServiceX ==  null || ServiceY == null) && ForceLoad)
        {
            throw new Exception("Missing Service: " + typeof(X).ToString() + " or "+typeof(Y).ToString());
        }
        return ServiceX != null && ServiceY != null;
    }

    public static bool TryGetServices<X, Y, Z>(out X ServiceX, out Y ServiceY, out Z ServiceZ, bool ForceLoad = false) where X : GameService where Y : GameService where Z : GameService
    {
        ServiceX = GetService<X>();
        ServiceY = GetService<Y>();
        ServiceZ = GetService<Z>();
        if ((ServiceX == null || ServiceY == null || ServiceZ == null) && ForceLoad)
        {
            throw new Exception("Missing Service: " + typeof(X).ToString() + " or " + typeof(Y).ToString() + " or " + typeof(Z).ToString());
        }
        return ServiceX != null && ServiceY != null;
    }

    public static void RunAfterServiceStart<T>(Action<T> Callback) where T : GameService
    {
        T Service = GetService<T>();
        if (Service == null)
            return;

        GameServiceDelegate<T> Delegate = new(Service, Callback);
        Instance.Delegates.Add(Delegate);
    }

    public static void RunAfterServiceStart<X, Y>(Action<X, Y> Callback) where X : GameService where Y : GameService
    {
        X ServiceX = GetService<X>();
        Y ServiceY = GetService<Y>();
        if (ServiceX == null || ServiceY == null || !Instance || Callback == null)
            return;

        GameServiceDelegate<X, Y> Delegate = new GameServiceDelegate<X, Y>(ServiceX, ServiceY, Callback);
        Instance.Delegates.Add(Delegate);
    }

    public static void RunAfterServiceInit<T>(Action<T> Callback) where T : GameService
    {
        T Service = GetService<T>();
        if (Service == null)
            return;

        GameServiceDelegate<T> Delegate = new(Service, Callback, GameServiceDelegate.DelegateType.OnInit);
        Instance.Delegates.Add(Delegate);
    }

    public static void RunAfterServiceInit<X, Y>(Action<X, Y> Callback) where X : GameService where Y : GameService
    {
        X ServiceX = GetService<X>();
        Y ServiceY = GetService<Y>();
        if (ServiceX == null || ServiceY == null || !Instance || Callback == null)
            return;

        GameServiceDelegate<X, Y> Delegate = new(ServiceX, ServiceY, Callback, GameServiceDelegate.DelegateType.OnInit);
        Instance.Delegates.Add(Delegate);
    }

    public static void RemoveServiceDelegate(GameServiceDelegate Delegate) { 
        Instance.Delegates.Remove(Delegate);
    }

}
