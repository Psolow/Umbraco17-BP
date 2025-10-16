using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;

namespace UmbracoProject1.Migrations;

public class UpdateUnitDataModelMigration : MigrationBase
{
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly PropertyEditorCollection _propertyEditorCollection;
    private readonly IConfigurationEditorJsonSerializer _serializer;

    public UpdateUnitDataModelMigration(
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
        // Create new document types
        CreateArmamentDocumentType();
        CreateSpecialsDocumentType();
        CreateCountryDocumentType();

        // Update Black Powder Unit with new fields
        UpdateBlackPowderUnitFields();
    }

    private void CreateArmamentDocumentType()
    {
        // Don't recreate if it already exists
        if (_contentTypeService.Get("armament") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "armament",
            Name = "Armament",
            Icon = "icon-target color-grey"
        };

        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var richtextEditor = _propertyEditorCollection["Umbraco.TinyMCE"];

        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var richtext = _dataTypeService.GetDataType("Richtext editor") ?? CreateDataType("Richtext editor", richtextEditor);

        var contentGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Content",
            Alias = "content"
        };

        contentGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "armamentName")
        {
            Name = "Armament Name",
            Description = "Name of the armament",
            Mandatory = true
        });
        contentGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, richtext, "armamentDescription")
        {
            Name = "Description",
            Description = "Description of this armament"
        });

        contentType.PropertyGroups.Add(contentGroup);

        _contentTypeService.Save(contentType);
    }

    private void CreateSpecialsDocumentType()
    {
        // Don't recreate if it already exists
        if (_contentTypeService.Get("special") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "special",
            Name = "Special",
            Icon = "icon-magic color-purple"
        };

        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var richtextEditor = _propertyEditorCollection["Umbraco.TinyMCE"];

        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var richtext = _dataTypeService.GetDataType("Richtext editor") ?? CreateDataType("Richtext editor", richtextEditor);

        var contentGroup = new PropertyGroup(new PropertyTypeCollection(true))
        {
            Name = "Content",
            Alias = "content"
        };

        contentGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "specialName")
        {
            Name = "Special Name",
            Description = "Name of the special rule",
            Mandatory = true
        });
        contentGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, richtext, "specialDescription")
        {
            Name = "Description",
            Description = "Description of this special rule"
        });

        contentType.PropertyGroups.Add(contentGroup);

        _contentTypeService.Save(contentType);
    }

    private void CreateCountryDocumentType()
    {
        // Don't recreate if it already exists
        if (_contentTypeService.Get("country") != null)
            return;

        var contentType = new ContentType(_shortStringHelper, -1)
        {
            Alias = "country",
            Name = "Country",
            Icon = "icon-flag color-green"
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

        contentGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "countryName")
        {
            Name = "Country Name",
            Description = "Name of the country",
            Mandatory = true
        });
        contentGroup.PropertyTypes.Add(new PropertyType(_shortStringHelper, textarea, "countryDescription")
        {
            Name = "Description",
            Description = "Description of this country"
        });

        contentType.PropertyGroups.Add(contentGroup);

        _contentTypeService.Save(contentType);
    }

    private void UpdateBlackPowderUnitFields()
    {
        var contentType = _contentTypeService.Get("blackPowderUnit");
        if (contentType == null)
            return;

        // Get editors and data types
        var textboxEditor = _propertyEditorCollection["Umbraco.TextBox"];
        var integerEditor = _propertyEditorCollection["Umbraco.Integer"];
        var richtextEditor = _propertyEditorCollection["Umbraco.TinyMCE"];
        var contentPickerEditor = _propertyEditorCollection["Umbraco.ContentPicker"];
        var multiNodeTreePickerEditor = _propertyEditorCollection["Umbraco.MultiNodeTreePicker"];

        var textstring = _dataTypeService.GetDataType("Textstring") ?? CreateDataType("Textstring", textboxEditor);
        var numeric = _dataTypeService.GetDataType("Numeric") ?? CreateDataType("Numeric", integerEditor);
        var richtext = _dataTypeService.GetDataType("Richtext editor") ?? CreateDataType("Richtext editor", richtextEditor);
        var contentPicker = _dataTypeService.GetDataType("Content Picker") ?? CreateDataType("Content Picker", contentPickerEditor);
        var multiPicker = _dataTypeService.GetDataType("Multi Node Tree Picker") ?? CreateDataType("Multi Node Tree Picker", multiNodeTreePickerEditor);

        // Get or create General tab
        var generalGroup = contentType.PropertyGroups.FirstOrDefault(x => x.Alias == "general");
        if (generalGroup == null)
        {
            generalGroup = new PropertyGroup(new PropertyTypeCollection(true))
            {
                Name = "General",
                Alias = "general",
                SortOrder = 0
            };
            contentType.PropertyGroups.Add(generalGroup);
        }

        // Update Unit Name - change from unitName to just use existing or create new
        if (!contentType.PropertyTypes.Any(x => x.Alias == "unitName"))
        {
            generalGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, textstring, "unitName")
            {
                Name = "Unit Name",
                Description = "Name of the unit",
                Mandatory = true,
                SortOrder = 0
            });
        }

        // Unit Type already exists as content picker from previous migration

        // Add Unit Armament picker
        if (!contentType.PropertyTypes.Any(x => x.Alias == "unitArmament"))
        {
            generalGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, contentPicker, "unitArmament")
            {
                Name = "Unit Armament",
                Description = "Select the armament for this unit",
                Mandatory = false,
                SortOrder = 2
            });
        }

        // Add Country picker
        if (!contentType.PropertyTypes.Any(x => x.Alias == "country"))
        {
            generalGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, contentPicker, "country")
            {
                Name = "Country",
                Description = "Select the country for this unit",
                Mandatory = false,
                SortOrder = 3
            });
        }

        // Add Unit Description (richtext)
        if (!contentType.PropertyTypes.Any(x => x.Alias == "unitDescription"))
        {
            generalGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, richtext, "unitDescription")
            {
                Name = "Unit Description",
                Description = "Detailed description of the unit",
                Mandatory = false,
                SortOrder = 4
            });
        }

        // Get or create Combat Stats tab
        var combatGroup = contentType.PropertyGroups.FirstOrDefault(x => x.Alias == "combatStats");
        if (combatGroup == null)
        {
            combatGroup = new PropertyGroup(new PropertyTypeCollection(true))
            {
                Name = "Combat Stats",
                Alias = "combatStats",
                SortOrder = 1
            };
            contentType.PropertyGroups.Add(combatGroup);
        }

        // Add Hand to Hand
        if (!contentType.PropertyTypes.Any(x => x.Alias == "handToHand"))
        {
            combatGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, numeric, "handToHand")
            {
                Name = "Hand to Hand",
                Description = "Hand to hand combat value",
                Mandatory = false,
                SortOrder = 0
            });
        }

        // Add Shooting
        if (!contentType.PropertyTypes.Any(x => x.Alias == "shooting"))
        {
            combatGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, numeric, "shooting")
            {
                Name = "Shooting",
                Description = "Shooting value",
                Mandatory = false,
                SortOrder = 1
            });
        }

        // Add Morale
        if (!contentType.PropertyTypes.Any(x => x.Alias == "morale"))
        {
            combatGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, numeric, "morale")
            {
                Name = "Morale",
                Description = "Morale value",
                Mandatory = false,
                SortOrder = 2
            });
        }

        // Add Stamina
        if (!contentType.PropertyTypes.Any(x => x.Alias == "stamina"))
        {
            combatGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, numeric, "stamina")
            {
                Name = "Stamina",
                Description = "Stamina value",
                Mandatory = false,
                SortOrder = 3
            });
        }

        // Get or create Special Rules tab
        var specialGroup = contentType.PropertyGroups.FirstOrDefault(x => x.Alias == "specialRules");
        if (specialGroup == null)
        {
            specialGroup = new PropertyGroup(new PropertyTypeCollection(true))
            {
                Name = "Special Rules",
                Alias = "specialRules",
                SortOrder = 2
            };
            contentType.PropertyGroups.Add(specialGroup);
        }

        // Add Special multi-picker
        if (!contentType.PropertyTypes.Any(x => x.Alias == "special"))
        {
            specialGroup.PropertyTypes!.Add(new PropertyType(_shortStringHelper, multiPicker, "special")
            {
                Name = "Special",
                Description = "Select multiple special rules for this unit",
                Mandatory = false,
                SortOrder = 0
            });
        }

        _contentTypeService.Save(contentType);

        // Update allowed child types on home page
        UpdateAllowedChildTypes();
    }

    private void UpdateAllowedChildTypes()
    {
        var homeType = _contentTypeService.Get("blackPowderHome");
        var unitTypeDocType = _contentTypeService.Get("unitType");
        var unitType = _contentTypeService.Get("blackPowderUnit");
        var armyType = _contentTypeService.Get("blackPowderArmy");
        var armamentType = _contentTypeService.Get("armament");
        var specialType = _contentTypeService.Get("special");
        var countryType = _contentTypeService.Get("country");

        if (homeType != null)
        {
            var allowedTypes = new List<ContentTypeSort>();
            var sortOrder = 0;

            if (unitTypeDocType != null)
                allowedTypes.Add(new ContentTypeSort(unitTypeDocType.Key, sortOrder++, unitTypeDocType.Alias));
            if (unitType != null)
                allowedTypes.Add(new ContentTypeSort(unitType.Key, sortOrder++, unitType.Alias));
            if (armyType != null)
                allowedTypes.Add(new ContentTypeSort(armyType.Key, sortOrder++, armyType.Alias));
            if (armamentType != null)
                allowedTypes.Add(new ContentTypeSort(armamentType.Key, sortOrder++, armamentType.Alias));
            if (specialType != null)
                allowedTypes.Add(new ContentTypeSort(specialType.Key, sortOrder++, specialType.Alias));
            if (countryType != null)
                allowedTypes.Add(new ContentTypeSort(countryType.Key, sortOrder++, countryType.Alias));

            homeType.AllowedContentTypes = allowedTypes;
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
