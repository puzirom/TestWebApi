# TestWebApi 
Solution has been created in February 2019

Prototype of TestWebApi solution which demonstrates a work of REST WebApi service and its possible Client Web application. 
WebApi service provides following WebApi methods: 
1. User Authorization, login
2. User logout (stop game session if present)
2. Get a List of avalable games collections with pagination. 
3. Get a game collection by specific ID
4. Get a List of available games with pagination. 
5. Get a game by specific ID. 
6. Start specific game (start game session)
7. Stop specific game (stop game session). 

Notes: 
1. Client UI - ugly UI just for simple demonstration for send request. 
2. XML files with serialization/deserialization approach have been used instead of real database. 

How to build and run:

1. Into C:\Windows\System32\drivers\etc\hosts add following: 

127.0.0.120 service.test.local
127.0.0.121 client.test.local

2. Clone TestWebApi repository to some folder on local PC like: 
C:\Work\TestWebApi

3. Open the TestWebApi.sln in MS Visual Studio and build this solution

4. Into IIS add two sites (under Sites): 

  1) Site Name: TestWebApi.Service;
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

5. Run in browser http://client.test.local

Perform test requests via UI and in the browser Console window check results.
Login: customer_XX@mail.com (XX in [01..30]) password: qwe123