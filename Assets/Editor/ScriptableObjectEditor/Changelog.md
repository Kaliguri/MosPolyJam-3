## Ver 1.2
- Fixed bug that wouldn't let you modify properties within an array or list:
    - ReorderableList drawer was at fault, resolved by handling data differently

- Improved Scriptable Object Inspector for better readability and workflow:
    - Bold object title
    - Rename button position changed, moved next to title
    - Delete button colour change and position change.

- Added Pagination.

- Added "Relocate to Packages" in SOE menu item:
    - Relocate to Packages will now automatically move the SOE to it's intended location within the project
    - Within Packages it can still be modified easily but will be out of the way and not appear in "Assets"
    - Done for cleaner operation experience (And my personal hatred for Asset Store Assets being unable to exist as packages by default)

- Added Settings Menu:
    - Toggle for Real-time vs Dirty object saving.
    - Toggle for Pagination.
    - Input for Pagination page size.

- More custom styles for "ScriptableEditorGUI":
    - Moved header and button styling to custom GUI skin.
    - Done for easy customisation of UI to improve UX.

- Improved Visuals of Scriptable Object Navigation Sidebar:
    - Custom button visuals
    - Right click an object rename or delete
    - Selected Scriptable Object now highlighted (Customisable)
    - Collapsable Sidebar for when the window is too small (Automatically collapses, expands with the menu icon located left of "Folder")

- Not Assembly Dependant:
    - Previous worked with default Unity assembly making it un-usable for more organised and bigger projects
    - Now sorts through all Assemblies including user made definitions.
    - Packages are now avaliable for folder filter. (Requires folder navigation to "[ProjectNameFolder]/Packages")

- TODO (For Future Updates):
    - Selecting [ProjectNameFolder] in Folder filter will display all assemblies loaded by Unity. Not just package specific and Asset specific definitions.
    - Scriptable Object create menu will be apart of the flow rather than a popup.
    - Creation Menu will include editor for the Scriptable Object class being created.
    - Draggable resizing Sidebar and Inspector
    - Renaming Scriptables can be done by double clicking "Name" field alongside a pencil icon to turn the field into a textfield. Will allow for easy renaming.
    - Optmisation for GUI drawing and data handling
    - Search Bar filter options (For searching for variables, data, and type)
    - Easier customisation through GUI skin.


## Ver 1.0 (Release Version):
- Create, Modify, and Delete ScriptableObjects within a centralized editor window.
- Select specific folders to look for objects within.
- Select the Type type of the Scriptable desired to be modified.
- Creation Menu for creating ScriptableObjects of Type type & for creating new ScriptableObject Types.
- Auto saves any modified changes.