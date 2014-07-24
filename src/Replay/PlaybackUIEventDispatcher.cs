using Domain;

namespace Replay
{
    public class PlaybackUIEventDispatcher:IUIEventDispatcher
    {
        public void Notify(string viewName, object o)
        {
            //don't update UI during playback
        }

        public void Notify(string viewName, string key, object o)
        {
            //don't update UI during playback
        }
    }
}