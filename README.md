# Bridge

## GitFlow
1. Collaboration on Git/GitHub shall follow the GitFlow working model.

Resources:
- ["Gitflow Workflow" - Atlassian]

## ASP.NET conventions
### File and directory structure

#### Bundling
1. Bundles shall be named with a "/bundles/..." prefix in order to avoid routing errors, e.g. "~/bundles/scripts/...".
2. ScriptBundles shall be named with a "/bundles/scripts/..." prefix and StyleBundles with a "/bundles/styles/..." prefix for clarity.
3. Bundles shall be organized as to minimize unnecessary import of files. The suggested convention is to make one StyleBundle and one ScriptBundle for each module.

Resources:
- ["Bundling and Minification" - Microsoft]
