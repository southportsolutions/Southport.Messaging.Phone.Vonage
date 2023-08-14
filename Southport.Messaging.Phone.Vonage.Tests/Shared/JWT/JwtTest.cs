using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Southport.Messaging.Phone.Vonage.Shared.Jwt;
using Xunit;

namespace Southport.Messaging.Phone.Vonage.Tests.Shared.JWT;

public class JwtTest
{

    private string _mockAppId => Guid.NewGuid().ToString("D");

    private string _mockPKCS8 = @"-----BEGIN PRIVATE KEY-----
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC+YVmgWTT9btGG
yC6Tz0kYTrXwM1zts1InU2xEjzI9FiP9HVnjkaEKVNGotsOhGdCMIf+czAZG/roK
9gLvAvf8z3EFgMA81jMMEF6/vy8ThtyYbTR3JeR7InA07PUsEuhRKNNvmSz80nQv
y3gPSRet8iFBstuuzCvvfhOdA/1pYkuYtbfR//rsNmVWLfajqdm8Zjk0+omTFc+k
ZyTOp28I1w/1l9WtNpckl4uiSepB8Ione7HON2hgkwA0gAtjzD7/v5WCgxQepinj
KF7QMmQWnpDS7OheJsqk47cK9cVAbLLnYrK+NufdgbWgTDfWzljuRJL7wVbQazNR
FOJywGdZAgMBAAECggEAU4xzgdxEVghBKY5GapWodWNtkulnmeV9Y0bIF9pj6M0D
pDwyS52Id/k7PXhfqB6lGCp/e/dJtfRp/w9xwCSkYi1DY2/abZhvNJcmja3AYiho
PPiH8tYNTECTz5ZCDkKJ87wf9jTZumY9mRAJM2QXYT4GfEebAz4U9fh1cQ3kM0+u
hzFFXEMH/xHvHbYv3XayTkB4xXjp1K1NCV/y8TIcJmM2XPBN/3q+06GJHbZ0QxHY
uCRO90xwvH1NKVUsQVzPQQIieFHhdSmUFheJ9QdFWQqZgxyliXdEBsIdklpAdDwu
VdnKzlNMxO3sMgFbdMgHqmMYcO9c+bvz4mPNPTpuAQKBgQDvaytFvmOPv1uAq8pb
8wgGzadTH16ip9qATc5PHAtU6TFfwlHS5Z13KwHhGe51w3WO9FvOqQdOJ8up4S4U
tzTnjvnF9tW41viG5rWTq/paktF9CJmty1YVrQLi43EopjnDTwK+2C9UXSXMzhem
A8/rB2MzmZ1h2DNnCNO2nRmhQQKBgQDLkL5cIYvg3xjXlZgCA+U7m68gR34gLGKp
3R345Ab/0SD38w5fI0VvfhuHmB/ocSVUUOgoU67MUkVHnY9ZH2vxqOzfaXV1642e
JZ5c+nZpYA4sgsTeyTXu1UUl97i17ho9WDvGARZngINZMjcDZiPRdIg2oCxA3fg/
pzi1sS2oGQKBgQCVTFs7rrIfXdENuBMEq9UBiRUivJkjDVEwWVSh+HcIiDKF6INl
5FIBkgwl9ynAvhZ9AtyNTtKDZkWWthkqSeTv0TTowjgcf9GTLiNk5wXDnXKNaeOL
gRU5hx4ZpoNWOfIjXQ31PJKnJT8BLDOLDy2E/qJZ9x0xesTzJ4n+gpENAQKBgGzt
pj9soAoTt5pc7Tte1EJiW4LdXstelOkqbkhp1Kj9QjQL9svH9vbjN14GdESQjxYg
OSqjJO0mtPXOhQ9+tedZqm8eYoFYK67NmIFOcSCQCuWckDZa2yZTLy5S8Z9Aqv/a
gBnDKTb3WNHZAgEqnc4OGnmImkWXwahmFf17st+5AoGBAKT2g3CO9bbzxtCYExYy
RbXFqhDUg8ptYuN5H2ReUSZ0YjU0gQgCgcyKOS56kJXRFvHJzQEzAKgkRcKaCuAT
qEn7wmWsP/cwBJGCFNhyhz+QFj1GgFL8FT4jfqeK5MxGDEphQ8JLaJULBUgfQwst
5reLSUDOFeTx9XaaIJzxxzzz
-----END PRIVATE KEY-----
";


