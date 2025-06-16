using Yarp.ReverseProxy.Transforms.Builder;

namespace BFF.Transforms
{
    internal class TransformProvider : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            var requestTransform = context.Services.GetRequiredService<AccessTokenRequestTransform>();
            var responseTransform = context.Services.GetRequiredService<AccessTokenResponseTransform>();
            context.RequestTransforms.Add(requestTransform);
            context.ResponseTransforms.Add(responseTransform);
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {

        }

        public void ValidateRoute(TransformRouteValidationContext context)
        {

        }
    }
}
