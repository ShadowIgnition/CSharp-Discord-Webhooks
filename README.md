# Unity Package Template Project

This repository serves as a template for creating custom Unity packages. It includes organized folders and assembly definitions for both runtime and editor scripts, streamlining the process of creating and managing Unity packages.

## Usage Instructions

To create a new Unity project using this template, follow these steps:

1. **Set Up A New Project**
   - Open Unity Hub and create a new project.
   - Choose a location for your project and select the desired template.

2. **Clone the Repository and Organize Your Scripts**
   - Clone this repository and when working on your package, ensure that your Git repository is set up within the `Assets` folder, like so: `Assets > YourPackageName > Runtime` or `Assets > YourPackageName > Editor`.

3. **Update `package.json`**
   - Navigate to `Assets > YourPackageName` and open the `package.json` file.
   - Modify the details to match your package's information, such as name, version, and description [see more](#package.json-template).

4. **Customize and Develop**
   - Begin adding your own scripts and assets under the `Runtime` and `Editor` folders. This template provides a clear structure to help you organize your codebase.

## Folder Structure

The repository is structured as follows:

- **Assets**
  - **YourPackageName**
    - Contains the `package.json` file.
    - **Runtime**
      - Contains scripts that are used during gameplay.
    - **Editor**
      - Contains scripts used for editor tools, inspectors, and custom editors.

##  Package.json template:

This file is utilized by Unity's Package Manager to identify and handle packages, streamlining their installation, updates, and removal within Unity projects.

The following is a `package.json` template:
```
{
  "name": "com.yourcompany.yourpackagename",
  "displayName": "Your Package Name",
  "description": "Description of your package.",
  "version": "1.0.0",
  "unity": "2019.4",
  "license": "MIT",
  "repository": {
    "type": "git",
    "url": "git+https://github.com/yourusername/Your-Repo.git"
  },
  "author": {
    "name": "Your Name",
    "email": "your.email@example.com",
    "url": "https://github.com/yourusername"
  },
  "dependencies": {}
}
```

To fill in this `package.json` template:

1. **Edit the "name" field**:
   - Replace `"com.yourcompany.yourpackagename"` with a unique identifier for your package. It's often in the format of `com.yourcompany.packagename`.

2. **Edit the "displayName" field**:
   - Replace `"Your Package Name"` with a user-friendly name for your package. This is what users will see.

3. **Edit the "description" field**:
   - Provide a brief, clear description of what your package does.

4. **Update the "version" field**:
   - Set the version of your package using semantic versioning (major.minor.patch).

5. **Specify Unity compatibility**:
   - Update the `"unity"` field with the version of Unity your package is compatible with.

6. **Choose a license**:
   - If you want to use a different license, replace `"MIT"` with the appropriate license.

7. **Add repository details**:
   - Replace the Git repository URL in the `"url"` field with your own repository's URL.

8. **Update author information**:
   - Replace `"Your Name"`, `"your.email@example.com"`, and `"https://github.com/yourusername"` with your own name, email, and GitHub profile URL.

9. **Add dependencies** (if needed):
    - Under `"dependencies"`, list any external packages or libraries your package relies on.
  
## Add your Unity Project via Package Manager

To add your package to your Unity project via the Package Manager, follow these steps:

1. Open your Unity project.
2. Open the Package Manager window by going to `Window > Package Manager`.
3. Click on the `+` button in the top left corner of the Package Manager window.
4. Select "Add package from git URL...".
5. In the text field that appears, enter the URL of your repository, adding `.git` to the end of the url. (Example: `https://github.com/ShadowIgnition/Unity-Package-Template.git`)
6. Click the `Add` button.

Your package will now be added to your project!
