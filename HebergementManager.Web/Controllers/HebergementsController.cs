using Microsoft.AspNetCore.Mvc;
using HebergementManager.Web.Models;
using HebergementManager.Web.Services;

namespace HebergementManager.Web.Controllers;

public class HebergementsController : Controller
{
    private readonly HebergementService _hebergementService;

    public HebergementsController(HebergementService hebergementService)
    {
        _hebergementService = hebergementService;
    }
    public async Task<IActionResult> Index()
    {
        var hebergements = await _hebergementService.GetAllAsync();
        var types = await _hebergementService.GetTypesHebergementAsync();
        var equipements = await _hebergementService.GetEquipementsAsync();

        ViewBag.TypeHebergements = types;
        ViewBag.Equipements = equipements;

        return View(hebergements);
    }
    public async Task<IActionResult> Details(int id)
    {
        var hebergement = await _hebergementService.GetByIdAsync(id);
        if (hebergement == null)
            return NotFound();

        var types = await _hebergementService.GetTypesHebergementAsync();
        var equipements = await _hebergementService.GetEquipementsAsync();
        ViewBag.TypeHebergements = types;
        ViewBag.Equipements = equipements;

        return View(hebergement);
    }

    public async Task<IActionResult> Create()
    {
        var types = await _hebergementService.GetTypesHebergementAsync();
        var equipements = await _hebergementService.GetEquipementsAsync();
        ViewBag.TypeHebergements = types;
        ViewBag.Equipements = equipements;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HebergementViewModel model)
    {
        if (ModelState.IsValid)
        {
            var success = await _hebergementService.CreateAsync(model);
            if (success)
            {
                TempData["Success"] = "Hébergement créé avec succès !";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Erreur lors de la création");
        }

        var types = await _hebergementService.GetTypesHebergementAsync();
        var equipements = await _hebergementService.GetEquipementsAsync();
        ViewBag.TypeHebergements = types;
        ViewBag.Equipements = equipements;
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var hebergement = await _hebergementService.GetByIdAsync(id);
        if (hebergement == null)
            return NotFound();

        var types = await _hebergementService.GetTypesHebergementAsync();
        var equipements = await _hebergementService.GetEquipementsAsync();
        ViewBag.TypeHebergements = types;
        ViewBag.Equipements = equipements;

        return View(hebergement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, HebergementViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var success = await _hebergementService.UpdateAsync(id, model);
            if (success)
            {
                TempData["Success"] = "Hébergement modifié avec succès !";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Erreur lors de la modification");
        }

        var types = await _hebergementService.GetTypesHebergementAsync();
        var equipements = await _hebergementService.GetEquipementsAsync();
        ViewBag.TypeHebergements = types;
        ViewBag.Equipements = equipements;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _hebergementService.DeleteAsync(id);
        if (success)
        {
            TempData["Success"] = "Hébergement supprimé avec succès !";
        }
        else
        {
            TempData["Error"] = "Erreur lors de la suppression";
        }
        
        return RedirectToAction(nameof(Index));
    }
}