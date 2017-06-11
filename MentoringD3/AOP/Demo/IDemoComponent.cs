namespace AOP.Demo
{
    public interface IDemoComponent
    {
        int RunWithResult(DemoContext context);

        void RunWithoutResult(DemoContext context);

        void RunWithException(int input);
    }
}
