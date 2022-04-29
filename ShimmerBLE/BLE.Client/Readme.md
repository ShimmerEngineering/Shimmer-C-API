# Demo
Note the purpose of this demo is to give users an idea how to interact with the various components required to develop a succesful application which can leverage the verisense device. In order for the upload functionality of the demo to work you will require an S3 account and bucket for the binary files to be uploaded to. Assuming you have the file parser (lambda) set up on your S3 service, once the binary file is succesfully uploaded the binary file will be parsed into a csv file and placed in the same S3 bucket. To setup an S3 account with binary file parsing capabilities please contact us for further details. We have future plans to make use of the FWVersion, but for now this is just a placeholder.
```
{
	"S3CloudInfo": {
 	   "S3AccessKey" : "",
 	   "S3SecretKey" : "",
  	   "S3RegionName" : "",
   	   "S3BucketName" : "verisense-api-demo"
	},
	
	"FWVersion" : "1.2.72"    
}
```
To specify the S3 details for the demo app to use please place the json file in the localstate folder of your package, if you are not sure where the location is, just run the app , press the upload button, and you will be greeted with the following warning.
![image](https://user-images.githubusercontent.com/2862032/132637808-9c96e835-545c-4201-bfc9-8bdb05722764.png)
