namespace Jobs.Reports
{
    public interface iJobReportFactory
    {
        iJobReport Create(bool pIncludeTasks);
    }
}