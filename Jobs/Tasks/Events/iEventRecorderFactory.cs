namespace Jobs.Tasks.Events
{
    public interface iEventRecorderFactory
    {
        iEventRecorder Create();
    }
}