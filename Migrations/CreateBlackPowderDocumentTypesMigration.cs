using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;

namespace UmbracoProject1.Migrations;

public class CreateBlackPowderDocumentTypesMigration : MigrationBase
{
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IFileService _fileService;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly PropertyEditorCollection _propertyEditorCollection;
    private readonly IConfigurationEditorJsonSerializer _serializer;

    public CreateBlackPowderDocumentTypesMigration(
        IMigrationContext context,
        IContentTypeService contentTypeService,
        IDataTypeService dataTypeService,
        IFileService fileService,
        IShortStringHelper shortStringHelper,
        PropertyEditorCollection propertyEditorCollection,
        IConfigurationEditorJsonSerializer serializer) : base(context)
    {
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _fileService = fileService;
        _shortStringHelper = shortStringHelper;
        _propertyEditorCollection = propertyEditorCollection;
        _serializer = serializer;
    }

    protected override void Migrate()
    {
        // Create template first
        CreateTemplate();

        // Create document types
        CreateUnitTypeDocumentType();
        CreateBlackPowderUnitDocumentType();
        CreateBlackPowderArmyDocumentType();
        CreateBlackPowderHomeDocumentType();

        // Set up relationships
        SetAllowedChildTypes();
    }

    private void CreateTemplate()
    {
        var template = _fileService.GetTemplate("BlackPowderHome");
        if (template == null)
        {
            template = new Template(_shortStringHelper, "Black Powder Home", "BlackPowderHome");
            _fileService.SaveTemplate(template);
        }
    }

    private void CreateUnitTypeDocumentType()
    {
        if (_contentTypeService.Get("unitType") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "unitType",
            Name = "Unit Type",
            Icon = "icon-badge color-orange"
        };

        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var textareaEditor = _propertyEditorCollection["Umbraco.TextArea"];

        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var textarea = _dataTypeService.GetDataType("Textarea") ?? CreateDataType("Textarea", textareaEditor);

        var contentGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Content",
            Alias = "content"
        };

        contentGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "unitType")
        {
            Name = "Unit Type",
            Description = "E.g., Infantry, Cavalry, Artillery, Light Cavalry, Heavy Cavalry, Dragoons",
            Mandatory = true
        });
        contentGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textarea, "description")
        {
            Name = "Description",
            Description = "Description of this unit type"
        });

        contentType.PropertyGroups.Add(contentGroup);

        _contentTypeService.Save(contentType);
    }

    private void CreateBlackPowderUnitDocumentType()
    {
        if (_contentTypeService.Get("blackPowderUnit") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "blackPowderUnit",
            Name = "Black Powder Unit",
            Icon = "icon-medal color-black"
        };

        // Get data types - use the editor aliases
        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var textareaEditor = _propertyEditorCollection["Umbraco.TextArea"];
        var integerEditor = _propertyEditorCollection["Umbraco.Integer"];
        var contentPickerEditor = _propertyEditorCollection["Umbraco.ContentPicker"];

        // Get or create data types
        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var textarea = _dataTypeService.GetDataType("Textarea") ?? CreateDataType("Textarea", textareaEditor);
        var numeric = _dataTypeService.GetDataType("Numeric") ?? CreateDataType("Numeric", integerEditor);
        var contentPicker = _dataTypeService.GetDataType("Content Picker") ?? CreateDataType("Content Picker", contentPickerEditor);

        // Add General tab
        var generalGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "General",
            Alias = "general"
        };

        generalGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "unitName") { Name = "Unit Name", Mandatory = true });
        generalGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, contentPicker, "unitType")
        {
            Name = "Unit Type",
            Description = "Select the type of unit (Infantry, Cavalry, etc.)",
            Mandatory = true
        });
        generalGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textstring, "unitSize") { Name = "Unit Size", Mandatory = true });
        generalGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "pointsCost") { Name = "Points Cost", Mandatory = true });
        generalGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textarea, "description") { Name = "Description" });

        contentType.PropertyGroups.Add(generalGroup);

        // Add Combat Stats tab
        var statsGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Combat Stats",
            Alias = "combatStats"
        };

        statsGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, numeric, "clash") { Name = "Clash", Mandatory = true });
        statsGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "sustainedFire") { Name = "Sustained Fire", Mandatory = true });
        statsGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "shortRangeFire") { Name = "Short Range Fire", Mandatory = true });
        statsGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "longRangeFire") { Name = "Long Range Fire", Mandatory = true });
        statsGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "moraleSave") { Name = "Morale Save", Mandatory = true });
        statsGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "stamina") { Name = "Stamina", Mandatory = true });

        contentType.PropertyGroups.Add(statsGroup);

        // Add Special Rules tab
        var rulesGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Special Rules",
            Alias = "specialRules"
        };

        rulesGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textarea, "specialRules") { Name = "Special Rules" });

        contentType.PropertyGroups.Add(rulesGroup);

        _contentTypeService.Save(contentType);
    }

    private void CreateBlackPowderArmyDocumentType()
    {
        if (_contentTypeService.Get("blackPowderArmy") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "blackPowderArmy",
            Name = "Black Powder Army",
            Icon = "icon-flag-alt color-red"
        };

        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var textareaEditor = _propertyEditorCollection["Umbraco.TextArea"];
        var integerEditor = _propertyEditorCollection["Umbraco.Integer"];

        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var textarea = _dataTypeService.GetDataType("Textarea") ?? CreateDataType("Textarea", textareaEditor);
        var numeric = _dataTypeService.GetDataType("Numeric") ?? CreateDataType("Numeric", integerEditor);

        var armyGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Army Details",
            Alias = "armyDetails"
        };

        armyGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "armyName") { Name = "Army Name", Mandatory = true });
        armyGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textstring, "faction") { Name = "Faction", Mandatory = true });
        armyGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textstring, "period") { Name = "Period" });
        armyGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, numeric, "targetPoints") { Name = "Target Points", Mandatory = true });
        armyGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textarea, "armyUnits") { Name = "Army Units (JSON)" });

        contentType.PropertyGroups.Add(armyGroup);

        _contentTypeService.Save(contentType);
    }

    private void CreateBlackPowderHomeDocumentType()
    {
        if (_contentTypeService.Get("blackPowderHome") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "blackPowderHome",
            Name = "Black Powder Home",
            Icon = "icon-home color-blue",
            AllowedAsRoot = true
        };

        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var textareaEditor = _propertyEditorCollection["Umbraco.TextArea"];

        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var textarea = _dataTypeService.GetDataType("Textarea") ?? CreateDataType("Textarea", textareaEditor);

        var contentGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Content",
            Alias = "content"
        };

        contentGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "pageTitle") { Name = "Page Title", Mandatory = true });
        contentGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textarea, "pageDescription") { Name = "Page Description" });

        contentType.PropertyGroups.Add(contentGroup);

        // Assign template
        var template = _fileService.GetTemplate("BlackPowderHome");
        if (template != null)
        {
            contentType.AllowedTemplates = new[] { template };
            contentType.SetDefaultTemplate(template);
        }

        _contentTypeService.Save(contentType);
    }

    private void SetAllowedChildTypes()
    {
        var homeType = _contentTypeService.Get("blackPowderHome");
        var unitTypeDocType = _contentTypeService.Get("unitType");
        var unitType = _contentTypeService.Get("blackPowderUnit");
        var armyType = _contentTypeService.Get("blackPowderArmy");

        if (homeType != null && unitTypeDocType != null && unitType != null && armyType != null)
        {
            homeType.AllowedContentTypes = new[]
            {
                new ContentTypeSort(unitTypeDocType.Key, 0, unitTypeDocType.Alias),
                new ContentTypeSort(unitType.Key, 1, unitType.Alias),
                new ContentTypeSort(armyType.Key, 2, armyType.Alias)
            };
            _contentTypeService.Save(homeType);
        }
    }

    private IDataType CreateDataType(string name, IDataEditor editor)
    {
        var dataType = new DataType(editor, _serializer)
        {
            Name = name
        };
        _dataTypeService.Save(dataType);
        return dataType;
    }
}
