namespace Domain
{
    public interface IUIEventDispatcher
    {
        void Notify(string viewName, object o);
        void Notify(string viewName, string key, object o);
    }
}