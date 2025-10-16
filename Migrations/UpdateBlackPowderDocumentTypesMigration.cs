using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;

namespace UmbracoProject1.Migrations;

public class UpdateBlackPowderDocumentTypesMigration : MigrationBase
{
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly PropertyEditorCollection _propertyEditorCollection;
    private readonly IConfigurationEditorJsonSerializer _serializer;

    public UpdateBlackPowderDocumentTypesMigration(
        IMigrationContext context,
        IContentTypeService contentTypeService,
        IDataTypeService dataTypeService,
        IShortStringHelper shortStringHelper,
        PropertyEditorCollection propertyEditorCollection,
        IConfigurationEditorJsonSerializer serializer) : base(context)
    {
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _shortStringHelper = shortStringHelper;
        _propertyEditorCollection = propertyEditorCollection;
        _serializer = serializer;
    }

    protected override void Migrate()
    {
        // Create Unit Type document type
        CreateUnitTypeDocumentType();

        // Update Black Powder Unit to use Content Picker
        UpdateBlackPowderUnitDocumentType();

        // Update allowed child types
        UpdateAllowedChildTypes();
    }

    private void CreateUnitTypeDocumentType()
    {
        // Don't recreate if it already exists
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

    private void UpdateBlackPowderUnitDocumentType()
    {
        var contentType = _contentTypeService.Get("blackPowderUnit");
        if (contentType == null)
            return;

        // Check if unitType property already exists
        var unitTypeProperty = contentType.PropertyTypes.FirstOrDefault(x => x.Alias == "unitType");
        if (unitTypeProperty == null)
            return;

        // Get the Content Picker editor
        var contentPickerEditor = _propertyEditorCollection["Umbraco.ContentPicker"];
        var contentPicker = _dataTypeService.GetDataType("Content Picker") ?? CreateDataType("Content Picker", contentPickerEditor);

        // Remove old property
        contentType.RemovePropertyType(unitTypeProperty.Alias);

        // Add new property with Content Picker
        var generalGroup = contentType.PropertyGroups.FirstOrDefault(x => x.Alias == "general");
        if (generalGroup != null)
        {
            var newProperty = new PropertyType(_shortStringHelper, contentPicker, "unitType")
            {
                Name = "Unit Type",
                Description = "Select the type of unit (Infantry, Cavalry, etc.)",
                Mandatory = true,
                SortOrder = 1
            };

            generalGroup.PropertyTypes!.Add(newProperty);
        }

        _contentTypeService.Save(contentType);
    }

    private void UpdateAllowedChildTypes()
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
