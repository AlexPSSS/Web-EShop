using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewComponents
{
    [ViewComponent(Name = "LoginLogout")]
    public class LogInOutViewComponents : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}