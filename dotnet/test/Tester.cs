using NUnit.Framework;
using COSSTS;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture()]
    public class Tests
    {

        Dictionary<string, object> values;

        [SetUp]
        public void Setup()
        {
            values = new Dictionary<string, object>();

            string bucket = "examplebucket-1253653367";  // 您的 bucket
            string region = "ap-guangzhou";  // bucket 所在区域
            string allowPrefix = "exampleobject"; // 这里改成允许的路径前缀，可以根据自己网站的用户登录态判断允许上传的具体路径，例子： a.jpg 或者 a/* 或者 * (使用通配符*存在重大安全风险, 请谨慎评估使用)
            string[] allowActions = new string[] {  // 允许的操作范围，这里以上传操作为例
                "name/cos:PutObject",
                "name/cos:PostObject",
                "name/cos:InitiateMultipartUpload",
                "name/cos:ListMultipartUploads",
                "name/cos:ListParts",
                "name/cos:UploadPart",
                "name/cos:CompleteMultipartUpload"
            };
            string secretId = Environment.GetEnvironmentVariable("COS_KEY"); // 云 API 密钥 Id
            string secretKey = Environment.GetEnvironmentVariable("COS_SECRET"); // 云 API 密钥 Key

            values.Add("bucket", bucket);
            values.Add("region", region);
            values.Add("allowPrefix", allowPrefix);
            values.Add("allowActions", allowActions);
            values.Add("durationSeconds", 1800);

            values.Add("secretId", secretId);
            values.Add("secretKey", secretKey);
        }

        [Test]
        public void TestSTSClient()
        {
            string credential = STSClient.genCredential(values);
            TestContext.Progress.WriteLine(credential);
            Assert.NotNull(credential);
        }

        [Test]
        public void TestPolicy()
        {
            
            string policy = STSClient.getPolicy(
                (string) values["region"], (string) values["bucket"], 
                (string) values["allowPrefix"], (string[]) values["allowActions"]);
            TestContext.Progress.WriteLine(policy);
            Assert.NotNull(policy);
        }
    }
}