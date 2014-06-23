MSBuild Copy To Azure Blob Storage Task
=======================================

[![Build status](https://ci.appveyor.com/api/projects/status/wcwgtv6bwu88clid)](https://ci.appveyor.com/project/rhysgodfrey/msbuildazure)

## Introduction ##

A MSBuild task to allow copying of files to Azure Blob Storage. To save unnecessary copying files are
only copied if the source files last write time is different to the one tagged against the blob in Azure.


## Download ##
The latest version of the library can be downloaded from the [Latest Release Page](https://github.com/rhysgodfrey/MSBuildAzure/releases/latest).

## Usage ##

Include the DLL in your MSBuild project

	<UsingTask TaskName="CopyToAzureBlobStorageTask" AssemblyFile="..\PATH-TO-DLL\RhysG.MSBuild.Azure.dll"/>

Create an item group referencing the files you want to copy to Azure Blob storage. For example to copy
all the javascript files from the *scripts* directory:

	<ItemGroup>
		<Scripts Include="..\PATH-TO-FILES\scripts\*.js" />
    </ItemGroup>

Then call the *CopyToAzureBlobStorageTask* this takes several parameters described below. To copy the files
selected above to a container named *scripts*:

	<CopyToAzureBlobStorageTask ContainerName="scripts" ContentType="text/javascript" ConnectionString="YOUR-AZURE-CONNECTION-STRING" Files="@(Scripts)" />

### Parameters ###

<table>
	<tr>
		<th>Parameter</th>
		<th>Description</th>
	</tr>
	<tr>
		<td>ContainerName</td>
		<td>The name of the Azure Blob container to copy the files to. If this does not exist it will be created</td>
	</tr>
	<tr>
		<td>ConnectionString</td>
		<td>The Azure connection string to use</td>
	</tr>
	<tr>
		<td>Files</td>
		<td>The files to copy</td>
	</tr>
	<tr>
		<td>ContentType</td>
		<td><em>Optional</em> The content type for Azure to serve the files as</td>
	</tr>
	<tr>
		<td>DestinationFolder</td>
		<td><em>Optional</em> The destination folder to upload the file(s)</td>
	</tr>
</table>

## Contributors ##

Thanks to [stephenwelsh](https://github.com/stephenwelsh) and [swythan](https://github.com/swythan) for the updates to this project. If you have an enhancement or fix, please fork the project and submit a pull request! Details of past pull requests can be found [here](https://github.com/rhysgodfrey/MSBuildAzure/pulls?direction=desc&page=1&sort=created&state=closed)

## License ##

This library is released under the *FreeBSD License*, see **LICENSE.txt** for more information.

Blog: http://www.rhysgodfrey.co.uk

Twitter: @rhysgodfrey
