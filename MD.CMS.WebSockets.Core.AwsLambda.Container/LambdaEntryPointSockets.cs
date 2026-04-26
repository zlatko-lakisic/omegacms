using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace MD.CMS.WS.Core.AwsLambda.Container
{
    public class LambdaEntryPointSockets : MD.CMS.AwsLambda.Container.Core.LambdaEntryPointSockets
    {
    }
}
