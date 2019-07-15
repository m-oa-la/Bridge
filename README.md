# Bridge


## Git/GitHub conventions
1. Collaboration on Git/GitHub shall follow the GitFlow working model.

Resources:
- ["Gitflow Workflow" - Atlassian](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)

## ASP.NET conventions
### File and directory structure

#### View structure
1. If two or more views have a similar layout a layout view shall be used.
2. The layout view shall include rendering of script bundles and styles (@Scripts.Render() and @Styles.Render()). It shall also include rendering of script and style sections for the specific views (@RenderSection()). The rendering of the sections shall succeed the rendering of the scripts and styles in order to avoid problems with order dependency.
3. Each view that utilizes a layout view shall the view specific scripts in a script section (@section Scripts{}) and view specific styles in a styles section (@section Styles{}). This and point 2 makes sure that order dependency is preserved.

#### Bundling
1. Bundles shall be named with a "/bundles/..." prefix, e.g. "~/bundles/scripts/...", in order to avoid routing errors.
2. ScriptBundles shall be organized as follows:
- One common ScriptBundle containing scripts that are used across multiple modules.
- One module specific ScriptBundle containing scripts that are only used within that respective module.
3. StyleBundles are not utilized at the moment. Instead, specific .css files are rendered in layout views.

Resources:
- ["Bundling and Minification" - Microsoft](https://docs.microsoft.com/en-us/aspnet/mvc/overview/performance/bundling-and-minification)