    [Fact]
    public void TestFullAcls()
    {
        var expected = @"{""paths"":{""/*/users/**"":{},""/*/conversations/**"":{},""/*/sessions/**"":{},""/*/devices/**"":{},""/*/image/**"":{},""/*/media/**"":{},""/*/applications/**"":{},""/*/push/**"":{},""/*/knocking/**"":{}}}";
        var acls = Acls.FullAcls();
        var json = JsonConvert.SerializeObject(acls);
        Assert.Equal(expected, json);
    }

    //[Fact]
    //public void GenerateTokenNoAcls()
    //{
    //    var appId = _mockAppId;
    //    var jwt = JwtGenerator.Generate(_mockPKCS8, appId);
    //    JToken acls;

    //    var decodedStr = JwtGenerator.Decode(jwt, _mockPKCS8);
    //    var decoded = JsonConvert.DeserializeObject<dynamic>(decodedStr);
    //    Assert.Equal(appId, decoded["application_id"].ToString());
    //    Assert.False(decoded.TryGetValue("acl", out acls));
    //}

    //[Fact]
    //public void GenerateTokenWithFullAcls()
    //{
    //    var appId = _mockAppId;
    //    var expected = JsonConvert.SerializeObject(Acls.FullAcls());
    //    //var expected = @"{""paths"":{""/*/users/**"":{},""/*/conversations/**"":{},""/*/sessions/**"":{},""/*/devices/**"":{},""/*/image/**"":{},""/*/media/**"":{},""/*/applications/**"":{},""/*/push/**"":{},""/*/knocking/**"":{}}}";
    //    var jwt = JwtGenerator.Generate(_mockPKCS8, appId, accessControls: Acls.FullAcls().Paths);
    //    var decoded = JsonConvert.DeserializeObject<JObject>(JwtGenerator.Decode(jwt, _mockPKCS8));
    //    Assert.Equal(appId, decoded["application_id"].ToString());
    //    Assert.Equal(expected, Regex.Replace(decoded["acls"].ToString(), @"\s+", ""));
    //}

    //[Fact]
    //public void TestSingleAcl()
    //{
    //    var appId = _mockAppId;
    //    var expected = @"{""paths"":{""/*/users/**"":{}}}";
    //    var acls = new Acls
    //    {
    //        Paths = new List<AclPath>
    //        {
    //            new AclPath
    //            {
    //                ApiVersion = "*", ResourceType = "users", Resource = "**", AccessLevels = new object()
    //            }
    //        }
    //    };
    //    var jwt = JwtGenerator.Generate(_mockPKCS8, appId, accessControls: acls.Paths);
    //    var decoded = JsonConvert.DeserializeObject<JObject>(JwtGenerator.Decode(jwt, _mockPKCS8));
    //    Assert.Equal(appId, decoded["application_id"].ToString());
    //    Assert.Equal(expected, Regex.Replace(decoded["acls"].ToString(), @"\s+", ""));
    //}

    //[Fact]
    //public void TestPkcs8KeyGeneration()
    //{
    //    var appId = _mockAppId;
    //    var jwt = JwtGenerator.Generate(_mockPKCS8, appId);
    //    var decoded = JsonConvert.DeserializeObject<JObject>(JwtGenerator.Decode(jwt, _mockPKCS8));
    //    Assert.Equal(appId, decoded["application_id"].ToString());
    //}

    //[Fact]
    //public void TestBadKey()
    //{
    //    var exception = Assert.Throws<ArgumentException>(() => JwtGenerator.Generate("badKey", _mockAppId));
    //    Assert.Equal("Invalid Private Key provided", exception.Message);
    //}

    [Fact]
    public void TestCache()
    {
        var appId = _mockAppId;
        var jwt = JwtGenerator.Generate(_mockPKCS8, appId, expiresInSeconds: 2);
        Task.Delay(2500).Wait();
        var jwt2 = JwtGenerator.Generate(_mockPKCS8, appId, expiresInSeconds: 2);
        Assert.NotEqual(jwt, jwt2);
    }
}