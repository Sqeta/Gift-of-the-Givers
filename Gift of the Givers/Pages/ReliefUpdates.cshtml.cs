using Gift_of_the_Givers.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class ReliefUpdatesModel : PageModel
    {
        private readonly PrototypeStore _store;

        public ReliefUpdatesModel(
            PrototypeStore store)
        {
            _store = store;
        }


        public List<ReliefUpdate> Updates { get; set; }
            = new List<ReliefUpdate>();


        public void OnGet()
        {
            Updates = _store.ReliefUpdates
                .OrderByDescending(
                    update => update.DatePosted
                )
                .ToList();
        }
    }
}