namespace Jobs.Tasks.Events
{
    public class EventRecorderFactory : iEventRecorderFactory
    {
        public iEventRecorder Create()
        {
            return new EventRecorder(100);
        }
    }
}