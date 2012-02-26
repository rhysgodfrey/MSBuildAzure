MSBuild Copy To Azure Blob Storage Task
=======================================

## Introduction ##

A MSBuild task to allow copying of files to Azure Blob Storage. To save unnecessary copying files are
only copied if the source files last write time is different to the one tagged against the blob in Azure.

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
</table>

## License ##

This library is released under the *FreeBSD License*, see **LICENSE.txt** for more information.

Blog: http://www.rhysgodfrey.co.uk
Twitter: @rhysgodfrey