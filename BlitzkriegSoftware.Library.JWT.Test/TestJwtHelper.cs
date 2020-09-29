using BlitzkriegSoftware.Library.JWT.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace BlitzkriegSoftware.Library.JWT.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestJwtHelper
    {
        #region "Boilerplate"

        private static TestContext testContext;

        [ClassInitialize]
        public static void InitClass(TestContext testContext)
        {
            TestJwtHelper.testContext = testContext;
        }

        #endregion

        #region "Helpers"

        public const string TestAudiance = "test";
        public const string TestIssuer = "self";

        public JwtIdentity MakeIdentity(string username)
        {
            var identity = new JwtIdentity(username);
            return identity;
        }

        public JwtHelper MakeHelper()
        {
            var secret = JwtHelper.MakeRandomSecret();
            var helper = new JwtHelper(secret);
            return helper;
        }


        public string MakeTestJwt(out JwtHelper helper)
        {
            helper = MakeHelper();

            var identity = MakeIdentity("user1");

            var rC = new Models.JwtCreateRequest()
            {
                Identity = identity,
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                Expires = new System.TimeSpan(0, 15, 0)
            };

            rC.Claims.Add(new Claim("claim1", "one"));
            rC.Claims.Add(new Claim("claim2", "two"));

            var jwt = helper.Create(rC);

            return jwt;
        }

        #endregion

        [TestMethod]
        [TestCategory("Unit")]
        public void TestBase64()
        {
            var expected = "Work 123 !@#$%^&*()_+.,/?";
            var base64 = JwtHelper.ToBase64(expected);
            var result = JwtHelper.FromBase64(base64);
            TestJwtHelper.testContext.WriteLine($"{expected}\n{base64}\n");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BadSecret1Create1()
        {
            var helper = new JwtHelper(null);

            var rC = new Models.JwtCreateRequest()
            {
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                Expires = new System.TimeSpan(0, 15, 0)
            };

            helper.Create(rC);
        }


        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadValidate1()
        {
            var helper = MakeHelper();
            helper.Validate(null);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBase64Bad1()
        {
            _ = JwtHelper.ToBase64("");
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBase64Bad2()
        {
            _ = JwtHelper.FromBase64(null);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestSimple1()
        {
            var jwt = MakeTestJwt(out JwtHelper helper);

            TestJwtHelper.testContext.WriteLine($"JWT: {jwt}");

            var vl = new Models.JwtValidationRequest()
            {
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                Token = jwt
            };

            TestJwtHelper.testContext.WriteLine($"{vl}");

            var result = helper.Validate(vl);

            Assert.IsTrue(result.IsValid);

            TestJwtHelper.testContext.WriteLine($"Name: {result.Identity.Name}");
            TestJwtHelper.testContext.WriteLine($"Issuer: {result.Issuer}");

            foreach (var c in result.Claims)
            {
                TestJwtHelper.testContext.WriteLine($"\tClaim: {c.Type}={c.Value}");
            }
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void TestSimple2()
        {
            var identity = new JwtIdentity("user2");

            var secret = JwtHelper.MakeRandomSecret(32);
            var helper = new JwtHelper(secret);

            var rC = new Models.JwtCreateRequest()
            {
                Identity = identity,
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                Expires = new System.TimeSpan(0, 15, 0)
            };

            var jwt = helper.Create(rC);

            TestJwtHelper.testContext.WriteLine($"JWT: {jwt}");

            var result = helper.Validate(new Models.JwtValidationRequest()
            {
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                Token = jwt
            });

            Assert.IsTrue(result.IsValid);

            TestJwtHelper.testContext.WriteLine($"Name: {result.Identity.Name}");
            TestJwtHelper.testContext.WriteLine($"Issuer: {result.Issuer}");

            foreach (var c in result.Claims)
            {
                TestJwtHelper.testContext.WriteLine($"\tClaim: {c.Type}={c.Value}");
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBadKey1()
        {
            var secret = JwtHelper.MakeRandomSecret(11);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBadKey2()
        {
            var secret = JwtHelper.MakeRandomSecret(-5);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBadCreate3()
        {
            var helper = MakeHelper();
            helper.Create(null);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBadRequestCreate1()
        {
            var identity = new JwtIdentity("user2");

            var secret = JwtHelper.MakeRandomSecret(32);
            var helper = new JwtHelper(secret);

            var rC = new Models.JwtCreateRequest()
            {
                Identity = identity,
                Audiance = TestAudiance,
                Expires = new System.TimeSpan(0, 0, 0)
            };

            var jwt = helper.Create(rC);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBadRequestCreate2()
        {
            var identity = new JwtIdentity("user2");

            var secret = JwtHelper.MakeRandomSecret(32);
            var helper = new JwtHelper(secret);

            var rC = new Models.JwtCreateRequest()
            {
                Identity = identity,
                Issuer = TestIssuer,
                Expires = new System.TimeSpan(0, 15, 0)
            };

            var jwt = helper.Create(rC);
        }


        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBadRequestCreate3()
        {
            var secret = JwtHelper.MakeRandomSecret(32);
            var helper = new JwtHelper(secret);

            var rC = new Models.JwtCreateRequest()
            {
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                Expires = new System.TimeSpan(0, 15, 0)
            };

            var jwt = helper.Create(rC);
        }


        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(Microsoft.IdentityModel.Tokens.SecurityTokenInvalidSignatureException))]
        public void TestBadValidate1()
        {
            var identity = new JwtIdentity("user1");

            var secret = JwtHelper.MakeRandomSecret();
            var helper = new JwtHelper(secret);

            string audiance = "test";
            string issuer = "self";

            string jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            TestJwtHelper.testContext.WriteLine($"JWT: {jwt}");

            var result = helper.Validate(new Models.JwtValidationRequest()
            {
                Audiance = audiance,
                Issuer = issuer,
                Token = jwt
            });

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBadValidateRequest1()
        {
            var token = MakeTestJwt(out JwtHelper helper);

            var vr = new JwtValidationRequest()
            {
                //Audiance = TestAudiance,
                Issuer = TestIssuer,
                Token = token
            };

            _ = helper.Validate(vr);
        }


        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBadValidateRequest2()
        {
            var helper = MakeHelper();

            var vr = new JwtValidationRequest()
            {
                Audiance = TestAudiance,
                Issuer = TestIssuer,
                // Token = token
            };

            _ = helper.Validate(vr);
        }
    }
}
