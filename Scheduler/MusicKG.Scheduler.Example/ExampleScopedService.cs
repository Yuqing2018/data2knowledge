namespace MusicKG.Scheduler.Example
{
    public interface IExampleScopedService
    {
        string GetResult();
    }

    public class ExampleScopedService : IExampleScopedService
    {
        public string GetResult()
        {
            return "Hello World";
        }
    }
}
