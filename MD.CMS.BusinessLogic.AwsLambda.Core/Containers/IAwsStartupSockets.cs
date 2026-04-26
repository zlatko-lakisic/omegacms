using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Collections;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.AwsLambda.Core.Containers
{
    /// <summary>
    /// AwsStartupSockets Interface
    /// </summary>
    public interface IAwsStartupSockets
    {
        /// <summary>
        /// OnConnect event handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="path"></param>
        /// <param name="environmentalVariables"></param>
        /// <returns></returns>
        Task<APIGatewayProxyResponse> OnConnectHandler(APIGatewayProxyRequest request, ILambdaContext context, string path, IDictionary environmentalVariables);

        /// <summary>
        /// SendMessage event handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="path"></param>
        /// <param name="environmentalVariables"></param>
        /// <returns></returns>
        Task<APIGatewayProxyResponse> SendMessageHandler(APIGatewayProxyRequest request, ILambdaContext context, string path, IDictionary environmentalVariables);

        /// <summary>
        /// OnDisconnect event handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="path"></param>
        /// <param name="environmentalVariables"></param>
        /// <returns></returns>
        Task<APIGatewayProxyResponse> OnDisconnectHandler(APIGatewayProxyRequest request, ILambdaContext context, string path, IDictionary environmentalVariables);
    }
}
