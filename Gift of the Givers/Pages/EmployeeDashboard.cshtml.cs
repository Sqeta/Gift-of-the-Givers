using System.ComponentModel.DataAnnotations;
using Gift_of_the_Givers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    [Authorize(Roles = "Employee")]
    public class EmployeeDashboardModel : PageModel
    {
        private readonly PrototypeStore _store;


        public EmployeeDashboardModel(
            PrototypeStore store)
        {
            _store = store;
        }


        public List<VolunteerSignup> Volunteers =>
            _store.Volunteers
                .OrderByDescending(
                    volunteer => volunteer.DateSubmitted)
                .ToList();


        public List<ReliefUpdate> ReliefUpdates =>
            _store.ReliefUpdates
                .OrderByDescending(
                    update => update.DatePosted)
                .ToList();


        [BindProperty]
        [Required(ErrorMessage = "Please enter the relief project name.")]
        public string ProjectName { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please enter an update.")]
        public string UpdateMessage { get; set; } = string.Empty;


        public void OnGet()
        {
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _store.ReliefUpdates.Add(
                new ReliefUpdate
                {
                    ProjectName = ProjectName,
                    UpdateMessage = UpdateMessage,

                    PostedBy =
                        User.Identity?.Name
                        ?? "Employee",

                    DatePosted = DateTime.Now
                }
            );


            TempData["UpdateSuccess"] =
                "Relief project update posted successfully.";


            return RedirectToPage(
                "/EmployeeDashboard"
            );
        }
    }
}