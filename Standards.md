*<- [Back to Repository](https://github.com/necronDOW/BoidWars)*

# Standards
This document outlines naming conventions and coding standards for this project.

## Asset Standards
To maintain consistency, please follow the Asset naming convention of "ExampleName_Type" (e.g. ExampleName_Material, ExampleName_Model) with the following exceptions:
* **Audio files**: Replace "Type" with the audio type (e.g. MenuMusic_Music, Shoot_SFX).
* **Scripts**: Do not include the Type unless it is an Editor Script (e.g. Waypoints, Waypoints_Editor).
* **Prefabs**: Do not include the Type (e.g. Player, Enemy).

Ensure that assets are added to a folder which associates with the basic type. If a folder doesn't exist, create one in a place which you deem appropriate.

## Coding Standards
Ensure to conform to the [Microsoft C# Conventions](https://msdn.microsoft.com/en-us/library/ff926074.aspx). Pay particular attention to the following standards to maintain consistency:
* Use camel-case for variables, classes and functions.
* Ensure that function and class names begin in uppercase, whereas variable names begin in lowercase.
* Name private variables with an underscore prefix (e.g. private var _someVar).
* Ensure that you always prefix the accessor to variables (e.g. DO: private var _someVar, DONT: var_someVar);
* DO NOT use shorthand statements, always include the braces regardless of content.

*<- [Back to Repository](https://github.com/necronDOW/BoidWars)*
