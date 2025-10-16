# Black Powder Army Builder - Complete Setup Guide

## Step 1: Run the Application

```bash
dotnet run
```

The application will start at `https://localhost:5001` (or similar port shown in console).

## Step 2: Access Umbraco Backoffice

1. Navigate to `https://localhost:5001/umbraco`
2. Complete the initial Umbraco setup if this is the first run
3. Create an admin account and log in

## Step 3: Create Document Types

You need to create three document types manually in the Umbraco backoffice.

### Document Type 1: Black Powder Home

1. Go to **Settings** → **Document Types**
2. Click **Create** → **Document Type**
3. Set Name: `Black Powder Home`
4. Set Alias: `blackPowderHome`
5. Set Icon: Search for "home" icon
6. Check **Allow as root**
7. Add Tab: **Content**
8. Add Properties:
   - **Page Title**
     - Alias: `pageTitle`
     - Data Type: Textstring
     - Mandatory: Yes
   - **Page Description**
     - Alias: `pageDescription`
     - Data Type: Textarea
9. Click **Save**

### Document Type 2: Black Powder Unit

1. Go to **Settings** → **Document Types**
2. Click **Create** → **Document Type**
3. Set Name: `Black Powder Unit`
4. Set Alias: `blackPowderUnit`
5. Set Icon: Search for "medal" icon
6. **Uncheck** "Allow as root"

7. Add Tab: **General**
8. Add Properties to General tab:
   - **Unit Name**
     - Alias: `unitName`
     - Data Type: Textstring
     - Mandatory: Yes
   - **Unit Type**
     - Alias: `unitType`
     - Data Type: Textstring
     - Mandatory: Yes
     - Description: "Infantry, Cavalry, Artillery, etc."
   - **Unit Size**
     - Alias: `unitSize`
     - Data Type: Textstring
     - Mandatory: Yes
     - Description: "Tiny, Small, Standard, Large, Very Large"
   - **Points Cost**
     - Alias: `pointsCost`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Description**
     - Alias: `description`
     - Data Type: Textarea

9. Add Tab: **Combat Stats**
10. Add Properties to Combat Stats tab:
   - **Clash**
     - Alias: `clash`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Sustained Fire**
     - Alias: `sustainedFire`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Short Range Fire**
     - Alias: `shortRangeFire`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Long Range Fire**
     - Alias: `longRangeFire`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Morale Save**
     - Alias: `moraleSave`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Stamina**
     - Alias: `stamina`
     - Data Type: Numeric
     - Mandatory: Yes

11. Add Tab: **Special Rules**
12. Add Properties to Special Rules tab:
    - **Special Rules**
      - Alias: `specialRules`
      - Data Type: Textarea

13. Click **Save**

### Document Type 3: Black Powder Army

1. Go to **Settings** → **Document Types**
2. Click **Create** → **Document Type**
3. Set Name: `Black Powder Army`
4. Set Alias: `blackPowderArmy`
5. Set Icon: Search for "flag" icon
6. **Uncheck** "Allow as root"

7. Add Tab: **Army Details**
8. Add Properties:
   - **Army Name**
     - Alias: `armyName`
     - Data Type: Textstring
     - Mandatory: Yes
   - **Faction**
     - Alias: `faction`
     - Data Type: Textstring
     - Mandatory: Yes
     - Description: "E.g., British, French, Prussian"
   - **Period**
     - Alias: `period`
     - Data Type: Textstring
     - Description: "E.g., Napoleonic Wars, American Civil War"
   - **Target Points**
     - Alias: `targetPoints`
     - Data Type: Numeric
     - Mandatory: Yes
   - **Army Units (JSON)**
     - Alias: `armyUnits`
     - Data Type: Textarea
     - Description: "JSON data containing the units (managed by army builder)"

9. Click **Save**

### Step 4: Set Allowed Child Content Types

1. Go back to **Black Powder Home** document type
2. Go to the **Structure** tab
3. Under **Allowed child node types**, add:
   - Black Powder Unit
   - Black Powder Army
4. Click **Save**

## Step 5: Create Template

1. Go to **Settings** → **Templates**
2. Click **Create** → **New Template**
3. Set Name: `BlackPowderHome`
4. Leave the template empty (the view already has Layout = null)
5. Click **Save**

6. Go back to **Settings** → **Document Types** → **Black Powder Home**
7. Go to the **Settings** tab
8. Under **Allowed templates**, add `BlackPowderHome`
9. Set it as the **Default template**
10. Click **Save**

## Step 6: Create the Home Page

1. Go to **Content**
2. Click **Create**
3. Select **Black Powder Home**
4. Fill in:
   - Name: `Home`
   - Page Title: `Black Powder Army Builder`
   - Page Description: `Build and manage your Black Powder armies and units`
5. Click **Save and Publish**

