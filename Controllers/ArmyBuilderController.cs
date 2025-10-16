using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoProject1.Models;
using System.Text.Json;
using Umbraco.Cms.Core.Models; // Add this using for IContentType

namespace UmbracoProject1.Controllers;

public class ArmyBuilderController : UmbracoApiController
{
    private readonly IContentService _contentService;

    public ArmyBuilderController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet]
    public IActionResult GetUnits()
    {
        var contentTypeService = (IContentTypeService)HttpContext.RequestServices.GetService(typeof(IContentTypeService));
        var unitType = contentTypeService?.Get("blackPowderUnit");
        if (unitType == null)
            return NotFound("Unit content type not found");

        // FIX: Replace GetContentOfContentType with GetPagedOfType
        long totalRecords;
        var units = _contentService.GetPagedOfType(
            unitType.Id,
            pageIndex: 0,
            pageSize: int.MaxValue,
            out totalRecords,
            filter: null
        )
        .Where(x => x.Published)
        .Select(u => new
        {
            Id = u.Key,
            Name = u.GetValue<string>("unitName"),
            Type = u.GetValue<string>("unitType"),
            Size = u.GetValue<string>("unitSize"),
            PointsCost = u.GetValue<int>("pointsCost"),
            Clash = u.GetValue<int>("clash"),
            SustainedFire = u.GetValue<int>("sustainedFire"),
            ShortRangeFire = u.GetValue<int>("shortRangeFire"),
            LongRangeFire = u.GetValue<int>("longRangeFire"),
            MoraleSave = u.GetValue<int>("moraleSave"),
            Stamina = u.GetValue<int>("stamina"),
            SpecialRules = u.GetValue<string>("specialRules"),
            Description = u.GetValue<string>("description")
        })
        .ToList();

        return Ok(units);
    }

    [HttpPost]
    public IActionResult SaveArmy([FromBody] BlackPowderArmy army)
    {
        if (string.IsNullOrWhiteSpace(army.Name))
            return BadRequest("Army name is required");

        // Use IContentTypeService to get the content type instead of _contentService.GetContentType
        var contentTypeService = (IContentTypeService)HttpContext.RequestServices.GetService(typeof(IContentTypeService));
        var armyType = contentTypeService?.Get("blackPowderArmy");
        if (armyType == null)
            return NotFound("Army content type not found");

        // Find the home page to create the army under
        var homePage = _contentService.GetRootContent()
            .FirstOrDefault(x => x.ContentType.Alias == "blackPowderHome");

        if (homePage == null)
            return NotFound("Home page not found");

        var newArmy = _contentService.Create(army.Name, homePage.Id, "blackPowderArmy");
        newArmy.SetValue("armyName", army.Name);
        newArmy.SetValue("faction", army.Faction);
        newArmy.SetValue("period", army.Period);
        newArmy.SetValue("targetPoints", army.TargetPoints);
        newArmy.SetValue("armyUnits", JsonSerializer.Serialize(army.Units));

        // Save and publish the content using available IContentService methods
        var saveResult = _contentService.Save(newArmy);
        if (!saveResult.Success)
            return StatusCode(500, "Failed to save army");

        // Publish the content after saving
        var publishResult = _contentService.Publish(newArmy, new[] { "*" });
        if (publishResult.Success)
            return Ok(new { Id = newArmy.Key, Message = "Army saved successfully" });

        return StatusCode(500, "Failed to publish army");
    }

    [HttpGet]
    public IActionResult GetArmy(Guid id)
    {
        var army = _contentService.GetById(id);
        if (army == null || army.ContentType.Alias != "blackPowderArmy")
            return NotFound();

        var unitsJson = army.GetValue<string>("armyUnits");
        var units = string.IsNullOrWhiteSpace(unitsJson)
            ? new List<ArmyUnit>()
            : JsonSerializer.Deserialize<List<ArmyUnit>>(unitsJson) ?? new List<ArmyUnit>();

        var armyData = new BlackPowderArmy
        {
            Name = army.GetValue<string>("armyName") ?? string.Empty,
            Faction = army.GetValue<string>("faction") ?? string.Empty,
            Period = army.GetValue<string>("period") ?? string.Empty,
            TargetPoints = army.GetValue<int>("targetPoints"),
            Units = units
        };

        return Ok(armyData);
    }
}
