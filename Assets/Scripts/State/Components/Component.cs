namespace Gamepackage
{
    public abstract class Component : IHasApplicationContext
    {
        protected ApplicationContext Context;

        public virtual void InjectContext(ApplicationContext context)
        {
            Context = context;
        }
    }
}