## Step 7: Add Sample Units

Now add some units to test with. Under the Home page:

### Example 1: British Line Infantry

1. Click **Create** under Home → Select **Black Powder Unit**
2. Fill in:
   - Name: `British Line Infantry`
   - **General Tab:**
     - Unit Name: `British Line Infantry`
     - Unit Type: `Infantry`
     - Unit Size: `Standard`
     - Points Cost: `100`
     - Description: `Standard British infantry of the line, reliable and disciplined`
   - **Combat Stats Tab:**
     - Clash: `7`
     - Sustained Fire: `3`
     - Short Range Fire: `3`
     - Long Range Fire: `2`
     - Morale Save: `4`
     - Stamina: `3`
   - **Special Rules Tab:**
     - Special Rules: `Steady Line, First Fire`
3. Click **Save and Publish**

### Example 2: French Dragoons

1. Click **Create** under Home → Select **Black Powder Unit**
2. Fill in:
   - Name: `French Dragoons`
   - **General Tab:**
     - Unit Name: `French Dragoons`
     - Unit Type: `Cavalry`
     - Unit Size: `Standard`
     - Points Cost: `120`
     - Description: `Versatile mounted infantry capable of fighting on horse or foot`
   - **Combat Stats Tab:**
     - Clash: `8`
     - Sustained Fire: `2`
     - Short Range Fire: `2`
     - Long Range Fire: `0`
     - Morale Save: `4`
     - Stamina: `3`
   - **Special Rules Tab:**
     - Special Rules: `Marauders, Mounted Infantry`
3. Click **Save and Publish**

### Example 3: Prussian Artillery

1. Click **Create** under Home → Select **Black Powder Unit**
2. Fill in:
   - Name: `Prussian 6-pounder`
   - **General Tab:**
     - Unit Name: `Prussian 6-pounder Battery`
     - Unit Type: `Artillery`
     - Unit Size: `Tiny`
     - Points Cost: `60`
     - Description: `Light artillery battery with 6-pounder guns`
   - **Combat Stats Tab:**
     - Clash: `4`
     - Sustained Fire: `1`
     - Short Range Fire: `2`
     - Long Range Fire: `2`
     - Morale Save: `5`
     - Stamina: `1`
   - **Special Rules Tab:**
     - Special Rules: `Artillery, Canister`
3. Click **Save and Publish**

## Step 8: View the Site

1. Open a new browser tab
2. Navigate to `https://localhost:5001` (or your port)
3. You should see the Black Powder Army Builder with:
   - **Units Library** tab showing all your units
   - **My Armies** tab (empty for now)
   - **Army Builder** tab for creating armies

## Step 9: Build an Army

1. Click on the **Army Builder** tab
2. Fill in your army details:
   - Army Name: e.g., "My Napoleonic British Army"
   - Faction: e.g., "British"
   - Period: e.g., "Napoleonic Wars"
   - Target Points: e.g., 1000
3. Click on units from the left panel to add them
4. Adjust quantities as needed
5. Watch the points total update in real-time
6. Click **Save Army** when done
7. Check the **My Armies** tab to see your saved army

## Troubleshooting

### Units not showing in Army Builder
- Make sure units are **Published** (not just Saved)
- Check browser console for JavaScript errors
- Verify the API endpoint works: `/umbraco/api/armybuilder/getunits`

### Cannot create content
- Check that allowed child types are set correctly
- Ensure you're logged into the backoffice
- Verify document type aliases match exactly

### Template not working
- Make sure template is assigned to document type
- Verify the template name matches the view file name
- Check that the view file exists at `Views/BlackPowderHome.cshtml`

## Next Steps

- Add more units based on your Black Powder rulebook
- Create different armies for different factions
- Customize the styling in the view file
- Add more features like army validation, printing, etc.

## Reference: Black Powder Stats

For reference when creating units:

### Unit Types
- Infantry
- Cavalry
- Artillery
- LightCavalry
- HeavyCavalry
- Dragoons
- LightInfantry
- HeavyInfantry

### Unit Sizes
- Tiny (small guns, skirmishers)
- Small (reduced strength)
- Standard (normal)
- Large (oversized)
- Very Large (massive formations)

### Typical Stat Ranges
- **Clash**: 4-9 (hand-to-hand combat)
- **Sustained Fire**: 1-3 (multiple shots)
- **Short Range Fire**: 1-3 (0-6" range)
- **Long Range Fire**: 0-3 (6-18" range)
- **Morale Save**: 3-6 (lower is better, e.g., 4+)
- **Stamina**: 1-4 (hits unit can take)

### Common Special Rules
- Steady Line
- First Fire
- Marauders
- Mounted Infantry
- Artillery
- Canister
- Elite 4+
- Stubborn
- Ferocious Charge
- And many more...
