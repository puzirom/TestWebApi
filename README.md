# TestWebApi
TestWebApi solution demonstrates a work of REST WebApi service and its Client Web application. 
Solution has been created in February 2019

How to build and run:

1. Into C:\Windows\System32\drivers\etc\hosts add following: 
127.0.0.120 service.test.local
127.0.0.121 client.test.local

2. Clone TestWebApi repository to some folder on local PC like: 
C:\Work\TestWebApi

3. Open the TestWebApi.sln in MS Visual Studio and build this solution

4. Into IIS add two sites (under Sites): 

  1) Site Name: TestWebApi.Service
     Path: C:\Work\TestWebApi\Service; 
     Type: Http; 
     IP address: 127.0.0.120; 
     Host Name: service.test.local; 
     (TestWebApi.Service -> "Edit permission" -> "Security" -> Everyone -> Full Control)

  2) Site Name: TestWebApi.Client;
     Path: C:\Work\TestWebApi\Client; 
     Type: Http; 
     IP address: 127.0.0.121; 
     Host Name: client.test.local

5. Run in browser http://client.test.local, perform tests and in the Console window check results
Login: customer_XX@mail.com (XX in [01..30]) password: qwe123