# MSc. in Devops - Continuous Software Delivery
## Continious Assessment 1
### Build & Release Pipeline for a Blood Pressure Application
| Name | Student ID | Email Address |
| ---- | ---------- | ------------- |
| Declan Smyth | x00151293 | declan.smyth@gmail.com |
---
# Implementation of Build & Release Pipeline
## Introduction
The blood pressure calculator application is a .Net Razor application based on Microsoft .Net Core 2.1. It provides a basic calculation of your blood pressure category. The application is developed using the following tools:
* Microsoft Visual Studio 2017
* Azure DevOps
* Microsoft Azure platform for hosting of the website

## Design
### Source Code Management
The blood pressure application is stored in a GIT based source code management system from GITHub. GIT a lightweight but powerful decentralised SCM. All code for the project can be found in the [msc-csdca1 repository](https://github.com/declan-smyth/msc-csdca1)

### Build Pipeline
Build services are provided by Azure DevOps. This suite of tools provide Agents and Task for building of applications across multiple platforms.  The build pipeline implements the following funcationality:
1. Initalization & Source Code Download
2. Application Compile
3. Unit Test Execution
4. Code Quality Analysis & Publication
5. Application Publication
6. Build Artifact Publication
7. Agent Cleanup

All builds in this pipeline are run on Microsoft Windows based servers in the VS 2017 Host pool 
(**TODO-ADD Image of default pool**). 
The pipeline is designed to build and unit test the application on a single host. It was design using the GUI pipeline development interface provided by the Azure Devops environment. A yaml based pipeline could also be used and this could be commited to the repository and updated inline with the code deliveries

### Release Pipeline
The release pipeline is developed in Azure DevOps. The release pipeline is linked to the Build Pipeline with artifcates being made available to the release after a successful build has completed. 
The release pipeline is comprised of the following stages:
1. Development
2. Acceptance Test
3. Staging & Performance Test
4. Production
#### Azure Platform 
The Azure platform is used for hosting the web application. The platform provides Web Application resource that will make the application available via a named url. The url for this application is  https://bpcalculator-dev-as.azurewebsites.net

**TODO** Add Screen shot of Azure Web App setup

The Azure Platform, also provides deployment slots that are used in the release pipeline to test the web application before it goes into produced.  In this pipeline we deploy a build into the following slots:
* DEV -> Used for testing of the application in a development environment
* TEST -> Used for acceptance testing 
* STAGE -> Used for performance testing of the application

The production slot is main application web URL, we use a slot swap to move from Stage into Production as part of the release pipline

#### Stage: Development

#### Stage: Acceptance Test

#### Stage: Staging & Performance Test

#### Stage: Production




---
# New Features
To enhance the application the following updates were made to the application
* Blood Pressure Categories at the High end of the scale were made more fine grain. 
These changes are in-line with information from the American Hearth Foundation, where there are additional categories for High Blood Pressure. The additional categories are
  * High Blood Pressure - Stage 1
  * High Blood Pressure - Stage 2
  * Hypertensive Crisis
* An improvement to the display of the calcualtion is placed on screen to improved the overall user experience and highlight the result
* Two new alert messages have been added to provide additional information to the user
  * Warning message for people with High Blood Pressure. This is to encourage them to seek improvements to their lifestyle
  * Danger message to alert the user that the information indicates a medical emergency. This is displayed in the case of Low Pressure and Hypertensive Crisis


---