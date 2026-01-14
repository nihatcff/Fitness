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

        string uniqueFileName = await vm.Image.UploadFileAsync(_folderPath);

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


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var trainer = await _context.Trainers.FindAsync(id);

        if(trainer is null)
        {
            return NotFound();
        }

        _context.Trainers.Remove(trainer);
        await _context.SaveChangesAsync();

        string deletedImagePath = Path.Combine(_folderPath, trainer.ImagePath);

        ExtensionMethods.DeleteFile(deletedImagePath);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        await _sendProfessionsWithViewBag();

        var trainer = await _context.Trainers.FindAsync(id);
        if(trainer is null) { return NotFound(); }

        TrainerUpdateVM vm = new()
        {
            Id = id,
            Name = trainer.Name,
            Description = trainer.Description,
            ProfessionId =trainer.ProfessionId
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TrainerUpdateVM vm)
    {
        await _sendProfessionsWithViewBag();
        if (!ModelState.IsValid)
            return View(vm);

        var isExistProfession = await _context.Professions.AnyAsync(x => x.Id == vm.ProfessionId);
        if (!isExistProfession)
        {
            ModelState.AddModelError("ProfessionId", "This profession doesn't exist");
            return View(vm);
        }

        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Image size must be less than 2 MB");
            return View(vm);
        }

        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("image", "You must upload be Image");
            return View(vm);
        }

        var existTrainer = await _context.Trainers.FindAsync(vm.Id);

        if (existTrainer is null) return BadRequest();

        existTrainer.Name = vm.Name;
        existTrainer.Description = vm.Description;
        existTrainer.ProfessionId = vm.ProfessionId;

        if (vm.Image is { })
        {
            string newImagePath = await vm.Image.UploadFileAsync(_folderPath);

            string oldImagePath = Path.Combine(_folderPath, existTrainer.ImagePath);
            ExtensionMethods.DeleteFile(oldImagePath);
            existTrainer.ImagePath = newImagePath;
        }

        _context.Trainers.Update(existTrainer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index)); 

    }



    private async Task _sendProfessionsWithViewBag()
    {
        var professions = await _context.Professions.ToListAsync();
        ViewBag.Professions = professions;
    }
}
