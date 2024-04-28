using Carter;

namespace WebApi.Extension
{
    public static class WebApplicationExtension
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger-client");
                c.OAuthClientSecret("client_secret");
            });

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapCarter();

            return app;
        }
    }
}
