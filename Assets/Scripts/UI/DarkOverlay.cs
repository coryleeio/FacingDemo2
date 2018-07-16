namespace Gamepackage
{
    public class DarkOverlay : UIComponent
    {
        public override void Hide()
        {
            GetComponent<DarkOverlay>().gameObject.SetActive(false);
        }

        public override void Show()
        {
          
            GetComponent<DarkOverlay>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {

        }
    }
}
