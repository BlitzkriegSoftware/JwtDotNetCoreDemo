# JwtDotNetCoreDemo

A small demo of creating and consuming JWT tokens in C#

Some helper methods added.

## Example

```csharp

// Create 
var identity = new JwtIdentity("user2");

var secret = JwtHelper.MakeRandomSecret(256);
var helper = new JwtHelper(secret);

var rC = new Models.JwtCreateRequest()
{
	Identity = identity,
	Audiance = TestAudiance,
	Issuer = TestIssuer,
	Expires = new System.TimeSpan(0, 15, 0)
};

var jwt = helper.Create(rC);

// Validate

var result = helper.Validate(new Models.JwtValidationRequest()
{
	Audiance = TestAudiance,
	Issuer = TestIssuer,
	Token = jwt
});



```


## About

Stuart Williams

* Cloud/DevOps Practice Lead

* Magenic Technologies Inc.
* Office of the CTO

* [e-mail](stuartw@magenic.com)

* [Blog](https://blitzkriegsoftware.azurewebsites.net/Blog)

* [LinkedIn](http://lnkd.in/P35kVT)

* [YouTube](https://www.youtube.com/user/spookdejur1962/videos)