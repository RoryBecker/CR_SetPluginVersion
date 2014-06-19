CR_SetPluginVersion
===================

This simple plugin adds a new item to the Project context menu of a plugin project.

The new '**Set Plugin Version**' prompts you for a new version number which is then written to 3 locations.

 - AssemblyInfo.cs 
   - AssemblyVersion
   - AssemblyFileVersion
 - source.extension.vsixmanifest
   - Version Element

