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

Licensed under the [MIT license](LICENSE).

## About me ##

**Stuart Williams**

* I Cloud. I Code. 
* <a href="mailto:stuart.t.williams@outlook.com" target="_blank">stuart.t.williams@outlook.com</a> (e-mail)
* LinkedIn: <a href="http://lnkd.in/P35kVT" target="_blank">http://lnkd.in/P35kVT</a>
* YouTube: <a href="https://www.youtube.com/user/spookdejur1962/videos" target="_blank">https://www.youtube.com/user/spookdejur1962/videos</a> 
