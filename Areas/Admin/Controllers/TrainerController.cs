using System.Threading.Tasks;
using Fitness.Contexts;
using Fitness.Helpers;
using Fitness.Models;
using Fitness.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Areas.Admin.Controllers;
[Area("Admin")]
public class TrainerController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private string _folderPath;

    public TrainerController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath, "assets" , "images");
    }

    public async Task<IActionResult> Index()
    {
        var trainers = await _context.Trainers.Select(x=>new TrainerGetVM()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            ImagePath = x.ImagePath,
            ProfessionName = x.Profession.Name
        }).ToListAsync();

        return View(trainers);
    }



    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await _sendProfessionsWithViewBag();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TrainerCreateVM vm)
    {
        await _sendProfessionsWithViewBag();

        if (!ModelState.IsValid)
            return View(vm);

        var isExistProfession = await _context.Professions.AnyAsync(x => x.Id == vm.ProfessionId);
        if (!isExistProfession)
        {
            ModelState.AddModelError("ProfessionId","This profession doesn't exist");
            return View(vm);
        }

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Image size must be less than 2 MB");
            return View(vm);
        }

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("image", "You must upload be Image");
            return View(vm);
        }

        string uniqueFileName = await vm.Image.UploadFile(_folderPath);

        Trainer trainer = new()
        {
            Name = vm.Name,
            Description =vm.Description,
            ProfessionId = vm.ProfessionId,
            ImagePath = uniqueFileName
        };

        await _context.Trainers.AddAsync(trainer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }



    private async Task _sendProfessionsWithViewBag()
    {
        var professions = await _context.Professions.ToListAsync();
        ViewBag.Professions = professions;
    }
}
