# Black Powder Army Builder for Umbraco

A comprehensive Black Powder 2nd Edition army list builder and unit manager built with Umbraco 17.

## Features

- **Unit Library**: Browse and manage all your Black Powder units with full stat profiles
- **Army Builder**: Interactive army list builder with real-time points calculation
- **Army Management**: Save and view multiple army lists
- **Unit Stats**: Complete stat profiles including:
  - Clash
  - Sustained Fire
  - Short Range Fire
  - Long Range Fire
  - Morale Save
  - Stamina
  - Special Rules

## Project Structure

```
UmbracoProject1/
├── Models/
│   ├── BlackPowderUnit.cs       # Unit model with stats and properties
│   └── BlackPowderArmy.cs       # Army model with unit composition
├── DocumentTypes/
│   └── BlackPowderUnitDocumentType.cs  # Umbraco document type definitions
├── Controllers/
│   └── ArmyBuilderController.cs # API endpoints for army management
├── Views/
│   └── BlackPowderHome.cshtml   # Main view template
└── wwwroot/
    └── army-builder.js          # Interactive army builder JavaScript
```

## Setup Instructions

### 1. Initial Setup

The project is already configured with:
- Umbraco 17 (beta)
- .NET 10.0
- SQLite database

### 2. Run the Application

```bash
dotnet run
```

The application will start and be available at `https://localhost:5001` (or the port shown in the console).

### 3. First Time Setup

1. Navigate to `https://localhost:5001/umbraco` to access the Umbraco backoffice
2. Complete the initial Umbraco setup wizard if prompted
3. Log in with your admin credentials

### 4. Create the Home Page

1. In the Umbraco backoffice, navigate to the Content section
2. Click "Create" and select "Black Powder Home"
3. Fill in:
   - Page Title: "Black Powder Army Builder"
   - Page Description: "Build and manage your Black Powder armies"
4. Save and Publish

### 5. Add Units

1. Under the home page, click "Create" and select "Black Powder Unit"
2. Fill in the unit details:
   - **General Tab**:
     - Unit Name (e.g., "British Line Infantry")
     - Unit Type (Infantry, Cavalry, Artillery, etc.)
     - Unit Size (Tiny, Small, Standard, Large, Very Large)
     - Points Cost
     - Description
   - **Combat Stats Tab**:
     - Clash
     - Sustained Fire
     - Short Range Fire
     - Long Range Fire
     - Morale Save
     - Stamina
   - **Special Rules Tab**:
     - Special Rules (e.g., "Steady Line, First Fire")
3. Save and Publish
4. Repeat for all your units

### Example Units

#### British Line Infantry
- Type: Infantry
- Size: Standard
- Points: 100
- Clash: 7
- Sustained Fire: 3
- Short Range Fire: 3
- Long Range Fire: 2
- Morale Save: 4+
- Stamina: 3
- Special Rules: "Steady Line, First Fire"

#### French Dragoons
- Type: Cavalry
- Size: Standard
- Points: 120
- Clash: 8
- Sustained Fire: 2
- Short Range Fire: 2
- Long Range Fire: -
- Morale Save: 4+
- Stamina: 3
- Special Rules: "Marauders, Mounted Infantry"

### 6. Using the Army Builder

1. Navigate to the home page on the frontend
2. Browse your units in the "Units Library" tab
3. Switch to the "Army Builder" tab
4. Fill in army details:
   - Army Name
   - Faction (e.g., British, French, Prussian)
   - Period (e.g., Napoleonic Wars)
   - Target Points (default: 1000)
5. Click on units from the left panel to add them to your army
6. Adjust quantities as needed
7. Click "Save Army" when done
8. View your saved armies in the "My Armies" tab

## API Endpoints

The Army Builder Controller provides the following endpoints:

### Get All Units
```
GET /umbraco/api/armybuilder/getunits
```
Returns all published units with their stats.

### Save Army
```
POST /umbraco/api/armybuilder/savearmy
Content-Type: application/json

{
  "name": "My Napoleonic Army",
  "faction": "British",
  "period": "Napoleonic Wars",
  "targetPoints": 1000,
  "units": [
    {
      "unitId": "guid-here",
      "name": "British Line Infantry",
      "quantity": 2,
      "pointsCost": 100,
      "notes": ""
    }
  ]
}
```

### Get Army
```
GET /umbraco/api/armybuilder/getarmy?id={guid}
```
Returns a specific army by ID.

## Black Powder Rules Reference

The site is designed to work with Black Powder 2nd Edition rules. The PDF reference is included in the project root.

### Unit Types
- **Infantry**: Line infantry, light infantry, heavy infantry
- **Cavalry**: Heavy cavalry, light cavalry, dragoons
- **Artillery**: Field guns, heavy guns

### Unit Sizes
- **Tiny**: Small detachments
- **Small**: Reduced strength units
- **Standard**: Normal strength units
- **Large**: Oversized units
- **Very Large**: Massive formations

### Combat Stats Explained
- **Clash**: Hand-to-hand combat value
- **Sustained Fire**: Multiple shots per turn
- **Short Range Fire**: Shooting at close range (0-6")
- **Long Range Fire**: Shooting at long range (6-18")
- **Morale Save**: Armor/training save (4+, 5+, etc.)
- **Stamina**: Number of hits the unit can take

## Customization

### Adding New Unit Types

Edit `Models/BlackPowderUnit.cs` to add new unit types to the enum:

```csharp
public enum UnitType
{
    Infantry,
    Cavalry,
    Artillery,
    LightCavalry,
    HeavyCavalry,
    Dragoons,
    LightInfantry,
    HeavyInfantry,
    YourNewType  // Add here
}
```

### Styling

All styles are contained in `Views/BlackPowderHome.cshtml`. The design uses:
- Purple/gradient color scheme
- Card-based layout
- Responsive grid system
- Clean, modern UI

## Technologies Used

- **Backend**: Umbraco 17 CMS, ASP.NET Core, C#
- **Frontend**: HTML, CSS, Vanilla JavaScript
- **Database**: SQLite (default, can be changed to SQL Server)
- **Data Format**: JSON for army unit storage

## Troubleshooting

### Document Types Not Appearing
- Restart the application after creating the document types
- Check the Umbraco logs in `umbraco/Logs/`

### Units Not Showing in Army Builder
- Ensure units are Published (not just Saved)
- Check browser console for JavaScript errors
- Verify the API endpoint is accessible at `/umbraco/api/armybuilder/getunits`

### Cannot Save Armies
- Ensure the home page exists and is published
- Check that you have permission to create content
- Verify the army has a name and at least one unit

## Future Enhancements

Potential features to add:
- Print-friendly army list view
- PDF export
- Army comparison tool
- Points calculator for different game sizes
- Special rules database
- Historical army templates
- Multi-language support
- Army sharing/export

## License

This is a custom Umbraco solution for Black Powder army list management.

## Support

For issues or questions:
1. Check the Umbraco documentation: https://docs.umbraco.com/
2. Review Black Powder 2nd Edition rulebook
3. Check browser console for JavaScript errors
4. Review Umbraco logs in `umbraco/Logs/`
