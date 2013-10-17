# Fosol.NuGetWrapper
Fosol.NuGetWrapper provides a way for any project to automatically build its own NuGet package.

If you have a project that you would like to convert into a NuGet package install Fosol.NuGetWrapper and compile your project.
MSBuild will automatically create a 'Package' folder within your build target folder which will contain your NuGet package.

The default package created for your project will only contain your output target.
In the case of a 'Library' project the package will contain the appropriate assembly ([projectName.dll]).
In the case of an executable project the package will contain the appropriate compiled application.

You will most likely need to manually update the default configuration so that other files are included in your package.
This requires that you update two files; first update the "Fosol.NuGetWrapper.csproj" configuration file, secondly update the "[projectName].nuspec" file to include additional files and information.
Both these files should reside in your project root folder.
Refer to the Configuration section below before updating the "[projectName].nuspec" configuraiton file.
Refer to the Configuration section below before updating the "Fosol.NuGetWrapper.csproj" configuration file. 

# Getting Started
It's as simple as installing the Fosol.NuGetWrapper package and building your project.

# Introduction
After installing Fosol.NuGetWrapper you can build your project and it will generate a NuGet package in the targeted output folder.

The process Fosol.NuGetWrapper follows to create your package is as follows;
1. Creates a 'Package' folder in the target output directory
2. Creates appropriate folders within the 'Package' folder (i.e. content, lib and tools)
3. Copy files from target output folder into the 'Package' folder if you have configured them to be included
4. Generate a nuspec file for the package based on your default "[projectName].nuspec" configuration file.  This uses the XSLT transform feature.
5. Build a NuGet package and place it in the 'Package' folder

### Package Manager Console Example
1. Open your solution/project in Visual Studio
2. Open the Package Manager Console
3. PM> Install-Package Fosol.NuGetWrapper

### Library Package Manager Tool
1. Open your solution/project in Visual Studio
2. In the "Tools" menu open the "Library Package Manager", then "Manage NuGet Packages for Solution..."
3. Search for "Fosol.NuGetWrapper"
4. Click "Install"

Now build your project and it will create a package in your target output directory (i.e. \bin\debug\Package\)

# Configuration
There are two configuration files that need to be updated if the NuGet package you wish to create requires addition files and information.
The first file is a project file named "Fosol.NuGetWrapper.csproj". 
it provides a way to dynamically control what files are included in the build output.
It also provides a way to dynamically configure the nuspec configuration file.
The second file is the NuGet nuspect file named after your project "[projectName].nuspec".

## Project Configuration
The project configuration for the automated Fosol.NuGetWrapper allows you to edit the build information without being forced to unload your project and edit your own project file.
The "Fosol.NuGetWrapper.csproj" file is imported into your project when you install the Fosol.NuGetWrapper package.
This project configuration file provides a way to select what files are included in your package and also provides a way to dynamically update the nuspec file.

### PropertyGroup Section
The property group variables provide a way to control the build process; where files are copied and what values are used in the nuspec configuration.
    
    Variable Name			Default Value					Description    
    `````````````           `````````````                   ```````````
    NuGetExtensionsDir      $(SolutionDir).nuget\           Location of the installed Fosol.NuGetWrapper helper files.
    NuGetSpecXsltName       Fosol.NuGetWrapper.xslt         File used to transform the default nuspec file in your project.
    NuGetOutDir             $(OutDir)Package\               Location of output directory that will contain the files which will be included in your package.
    
    NuGetSpecId                                             The id of your NuGet package.  If no value specified it will use your nuspec file value.
    NuGetSpecVersion                                        The version of your NuGet package.  If no value specified it will use your nuspec file value.
    NuGetSpecTitle          $(ProjectName)                  The title of your NuGet package.  If no value specified it will use your nuspec file value.
    NuGetSpecAuthors                                        The authors of your NuGet package.  If no value specified it will use your nuspec file value.
    NuGetSpecOwners                                         The owners of your NuGet package.  If no value specified it will use your nuspec file value.
    NuGetSpecDescription                                    The description of your NuGet package.  If no value specified it will use your nuspec file value.
    
    
### ItemGroup Section
The item group variables provide a way to control what files are included in the NuGet package that will be built.
    
    Variable Name           Default Value                   Description
    `````````````           `````````````                   ```````````
    NuGetBuild												The file(s) that will be included in the package "build" folder.
    NuGetContent                                            The file(s) that will be included in the package "content" folder.
    NuGetLib                $(TargetPath)                   The file(s) that will be included in the package "lib" folder.
    NuGetTool               $(TargetPath)                   The file(s) that will be included in the package "tools" folder.


## NuSpec Configuration
Fosol.NuGetWrapper automatically generates a default nuspec configuration file for your project.
It will be given the same name as your project "[projectName].nuspec" and will reside in the root project folder.
This nuspec configuration file is the template which is used to generate the nuspec file that will be send to your package output folder when you build your project.
When updating this file you will need to remember that the file locations will be relative to package output folder (see NuGetOutDir).
Refer to <http://docs.nuget.org/docs/reference/nuspec-reference> for more information on the nuspec configuration file.