using System.Diagnostics;
using System.Threading.Tasks;
using Fitness.Contexts;
using Fitness.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Controllers
{
    public class HomeController(AppDbContext _context) : Controller    
    {
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers.Select(x=>new TrainerGetVM()
            {
                Name = x.Name,
                Description = x.Description,
                ImagePath = x.ImagePath,
                ProfessionName = x.Profession.Name
            }).ToListAsync();

            return View(trainers);
        }
    }
}
