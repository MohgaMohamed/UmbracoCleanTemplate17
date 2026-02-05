# Ministry Of Health & People - Website Portal
**Author**: Muhammad Ayman Salahuddin

# Getting Started
The solution is based on Umbraco CMS v13, with uSync Package. Installation Requirements:
1.	The Solution requires a MSSQL Database named "MOHP.LocalDev"
2.	The Installation Process runs automatically unattended 
3.	Admin User is generated on Fresh installations 
	1. Username: Administrator@asset.com.eg
	1. Password: Admin@123456
4.	uSync import is ran Automatically on Startup, no further configurations are needed


# Development Guidlines
This section highlights some important naming & modeling conventions regarding Development

## Common Definitions
### Document Types
#### Page Type
A page that is only meant to be displayed on a website such as "About Us", "Contact Us" and so on..
- A Page type MUST define an Alias in the following Pattern ":DocumentName:Page"
- A Page type MUST define a Render Controller with the following name ":DocumentAlias:RenderController"
- A Page type MUST define a View Template named as its "Alias"
- A Page type MUST inherit the following Compositions
	- ISEOContentComposition
	- IPageComposition
	- ISiteMapContentComposition

#### Listing Page Type
A Page that displays a list of its children in a listing manner
- A Listing Page type MUST define an Alias in the following Pattern ":DocumentName:ListingPage"
- A Listing Page type MUST define a Render Controller with the following name: ":DocumentAlias:RenderController"
- A Listing Page type MUST define a Child of Type "Entity"
- A Listing Page type MUST define a View Template named as its "Alias"
- A Listing Page type MUST inherit the following Compositions
	- ISEOContentComposition
	- IListingPageComposition --> IPageComposition
	- ISiteMapContentComposition
	- IPaginatedListingComposition (optional) if pagination exists 

#### Entity Type
An Entity represents a Core Business Domain Concept, such as "News", "Event", "BlogPost", "Product"
- An Entity type MUST define an Alias in the following Pattern ":DocumentName:Entity"
- An Entity type CAN define a Render Controller with the following name: ":DocumentAlias:RenderController" (CAN = optional)
- An Entity type CAN define a Child of Type "Entity" (CAN = optional)
- An Entity type CAN define a View Template named after its "Alias" (CAN = optional)
- An Entity type MUST inherit the following Compositions
	- ISEOContentComposition
	- IContentEntityComposition (Marker Interface)
	- ISiteMapContentComposition

#### Composition Type
A Composition can represents a Reusable set of Properties or a Common Concept between Other Types
- A Composition type MUST define an Alias in the following Pattern ":DocumentName:Composition"
- A Composition type Can inherit other Compositions

#### Element Type
An Element Type is used to represent a value type that is used inside other Types in a Blocklist or a BlockGrid
- An Element type MUST define an Alias in the following Pattern ":DocumentName:Element"
- An Element type CAN define a PartialView named as its "Alias" (CAN = optional)
	- Partial view path MUST follow the pattern "Partials/Elements/:ContainingType:/:ElementAlias:.cshtml"
- An Element type CAN inherit the Compositions

#### Container Type
A Container represents a Tree Node that groups "Content" of other Types undes a single node
- An Entity type MUST define an Alias in the following Pattern ":DocumentName:Container"
- An Entity type CAN define a Render Controller with the following name: ":DocumentAlias:RenderController" (CAN = optional)
	- ContainerRenderController can be used to redirect to the its first child 
- An Entity type CANNOT define a View Template
- An Entity type CAN define Children of other Types
- An Entity type MUST inherit the following Compositions
	- IContainerComposition (Marker Interface)


#### Lookup Type
A lookup type is a simple type that is used to classify "Entity" Types, examples include "Category", "Topic", etc..
- A Lookup type MUST define an Alias in the following Pattern ":DocumentName:Lookup"
- A Lookup type MUST ONLY as a Picker Property be used in Entity Types
- A Lookup type MUST inherit the following Compositions
	- ILookupComposition


## Common Naming Conventions
### Localization
- Document Name: #:area:_DocumentName, where :area: is the Document Name 
- Element Name: #:area:_ElementName, where :area: is the Element Name 
- Composition Name: #:area:_CompositionName, where :area: is the Composition Name 
- Property Name: #:area:_:alias:PropertyName, where :area: is the Document Name & :alias: is the Property alias
- Property Description: #:area:_:alias:PropertyDescription, where :area: is the Document Name & :alias: is the Property alias

**If the document name is "Example" with a property named "Test" the convensions will be:**
- Document Name: #Example_DocumentName
- Element Name: #Example_ElementName, 
- Composition Name: #Example_CompositionName
- Property Name: #Example_TestPropertyName
- Property Description: #Example_TestPropertyDescription

**Common Areas include:**
- **Validation**: #Validation_:criteria:, where :criteria: can be something such as "Required", "TooShort"
	- examples: #Validation_Required, #Validation_TooShort, etc...
- **Common**: #Common_:value:, where :value: can be something such as "Content", "Add", "Search"
	- examples: #Common_Content, #Common_Add, #Common_Search etc...
- **Tabs**: #Tabs_:value:, where :value: can be something such as "Content", "Settings", "SEO"
	- examples: #Tabs_Content, #Tabs_Settings, #Tabs_SEO etc...


### Document Modeling
- If the Document represents a Page: name must be (alias)Page
- If the Document represents a Listing Page: name must be (alias)ListingPage
- If the Document represents an Entity has a Details Page: name must be (alias)Entity
- If the Document is a Composition: name must be (alias)Composition
- If the Document is an Element: name must be (alias)Element
- If the Document represents a Container: name must be (alias)Container
- If the Document represents a Lookup: name must be (alias)Lookup





If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)