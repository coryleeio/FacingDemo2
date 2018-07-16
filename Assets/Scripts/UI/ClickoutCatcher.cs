namespace Gamepackage
{
    public class ClickoutCatcher : UIComponent
    {
        public override void Hide()
        {
            GetComponent<ClickoutCatcher>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<ClickoutCatcher>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {
        }
    }
}
